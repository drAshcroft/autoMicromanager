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
    public partial class PropertiesList : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private CoreDevices.Devices.MMDeviceBase  device;
        private EasyCore Ecore;

        public string DeviceType() { return "GUIControl"; }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public PropertiesList()
        {
            InitializeComponent();
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Device Property List");
        }

        /// <summary>
        /// Device name referes the the micromanager or physical device name
        /// </summary>
        /// <param name="Ecore"></param>
        /// <param name="DeviceName"></param>
        public void SetCore(EasyCore Ecore,string DeviceName)
        {
            try
            {
                if (DeviceName != "")
                {
                     device =Ecore.GetDevice(DeviceName);
                }
                else
                {
                   
                }
            }
            catch { }
            if (device  != null)
            {
                try
                {
                    this.Ecore = Ecore;
                    device.SetPropUI((IPropertyList) propertyList1);
                }
                catch { }
                
            }
        }
       
      
    }
}
