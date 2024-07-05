using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Micromanager_net
{
    public partial class CameraProperties : UserControl, UI.GUIDeviceControl 
    {
        //string cameraLabel;
        CoreDevices.EasyCore  core;
        //bool IgnoreEvents = false;
        CoreDevices.PropertyInfo Gain;
        CoreDevices.PropertyInfo Exposure;

        public string DeviceType() { return "Camera"; }
        
        public CameraProperties()
        {
            InitializeComponent();
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Camera Properties");
        }
        public void SetCore(CoreDevices.EasyCore Ecore,string DeviceName)
        {
          //  cameraLabel = core.getCameraDevice();
            filterWheelControl1.SetCore(Ecore,"");
            recordTools1.SetCore(Ecore,DeviceName );
            core = Ecore;
            Ecore.MMCamera.SetPropUI((IPropertyList) propertyList1);
            
            Ecore.MMCamera.OnHistogram += new Micromanager_net.CoreDevices.OnHistogramMadeEvent(MMCamera_OnHistogram);
            double e = Ecore.MMCamera.GetExposure();
            ExposureS.LogScale = true;
            ExposureS.MinValue = 0;
            ExposureS.MaxValue = 1000;
           
            try
            {
                Exposure = Ecore.MMCamera.GetDevicePropertyInfo("exposure");
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
                Gain= Ecore.MMCamera.GetDevicePropertyInfo("MultiplierGain");
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

        void MMCamera_OnHistogram(object sender, int[] Values, double  MaxValue,double  MinValue,long MaxPossible, CoreDevices.CoreImage CI)
        {
            lutGraph1.ShowGraph(Values,MaxValue,MinValue );
            //if (lutGraph1.AutoContrast )  core.MMCamera.SetContrast(MinValue, MaxValue);
            CoreDevices.Camera c=(CoreDevices.Camera ) sender ;
            c.SetContrast(lutGraph1.minValue ,lutGraph1.maxValue );
            if (!lutGraph1.AutoContrast)
            {
                CI.MaxContrast = lutGraph1.maxValue;
                CI.MinContrast = lutGraph1.minValue;
            }
        }

       


        private void CameraProperties_Resize(object sender, EventArgs e)
        {
            ExposureS.Width = tabPage1.Width -  5 - ExposureS.Left ;
            GainS.Width = tabPage1.Width -  5 - ExposureS.Left;
           // Contrast.Width = tabPage1.Width - Contrast.Left;
        }

        private void ExposureS_OnValueChanged(object sender, float value)
        {
            Exposure.Value = ExposureS.Text;
        }

        private void GainS_OnValueChanged(object sender, float value)
        {
            Gain.Value = GainS.Text;
        }

     }
}
