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
    public partial class FunctionGeneratorForm : DockContent 
    {
        public FunctionGeneratorForm()
        {
            InitializeComponent();
        }
        public void SetSignalIO(CoreDevices.FunctionGenerator FG)
        {
            frequencyGenerator1.SetSignalIO(FG);
        }
    }
}
