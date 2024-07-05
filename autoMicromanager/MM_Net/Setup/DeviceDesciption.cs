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

namespace Micromanager_net.Setup
{
    /// <summary>
    /// Used to contain the description of various devices available through micromanager
    /// </summary>
    [Serializable]
    public partial class DeviceDesciption : UserControl
    {
        
        public bool DeviceEnabled
        {
            get { return XDevEnabled.Checked; }
        }
        public string DeviceName
        {
            get { return xDeviceName.Text; }
        }
        public string DeviceLib
        {
            get { return xDeviceLib.Text; }
        }
        public string DeviceAdapter
        {
            get { return XDeviceAdapter.Text; }
        }
        public string DeviceGUI
        {
            get { return XDeviceUI.Text; }
        }
        public DeviceDesciption()
        {
            InitializeComponent();
        }
        EasyCore ECore;
        public void SetCore(EasyCore eCore)
        {
            ECore = eCore;
            string[] adapters = ECore.GetDeviceLibraries;

            LoadAdapters(xDeviceLib , adapters);
            LoadGUIs(XDeviceUI );
        }

        /// <summary>
        /// Shows all devices in given library
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="LibraryName"></param>
        private void LoadDevices(ComboBox cb, string LibraryName)
        {
            try
            {
                string[] Devices = ECore.GetDevicesFromLibrary(LibraryName);
                cb.Items.AddRange(Devices);
                cb.Text = "Select";
            }
            catch
            {
                cb.Text = "Please Select Library";
            }
        }
        /// <summary>
        /// Shows poassible adapter libraries
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="Adapters"></param>
        private void LoadAdapters(ComboBox cb, string[] Adapters)
        {
            cb.Items.Add("None");
            cb.Items.Add("GUI Only");
            cb.Items.AddRange(Adapters);
            cb.Text = "None";
        }
        /// <summary>
        /// Shows all GUIs compatable with library.
        /// </summary>
        /// <param name="cb"></param>
        private void LoadGUIs(ComboBox cb)
        {
            cb.Items.Add("None");
            cb.Items.AddRange(ECore.GetAllDeviceGUIs());
            cb.Text = "None";
        }
        private void xDeviceLib_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDevices(XDeviceAdapter, xDeviceLib.SelectedItem.ToString());
        }
    }
}
