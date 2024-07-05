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
using ScriptNET;
using CoreDevices;

namespace ScriptModules
{
    public partial class SSharpScriptModule : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private EasyCore ECore = null;
        
        public string DeviceType() { return "GUIControl"; }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("sSharp ScriptControl");
        }
        private CoreDevices.Devices.MMEmptyGuiDevice MyGuiDev;
        
        public void SetCore(EasyCore Ecore, string DeviceName)
        {
            if (DeviceName == "")
                DeviceName = "sSharp";
            try
            {
                MyGuiDev = (CoreDevices.Devices.MMEmptyGuiDevice)Ecore.GetDevice(DeviceName);
            }
            catch
            {
                MyGuiDev = new CoreDevices.Devices.MMEmptyGuiDevice(Ecore, DeviceName);
                
            }
            ECore = Ecore;
            ECore.AddImageProcessor("sSharp", new ImageProcessorStep(getImage));
           
        }
        string ScriptCommand = "Init";
        private delegate CoreDevices.CoreImage[] getImageDelg(CoreImage[] cImage);
        private CoreDevices.CoreImage[] getImage(CoreImage[] cImage)
        {
            
            if (InvokeRequired)
            {
                return (CoreImage[]) this.Invoke(new getImageDelg(getImage), cImage);
            }
            else
            {
                ScriptCommand = "ProcessImage";
                ic.InImage = cImage;
                ExecuteCommand(this, EventArgs.Empty);
                return ic.OutImage;
            }
            
        }
        public SSharpScriptModule()
        {
            InitializeComponent();
            syntaxDocument1.SetSyntaxFromEmbeddedResource(Assembly.GetExecutingAssembly(), "ScriptIde.Resources.ScriptNET.syn");
        
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

        #region Commands
        private void OpenCommand(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "All|*.*;";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                syntaxDocument1.Lines = File.ReadAllLines(openDialog.FileName);
            }
        }

        private void ExitCommand(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ParseCommand(object sender, EventArgs e)
        {
            lbStatus.ToolTipText = "";
            lbStatus.Text = "Start parsing...";
            try
            {
                DateTime time = BeginAction();
                Script.Compile(syntaxDocument1.Text);
                lbStatus.Text = "Parsing successful, " + EndAction(time).ToString();
                lbStatus.ToolTipText = "";
                richTextBox1.Text = "";
            }
            catch (ScriptSyntaxErrorException s)
            {
                lbStatus.Text = "Syntax error";
                lbStatus.ToolTipText = s.Message;
                richTextBox1.Text = s.Message;
            }
            catch (ScriptException)
            {
                lbStatus.Text = "Script exception";
            }
            catch
            {
                lbStatus.Text = "Application exception";
            }
        }
        ImageContainer ic = new ImageContainer();
        private void ExecuteCommand(object sender, EventArgs e)
        {
            lbStatus.ToolTipText = "";
            lbStatus.Text = "Parsing...";
            try
            {
                Script s = Script.Compile(syntaxDocument1.Text);
                s.Context.SetItem("Console", ContextItem.Variable, this);
                if (ic.InImage == null)
                {
                    long[,] a = new long[10, 10];
                    ic.InImage = new CoreImage[] { CoreImage.CreateImageFromArray(a)};
                }
                ic.OutImage = null;

                s.Context.SetItem("cImage", ContextItem.Variable,ic );
                s.Context.SetItem("ECore", ContextItem.Variable, ECore);
                //s.Context.SetItem("EasyCore", ContextItem.Type, typeof(EasyCore));
                s.Context.SetItem("ScriptCommand", ContextItem.Variable, ScriptCommand);
                lbStatus.Text = "Parsing successful";
                lbStatus.Text = "Executing...";
                Update();
                DateTime time = BeginAction();
                
                object rez = s.Execute();
                if (ic.OutImage == null)
                    System.Diagnostics.Debug.Print("Darn2");
                lbStatus.Text = "Execution succeded, " + EndAction(time).ToString();
                if (rez != null)
                    lbStatus.ToolTipText = "Result:" + rez.ToString();
                else
                    lbStatus.ToolTipText = "";
            }
            catch (ScriptSyntaxErrorException s)
            {
                lbStatus.Text = "Syntax error";
                lbStatus.ToolTipText = s.Message;
            }
            catch (ScriptException)
            {
                lbStatus.Text = "Script exception";
            }
            catch (Exception ex)
            {
                lbStatus.Text = "Application exception";
            }
        }

        private void PasteCommand(object sender, EventArgs e)
        {
            syntaxBoxControl1.Paste();
        }

        private void CopyCommand(object sender, EventArgs e)
        {
            syntaxBoxControl1.Copy();
        }

        private void CutCommand(object sender, EventArgs e)
        {
            syntaxBoxControl1.Cut();
        }

        private void RedoCommand(object sender, EventArgs e)
        {
            syntaxBoxControl1.Redo();
        }

        private void UndoCommand(object sender, EventArgs e)
        {
            syntaxBoxControl1.Undo();
        }

        private void SelectAllCommand(object sender, EventArgs e)
        {
            syntaxBoxControl1.SelectAll();
        }

        private void NewCommand(object sender, EventArgs e)
        {
            syntaxDocument1.Clear();
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
        #endregion

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
