using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace InstallScriptFixer
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
        private List<string> InstalledFiles = new List<string>();
        private List<string> InstalledShortcuts = new List<string>();
        private List<string> InstalledDirectories = new List<string>();

        private string CurrentFilename = "";

        private void ProcessFile(string FileName)
        {
            StreamReader re = File.OpenText(FileName);
            string TemplateText = re.ReadToEnd();

            re.Close();

            InstalledFiles = new List<string>();
            InstalledShortcuts = new List<string>();
            InstalledDirectories = new List<string>();


            string OutText = "";
            string BaseDir = Path.GetDirectoryName(openFileDialog1.FileName);
            string[] Lines = TemplateText.Split(new string[] { "\n\r", "\r\n", "\n" }, StringSplitOptions.None);
            for (int i = 0; i < Lines.Length; i++)
            {
                string s = Lines[i];
                if (s.Contains("<%") == true && s.Contains("%>") == true)
                {
                    int startOf = s.IndexOf("<%");
                    int endOf = s.IndexOf("%>");
                    string[] Commands;
                    if (startOf < endOf)
                    {
                        string instruction = s.Substring(startOf + 2, endOf - startOf - 3);
                        Commands = instruction.Split(new string[] { "\" \"", " \"", "\" ", "\"\"" }, StringSplitOptions.RemoveEmptyEntries);

                        if (Commands[0].ToLower().Contains("insertfiles") == true)
                        {
                            string junk = InsertFiles(Commands[1], BaseDir + "\\" + Commands[2]);
                            Lines[i] = junk;
                        }
                        if (Commands[0].ToLower().Contains("deletefiles") == true)
                        {
                            string junk = DeleteFiles(Commands[1], BaseDir + "\\" + Commands[2]);
                            Lines[i] = junk;
                        }
                        if (Commands[0].ToLower().Contains("insertshortcuts") == true)
                        {
                            string junk = InsertShortCuts(Commands[1], Commands[2], BaseDir + "\\" + Commands[3]);
                            Lines[i] = junk;
                        }
                        if (Commands[0].ToLower().Contains("deleteshortcuts") == true)
                        {
                            string junk = DeleteShortCuts(Commands[1], BaseDir + "\\" + Commands[2]);
                            Lines[i] = junk;
                        }
                        if (Commands[0].ToLower().Contains("deleteallshortcuts") == true)
                        {
                            string junk = DeleteAllShortCuts();
                            Lines[i] = junk;
                        }
                        if (Commands[0].ToLower().Contains("deleteallfiles") == true)
                        {
                            string junk = DeleteAllFiles();
                            Lines[i] = junk;
                        }
                        if (Commands[0].ToLower().Contains("deletealldirectories") == true)
                        {
                            string junk = DeleteAllDirectories();
                            Lines[i] = junk;
                        }
                    }
                }
                OutText += Lines[i] + "\n";
            }


            File.WriteAllText(Path.GetDirectoryName(openFileDialog1.FileName) + "\\out.nsi", OutText);
            label1.Text = "Done";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Processing";
            if ( openFileDialog1.ShowDialog() ==DialogResult.OK) 
                ProcessFile(openFileDialog1.FileName);
            CurrentFilename = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentFilename != "")
                ProcessFile(CurrentFilename);
            else
                button1_Click(sender, e);
        }

        private string InsertShortCuts(string InstallDir, string TargetMenuFolder, string SourceDir)
        {
            string Foldername;
            string FileList = "";


            Foldername =Path.GetDirectoryName( SourceDir);
            string SearchPattern = Path.GetFileName(SourceDir);
            if (SearchPattern.Trim() == "") SearchPattern = "*.*";
            string[] Files = Directory.GetFiles(Foldername, SearchPattern , SearchOption.AllDirectories);

            if (InstalledDirectories.Contains(TargetMenuFolder ) ==false )   InstalledDirectories.Add(TargetMenuFolder);

            string lInstallDir = "";
            foreach (string s in Files)
            {
                //<%InsertShortCuts "$SMPROGRAMS\Micromanager.NET\Tutorials\" "$INSTDIR\Tutorials\" "..\LabViewMM\*.vi"%>
                //CreateShortCut "$SMPROGRAMS\Micromanager.NET\Tutorials\Tutorial1.lnk" "$INSTDIR\Tutorials\Tutorial1.vi"

                FileList += "CreateShortCut \"" + InstallDir + Path.GetFileNameWithoutExtension(s) + ".lnk\" \"" + TargetMenuFolder + Path.GetFileName(s) + "\"\r\n";
                System.Diagnostics.Debug.Print(FileList);
                InstalledShortcuts.Add(InstallDir + Path.GetFileNameWithoutExtension(s) + ".lnk");
            }
            return FileList;

        }
        private string DeleteShortCuts( string TargetMenuFolder, string SourceDir)
        {
            string Foldername;
            string FileList = "";


            Foldername = Path.GetDirectoryName(SourceDir);
            string SearchPattern = Path.GetFileName(SourceDir);
            if (SearchPattern.Trim() == "") SearchPattern = "*.*";
            string[] Files = Directory.GetFiles(Foldername, SearchPattern, SearchOption.AllDirectories);

            
            foreach (string s in Files)
            {
                //<%InsertShortCuts "$SMPROGRAMS\Micromanager.NET\Tutorials\" "$INSTDIR\Tutorials\" "..\LabViewMM\*.vi"%>
                //CreateShortCut "$SMPROGRAMS\Micromanager.NET\Tutorials\Tutorial1.lnk" "$INSTDIR\Tutorials\Tutorial1.vi"

                FileList += "Delete \"" + TargetMenuFolder  + Path.GetFileNameWithoutExtension(s) + ".lnk\"\r\n";
                
            }
            return FileList;

        }
        private string DeleteAllShortCuts()
        {
           
            string FileList = "";


            foreach (string s in InstalledShortcuts )
            {
                //<%InsertShortCuts "$SMPROGRAMS\Micromanager.NET\Tutorials\" "$INSTDIR\Tutorials\" "..\LabViewMM\*.vi"%>
                //CreateShortCut "$SMPROGRAMS\Micromanager.NET\Tutorials\Tutorial1.lnk" "$INSTDIR\Tutorials\Tutorial1.vi"

                FileList += "Delete \"" + s + "\"\r\n";

            }
            return FileList;

        }
        private string InsertFiles(string InstallDir, string SourceDir)
        {
            string Foldername;
            string FileList = "";
           

            Foldername = SourceDir ;
            string[] Files = Directory.GetFiles(Foldername, "*.*", SearchOption.AllDirectories);

            string lInstallDir = "";
            string outDir = "";
            foreach (string s in Files)
            {
                if (Path.GetExtension(s).ToLower() != ".pdb")
                {
                    string shorterS = s.Replace(Foldername + "\\", "");
                    shorterS = shorterS.Replace(Foldername, "");
                    string tInstallDir = "";
                    if (shorterS.Contains("\\") == true)
                    {
                        string[] DirNames = shorterS.Split(new char[] { '\\' });
                        string DirName = "";
                        for (int i = 0; i < DirNames.Length - 1; i++)
                        {
                            DirName += "\\" + DirNames[i];
                        }
                        tInstallDir = "SetOutPath \"" +  InstallDir.Remove(InstallDir.Length-1 )  + DirName + "\"\r\n ";
                        outDir = InstallDir.Remove(InstallDir.Length-1) + DirName;
                    }
                    else
                    {
                        tInstallDir = "SetOutPath \"" + InstallDir + "\" \r\n";
                        outDir = InstallDir.Remove(InstallDir.Length - 1);
                    }
                    if (lInstallDir != tInstallDir)
                        FileList += tInstallDir;
                    lInstallDir = tInstallDir;
                    FileList += "File \"" + s + "\"\r\n";
                    InstalledFiles.Add(outDir + "\\" + Path.GetFileName (s));
                    if (InstalledDirectories.Contains(outDir + "\\") == false) 
                        InstalledDirectories.Add(outDir + "\\");
                }
            }
            return FileList;

        }
        private string DeleteAllFiles()
        {
           
            string FileList = "";

            foreach (string s in InstalledFiles )
            {
                FileList += "Delete \"" +s + "\"\r\n";
            }
            return FileList;

        }
        private string DeleteAllDirectories()
        {
            string FileList = "";

            foreach (string s in InstalledDirectories )
            {
                FileList += "RMDir \"" + s + "\"\r\n";
            }
            return FileList;
        }
        private string DeleteFiles(string InstallDir, string SourceDir)
        {
            string Foldername;
            string FileList = "";


            Foldername = SourceDir;
            string[] Files = Directory.GetFiles(Foldername, "*.*", SearchOption.AllDirectories);

            string lInstallDir = "";
            foreach (string s in Files)
            {
                if (Path.GetExtension(s).ToLower() != ".pdb")
                {
                    string shorterS = s.Replace(Foldername + "\\", "");
                    shorterS = shorterS.Replace(Foldername, "");
                    string tInstallDir = "";
                    if (shorterS.Contains("\\") == true)
                    {
                        string[] DirNames = shorterS.Split(new char[] { '\\' });
                        string DirName = Path.GetDirectoryName(shorterS);

                        tInstallDir = "" + InstallDir + DirName + "\\";
                    }
                    else
                        tInstallDir = InstallDir;

                    FileList += "Delete \"" + tInstallDir + Path.GetFileName(s) + "\"\r\n";
                }
            }
            return FileList;

        }

       
       
    }
}
