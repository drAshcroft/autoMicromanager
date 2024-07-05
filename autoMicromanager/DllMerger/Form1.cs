using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DllMerger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void DoMerge(string Configuration,string Initial)
        {
            ILMerging.ILMerge Merger = new ILMerging.ILMerge();
            Merger.Closed = false ;

            Merger.TargetKind = ILMerging.ILMerge.Kind.Dll;
            Merger.UnionMerge = true;
            
            string DirPath;
            if (System.Diagnostics.Debugger.IsAttached == true)
                DirPath = @"C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\csharp_core 2.1.1\MM_Net\bin\Debug";
            else
                DirPath = Application.StartupPath;
            Merger.OutputFile = DirPath + "\\test.dll";
           
            string AssembDirPath = @"C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\csharp_core 2.1.1\MM_Net\bin\Debug" ;
            Merger.SetSearchDirectories(new string[] { AssembDirPath, @"C:\Documents and Settings\Administrator\Desktop\Microscopy Toolkit\csharp_core 2.1.1\MM_Net\bin\Debug" });
            System.Diagnostics.Debug.Print(DirPath);
            string[] Assemblies = null;
            try
            {
                Assemblies = Directory.GetFiles(AssembDirPath, "*.dll");
            }
            catch
            {
                Assemblies = null;
            }
            //string[] 
            Assemblies = new string[] { "freeimagenet.dll","Micromanager_net.dll","ICSharpCode.TextEditor.dll","MMCoreCS.dll",
            };
            List<string> MergeAssmb = new List<string>();
            foreach (string s in Assemblies)
            {
                MergeAssmb.Add(AssembDirPath + "\\" + s);
                /*if (s.Contains("ZedGraph.dll") != true && 
                    s.Contains("FreeImage") != true && 
                    s.Contains("WinFormsUI") != true  &&
                    s.ToLower().Contains("mmui") != true &&
                    s.ToLower().Contains("mmgr") != true
                    )
                {
                    MergeAssmb.Add(s);
                }*/
            }
            Merger.SetInputAssemblies(MergeAssmb.ToArray());
            Merger.Merge();
           // File.Copy(Merger.OutputFile, DirPath + "\\LabViewMM\\MicroManager\\PaintWhole" + Initial  + ".dll", true);
        }
        private void DoDebugBuild()
        {
            DoMerge("Debug", "D");
        }
        private void DoReleaseBuild()
        {
            DoMerge("Release", "R");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DoDebugBuild();
            try
            {
                DoReleaseBuild();
            }
            catch
            { }

            
        }
    }
}
