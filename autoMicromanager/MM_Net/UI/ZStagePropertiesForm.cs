using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//using VS2005Extender;

namespace Micromanager_net
{
    public partial class ZStagePropertiesForm : DockContent 
    {
        public ZStagePropertiesForm()
        {
            InitializeComponent();
        }
        public void SetStage(CoreDevices.ZStage stage)
        {
            zStageProperties1.SetStage(stage );
        }
    }
}
