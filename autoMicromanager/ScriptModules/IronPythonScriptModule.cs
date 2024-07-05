using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Microsoft.VisualBasic;
using CoreDevices;
using Microsoft.Win32;
using System.Diagnostics;
using ICSharpCode.TextEditor.Document;

namespace MMUI_ScriptModules
{
    public partial class IronPythonScriptModule : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private EasyCore ECore = null;
        
        public string DeviceType() { return "GUIControl"; }
        public string ExtraInformation
        {
            get { return CodeText.Text ; }
            set { CodeText.Text = value; }
        }
        
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("IronPython ScriptControl");
        }
        private CoreDevices.Devices.MMEmptyGuiDevice MyGuiDev;
       

           
        public void SetCore(EasyCore Ecore, string DeviceName)
        {

            if (DeviceName == "")
                DeviceName = "sIronPython";
            try
            {
                MyGuiDev = (CoreDevices.Devices.MMEmptyGuiDevice)Ecore.GetDevice(DeviceName);
            }
            catch
            {
                MyGuiDev = new CoreDevices.Devices.MMEmptyGuiDevice(Ecore, DeviceName);
                
            }
            ECore = Ecore;
            
            EP = new EvaluatePython();
          //  EP.SetOutputPath();
            CodeText.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("Python");

            CodeText.Text =
@"
#This is an example template that shows how to interact with devices, capture an image. You can also fully automate the system from 
#this script. 

#ECore is already defined to refer the running easycore and 
#NIEasyCore is defined to help the scripting match the tutorials and is already running.
#ScriptPanel is the panel that shows within the display tab.  You can add buttons and such for a GUI
import clr
clr.AddReference('System.Windows.Forms')
from System.Windows.Forms import *

#If you wish to catch the images that are produced by the system, you must register as an image processor, and then watch for the events
#Console.RegisterAsImageProcessor('IgorProcess')
def ImageCatch(CoreImages):
    #Your image processing code here.
    Console.WriteLine('Number Of Channels:' + len(CoreImages).ToString())

    return CoreImages

def EndImageCatch():
    #Cleans all the event handlers out of the system for the next code
    Console.WriteLine('Stopping Eventhandlers')
    Console.KillRequested -= EndImageCatch;
    Console.RemoveImageProcessor('IgorProcess')
    Console.ImageProduced -= ImageCatch

#This allows the python routines to watch for the image produced events.
#Console.ImageProduced += ImageCatch
#This turns off the script for the next script or for the closing of the program
Console.KillRequested += EndImageCatch

#Example of capturing a devices from ECore and then just a trivial example of how to use it.
#camera=ECore.GetDevice('camera')
#Console.WriteLine(camera.GetType().ToString())

";
        }
        private EvaluatePython EP;
        private delegate CoreImage[] getImageDelg(CoreImage[] cImage);
        
        public CoreDevices.CoreImage[] getImage(CoreImage[] cImage)
        {
            
            if (InvokeRequired)
            {
                getImageDelg gid= new getImageDelg(getImage);
               // List<CoreImage> LC = new List<CoreImage>(cImage);
               
                object o2=this.Invoke(gid,new object[]{cImage});
                return (CoreImage[])o2;
            }
            else
            {
               // MessageBox.Show("ImageProduced");
                if (ImageProduced != null)
                {
                    try
                    {
                        object o = ImageProduced(cImage);
                        WriteLine(  IPEStreamWrapper.sbOutput.ToString());
                        
                        //added to fix "rearviewmirror" (IPEStreamWrapper.sbOutput not clearing) bug.
                        IPEStreamWrapper.sbOutput.Remove(0, IPEStreamWrapper.sbOutput.Length);
                        if (o.GetType() == typeof(CoreImage))
                            return new CoreImage[] { (CoreImage)o };
                        else
                        {
                            return (CoreImage[])o;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message);
                        if (ex.InnerException != null)
                            WriteLine(ex.InnerException.Message);
                        return cImage;
                    }
                }
                else 
                    return cImage ;
            }
            
        }
        public IronPythonScriptModule()
        {
            InitializeComponent();
            
        }

      
        #region Time Measure
        private DateTime BeginAction()
        {
            return DateTime.Now;
        }

        private TimeSpan EndAction(DateTime startTime)
        {
            return DateTime.Now.Subtract(startTime);
        }
        #endregion


        #region Console
        public void Write(object o)
        {
            richTextBox1.Text += o.ToString();
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        public void WriteLine(object o)
        {
            richTextBox1.Text += "\r\n" + o.ToString();
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private object AutomationObject = null;
        private string AutomationName = "";
        public object GetAutomationObject(string ProgID)
        {
            if (AutomationObject == null || ProgID != AutomationName)
            {

                AutomationObject = System.Activator.CreateInstance(Type.GetTypeFromProgID(ProgID));
                AutomationName = ProgID;
                return AutomationObject;
            }
            else
                return AutomationObject;
           /*try
           {
              object o= Microsoft.VisualBasic.Interaction.GetObject(""  ,ProgID);
              return o;
           }
            catch (Exception ex)
           {
            return System.Activator.CreateInstance(Type.GetTypeFromProgID(ProgID));
            }*/
            
        }

        public event ImageProcessorStep ImageProduced;
        public void RegisterAsImageProcessor(string ProcessorName)
        {
            ECore.AddImageProcessor(ProcessorName , new ImageProcessorStep(getImage));
        }
        public void RemoveImageProcessor(string ProcessorName)
        {
            try
            {
                ECore.RemoveImageProcessor(ProcessorName);
            }
            catch
            { }
        }
        public event KillRequestedEvent KillRequested;
        public delegate void KillRequestedEvent();
        #endregion

       

       

        private void runScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (KillRequested != null)
            {
                try
                {
                    KillRequested();
                }
                catch { }
            }
            WriteLine(EP.evaluate(CodeText.Text, this, ECore));
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeText.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Filename = openFileDialog1.FileName;
            StreamReader re = File.OpenText(Filename );
            string input = re.ReadToEnd();
            
            re.Close();
            
        }
        private string Filename = "";
        private void SaveFile(string sFilename)
        {

            if (sFilename == "")
            {
                
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                {
                    return;

                }
                sFilename = saveFileDialog1.FileName;
            }
            Filename = sFilename;
            FileInfo t = new FileInfo(sFilename);
            StreamWriter Tex = t.CreateText();
            Tex.Write(CodeText.Text);
            Tex.Close();
             
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(Filename);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile("");
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartURL("http://labviewmicroman.sourceforge.net/");
        }


        /// <summary>

        /// Reads path of default browser from registry

        /// </summary>

        /// <returns></returns>

        private static string GetDefaultBrowserPath()
        {

            string key = @"htmlfile\shell\open\command";

            RegistryKey registryKey =

            Registry.ClassesRoot.OpenSubKey(key, false);

            // get default browser path

            return ((string)registryKey.GetValue(null, null)).Split('"')[1];

        }

        private void StartURL(string URL)
        {


            // open URL in separate instance of default browser

            Process p = new Process();

            p.StartInfo.FileName = GetDefaultBrowserPath();

            p.StartInfo.Arguments = URL ;

            p.Start();

        }

        private void classReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pythonHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartURL("http://python.org/doc/");
        }

        private void ironPythonHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartURL("http://ironpython.net/");
        }

        private void sToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CodeText.Text = 
