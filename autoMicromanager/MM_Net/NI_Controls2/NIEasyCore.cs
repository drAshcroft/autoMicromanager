using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Runtime.InteropServices;
using Micromanager_net.Setup;
using System.IO;
using Microsoft.Win32;
namespace CoreDevices.NI_Controls
{
    [Serializable]
    /// <summary>
    /// A dummy control for placing a nice box on the front of a labview application
    /// </summary>
    public partial class NIEasyCore : UserControl
    {
        public EasyCore Ecore;
        private NiHelpers helper;
        
        public NIEasyCore()
        {
            InitializeComponent();
        }
        public EasyCore StartECoreScriptGui(string ConfigFile)
        {
            EasyCore ecore = StartEcore(ConfigFile);
            Micromanager_net.MMCOMViewer view = new Micromanager_net.MMCOMViewer();
            view.Show();
            ecore.PaintSurface( view.GetPictureBoard());
            Micromanager_net.MMDeviceHolder devholder = new Micromanager_net.MMDeviceHolder();
            devholder.GetDeviceHolder().DisplayGUIs(this);
            devholder.Show();
            return ecore;
        }
        public EasyCore StartSuperForm( string ConfigFile)
        {
            string CoreFilePath = "";
            //check if the installation worked correctly and specifying the important directory
            try
            {
                RegistryKey regKeyAppRoot = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\autoMicromanager");

                CoreFilePath = (string)regKeyAppRoot.GetValue("Path");
            }
            catch { }
            //if not then just search through all the most likely locations
            if (CoreFilePath == "")
            {
                CoreFilePath = @"C:\Program Files\autoMicromanager";

                System.Environment.SpecialFolder typeSpecialFolder;

                foreach (System.Environment.SpecialFolder atypeSpecialFolder in
                                  System.Enum.GetValues(typeof(Environment.SpecialFolder)))
                {
                    typeSpecialFolder = atypeSpecialFolder;
                    string Pathname = System.Environment.GetFolderPath(typeSpecialFolder) + "\\autoMicromanager";
                    if (Directory.Exists(Pathname))
                        CoreFilePath = Pathname;
                }

            }
            Micromanager_net.Form1 Form2 = new Micromanager_net.Form1();
            Form2.HardwareConfigFilename = ConfigFile;
            Form2.HardwareConfigPath = CoreFilePath;
            helper = new NiHelpers(Ecore);
            Form2.Show();
            
            return Form2.Easycore;
        }
        public EasyCore  StartEcore( string ConfigFile)
        {
            string CoreFilePath = "";
            //check if the installation worked correctly and specifying the important directory
            try
            {
                RegistryKey regKeyAppRoot = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\autoMicromanager");
               
                CoreFilePath = (string)regKeyAppRoot.GetValue("Path");
                if (CoreFilePath ==null)
                    CoreFilePath = (string)regKeyAppRoot.GetValue("(Default)");
            }
            catch { }
            //if not then just search through all the most likely locations
            if (CoreFilePath ==null ||  CoreFilePath == "" )
            {
                CoreFilePath = @"C:\Program Files\autoMicromanager";

                System.Environment.SpecialFolder typeSpecialFolder;

                foreach (System.Environment.SpecialFolder atypeSpecialFolder in
                                  System.Enum.GetValues(typeof(Environment.SpecialFolder)))
                {
                    typeSpecialFolder = atypeSpecialFolder;
                    string Pathname=   System.Environment.GetFolderPath(typeSpecialFolder) + "\\autoMicromanager";
                    if (Directory.Exists(Pathname )) 
                           CoreFilePath = Pathname;
                }

            }

            Ecore = new EasyCore();
            Ecore.StartCore(CoreFilePath , ConfigFile );

            if (ConfigFile != "")
            {
                try
                {
                    RequestedGUIs = Ecore.LoadGUIDescriptionFromConfigFile(ConfigFile);
                     
                }
                catch { }
                GUIControls = new List<UserControl>();
                foreach (string[] sss in RequestedGUIs)
                {
                    try
                    {
                       // MessageBox.Show (sss[0] + " " +  sss[1] + " " + sss[2]);
                        UserControl uc = ReloadContent(sss[0], sss[1] + "|" + sss[2]);
                        if (uc!=null)
                            GUIControls.Add( uc  );
                        //else 
                          //  MessageBox.Show ("Null");
                    }
                    catch { }
                }
            }

            helper = new NiHelpers(Ecore);
            return Ecore;
        }
        public NiHelpers GetHelpers()
        {
            return helper ;
        }

       
        private List< UserControl> GUIControls=null;//new List<UserControl>();
        public List<UserControl> Guis
        {
            get 
            { 
                return GUIControls; 
            }
            set 
            { 
                GUIControls = value; 
            }
        }
        List<string[]> RequestedGUIs;
        public void NewHardwareConfig(string ConfigFilePath,string ConfigFileName)
        {
            if (ConfigFileName == "")
                ConfigFileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\ConfigFiles";

            Ecore.ClearDevices();
           
            HardwareSetup2 hs = new HardwareSetup2(Ecore, null,false  );
            hs.ShowDialog();
            RequestedGUIs = hs.RequestedGUIs;
            GUIControls = new List<UserControl>();
            foreach (string[] s in RequestedGUIs )
            {
               GUIControls.Add(  ReloadContent(s[0],s[1] + "|" + s[2]) );
            }
            Ecore.SaveConfigFile(ConfigFilePath + "\\" + ConfigFileName);
            Ecore.SaveFakeGUIFile(ConfigFilePath + "\\" + ConfigFileName , RequestedGUIs);
        }
       
