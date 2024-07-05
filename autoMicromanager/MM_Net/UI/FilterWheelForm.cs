using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Micromanager_net
{
    public partial class FilterWheelForm : DockContent 
    {
        public FilterWheelForm()
        {
            InitializeComponent();
        }
        public void SetCore(CoreDevices.EasyCore ecore)
        {
            filterWheelControl1.SetCore(ecore);
        }
    }
}