@"import clr
import sys
clr.AddReference('System.Windows.Forms')
from System.Windows.Forms import *

IgorApp = Console.GetAutomationObject('IgorPro.Application')
IgorApp.Visible=True
datafolder=IgorApp.DataFolder(':')

def ExecuteIgorProCmd(IgorApp, cmd, PrintDiagnostics):
        igorErrorCode=0
        errorMsg=''
        history=''
        results=''
        IgorApp.Execute2(0, 0, cmd, igorErrorCode, errorMsg, history, results)

        if PrintDiagnostics ==True :
            if results <> '':
                Console.WriteLine('Results: ' + results)
            if history <> '':
                Console.WriteLine('History: ' + history)
            if errorMsg <> '':
                Console.WriteLine('Error: ' + errorMsg)
        if errorMsg <> '':
            return 1
        else:
            return 0
 


Console.RegisterAsImageProcessor('IgorProcess')
def ImageCatch(CoreImages):
    Console.WriteLine(len(CoreImages))
    if datafolder.WaveExists('ImageMatrix')==False:
          cmd= 'Make/N=(' + CoreImages[0].Width.ToString() + ',' + CoreImages[0].Height.ToString() + ') ImageMatrix; Edit ImageMatrix'
          ExecuteIgorProCmd(IgorApp,cmd,True)
          cmd = 'Display;AppendImage ImageMatrix'
          ExecuteIgorProCmd(IgorApp,cmd,True)
    DataMatrix = datafolder.Wave('ImageMatrix')
    Data = CoreImages[0].GetArrayDouble()
    DataMatrix.SetNumericWaveDataAsDouble(Data)
    cmd= 'MatrixTranspose ImageMatrix'
    ExecuteIgorProCmd(IgorApp,cmd,True)
    cmd = 'WaveStats ImageMatrix'
    ExecuteIgorProCmd(IgorApp,cmd,True)
    vAvg=datafolder.Variables.Item('V_avg')
    realPart=clr.Reference[float](0)
    imagPart=clr.Reference[float](0)
    vAvg.GetNumericValue(realPart,imagPart)
    Console.WriteLine(realPart.ToString())
    Data= DataMatrix.GetNumericWaveDataAsDouble()
    CoreImages[0]= CoreImages[0].CreateImageFromArray(Data)
    return CoreImages

