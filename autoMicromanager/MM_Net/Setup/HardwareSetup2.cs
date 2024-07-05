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
using System.Runtime.InteropServices;
using CoreDevices;
using CWrapper;
using System.IO;

namespace Micromanager_net.Setup
{
    [Serializable]
    public partial class HardwareSetup2 : Form
    {
        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);

        EasyCore ECore;
        
        Micromanager_net.Form1 MainForm;
        List<DeviceDesciption> ExtraDevices = new List<DeviceDesciption>();

        TreeNode mainNode = new TreeNode();
        TreeNode mainNode2 = new TreeNode();

        bool SaveConfigFile = false; 

        /// <summary>
        /// Mainform must point at the MDI host and is used to control all the other windows.
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="MainForm"></param>
        public HardwareSetup2(EasyCore eCore, Form1 MainForm, bool SaveConfigFile )
        {
            this.MainForm = MainForm;
            this.SaveConfigFile = SaveConfigFile;
            ECore = eCore;
            InitializeComponent();

          

            string[] adapters=ECore.GetDeviceLibraries ;

            mainNode.Name = "mainNode";
            mainNode.Text = "Libraries";
            this.treeView1.Nodes.Add(mainNode);

            mainNode2.Name = "mainNode";
            mainNode2.Text = "My Microscope";
            this.treeView2.Nodes.Add(mainNode2);

            TreeNode nod1 = new TreeNode();
            nod1.Name = "Graphical Interfaces";
            nod1.Text = "Graphical Interfaces : Image Viewer";
            nod1.Tag = "Graphical Interfaces : Image Viewer";
            mainNode2.Nodes.Add(nod1);

            TreeNode nod3 = new TreeNode();
            nod3.Name = "Graphical Interfaces";
            nod3.Text = "Graphical Interfaces : Image Capture and Record Tools";
            nod3.Tag = "Graphical Interfaces : Image Capture and Record Tools";
            mainNode2.Nodes.Add(nod3);

            TreeNode nod2 = new TreeNode();
            nod2.Name = "Graphical Interfaces";
            nod2.Text = "Graphical Interfaces : Joystick Control";
            nod2.Tag = "Graphical Interfaces : Joystick Control";
            mainNode2.Nodes.Add(nod2);

          

            TreeNode nod4 = new TreeNode();
            nod4.Name = "Graphical Interfaces";
            nod4.Text = "Graphical Interfaces : Corelog Viewer";
            nod4.Tag = "Graphical Interfaces : Corelog Viewer";
            mainNode2.Nodes.Add(nod4);

            TreeNode nod5 = new TreeNode();
            nod5.Name = "Graphical Interfaces";
            nod5.Text = "Graphical Interfaces : Scripting Tools";
            nod5.Tag = "Graphical Interfaces : Scripting Tools";
            mainNode2.Nodes.Add(nod5);

            foreach (string s in adapters)
            {
                TreeNode nod = new TreeNode();
                nod.Name = s;
                nod.Text = s;
                nod.Tag = s;
               
                mainNode.Nodes.Add(nod);
                string[] Devices =  LoadDevices(s);
                foreach (string s2 in Devices)
                {
                    TreeNode nodS = new TreeNode();
                    nodS.Name = s2;
                    nodS.Text = s2;
                    nodS.Tag = s2;
                    nod.Nodes.Add(nodS);
                }
            }
            mainNode.ExpandAll();
            mainNode2.ExpandAll();
        }

        /// <summary>
        /// Shows all the devices in a library
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="LibraryName"></param>
        private string[] LoadDevices( string LibraryName)
        {
            try
            {
                if (LibraryName != "None")
                {
                    string[] Devices = ECore.GetDevicesFromLibrary(LibraryName);
                    return Devices;
                }
                else
                {
                    return new string[]{ "Cannot Open Library"};
                }
            }
            catch 
            {
                return new string[] { "Cannot Open Library" };
            }
        }
       
       
        /// <summary>
        /// Loads all the GUIs into the combo box.
        /// </summary>
        /// <param name="cb"></param>
        private void LoadGUIs(ComboBox cb)
        {
            cb.Items.Clear();
            cb.Items.Add ("None");
            foreach (string s in ECore.GetAllDeviceGUIs())
            {
                string t = s.Replace("Micromanager_net.UI.", "");

                cb.Items.Add(t);
            }
            cb.Text = "None";
        }

        private List<DeviceDesc> Devs = new List<DeviceDesc>();
       
