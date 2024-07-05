using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DILoader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            short[,] data= FileLoader.LoadDI("C:\\Documents and Settings\\Administrator\\Desktop\\BackedUP\\Microparticles\\2009-06-19 purified Microparticles\\brian2009-06-17_cd41_mf_2.001");
            pictureBox1.Image = FileLoader.MakeBitmap(data);
        }
    }
}
