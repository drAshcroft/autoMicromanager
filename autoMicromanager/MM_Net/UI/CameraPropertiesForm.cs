using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking ;// WinFormsUI;
//using VS2005Extender;

namespace Micromanager_net
{
    public partial class CameraPropertiesForm : DockContent 
    {
        public CameraPropertiesForm()
        {
            InitializeComponent();
        }
        public void SetCore(CoreDevices.EasyCore  Core)
        {
            cameraProperties1.SetCore(Core,"");
            

        }

        private void cameraProperties1_Load(object sender, EventArgs e)
        {

        }
    }
}
