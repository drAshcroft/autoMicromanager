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

using System.Threading;
using System.Runtime.InteropServices;

namespace Micromanager_net.NI_Controls
{

    [Guid("1514adf6-7cb1-4561-0005-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class NI_BaseControl : UserControl,INI_BaseControl
    {
        private CoreDevices.EasyCore Ecore;
        public UserControl fView;
        public StageJoystick SJForm;
        private List<UserControl> DeviceControls = new List<UserControl>();

        public List<UserControl> GetAllStartedDeviceGUIs()
        {
            return DeviceControls;
        }
        // public Thread PaintThread;
        private Random rnd = new Random();
        public NI_BaseControl()
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
            Guis.Add("PaintDotNet.View");
            

            return Guis.ToArray();

        }

        public IPictureView ViewControl
        {
            get { return (IPictureView) fView; }
        }
        public CoreDevices.EasyCore EasyCore
        {

            get {
                if (Ecore==null)
                    Ecore = new Micromanager_net.CoreDevices.EasyCore();
                return Ecore; }
        }
            
        public UserControl ReloadContent(string GuiName, string DeviceTarget)
        {

            
                switch (GuiName )
                {
                    case "Micromanager_net.UI.RecordToolsForm":
                        UI.MoreRecordTools  rtf = new UI.MoreRecordTools ();
                        rtf.SetCore(Ecore, "");
                        DeviceControls.Add ( rtf);
                        return (UserControl)rtf;
                       
                    case "Micromanager_net.UI.CoreLog":
                        UI.CoreLogControl cl = new UI.CoreLogControl();
                        DeviceControls.Add (cl);
                        return cl;
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

                    case "PaintDotNet.View":
                    case "Science.Image":
                        if (fView == null)
                        {
                            fView = new PaintDotNet.NiViewer ();
                            fView.Dock = DockStyle.Fill;
                            Controls.Add(fView);
                            ((PaintDotNet.NiViewer)fView).CreateBasicSetup();
                            //fView = new View();

                            ((IPictureView)fView).SetCore(Ecore);
                            //if (pToolsForm!=null ) fView.SetToolsbars(pToolsForm);
                            Ecore.PaintSurface = (IPictureView)fView;
                           
                        }
                        break;
                    default:
                        try
                        {

                            Micromanager_net.UI.GUIDeviceControl gDC = Ecore.GetDeviceGUI(GuiName);

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
                                return ReloadContent("PaintDotNet.View","");
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
            try { SJForm.Close(); }
            catch { }
            fView = null;
            SJForm = null;
        }
        
        ~NI_BaseControl ()
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
       
        
        public CoreDevices.EasyCore  StartEcore(string CoreDirPath, string ConfigFilePath)
        {
            // Create a document
            Ecore = new Micromanager_net.CoreDevices.EasyCore();
           
            string Errorstring = Ecore.StartCore(CoreDirPath, ConfigFilePath );

            Ecore.RequestAllFormsClose += new Micromanager_net.CoreDevices.ClearAllFormsEvent(Ecore_RequestAllFormsClose);
            string GraphconfigFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            return Ecore;

        }
        public void ShowImage(CoreDevices.CoreImage Image)
        {
            ViewControl.SendImage(Image.ImageRGB);

        }

        public PaintDotNet.NiViewer ConverToNiViewer(IPictureView genericInterface)
        {
            return (PaintDotNet.NiViewer)genericInterface;
        }
        public Micromanager_net.NI_Controls.PictureBoard ConvertToPictureBoard(IPictureView genericInterface)
        {
            return (Micromanager_net.NI_Controls.PictureBoard)genericInterface;
        }

    }

    [Guid("5A88092E-69DF-4bb8-0005-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface  INI_BaseControl
    {
        PaintDotNet.NiViewer ConverToNiViewer(IPictureView genericInterface);
        Micromanager_net.NI_Controls.PictureBoard ConvertToPictureBoard(IPictureView genericInterface);
        void ShowImage(CoreDevices.CoreImage Image);
        string[] GetPossibleGUIs();
        IPictureView ViewControl
        {
            get;
        }
        CoreDevices.EasyCore EasyCore
        {

            get;
        }
            
        UserControl ReloadContent(string GuiName, string DeviceTarget);
        void Ecore_RequestAllFormsClose(object sender);
        void StartFocusMovie();
        
        void NewHardwareConfig();
        CoreDevices.EasyCore  StartEcore(string CoreDirPath, string ConfigFilePath);
    }

}
