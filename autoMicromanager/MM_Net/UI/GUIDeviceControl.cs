using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Micromanager_net.UI
{
    public interface GUIDeviceControl
    {
        void SetCore(CoreDevices.EasyCore Ecore, string DeviceName);
        string DeviceType();
        string Caption();
        Control GetControl();        
    }
}
