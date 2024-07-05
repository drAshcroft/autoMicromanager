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
    public partial class FilterWheelControl : UserControl, UI.GUIDeviceControl 
    {
        private CoreDevices.EasyCore ECore = null;
        private CoreDevices.FilterWheel filterWheel;

        public string DeviceType() { return "FilterWheel"; }
        

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
        public void SetCore(CoreDevices.EasyCore Ecore, string DeviceName)
        {
            this.ECore = Ecore;
            try
            {
                if (DeviceName != "")
                {
                    filterWheel  = (CoreDevices.FilterWheel)Ecore.GetDevice(DeviceName);
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
