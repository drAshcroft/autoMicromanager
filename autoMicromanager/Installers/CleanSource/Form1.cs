using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CleanSource
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();


            string [] Dirs= Directory.GetDirectories(folderBrowserDialog1.SelectedPath, "*", SearchOption.AllDirectories);
            foreach (string s in Dirs)
            {
                if (s.ToLower().Contains("freeimage") == false && s.ToLower().Contains("3rdparty")==false && s.ToLower(). Contains("\\bin")==true || s.ToLower().Contains ("\\obj")==true  )
                {
                    string[] Files =Directory.GetFiles(s);
                    foreach (string f in Files)
                    {
                        try
                        {
                            File.Delete(f);
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
