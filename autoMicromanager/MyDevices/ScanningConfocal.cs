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
    /// Designed to control a stage for scanning stage confocal microscopy with a camera.  Needs a lot of work
    /// </summary>
    public partial class ScanningConfocal : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        public ScanningConfocal()
        {
            InitializeComponent();
        }

           //string cameraLabel;
        EasyCore  core;
        //bool IgnoreEvents = false;
        PropertyInfo Gain;
        PropertyInfo Exposure;
        CoreDevices.Devices.Camera MMCamera;
        CoreDevices.Devices.XYStage MMStage;
        public string DeviceType() { return CWrapper.DeviceType.CameraDevice.ToString(); }

        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Scanning Confocal Properties");
        }
        public void SetCore(EasyCore Ecore,string DeviceName)
        {
                    
            core = Ecore;
            MMCamera = (CoreDevices.Devices.Camera)Ecore.GetDevice(DeviceName);
            MMCamera.SetPropUI((IPropertyList) propertyList1);
            MMCamera.OnHistogram += new CoreDevices.Devices .OnHistogramMadeEvent(MMCamera_OnHistogram);
            double e = MMCamera.GetExposure();
            ExposureS.LogScale = true;
            ExposureS.MinValue = 0;
            ExposureS.MaxValue = 1000;
           
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
            if (typeof(CoreDevices.Devices.ScanningConfocalCamera).IsAssignableFrom(MMCamera.GetType()) == true)
                cBPreviewImage.Visible = true;
            else
                cBPreviewImage.Visible = false;
           
        }

      

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

      

        private void cBPreviewImage_CheckedChanged(object sender, EventArgs e)
        {
            if (typeof(CoreDevices.Devices.ScanningConfocalCamera).IsAssignableFrom(MMCamera.GetType()) == true)
                ((CoreDevices.Devices.ScanningConfocalCamera)MMCamera).DoImagePreview = cBPreviewImage.Checked;
        }

   
        private void ExposureS_OnValueChanged_1(object sender, float value)
        {
            //Exposure.Value = ExposureS.Text;
            double exposure = 0;
            double.TryParse(ExposureS.Text, out exposure);
            MMCamera.SetExposure(exposure);
        }

        private void GainS_OnValueChanged_1(object sender, float value)
        {
            Gain.Value = GainS.Text;
        }

        

        

     }
    }

