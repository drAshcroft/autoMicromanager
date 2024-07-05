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

namespace CoreDevices.Channels
{
    [Serializable]
    public partial class StateCommand : UserControl
    {
        private EasyCore ECore;
        /// <summary>
        /// Outputs the command the user has entered as a array of strings
        /// </summary>
        /// <returns></returns>
        public string[] GetSelectedCommand()
        {
            string[] Command = new string[3];
            Command[0] = DeviceName.Text;
            Command[1] = MethodName.Text;
            Command[2] = ValueTB.Text;
            return Command;
        }
        /// <summary>
        /// Show an existing command
        /// </summary>
        /// <param name="Command"></param>
        public void SetSelectedCommand(string[] Command)
        {
            DeviceName.SelectedText = Command[0];
            MethodName.SelectedText = Command[1];
            ValueTB.Text = Command[2];
            DeviceName.Text = Command[0];
            MethodName.Text = Command[1];
        }
        public void SetCore(EasyCore eCore)
        {
            ECore = eCore;
            DeviceName.Items.AddRange(ECore.GetAllLoadedDeviceNames());

        }
        public StateCommand()
        {
            InitializeComponent();
        }

        private void DeviceName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string DeviceNameS = DeviceName.SelectedItem.ToString();
            Devices.MMDeviceBase device=ECore.GetDevice(DeviceNameS );
            string [] Propnames=device.GetDevicePropertyNames();
            MethodName.Items.Clear();
            for (int i = 0; i < Propnames.Length; i++)
            {
                if (Propnames[i] != null) MethodName.Items.Add(Propnames[i]);
                
            }
            
        }

    }
}
