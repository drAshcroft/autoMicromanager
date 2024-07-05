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
using WeifenLuo.WinFormsUI.Docking;
//using VS2005Extender;
using System.Threading;
using System.Runtime.InteropServices;
using CoreDevices;
using System.IO;
using System.Xml;

namespace Micromanager_net
{
    [Guid("5A88092E-69DF-4bb8-0015-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMMMainForm
    {
       
        void Show();
        EasyCore Easycore { get; }
        void SetupBasic(string DisplayString, string extraInfo, string DeviceTarget);
        IDockContent ReloadContent(string persistString, string extraInformation);
        void Ecore_RequestAllFormsClose(object sender);
        void SaveDesktop(string configFile);

    }
    
    [ProgId("Micromanager_net.Form1")]
    [Guid("1514adf6-7cb1-4561-0015-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class Form1 : Form,IMMMainForm 
    {
        private EasyCore Ecore;
        public DockContent  fView;
        public DockContent  SJForm;
        
        public EasyCore Easycore
        {
            get {return Ecore;}

        }

       // public Thread PaintThread;
        private Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            Application.DoEvents();
        }

        /// <summary>
        /// Method for docking forms
        /// </summary>
        private void CreateBasicLayout()
        {
            newHardwareConfigurationToolStripMenuItem_Click(this, new EventArgs());
        }

        void Form1_OnCreateForm(object sender, NewDockableFormEvents e)
        {
            e.NewForm.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
        }

        /// <summary>
        /// Method for docking forms
        /// </summary>
        /// <param name="DisplayString"></param>
        /// <param name="extraInfo"></param>
        /// <param name="DeviceTarget"></param>
        public void SetupBasic(string DisplayString,string extraInfo, string DeviceTarget)
        {
            switch (DisplayString )
            {
                  case "CoreDevices.NI_Controls.PictureBoard":
                  case "PictureBoard":
                  case "Picture Board":
                  case "SciImage.View":
                  case "Science.Image":
                    ((DockContent)ReloadContent(DisplayString, extraInfo )).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document  );
                    break;
                  default :
                    if (extraInfo.Contains("Joystick") == true)
                        ((DockContent)ReloadContent(DisplayString, extraInfo + "|" + DeviceTarget)).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Float    );
                    else if (extraInfo.Contains("CoreLog") == true)
                        ((DockContent)ReloadContent(DisplayString, extraInfo + "|" + DeviceTarget)).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document     );
                    else if (extraInfo.ToLower().Contains("python")==true )
                        ((DockContent)ReloadContent(DisplayString, extraInfo + "|" + DeviceTarget)).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                    else 
                        ((DockContent)ReloadContent(DisplayString, extraInfo + "|" + DeviceTarget)).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
                    break;
            }

        }

        /// <summary>
        /// The scripts setup a fake windowing system, this reads that fake system into the basic setup.
        /// </summary>
        /// <param name="Filename"></param>
        public bool  LoadFakeFile(string Filename)
        {
            XmlDocument reader = new XmlDocument();
            try
            {
                reader.Load(Filename);
            }
            catch 
            {
                return false ;
            }
            XmlNode oNode = reader.DocumentElement;
            XmlNodeList oNodeList = oNode.SelectNodes("/DockPanel/Contents")[0].ChildNodes;

           
            for (int x = 0; x < oNodeList.Count; x++)
            {
                XmlNode device = oNodeList.Item(x);
                Dictionary<string, string> Proplist = new Dictionary<string, string>();
                XmlAttributeCollection attributes = device.Attributes;
                for (int i = 0; i < attributes.Count; i++)
                {
                    Proplist.Add(attributes[i].Name.Replace("_", " "), attributes[i].Value);
                }
                string[] ExtraInfo = Proplist["ExtraInformation"].Split(new char[] { '|' });

                SetupBasic(Proplist["PersistString"], ExtraInfo[0], ExtraInfo[1]);
               // AllProps.Add(oNodeList.Item(x).Name.Replace("___", " "), Proplist);
            }
            return true;
        }
           
        /// <summary>
        /// Method for docking forms
        /// </summary>
        /// <param name="persistString"></param>
        /// <param name="extraInformation"></param>
        /// <returns></returns>
        public IDockContent ReloadContent(string persistString, string extraInformation)
        {

            if (persistString == "" || persistString == "Micromanager_net.UI.GUIDeviceForm")
            {
               // try 
                {
                    string[] parts = extraInformation.Split(new char[] { '|' });
                    persistString = parts[0];
                    CoreDevices.UI.GUIDeviceControl gDC;
                    try
                    {
                        gDC  = Ecore.GetDeviceGUI(parts[0]);
                    }
                    catch (Exception ex)
                    {
                        Ecore.LogMessage("Trying to load gui" + parts[0] + "\t" +  ex.Message );
                        gDC = Ecore.GetDeviceGUI("Micromanager_net.UI." + parts[0]);
                    }
                    UI.GUIDeviceForm  fwf = new UI.GUIDeviceForm(gDC) ;
                    fwf.SetCore(Ecore,parts[1]);
                    fwf.ExtraInformation = extraInformation ;
                    return fwf;
                }
              //  catch 
                {
                    return null;
                }
            }
            else 
              switch (persistString)
              {
                 /* case "Micromanager_net.UI.RecordToolsForm":
                      UI.RecordToolsForm rtf = new Micromanager_net.UI.RecordToolsForm();
                      rtf.SetCore(Ecore, "");
                      return rtf;
                  case "Micromanager_net.UI.CoreLog":
                      UI.CoreLog cl = new UI.CoreLog();
                      return cl;
                  case "Micromanager_net.StageJoystick":
                      StageJoystick sj=new StageJoystick();
                      
                      try 
                      {
                          sj.SetCore(Ecore);// ((CoreDevices.XYStage)Ecore.GetDevice("xystage"));
                      }
                      catch {}
                      return (sj);*/
                  case "Micromanager_net.DockContentForm":
                      return ReloadContent(extraInformation, extraInformation);
                  case "Picture Board":
                  case "PictureBoard":
                  case "CoreDevices.NI_Controls.PictureBoard":
                
                      IPictureView ipV= Ecore.GetViewerObject("CoreDevices.NI_Controls.PictureBoard");
                      fView = new DockContentForm((UserControl)ipV );
                      //fView = new View();
                      fView.ExtraInformation = persistString;
                      ipV.SetCore(Ecore);
                      //if (pToolsForm!=null ) fView.SetToolsbars(pToolsForm);
                      Ecore.PaintSurface( ipV );
                      //((ISubMDI)fView).OnNewDockableForm += new EventHandler<NewDockableFormEvents>(Form1_OnCreateForm);
                      //((ISubMDI)fView).OnNewForm += new EventHandler<NewFormEvents>(Form1_OnNewForm);
                      fView.Text = "Image Viewer";
                      fView.TabText = "Image Viewer";
                      return fView;

                  case "SciImage.View":
                  case "Science.Image":
                    if (fView == null)
                    {
                        try
                        {
                            fView = (DockContent)Ecore.GetViewerObject("SciImage.View");

                            //fView = new View();
                            fView.ExtraInformation = persistString;
                            ((IPictureView)fView).SetCore(Ecore);
                            //if (pToolsForm!=null ) fView.SetToolsbars(pToolsForm);
                            Ecore.PaintSurface((IPictureView)fView);
                            ((ISubMDI)fView).OnNewDockableForm += new EventHandler<NewDockableFormEvents>(Form1_OnCreateForm);
                            ((ISubMDI)fView).OnNewForm += new EventHandler<NewFormEvents>(Form1_OnNewForm);
                        }
                        catch (InvalidCastException ex)
                            {
                                return ReloadContent("PictureBoard", extraInformation);

                            }
                    }
                    return fView;

               
                default:
                    if (fView == null)
                    {
                        ReloadContent("SciImage.View", extraInformation);
                    }
                    return ((ISubMDI)fView).GetForm(persistString, extraInformation);
                    //return (null);
             }

        }
        private delegate void  OnNewFormEvent(object sender, NewFormEvents e);
        private string _HardwareConfigFilename = "";
        private string _HardwareConfigPath = "";

        public string HardwareConfigFilename
        {
            get { return _HardwareConfigFilename; }
            set { _HardwareConfigFilename = value; }
        }
        public string HardwareConfigPath
        {
            get { return _HardwareConfigPath; }
            set { _HardwareConfigPath = value; }
        }
        void Form1_OnNewForm(object sender, NewFormEvents e)
        {
            if (this.InvokeRequired)
            {
                object[] pars = { sender, e };
                this.Invoke(new OnNewFormEvent(Form1_OnNewForm), pars);
            }
            else
            {
                try
                {
                    DockContent dc = ((DockContent)e.NewForm);
                    //DockPanel.FloatWindowFactory.CreateFloatWindow(DockPanel,DockPanel.Panes[0],
                    //fw = dockPanel.FloatWindowFactory.CreateFloatWindow(dockPanel, pane, floatWindows[i].Bounds);
                    // dc.DesktopBounds = new Rectangle(10, 100, e.NewForm.DesktopBounds._Width, e.NewForm.DesktopBounds._Height);

                    Size s = SizeFromClientSize(dc.ClientSize );
                    DockPanel.DefaultFloatWindowSize = s;
                    int SizeX = s.Width ;
                    int SizeY = s.Height ;
                    dc.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Float);
                    dc.Pane.FloatWindow.SetDesktopBounds(dc.Pane.FloatWindow.DesktopLocation.X, dc.Pane.FloatWindow.DesktopLocation.Y,
                                                          SizeX, SizeY);
                    //System.Diagnostics.Debug.Print( DockPanel.FloatWindows[0].Text );
                }
                catch
                {
                    if (e.NewForm.Visible ==false  )
                        e.NewForm.Show(this);
                }
            }
            //.Show(this);
        }
        private void Form1_Load(object sender, EventArgs e)
        {


            // Start the easycore
            Ecore = new EasyCore();
            if (_HardwareConfigPath == "")
                Ecore.StartCoreCore();
            else
                Ecore.StartCoreCore(_HardwareConfigPath);

            Ecore.RequestAllFormsClose += new ClearAllFormsEvent(Ecore_RequestAllFormsClose);
            string Errorstring;
            string GraphconfigFile;
            if (_HardwareConfigFilename == "")
            {
              
                // Do login for each hardware setup
                SignIn signin = new SignIn(Ecore, this);
                signin.ShowDialog(this);
                if (signin.StartUpHandled == true)
                    return;
                if (signin.HardwareConfigFile == "")
                {
                    this.Close();
                    return;
                }
                //If hardwareconfiguration file is available, then loadup configuration
                Ecore.ExperimentFolder = signin.experimentFolder;
                Errorstring = Ecore.StartCore(signin.HardwareConfigFile);
                _HardwareConfigFilename = signin.HardwareConfigFile;
                GraphconfigFile = signin.GraphicConfigFile  ;//System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            }
            else
            {
                //Ecore.ExperimentFolder = _HardwareConfigFilename ;
                Errorstring = Ecore.StartCore(_HardwareConfigPath , _HardwareConfigFilename);
               // MessageBox.Show(Errorstring);
                GraphconfigFile = System.IO.Path.Combine(_HardwareConfigPath , Path.GetFileNameWithoutExtension(_HardwareConfigFilename) + "_Desktop.config");
            }
            // Apply a gray professional renderer as a default renderer
            //ToolStripManager.Renderer = oDefaultRenderer;
            //oDefaultRenderer.RoundedEdges = true   ;
            
            // Set DockPanel properties
            DockPanel.ActiveAutoHideContent = null;
            DockPanel.Parent = this;

            //VS2005Extender.VS2005Style.Extender.SetSchema(DockPanel, VS2005Extender.VS2005Style.Extender.Schema.FromBase);

            DockPanel.SuspendLayout(true);

            //todo: load the configfile dependant on the user so each person gets their own look and feel
            bool SkipLoading = false;
            if (Errorstring != "")
            {

                DialogResult ret= MessageBox.Show(this,"The program was not able to initialize correctly.  Do you wish to edit the configuration file?\n" + Errorstring,"Error Loading Config File",MessageBoxButtons.YesNoCancel );
                if (ret == DialogResult.Cancel)
                {
                    Application.Exit();
                }
                if (ret == DialogResult.Yes)
                {
                    SkipLoading = true;
                    NewHardwareConfig();
                }
            }
            if (!SkipLoading)
            {
                //MessageBox.Show(GraphconfigFile);
                if (System.IO.File.Exists(GraphconfigFile))
                {
                   // try
                    {
                        //Load last configurations
                        try
                        {
                            DockPanel.LoadFromXml(GraphconfigFile, ReloadContent);
                        }
                        catch (ArgumentException ex)
                        {
                            if (!LoadFakeFile(GraphconfigFile))
                               CreateBasicLayout();
                        }
                    }
                   // catch
                    {
                     //   CreateBasicLayout();
                    }
                }
                else
                {
                    // Load a basic layout
                    CreateBasicLayout();
                }
            }
           
            DockPanel.ResumeLayout(true, true);

            // This is for the python adapters.  It allows images to be passed to the python viewer
            if (_InterceptImages == true)
                Ecore.AddImageProcessor("Form2", new ImageProcessorStep(GetImage));
            Application.DoEvents();
        }
        /// <summary>
        /// Used to force a shutdown of the program
        /// </summary>
        /// <param name="sender"></param>
        public void Ecore_RequestAllFormsClose(object sender)
        {
            while (DockPanel.Contents.Count > 0)
            {
                DockContent dc = (DockContent)DockPanel.Contents[0];
                dc.Close();
            }
            try { fView.Close(); }
            catch { }
            try { SJForm.Close(); }
            catch { }
            fView = null;
            SJForm = null;
        }
        
        /// <summary>
        /// Saves the layout of the forms in program
        /// </summary>
        /// <param name="configFile"></param>
        public void SaveDesktop(string configFile)
        {
            //string configFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            DockPanel.SaveAsXml(configFile);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           /* string MMconfigFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "MMDefaultConfig.xml");
            Ecore.SaveConfigFile(MMconfigFile);

            string configFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            
            DockPanel.SaveAsXml(configFile);
            */
            try
            {
                Ecore.StopCore();
            }
            catch { }
            while (DockPanel.Contents.Count > 0)
            {
                DockContent dc = (DockContent)DockPanel.Contents[0];
                dc.Close();
            }
            try
            {
                File.Copy(Path.GetDirectoryName(Application.ExecutablePath) + "\\corelog.txt", Ecore.ExperimentFolder + "\\CoreLog.txt");
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ImageProcessButton_Click(object sender, EventArgs e)
        {
            //            PaintForm pf = new PaintForm();
            // pf.StartPaint(Ecore.MMCamera.Canvas.Image);
            // pf.Show(DockPanel, WeifenLuo.WinFormsUI.DockState.Document );
        }

      
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Ecore.MMCamera.StartFocusMovie();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
        }
        #region ViewForms
        //These are for the buttons for a easy menu
        private void cameraPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.CameraPropertiesForm")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch { }
        }

        private void stageControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.StagePropertiesForm")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch { }
        }

