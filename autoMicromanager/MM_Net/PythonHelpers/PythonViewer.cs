using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoreDevices.NI_Controls;

namespace Micromanager_net.PythonHelpers
{
    [Serializable]
    public partial class PythonViewer : Form
    {
        public PythonViewer()
        {
            InitializeComponent();
        }
        public PictureBoard ViewSurface
        {
            get { return pictureBoard1; }
        }
        public void DotNetDoEvents()
        {
            Application.DoEvents();
        }
    }
}
