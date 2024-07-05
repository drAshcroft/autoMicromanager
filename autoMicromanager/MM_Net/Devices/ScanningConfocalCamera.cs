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
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CWrapper;

namespace CoreDevices.Devices
{
    /// <summary>
    /// This is mostly specialized to my scanning confocal setup.  Scans stage and returns an image
    /// </summary>
    [Serializable]
    public class ScanningConfocalCamera : Camera
    {
        XYStage ScanningStage = null;
        double ScanSizeX_um = 140;
        double ScanSizeY_um = 140;
        double ScanResolution_um = 2;
        #region Constructors
        public ScanningConfocalCamera()
            : base()
        {

        }

        public ScanningConfocalCamera(EasyCore ECore, string DeviceLabel, string LibraryName, string DeviceAdapter)
            : base(ECore, DeviceLabel, LibraryName, DeviceAdapter)
        {
        }
        public ScanningConfocalCamera(EasyCore ECore, string DeviceLabel)
            : base(ECore, DeviceLabel)
        {

        }
        #endregion

        private bool _DoImagePreview = true;
        /// <summary>
        /// Camera will update Viewer with partial image as it is built
        /// </summary>
        public bool DoImagePreview
        {
            set { _DoImagePreview = value; }
        }

        /// <summary>
        /// Builds an image by scanning the stage.
        /// </summary>
        /// <returns></returns>
        protected override CoreImage SnapFrame()
        {

            try
            {
                ClearPropBuffer();

                
                //todo: Something is wrong with the loading procedure that needs to be fixed
                //this is just a hack to get this running.  It should be initialized through the 
                //properties
                if (ScanningStage == null)
                {
                    ScanningStage = Ecore.MMXYStage;
                }
                double XMid_um ;
                double YMid_um;
                double x;
                double y;
                CoreImage b;
                if (_DoImagePreview == true)
                {
                    DeviceBusy = true;
                    core.snapImage();
                    Thread.Sleep((int)5);
                    b = core.getImage();
                   // b.Save("c:\\test.bmp", false);
                    DeviceBusy = false;
                    
                }
                else
                {
                    int NPoints = (int)Math.Round(ScanSizeX_um / ScanResolution_um);
                    long[,] Image = new long[NPoints, NPoints];
                    ScanningStage.GetStagePosition(out XMid_um, out YMid_um);
                    //core.setROI(200, 200, 5, 5);
                    for (int i = 0; i < NPoints; i++)
                        for (int j = 0; j < NPoints; j++)
                        {
                            x = XMid_um - ScanSizeX_um / 2 + ScanResolution_um * i;
                            y = YMid_um - ScanSizeY_um / 2 + ScanResolution_um * j;
                            ScanningStage.MoveStageGuarantee(x, y);
                            //ScanningStage.MoveStageAbsolute(x, y);
                            ClearPropBuffer();
                            DeviceBusy = true;
                            core.snapImage();
                            Thread.Sleep((int)5);
                            CoreImage c = core.getImage();
                            Ecore.UpdatePaintSurface(c);
                            Image[j, i] = (long)c.GetCenterPoint();//.AverageIntensity() ;
                            System.Windows.Forms.Application.DoEvents();
                            DeviceBusy = false;

                        }
                    b = CoreImage.CreateImageFromArray(Image);
                    b.Save("c:\\test.bmp", false);
                }
                DeviceBusy = false;
                return (b);
            }
            catch (Exception ex)
            {
                Ecore.LogErrorMessage("Camera is malfunctioning \n" + ex.Message, 1);
                return (null);
            }
        }

        public override  double ScreenSize_um
        {
            get
            {
                   
                    return ( ScanSizeX_um ); 
            }
        }

        #region Property_Overrrides

