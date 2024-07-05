using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName ="rundll32.exe";
            p.StartInfo.Arguments = "/x /s dfshim.dll,ShArpMaintain DotNetChecker.application, Culture=neutral, PublicKeyToken=58f8ffbcf2034730, processorArchitecture=msil";
            p.Start();
            this.Close();
        }
    }
}
