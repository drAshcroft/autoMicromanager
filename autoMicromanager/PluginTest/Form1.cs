using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PluginTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            niViewer1.CreateBasicSetup();
            CoreDevices.CoreImage cI = new CoreDevices.CoreImage(@"C:\untitled.tif");
            //pictureBox1.Image = cI.ImageRGB;
            //pictureBox1.Invalidate();
            cI.MaxContrast = 255;
            niViewer1.SendImage(cI); 
        }
    }
}