        /// <summary>
        /// Custom handling of a few properties that are essential for the scanning.  Base class handles all the normal stuff
        /// </summary>
        /// <param name="PropName"></param>
        /// <returns></returns>
        public override string GetDevicePropertyValue(string PropName)
        {
            if (PropName == "StageName")
            {
                return ScanningStage.deviceName;
            }
            else if (PropName == "ScanSizeX_um")
            {
                return  ScanSizeX_um.ToString();
               
            }
            else if (PropName == "ScanSizeY_um")
            {
                return  ScanSizeY_um.ToString();
              
            }
            else if (PropName == "ScanResolution_um")
            {
                return  ScanResolution_um.ToString();
               
            }
            else 
                return base.GetDevicePropertyValue(PropName);
        }
        /// <summary>
        /// Custom handling of a few properties that are essential for the scanning.  Base class handles all the normal stuff
        /// </summary>
        /// <param name="PropName"></param>
        /// <param name="_Value"></param>
        /// <param name="HasLimits"></param>
        /// <param name="MinValue"></param>
        /// <param name="MaxValue"></param>
        /// <param name="tType"></param>
        /// <param name="ReadOnly"></param>
        /// <param name="HasAllowedValues"></param>
        /// <param name="AllowedValues"></param>
        public override void GetDevicePropertyInfoDetails(string PropName, out string _Value, out bool HasLimits, out double MinValue, out double MaxValue, out PropertyType tType, out bool ReadOnly, out bool HasAllowedValues, out string[] AllowedValues)
        {
            if (PropName == "StageName")
            {
                _Value = ScanningStage.deviceName ;
                HasLimits = false ;

                tType = PropertyType.String;
                ReadOnly = false ;

                MinValue = 0;
                MaxValue = 0;

                List<string> Possibilities = new List<string>();
                foreach (MMDeviceBase db in ECore.GetLoadedDevices())
                {
                    if (db.GetType() == typeof(XYStage))
                        Possibilities.Add(db.deviceName);
                }
                AllowedValues = Possibilities.ToArray();
                HasAllowedValues = true;

               
            }
            else if (PropName == "ScanSizeX_um")
            {
                _Value = ScanSizeX_um.ToString() ;
                HasLimits = false;

                tType = PropertyType.Float ;
                ReadOnly = false;

                MinValue = 0;
                MaxValue = 0;

                
                AllowedValues = null ;
                HasAllowedValues = false ;


            }
            else if (PropName == "ScanSizeY_um")
            {
                _Value = ScanSizeY_um.ToString();
                HasLimits = false;

                tType = PropertyType.Float;
                ReadOnly = false;

                MinValue = 0;
                MaxValue = 0;


                AllowedValues = null;
                HasAllowedValues = false;


            }
            else if (PropName == "ScanResolution_um")
            {
                _Value = ScanResolution_um .ToString();
                HasLimits = false;

                tType = PropertyType.Float;
                ReadOnly = false;

                MinValue = 0;
                MaxValue = 0;


                AllowedValues = null;
                HasAllowedValues = false;


            }
            else 
                base.GetDevicePropertyInfoDetails(PropName, out _Value, out HasLimits, out MinValue, out MaxValue, out tType, out ReadOnly, out HasAllowedValues, out AllowedValues);
        }
        /// <summary>
        /// Custom handling of a few properties that are essential for the scanning.  Base class handles all the normal stuff
        /// </summary>
        /// <param name="PropName"></param>
        /// <param name="PropValue"></param>
        public override void SetDeviceProperty(string PropName, string PropValue)
        {
            if (PropName == "StageName")
            {
                AllProperties["StageName"].Value = PropValue;
                ScanningStage = (XYStage) ECore.GetDevice(PropValue);

            }
            else if (PropName == "ScanSizeX_um")
            {
                double.TryParse(PropValue, out ScanSizeX_um);
            }
            else if (PropName == "ScanSizeY_um")
            {
                double.TryParse(PropValue, out ScanSizeY_um);
            }
            else if (PropName == "ScanResolution_um")
            {
                double.TryParse(PropValue, out ScanResolution_um);
            }
            else
                base.SetDeviceProperty(PropName, PropValue);
        }
        
        /// <summary>
        /// Adds custom properties to the property list and then uses the base class to finish up
        /// </summary>
        /// <param name="ClearProperties"></param>
        protected override void InitializePropertyList(bool ClearProperties)
        {
            if (ClearProperties ==true)
                AllProperties = new Dictionary<string, PropertyInfo>();
            if (Ecore.MMXYStage !=null)
                AllProperties.Add("StageName", new PropertyInfo(ECore, this.deviceName, "StageName", Ecore.MMXYStage.deviceName));
            else
                AllProperties.Add("StageName", new PropertyInfo(ECore, this.deviceName, "StageName", "xyStage"));
            AllProperties.Add("ScanSizeX_um", new PropertyInfo(ECore, this.deviceName, "ScanSizeX_um", ScanSizeX_um.ToString()));
            AllProperties.Add("ScanSizeY_um", new PropertyInfo(ECore, this.deviceName, "ScanSizeY_um", ScanSizeY_um.ToString()));
            AllProperties.Add("ScanResolution", new PropertyInfo(ECore, this.deviceName, "ScanResolution_um", ScanResolution_um.ToString()));
            base.InitializePropertyList(false );
        }
        #endregion

        #region Sequences
        public override void StartSequence(int NumFrames, double Interval, bool DisplayImage)
        {

        }

        public override CoreImage GetSequenceFrame()
        {
            return SnapFrame();
        }
        public override CoreImage GetBlockingSequenceFrame(double MaxWaitTime)
        {
            return GetSequenceFrame();
        }
        public override void EndSequence()
        {
            if (PausedFocus)
            {
                FocusThread.Resume();
                PausedFocus = false;
            }
        }
        #endregion

    }
}