        private void focusControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.ZStagePropertiesForm")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch { }
        }

        private void joystickControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.StageJoystick")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch { }
            
        }

        private void filterWheelControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.FilterWheelForm")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch { }
        }

        private void frequencyGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.FunctionGeneratorForm")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch { }
        }

        private void paintToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.PaintToolBars")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch { }
        }

        private void laserControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("", "Micromanager_net.UI.LaserControl|LaserControl")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ((DockContent)ReloadContent("","Micromanager_net.View")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document );
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// Brings up the setup screen
        /// </summary>
        private void NewHardwareConfig()
        {
            Ecore.ClearDevices();
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }
            Setup.HardwareSetup hs = new Micromanager_net.Setup.HardwareSetup(Ecore, this);
            hs.ShowDialog();
        }

        private void newHardwareConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewHardwareConfig();
           
        }

        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);

        private void saveCurrentConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setup.SaveProfiles sp = new Setup.SaveProfiles(Ecore, this);
            sp.Show();
            SetParent((int)sp.Handle, (int)this.Handle); 
        }
        




        
        private bool _InterceptImages=false ;
        /// <summary>
        /// Convience method for python to help images pass over to python
        /// </summary>
        public bool InterceptImages
        {
            set
            {
                if (Ecore != null && value == true)
                    Ecore.AddImageProcessor("Form2_Py", new ImageProcessorStep(GetImage));
                _InterceptImages = value;
            }
            get { return _InterceptImages; }
        }
        
        public event CoreDevices.ImageProcessorStep DoImageProcessing;

        /// <summary>
        /// Convience method for showing .net messages from python
        /// </summary>
        /// <param name="Message"></param>
        public void ShowMessage(string Message)
        {
            MessageBox.Show(Message);
        }

        /// <summary>
        /// This is used because the normal property accessor does not seem to work with python
        /// </summary>
        /// <returns></returns>
        public EasyCore GetEasyCore()
        {
            return Ecore;
        }
        /// <summary>
        /// Calls python event to handle image and then waits for image return
        /// </summary>
        /// <param name="coreImage"></param>
        /// <returns></returns>
        private CoreDevices.CoreImage[] GetImage(CoreDevices.CoreImage[] coreImage)
        {
             return DoImageProcessing(coreImage);
        }

        private void scriptControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //((DockContent)ReloadContent("", "Micromanager_net.FunctionGeneratorForm")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
                ((DockContent)ReloadContent("MMUI_ScriptModules.IronPythonScriptModule", "Scripting Tools" + "|" + "Scripting")).Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            catch { }
        }
       
    }
}
