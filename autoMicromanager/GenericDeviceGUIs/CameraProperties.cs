// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
//                License text is included with the source distribution.
//
//                This file is distributed in the hope that it will be useful,
//                but WITHOUT ANY WARRANTY; without even the implied warranty
//                of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//                IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//                CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoreDevices;

namespace Micromanager_net.UI
{
    /// <summary>
    /// Provides a generic interface to a camera.  Most properties are shown in the property list, still needs proper ROI support
    /// </summary>
    public partial class CameraProperties : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        //string cameraLabel;
        EasyCore  ECore;
        //bool IgnoreEvents = false;
        PropertyInfo Gain;
        PropertyInfo Exposure;
        CoreDevices.Devices.Camera MMCamera;
        public string DeviceType() { return CWrapper.DeviceType.CameraDevice.ToString() ; }

        /// <summary>
        /// Used to store (to file) any specialized information about camera on a save.  Should be always updated
        /// </summary>
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }

        public CameraProperties()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns a control to allow display on any number of forms
        /// </summary>
        /// <returns></returns>
        public Control GetControl()
        {
            return this;
        }

        /// <summary>
        /// Caption that is shown on the parent form
        /// </summary>
        /// <returns></returns>
        public string Caption()
        {
            return ("Camera Properties");
        }

        public void SetCore(EasyCore Ecore,string DeviceName)
        {
          //  cameraLabel = core.getCameraDevice();
            try
            {
                filterWheelControl1.SetCore(Ecore, "");
            }
            catch { }
           
            ECore = Ecore;
            MMCamera = (CoreDevices.Devices.Camera)Ecore.GetDevice(DeviceName);
            ///Loads the property list from micromanager
            MMCamera.SetPropUI((IPropertyList) propertyList1);
            //Provides a visual display of LUT
            MMCamera.OnHistogram += new CoreDevices.Devices.OnHistogramMadeEvent(MMCamera_OnHistogram);

            //Todo:  This does not work correctly
            double e = MMCamera.GetExposure();

            //By default the camera exposure scale is log.  This can be set to a linear scale as well
            //Todo: Allow use to chose Log or linear
            ExposureS.LogScale = true;
            ExposureS.MinValue = 0;
            ExposureS.MaxValue = 1000;
           
            //Set up the slider to have the correct limits and current value
            try
            {
                Exposure = MMCamera.GetDevicePropertyInfo("exposure");
                bool hasLimits = Exposure.HasLimits;// core.hasPropertyLimits(cameraLabel, "Gain");
                if (hasLimits)
                {
                    int lL = (int)Exposure.MinValue;// core.getPropertyLowerLimit(cameraLabel, "Gain");
                    int uL = (int)Exposure.MaxValue;// core.getPropertyUpperLimit(cameraLabel, "Gain");
                    if (lL != uL)
                    {
                        try
                        {
                            ExposureS.MinValue = lL;
                            ExposureS.MaxValue = uL;
                        }
                        catch { }
                    }
                }
            }
            catch {   }
            
            ExposureS.Value = (float)e; 
            //If there is a gain setting then try to collect the ranges and values
            try
            {
                Gain= MMCamera.GetDevicePropertyInfo("MultiplierGain");
                string gain = Gain.Value;// core.getProperty(cameraLabel, "Gain");
                double GainD;
                double.TryParse(gain,out GainD);
                bool hasLimits =Gain.HasLimits;// core.hasPropertyLimits(cameraLabel, "Gain");
                if (hasLimits)
                {
                    int lL = (int)Gain.MinValue;// core.getPropertyLowerLimit(cameraLabel, "Gain");
                    int uL = (int)Gain.MaxValue;// core.getPropertyUpperLimit(cameraLabel, "Gain");
                    if (lL != uL)
                    {
                        try
                        {
                            GainS.MinValue  = lL;
                            GainS.MaxValue= uL;
                        }
                        catch { }
                    }
                }
            }
            catch
            {
                label2.Visible = false;
                GainS.Visible = false;
            }

           
        }

      
        /// <summary>
        /// Captures the histogram outputed by easycore for display and then allows setting of max and min values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Values"></param>
        /// <param name="MaxValue"></param>
        /// <param name="MinValue"></param>
        /// <param name="MaxPossible"></param>
        /// <param name="CI"></param>
        void MMCamera_OnHistogram(object sender, int[] Values, double  MaxValue,double  MinValue,long MaxPossible, CoreImage CI)
        {
            try 
            {
                lutGraph1.ShowGraph(Values,MaxValue,MinValue );
            }
            catch {}
            //if (lutGraph1.AutoContrast )  core.MMCamera.SetContrast(MinValue, MaxValue);
            CoreDevices.Devices.Camera c = (CoreDevices.Devices.Camera)sender;
            double length = lutGraph1.maxValue - lutGraph1.minValue;

            c.SetContrast(lutGraph1.minValue + length * .1, lutGraph1.minValue + length * .9);
            if (!lutGraph1.AutoContrast)
            {
                CI.MaxContrast =(long)( lutGraph1.maxValue*MaxPossible) ;
                CI.MinContrast = (long)( lutGraph1.minValue*MaxPossible);
            }
        }

       


        private void CameraProperties_Resize(object sender, EventArgs e)
        {
            ExposureS.Width = tabPage1.Width -  5 - ExposureS.Left ;
            GainS.Width = tabPage1.Width -  5 - ExposureS.Left;
           // Contrast._Width = tabPage1._Width - Contrast.Left;
        }

        private void ExposureS_OnValueChanged(object sender, float value)
        {
            double exposure = 0;
            double.TryParse(ExposureS.Text, out exposure);
            MMCamera.SetExposure(exposure);
        }

        private void GainS_OnValueChanged(object sender, float value)
        {
            Gain.Value = GainS.Text;
        }

        private CoreDevices.CoreImage BackgroundImage = null;
       
        private Int64[,] AddImages(Int64[,] BaseArray, CoreImage AddImage)
        {
            Int64[,] addArray = AddImage.GetArrayLong64();
            for (int i = 0; i < AddImage.Width; i++)
                for (int j = 0; j < AddImage.Height; j++)
                    BaseArray[i, j] += addArray[i, j];
            return BaseArray;
        }

        private Int64[,] DivideArray(Int64[,] BaseArray, double Denominator)
        {
            for (int i = 0; i < BaseArray.GetLength(0); i++)
                for (int j = 0; j < BaseArray.GetLength(1); j++)
                    BaseArray[i, j] = (long)((double) BaseArray[i,j]/ Denominator) ;
            return BaseArray;
        }

        /// <summary>
        /// Acquires a backgroundimage, averages over 15 frames to make sure that the image is smooth and low on random noise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoBackgroundImage_Click(object sender, EventArgs e)
        {
            BackgroundImage = MMCamera.SnapOneFrame(false );
            Int64[,] ImageArray = BackgroundImage.GetArrayLong64();
            int cc = 0;
            for (int i=0;i<15;i++)
            {
                CoreImage tempImage = MMCamera.SnapOneFrame(false );
                ImageArray =  AddImages(ImageArray, tempImage);
                
                ECore.UpdatePaintSurface(tempImage);
                
                Application.DoEvents();
                cc++;
            }
            DivideArray(ImageArray, cc);
            BackgroundImage= CoreImage.CreateImageFromArray(ImageArray);
            BackgroundImage.MinContrast = -1;
            BackgroundImage.MaxContrast = -1;

            ECore.UpdatePaintSurface(BackgroundImage);
            pBBackgroundImage.Image = BackgroundImage.ImageRGB;
        }

        /// <summary>
        /// Produces a background image by moving the stage around and averaing the resulting images.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovingBackground_Click(object sender, EventArgs e)
        {
            
            List <CoreImage > Frames =new List<CoreImage>();
            for (int i=0;i<20;i++)
            {
                ECore.MMXYStage.MoveStageRelative(100, 100);
                Frames.Add (MMCamera.SnapOneFrame(true ));
                ECore.MMXYStage.MoveStageRelative(-100, -100);
                Frames.Add(MMCamera.SnapOneFrame(true ));
                Application.DoEvents();
            }


            BackgroundImage = Frames[0];
            Int64[,] ImageArray = BackgroundImage.GetArrayLong64();
            int cc = 0;
            for (int i = 1; i < Frames.Count ; i++)
            {
                CoreImage tempImage = Frames[i];
                ImageArray = AddImages(ImageArray, tempImage);

                ECore.UpdatePaintSurface(tempImage);

                Application.DoEvents();
                cc++;
            }
            DivideArray(ImageArray, cc);
            BackgroundImage = CoreImage.CreateImageFromArray(ImageArray);
            BackgroundImage.MinContrast = -1;
            BackgroundImage.MaxContrast = -1;

            ECore.UpdatePaintSurface(BackgroundImage);
            pBBackgroundImage.Image = BackgroundImage.ImageRGB;
        }
      
        private void cbBackgroundSubtract_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBackgroundSubtract.Checked == true)
                ECore.AddImageProcessor("BackgroundSubtract", new ImageProcessorStep(DoBackgroundSubtraction));
            else
            {
                try
                {
                    ECore.RemoveImageProcessor("BackgroundSubtract");
                }
                catch { }
            }
        }

        /// <summary>
        /// Handles a background subtraction set.  This is called from easycore and then does the subtraction here
        /// </summary>
        /// <param name="coreImage"></param>
        /// <returns></returns>
        private CoreDevices.CoreImage[] DoBackgroundSubtraction(CoreDevices.CoreImage[] coreImage)
        {
            if (BackgroundImage != null)
            {
                coreImage[0].SubtractBackground (BackgroundImage);
                coreImage[0].MinContrast = -1;
                coreImage[0].MaxContrast = -1;
            }
            return coreImage;
        }

        /// <summary>
        /// Averages a number of frames together and then presents the result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AverageMacro_Click(object sender, EventArgs e)
        {
            CoreImage AverageImage = MMCamera.SnapOneFrame(false);
            Int64[,] ImageArray = AverageImage.GetArrayLong64();
            int cc = 0;
            for (int i = 0; i < 35; i++)
            {
                CoreImage tempImage = MMCamera.SnapOneFrame(false);
                ImageArray = AddImages(ImageArray, tempImage);

                ECore.UpdatePaintSurface(tempImage);

                Application.DoEvents();
                cc++;
            }
            DivideArray(ImageArray, cc);
            AverageImage = CoreImage.CreateImageFromArray(ImageArray);
            if (cbBackgroundSubtract.Checked == true)
            {
                AverageImage.SubtractBackground(BackgroundImage);
            }
            AverageImage.MinContrast = -1;
            AverageImage.MaxContrast = -1;

            ECore.UpdatePaintSurface(AverageImage);
            pBBackgroundImage.Image = BackgroundImage.ImageRGB;
            AverageImage.Save("c:\\test.bmp",false );
        }

        /// <summary>
        /// Saves current image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BSaveSpecialImage_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MMCamera.ClearROI();
            }
            catch { }
            int x=0;int y=0;int h=0;int w=0;
            int.TryParse(ROIX.Text ,out x);
            int.TryParse(ROIY.Text ,out y);
            int.TryParse(ROIW.Text ,out w);
            int.TryParse(ROIH.Text ,out h);
            try
            {
                MMCamera.SetROI(x, y, w, h);
            }
            catch { }
        }

        

       

     }
}
