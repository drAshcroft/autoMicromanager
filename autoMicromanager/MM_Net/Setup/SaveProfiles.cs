// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
//                License text is included with the source distribution.
//
//                This file is distributed in the hope that it will be useful,
//                but WITHOUT ANY WARRANTY; without even the implied warranty
//                of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//                IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//                CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoreDevices;
using System.IO;

namespace Micromanager_net.Setup
{
    /// <summary>
    /// Provides the ability to save the current microscopy setup
    /// </summary>
    [Serializable]
    public partial class SaveProfiles : Form
    {
        EasyCore ECore;
        Form1 MainForm;

        /// <summary>
        /// Constructor: Mainform refers to the MDIhost of all the forms.
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="MainForm"></param>
        public SaveProfiles(EasyCore eCore,Form1 MainForm)
        {
            InitializeComponent();
            ECore = eCore;
            this.MainForm = MainForm;
            string[] files = Directory.GetFiles(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\ConfigFiles");
            foreach (string s in files)
            {
                if (s.Contains("_Full") == true || s.Contains("_full") == true)
                {
                    string f = Path.GetFileNameWithoutExtension(s);
                    f = f.Replace("_full", "");
                    f = f.Replace("_Full", "");

                    lbConfigs.Items.Add(f);
                }

            }
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            string Filename = textBox1.Text;
            if (Filename.Trim() == "")
            {
                MessageBox.Show("Please enter a name for this config (no need for an extenstion).");
                return;
            }
            string MMconfigFile;
            if (File.Exists(Filename) == false)
            {
                Filename = Filename.Replace(".xml", "");
                MMconfigFile =
                 System.IO.Path.Combine(
                   System.IO.Path.GetDirectoryName(Application.ExecutablePath)
                   , "ConfigFiles\\" + Filename + ".xml");
                //ECore.Core.saveSystemConfiguration(MMconfigFile);
            }
            else
                MMconfigFile = Filename;
            ECore.SaveConfigFile(MMconfigFile );
            MainForm.SaveDesktop(
                Path.GetDirectoryName(MMconfigFile ) 
                + "\\" + Path.GetFileNameWithoutExtension(MMconfigFile )
                + "_Desktop.config");
                
            this.Close();
        }

        private void lbConfigs_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = lbConfigs.SelectedItem.ToString();
        }

       
    }
}
