using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micromanager_net.Setup
{
    public class DeviceDesc
    {
        public bool Enabled;
        public string DeviceName;
        public string DeviceLib;
        public string DeviceAdapter;
        public string DeviceGUI;
        public CWrapper.DeviceType DeviceType;
        private CoreDevices.Devices.MMDeviceBase physicalDevice;

        public CoreDevices.Devices.MMDeviceBase PhysicalDevice
        {
            get { return physicalDevice; }
            set { physicalDevice = value; }

        }
        public DeviceDesc(bool Enabled, string DeviceName, string DeviceLib, string DeviceAdapter, string DeviceGUI)
        {
            this.Enabled = Enabled;
            this.DeviceName = DeviceName;
            this.DeviceAdapter = DeviceAdapter;
            this.DeviceGUI = DeviceGUI;
            this.DeviceLib = DeviceLib;

        }
    }
}
