using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Micromanager_net.UI
{
    public partial class FrequencyGenerator : UserControl, UI.GUIDeviceControl 
    {
        CoreDevices.FunctionGenerator FuncGen;

        public string DeviceType() { return "IOBoard"; }
        
        public FrequencyGenerator()
        {
            InitializeComponent();
            
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Function Generator Properties");
        }
        public void SetCore(CoreDevices.EasyCore Ecore,string DeviceName)
        {
        //public void SetSignalIO(CoreDevices.FunctionGenerator fg)
            try
            {
                if (DeviceName != "")
                {
                    FuncGen = (CoreDevices.FunctionGenerator)Ecore.GetDevice(DeviceName);
                }
                else
                {
                    FuncGen = Ecore.MMFunctionGenerator;
                }
            }
            catch { }
            if (FuncGen != null)
            {
                FuncGen.SetPropUI((IPropertyList )propertyList1);
                textBox1.Text = FuncGen.UpdateFrequency.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            double d;
            try
            {
                d = double.Parse(textBox1.Text);
            }
            catch
            {
                d = 150;
            }
            FuncGen.UpdateFrequency =d;
        }

        private void StartGenerationB_Click(object sender, EventArgs e)
        {
            FuncGen.StartGenerating();
        }

        private void StopB_Click(object sender, EventArgs e)
        {
            FuncGen.StopGenerating();
        }

        private void AmplitudeTB_TextChanged(object sender, EventArgs e)
        {
            double d;
            try
            {
                d = double.Parse(AmplitudeTB.Text);
            }
            catch
            {
                d = .03;
            }
            FuncGen.Amplitude =d;
        }

       

       
        

    }
}
