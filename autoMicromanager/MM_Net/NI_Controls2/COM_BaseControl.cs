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

using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Runtime.InteropServices;
using CoreDevices;

namespace Micromanager_net
{
    /// <summary>
    /// this was intended to be a plugin for an activex control that could be used to
    /// start and setup a ui for micromanager, but .net and com just wouldnt play together
    /// well so it is just in storage for now
    /// </summary>
    [Guid("1514adf6-7cb1-4561-0005-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    private partial class COM_BaseControl : UserControl,INI_BaseControl
    {
        
        private EasyCore  Ecore;
        public UserControl fView;
        //public StageJoystick SJForm;
        private List<UserControl> DeviceControls = new List<UserControl>();

        public List<UserControl> GetAllLoadedDeviceGUIs()
        {
            return DeviceControls;

        }
        // public Thread PaintThread;
        private Random rnd = new Random();
        public COM_BaseControl()
        {
            InitializeComponent();
        }
        public string[] GetPossibleGUIs()
        {
            List<string> Guis = new List<string>();
            Guis.AddRange( Ecore.GetAllDeviceGUIs());
            Guis.Add("Micromanager_net.UI.RecordToolsForm");  
            Guis.Add("Micromanager_net.UI.CoreLog");
            Guis.Add("Micromanager_net.StageJoystick");
            Guis.Add("Picture Board");
            Guis.Add("SciImage.View");
            

            return Guis.ToArray();

        }

        public IPictureView ViewControl
        {
            get { return (IPictureView)fView; }
        }
        public EasyCore EasyCore
        {

            get {
                if (Ecore==null)
                    Ecore = new EasyCore();
                return Ecore; }
        }
            
        public UserControl ReloadContent(string GuiName, string DeviceTarget)
        {

            
                switch (GuiName )
                {
                    case "Micromanager_net.UI.RecordToolsForm":
                      /*  UI.MoreRecordTools  rtf = new UI.MoreRecordTools ();
                        rtf.SetCore(Ecore, "");
                        DeviceControls.Add ( rtf);
                        return (UserControl)rtf;*/
                       
                    case "Micromanager_net.UI.CoreLog":
                       /* UI.CoreLogControl cl = new UI.CoreLogControl();
                        DeviceControls.Add (cl);
                        return cl;*/
                    case "Micromanager_net.StageJoystick":
                        //StageJoystick sj = new StageJoystick();

                        try
                        {
                          //  sj.SetCore(Ecore);// ((CoreDevices.XYStage)Ecore.GetDevice("xystage"));
                        }
                        catch { }
                        //return (sj);
                        break;
                    case "Picture Board":
                        if (fView == null)
                        {
                            fView = new CoreDevices.NI_Controls.PictureBoard  ();
                            fView.Dock = DockStyle.Fill;
                            Controls.Add(fView);
                           

                            ((IPictureView)fView).SetCore(Ecore);
                            //if (pToolsForm!=null ) fView.SetToolsbars(pToolsForm);
                            Ecore.PaintSurface = (IPictureView)fView;
                           
                            return fView;
                        }
                        break;
                       
                    case "SciImage.View":
                    case "Science.Image":
                        if (fView == null)
                        {
                            fView = new SciImage.NiViewer ();
                            fView.Dock = DockStyle.Fill;
                            Controls.Add(fView);
                            ((SciImage.NiViewer)fView).CreateBasicSetup();
                            //fView = new View();

                            ((IPictureView)fView).SetCore(Ecore);
                            //if (pToolsForm!=null ) fView.SetToolsbars(pToolsForm);
                            Ecore.PaintSurface = (IPictureView)fView;
                           
                            return fView;
                        }
                        break;
                    default:
                        try
                        {

                            CoreDevices.UI.GUIDeviceControl gDC = Ecore.GetDeviceGUI(GuiName);

                            gDC.SetCore(Ecore, DeviceTarget );
                            gDC.ExtraInformation = "";// extraInformation;
                            UserControl uc = (UserControl)gDC.GetControl();
                            DeviceControls.Add(uc);
                            return uc;
                        }
                        catch
                        {

                            if (fView == null)
                            {
                                return ReloadContent("SciImage.View","");
                            }
                        }
                        break;
                }
            return null;

        }
        
        public void Ecore_RequestAllFormsClose(object sender)
        {
            foreach (UserControl uc in DeviceControls)
            {
                uc.Dispose();
            }
            DeviceControls.Clear();
            try { fView.Dispose(); }
            catch { }
           
            fView = null;
           
        }
        
        ~COM_BaseControl ()
        {

            try
            {
                Ecore.StopCore();
            }
            catch { }
            
        }

        public void StartFocusMovie()
        {
            Ecore.MMCamera.StartFocusMovie();
        }

               
        public void NewHardwareConfig()
        {
            Ecore.ClearDevices();
            foreach (UserControl uc in DeviceControls )
            {
                uc.Dispose();
            }
            //Setup.HardwareSetup hs = new Micromanager_net.Setup.HardwareSetup(Ecore, this);
            //hs.ShowDialog();
        }


        public EasyCore StartEcore(string CoreDirPath, string ConfigFilePath)
        {
           // MessageBox.Show(Application.ExecutablePath);
           // MessageBox.Show(Application.StartupPath);
           // MessageBox.Show(Environment.CurrentDirectory);
           // MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
           // MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
           // MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            if (CoreDirPath == "")
            {
                string aPath;
                string aName;

                aName = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;

                aPath = System.IO.Path.GetDirectoryName(aName);
                CoreDirPath = aPath;
                
            }
            //MessageBox.Show(CoreDirPath );

            // Create a document
            Ecore = new EasyCore();
            
            string Errorstring = Ecore.StartCore(CoreDirPath, ConfigFilePath );
            
            try
            {
                Ecore.RequestAllFormsClose += new ClearAllFormsEvent(Ecore_RequestAllFormsClose);
                //string GraphconfigFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            }
            catch { }
            return Ecore;

        }
        public void ShowImage(CoreImage Image)
        {
            ViewControl.SendImage(Image.ImageRGB);
            this.Refresh();
        }

        public SciImage.NiViewer ConverToNiViewer(IPictureView genericInterface)
        {
            return (SciImage.NiViewer)genericInterface;
        }
        public CoreDevices.NI_Controls.PictureBoard ConvertToPictureBoard(IPictureView genericInterface)
        {
            return (CoreDevices.NI_Controls.PictureBoard)genericInterface;
        }

    }

    [Guid("5A88092E-69DF-4bb8-0005-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface  INI_BaseControl
    {
        SciImage.NiViewer ConverToNiViewer(IPictureView genericInterface);
        CoreDevices.NI_Controls.PictureBoard ConvertToPictureBoard(IPictureView genericInterface);
        void ShowImage(CoreImage Image);
        string[] GetPossibleGUIs();
        IPictureView ViewControl
        {
            get;
        }
        EasyCore EasyCore
        {

            get;
        }
            
        UserControl ReloadContent(string GuiName, string DeviceTarget);
        void Ecore_RequestAllFormsClose(object sender);
        void StartFocusMovie();
        
        void NewHardwareConfig();
        EasyCore StartEcore(string CoreDirPath, string ConfigFilePath);
    }

}
