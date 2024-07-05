using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace RemovePDNREsources
{
    public partial class Form1 : Form
    {
        List<string> Targetfiles = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.IO.StreamReader sIn = new System.IO.StreamReader("ScImage.strings.text");
            string[] sep = { "\t\t" };
            Dictionary<string, string> Infos = new Dictionary<string, string>();

            while (!sIn.EndOfStream)
            {
                string line = sIn.ReadLine();
                string[] parts = line.Split(sep, StringSplitOptions.None);
                try
                {
                    Infos.Add(parts[0], parts[1]);
                }
                catch
                { }
            }
            sIn.Close();



            ProcessDirectory("C:\\Documents and Settings\\Administrator\\Desktop\\micro-manager-1.1.47\\csharp_core");
        }

        public void GetFiles(string path)
        {

            if (File.Exists(path))
            {

                // This path is a file

                ProcessFile(path);

            }

            else if (Directory.Exists(path))
            {

                // This path is a directory

                ProcessDirectory(path);

            }

        }



        // Process all files in the directory passed in, recurse on any directories

        // that are found, and process the files they contain.

        public void ProcessDirectory(string targetDirectory)
        {

            // Process the list of files found in the directory.

            string[] fileEntries = Directory.GetFiles(targetDirectory);

            foreach (string fileName in fileEntries)

                ProcessFile(fileName);



            // Recurse into subdirectories of this directory.

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);

            foreach (string subdirectory in subdirectoryEntries)

                ProcessDirectory(subdirectory);

        }



        // Insert logic for processing found files here.

        public void ProcessFile(string path)
        {

            FileInfo fi = new FileInfo(path);
            Targetfiles.Add(path);
        }
    }
}