def EndImageCatch():
    Console.WriteLine('Stopping Eventhandlers')
    Console.KillRequested -= EndImageCatch;
    Console.RemoveImageProcessor('IgorProcess')
    Console.ImageProduced -= ImageCatch

Console.ImageProduced += ImageCatch
Console.KillRequested += EndImageCatch

camera=ECore.GetDevice('camera')
Console.WriteLine(camera.GetType().ToString())

";
            
        
        }

        private void sToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CodeText.Text =
@"import clr
clr.AddReference('System.Windows.Forms')
from System.Windows.Forms import *
clr.AddReferenceToFileAndPath('c:\\program files\\scilab-5.2.1\\bin\\dotnet-component-scilab.dll')
import DotNetScilab
from DotNetScilab import *

oScilab = Scilab(True)

Console.RegisterAsImageProcessor('IgorProcess')
def ImageCatch(CoreImages):
    Console.WriteLine('Number Of Channels:' + len(CoreImages).ToString())
    Data= CoreImages[0].GetLinearArrayDouble()
    mA = CoreImages[0].Width
    nA = CoreImages[0].Height
    oScilab.createNamedMatrixOfDouble('ImageMatrix', mA, nA, Data)
    ret =oScilab.SendScilabJob('d = 0:' + (mA-1).ToString() + ';')
    Console.WriteLine(ret)
    ret=oScilab.SendScilabJob('plot3d(d,d,ImageMatrix);')
    Console.WriteLine(ret)
    ret=oScilab.SendScilabJob('ImageMatrix = ImageMatrix .* ImageMatrix')
    Console.WriteLine(ret)
    ret = oScilab.SendScilabJob('m=min(ImageMatrix)')
    m=oScilab.readNamedMatrixOfDouble('m')
    Console.WriteLine('Min Value is :' + m[0].ToString() )
    DimB = oScilab.getNamedVarDimension('ImageMatrix')
    Data= oScilab.readNamedMatrixOfDouble('ImageMatrix')
    CoreImages[0]= CoreImages[0].CreateImageFromArray(Data,DimB[0],DimB[1])
    return CoreImages

def EndImageCatch():
    Console.WriteLine('Stopping Eventhandlers')
    Console.KillRequested -= EndImageCatch;
    Console.RemoveImageProcessor('IgorProcess')
    Console.ImageProduced -= ImageCatch

Console.ImageProduced += ImageCatch
Console.KillRequested += EndImageCatch

camera=ECore.GetDevice('camera')
Console.WriteLine(camera.GetType().ToString())

";
        }

    }
}
