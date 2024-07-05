using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreDevices.Devices
{
    public class GenericDevice: MMDeviceBase  
    {
       public GenericDevice(EasyCore ECore, string DeviceLabel, string LibraryName, string DeviceAdapter)
        {
            SetCore(ECore, DeviceLabel, LibraryName, DeviceAdapter);
        }

       public GenericDevice(EasyCore ECore, string DeviceLabel)
        {
            DeviceName = DeviceLabel;
            SetCore(ECore);
        }
       public override void StopDevice()
       {
           
       }
       public override void MakeOffical()
       {
           
       }
    }
}