        private void button1_Click(object sender, EventArgs e)
        {
            List<DeviceDesc> tDevs = new List<DeviceDesc>();
            foreach (TreeNode tn in mainNode2.Nodes  )
            {
                string lab=tn.Text  ;
                 string s = lab.Substring(0, lab.IndexOf(':')-1).Trim();
                 string s2 = lab.Substring(lab.IndexOf(':')+1 ).Trim();
                 DeviceDesc dd = new DeviceDesc(true, "", s, s2, "");
                tDevs.Add(dd);
            }

            for (int i = 0; i < tDevs.Count; i++)
                Devs.Add(tDevs[tDevs.Count -1 - i]);
         
            this.Hide();
            HardwareSetup3 hs3 = new HardwareSetup3(ECore,ref  Devs);
            hs3.ShowDialog  ();


            //now load each device into the easycore
           // List<CoreDevices.Devices.MMDeviceBase> LoadedDevices = new List<CoreDevices.Devices.MMDeviceBase>();
            foreach (DeviceDesc  dh in Devs )
            {
                if (dh.DeviceLib == "Graphical Interfaces")
                {
                    CoreDevices.Devices.MMEmptyGuiDevice c = new CoreDevices.Devices.MMEmptyGuiDevice(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                    dh.PhysicalDevice =c;
                }
                else
                {
                    CWrapper.DeviceType dt = ECore.Core.getDeviceType(dh.DeviceName);
                    switch (dt)
                    {
                        case DeviceType.CameraDevice:
                            CoreDevices.Devices.Camera c = new CoreDevices.Devices.NormalCamera(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                            if (dh.DeviceName == "camera") c.MakeOffical();
                            dh.PhysicalDevice =(c);
                            break;
                        case DeviceType.StateDevice:
                            CoreDevices.Devices.MMDeviceBase f;
                            if (dh.DeviceGUI == "Micromanager_net.UI.FilterWheelControl" || dh.DeviceGUI.Contains("Filter") == true)
                                f = new CoreDevices.Devices.FilterWheel(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                            else
                                f = new CoreDevices.Devices.StateDevice(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                            if (dh.DeviceName == "filter") f.MakeOffical();
                            dh.PhysicalDevice =(f);
                            break;
                        case DeviceType.SignalIODevice:
                            CoreDevices.Devices.FunctionGenerator si = new CoreDevices.Devices.FunctionGenerator(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                            dh.PhysicalDevice =(si);
                            break;
                        case DeviceType.XYStageDevice:
                            CoreDevices.Devices.XYStage x = new CoreDevices.Devices.XYStage(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                            if (dh.DeviceName == "xyStage") x.MakeOffical();
                            dh.PhysicalDevice =(x);
                            break;
                        case DeviceType.StageDevice:
                            CoreDevices.Devices.ZStage z = new CoreDevices.Devices.ZStage(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                            if (dh.DeviceName == "zStage") z.MakeOffical();
                            dh.PhysicalDevice =(z);
                            break;
                        default:
                            CoreDevices.Devices.GenericDevice g = new CoreDevices.Devices.GenericDevice(ECore, dh.DeviceName, dh.DeviceLib, dh.DeviceAdapter);
                            dh.PhysicalDevice =(g);
                            break;
                    }
                }
            }
            //set all the properties for the devices, skip the graphical only objects for now.
            for (int i = 0; i < Devs.Count; i++)
            {
                if (Devs [i].PhysicalDevice .GetType() == typeof(CoreDevices.Devices.MMEmptyGuiDevice))
                {

                }
                else
                {
                    SetProperties sp = new SetProperties();
                    sp.SetCore(Devs [i].PhysicalDevice.deviceName, ECore);
                    sp.ShowDialog();
                }
            }
            //now load the interfaces for all the devices
            for (int i = 0; i < Devs.Count; i++)
            {
                switch (Devs[i].DeviceGUI  )
                {
                    case "CoreDevices.NI_Controls.PictureBoard":
                    case "Picture Board":
                    case "pictureboard":
                    case "picture board":
                    case "SciImage.View":
                    case "Science.Image":
                        BuildGUI(Devs[i].DeviceGUI, Devs[i].DeviceAdapter, Devs[i].DeviceName);
                        break;
                    default:
                        BuildGUI("Micromanager_net.UI.GUIDeviceForm", Devs[i].DeviceGUI, Devs[i].DeviceName);
                        break;
                }
                    
            }
            
            if (MainForm != null)
            {
                this.Close();
                SaveProfiles spf = new SaveProfiles(ECore, MainForm);
                spf.Show();
                SetParent((int)spf.Handle, (int)MainForm.Handle);
            }
            else if (SaveConfigFile ==true)
            {
                //now save the information from ecore.
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save Config File";
                sfd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\ConfigFiles";
                DialogResult DR = sfd.ShowDialog();
                
                if (DR == DialogResult.OK)
                {
                    
                    ECore.SaveConfigFile(sfd.FileName);
                    ECore.SaveFakeGUIFile(sfd.FileName, RequestedGUIs);

                }
                this.Close();
            }
        }

        private void BuildGUI(string GUIFormType, string GUIControlType, string DeviceName)
        {
            if (MainForm != null)
                MainForm.SetupBasic(GUIFormType, GUIControlType, DeviceName);
            else
                _RequestedGUIs.Add(new string[] { GUIFormType, GUIControlType, DeviceName });
        }
        private List<string[]> _RequestedGUIs = new List<string[]>();
        public List<string[]> RequestedGUIs
        {
            get { return _RequestedGUIs; }
        }
        private bool FirstChoice = true ; 
        private void button2_Click(object sender, EventArgs e)
        {


            if (treeView1.SelectedNode.Parent != mainNode && treeView1.SelectedNode.Parent != null)
            {
                TreeNode nod = new TreeNode();
                
                nod.Text = treeView1.SelectedNode.Parent.Text + " : " + treeView1.SelectedNode.Text;
                //treeView1.Nodes.Remove(nod);

                mainNode2.Nodes.Add(nod);
            }
            if (FirstChoice) mainNode2.Expand();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TreeNode nod = treeView2.SelectedNode;
            treeView2.Nodes.Remove(nod);
            string s = nod.Text.Substring(0, nod.Text.IndexOf(':')-1).Trim();
            nod.Text = nod.Text.Substring(nod.Text.IndexOf(':') ).Trim();
            TreeNode parent =  mainNode.Nodes.Find(s, true)[0];
            //parent.Nodes.Add(nod);
        }

       
    }
    
}
