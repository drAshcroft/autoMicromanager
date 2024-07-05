using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Micromanager_net
{
    public partial class SignIn : Form
    {
        private CoreDevices.EasyCore EasyCore;
        private Form1 MainForm;
        private string ExperimentFolder = "";
        private string ConfigFile="";
        private string GConfigFile = "";
        public bool StartUpHandled=false ;

        public string experimentFolder
        {
            get { return ExperimentFolder; }
        }
        public string GraphicConfigFile
        {
            get { return GConfigFile; }

        }
        public string HardwareConfigFile
        {
            get { return ConfigFile; }
        }

        public SignIn(CoreDevices.EasyCore easycore,Form1 MainForm)
        {
            InitializeComponent();
            EasyCore = easycore;
            this.MainForm = MainForm;

            List<string> Fs = new List<string>();
            try{Fs.AddRange( Directory.GetFiles(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\ConfigFiles") );}catch{}
            try{Fs.AddRange(Directory.GetFiles(System.IO.Path.GetDirectoryName(Application.ExecutablePath) ));}catch{}
            try{Fs.AddRange(Directory.GetFiles(easycore.PluginFolder  + "\\ConfigFiles"));}catch{}
            try { Fs.AddRange(Directory.GetFiles(easycore.PluginFolder)); }            catch { }

            foreach (string s in Fs )
            {
                try
                {
                    if (s.Contains("_Full") == true || s.Contains("_full") == true)
                    {
                        string f = Path.GetFileNameWithoutExtension(s);
                        f = f.Replace("_full", "");
                        f = f.Replace("_Full", "");
                        if (lbConfigs.Items.Contains(f)==false )
                            lbConfigs.Items.Add(f);
                    }
                }
                catch { }

            }
            tbExperimentFolder.Text = Path.GetDirectoryName(Application.ExecutablePath) + "\\UserData";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            tbExperimentFolder.Text = folderBrowserDialog1.SelectedPath;
            ExperimentFolder = tbExperimentFolder.Text;
        }

        private void bLoadConfig_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            int i= lbConfigs.Items.Add(openFileDialog1.FileName);
            lbConfigs.SelectedIndex = i;
            ConfigFile = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExperimentFolder = "";
            ConfigFile = "";
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string Filename;
            try
            {
                Filename  = lbConfigs.SelectedItem.ToString();
            }
            catch
            {
                Filename = lbConfigs.Items[0].ToString();
            }
            if (Filename.Trim() == "")
            {
                MessageBox.Show("Please select a desired hardware config file or create one with the create button");
                return;
            }
            if (ExperimentFolder.Trim() == "")
            {
                MessageBox.Show("Please select a folder where all images and files will be saved.");
                return;
            }
            string MMconfigFile;
            if (File.Exists(Filename) == false)
            {
                Filename = Filename.Replace(".xml", "");
                if (File.Exists( System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\ConfigFiles\\" +  Filename + ".xml")==true  )
                    MMconfigFile =  System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\ConfigFiles\\" +  Filename + ".xml";

                else if (File.Exists(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Filename + ".xml") == true)
                    MMconfigFile = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Filename + ".xml";
               
                else if (File.Exists( EasyCore.PluginFolder + "\\" +  Filename + ".xml")==true  )
                     MMconfigFile = EasyCore.PluginFolder + "\\" +  Filename + ".xml";

                else if (File.Exists(EasyCore.PluginFolder + "\\ConfigFiles\\" + Filename + ".xml") == true)
                    MMconfigFile = EasyCore.PluginFolder + "\\ConfigFiles\\" + Filename + ".xml";
                else
                {
                    MMconfigFile =
                     System.IO.Path.Combine(
                       System.IO.Path.GetDirectoryName(Application.ExecutablePath)
                       , "ConfigFiles\\" + Filename + ".xml");
                }
            }
            else
                MMconfigFile = Filename;
            ConfigFile = MMconfigFile;
            GConfigFile = Path.GetDirectoryName(MMconfigFile)
                + "\\" + Path.GetFileNameWithoutExtension(MMconfigFile)
                + "_Desktop.config";
                
            this.Hide();
        }

        private void bCreateConfig_Click(object sender, EventArgs e)
        {
            Setup.HardwareSetup2 hs = new Micromanager_net.Setup.HardwareSetup2(EasyCore, MainForm ,false );
            hs.ShowDialog();
            StartUpHandled = true;
            this.Hide();
        }

        private void tbExperimentFolder_TextChanged(object sender, EventArgs e)
        {
            ExperimentFolder = tbExperimentFolder.Text;
        }
    }
}