        /// <summary>
        /// Method for loading all the GUI controls.
        /// </summary>
        /// <param name="persistString"></param>
        /// <param name="extraInformation"></param>
        /// <returns></returns>
        public UserControl  ReloadContent(string persistString, string extraInformation)
        {
            //MessageBox.Show(persistString);
            if (persistString == "" || persistString == "Micromanager_net.UI.GUIDeviceForm")
            {
                try 
                {
                    string[] parts = extraInformation.Split(new char[] { '|' });
                    persistString = parts[0];
                    CoreDevices.UI.GUIDeviceControl gDC;
                    try
                    {
                        gDC  = Ecore.GetDeviceGUI(parts[0]);
                    }
                    catch
                    {
                        gDC = Ecore.GetDeviceGUI("Micromanager_net.UI." + parts[0]);
                    }
                    gDC.SetCore(Ecore, parts[1]);
                   // UI.GUIDeviceForm  fwf = new UI.GUIDeviceForm(gDC) ;
                   // fwf.SetCore(Ecore,parts[1]);
                   // fwf.ExtraInformation = extraInformation ;
                    return (UserControl)gDC ;
                }
                catch 
                {
                    return null;
                }
            }
            else 
              switch (persistString)
              {
                 
                  case "Micromanager_net.DockContentForm":
                 //     return ReloadContent(extraInformation, extraInformation);
                  case "Picture Board":
                  case "PictureBoard":
                
                      //IPictureView ipV= Ecore.GetViewerObject("CoreDevices.NI_Controls.PictureBoard");
                      // fView = new DockContentForm((UserControl)ipV );
                      //fView = new View();
                      //fView.ExtraInformation = persistString;
                      //ipV.SetCore(Ecore);
                      //if (pToolsForm!=null ) fView.SetToolsbars(pToolsForm);
                      //Ecore.PaintSurface( ipV );
                      //((ISubMDI)fView).OnNewDockableForm += new EventHandler<NewDockableFormEvents>(Form1_OnCreateForm);
                      //((ISubMDI)fView).OnNewForm += new EventHandler<NewFormEvents>(Form1_OnNewForm);

                      //return (UserControl)ipV ;

                  case "SciImage.View":
                  case "Science.Image":
                    
                        //IPictureView ipV2=  Ecore.GetViewerObject("SciImage.View");
                        //ipV2.SetCore(Ecore);
                        //fView = new View();
                        //fView.ExtraInformation = persistString;
                        //((IPictureView ) fView).SetCore(Ecore);
                        //if (pToolsForm!=null ) fView.SetToolsbars(pToolsForm);
                        //Ecore.PaintSurface ( ipV2 );
                        //((ISubMDI)fView).OnNewDockableForm += new EventHandler<NewDockableFormEvents>(Form1_OnCreateForm);
                        //((ISubMDI)fView).OnNewForm += new EventHandler<NewFormEvents>(Form1_OnNewForm);
                        // return (UserControl) ipV2 ;
                      return null;
                 
             }
            return null;
        }

        
            
        

    }
   
}
