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
    /// These are a number of GUIs that I use my lab.  Their use is pretty straight forward and are intended as an example of the customization you can perform.
    /// </summary>
    public partial class DigitalIOLineController : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        EasyCore ECore = null;
        CoreDevices.Devices.StateDevice myState = null;
        private bool[] Lines = new bool[8];
        public DigitalIOLineController()
        {
            InitializeComponent();
        }

        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }

        public string DeviceType() { return CWrapper.DeviceType.StateDevice .ToString(); }
        

        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Digital IO Control");
        }
        public void SetCore(EasyCore Ecore, string DeviceName)
        {
            this.ECore = Ecore;
            try
            {
                if (DeviceName != "")
                {
                    myState = (CoreDevices.Devices.StateDevice)Ecore.GetDevice(DeviceName);
                }
                
            }
            catch { }


            if (myState != null)
            {
                myState.SetPropUI((IPropertyList)propertyList1);
                this.Visible = true;
            }
            else
                this.Visible = false;
        }

        private void glassButton1_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(0, glassButton1);
        }
        private void LightUpGlassButton(int index, CoreDevices.DeviceControls.GlassButton gButton)
        {
            Lines[index] = (!Lines[index]);
            if (Lines[index ] == true)
                gButton.BackColor = Color.Red;
            else
                gButton.BackColor = Color.Black;

            long PowerOf2=1;
            long Sum=0;
            for (int i=0;i<Lines.Length ;i++)
            {
                if (Lines[i]==true )
                    Sum+=PowerOf2 ;
                PowerOf2*=2;
            }
            StateLabel.Text = Sum.ToString();
            myState.State = Sum.ToString();
        }
        private void glassButton2_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(1, glassButton2);
        }

        private void glassButton3_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(2, glassButton3);
        }

        private void glassButton4_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(3, glassButton4);
        }

        private void glassButton5_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(4, glassButton5);
        }

        private void glassButton6_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(5, glassButton6);
        }

        private void glassButton7_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(6, glassButton7);
        }

        private void glassButton8_Click(object sender, EventArgs e)
        {
            LightUpGlassButton(7, glassButton8);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LineLabel0.Text = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label1.Text = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label2.Text = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label3.Text = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            label4.Text = textBox5.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            label5.Text = textBox6.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            label6.Text = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            label7.Text = textBox8.Text;
        }

        private void EditNames_Click(object sender, EventArgs e)
        {
            Names.Visible = (!Names.Visible);
        }
    }
}
