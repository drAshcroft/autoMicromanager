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
    /// A control to provide a nice way to control a filter wheel.  Not finished yet.
    /// </summary>
    public partial class FilterWheelControl : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private EasyCore ECore = null;
        private CoreDevices.Devices.FilterWheel filterWheel;

        public string DeviceType() { return CWrapper.DeviceType.StateDevice.ToString(); }

        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public FilterWheelControl()
        {
            InitializeComponent();
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Filter Wheel Properties");
        }
        public void SetCore(EasyCore Ecore, string DeviceName)
        {
            this.ECore = Ecore;
            try
            {
                if (DeviceName != "")
                {
                    filterWheel = (CoreDevices.Devices.FilterWheel)Ecore.GetDevice(DeviceName);
                }
                else
                {
                    filterWheel  = Ecore.MMFilterWheel ;
                }
            }
            catch { }


            if (ECore.MMFilterWheel != null)
            {
             
                filterWheel.SetPropUI((IPropertyList) propertyList1);
                this.Visible = true;
            }
            else
                this.Visible = false;
        }
    }
}
