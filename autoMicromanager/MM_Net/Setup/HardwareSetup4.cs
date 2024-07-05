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

namespace Micromanager_net.Setup
{
    [Serializable]
    public partial class HardwareSetup4 : Form
    {
        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);

        EasyCore ECore;

        private class DeviceHolders
        {
            public ComboBox GUIs;
            public DeviceDesc deviceDesc;
            public DeviceHolders(ComboBox GUIs, DeviceDesc DeviceDesc)
            {
                this.GUIs = GUIs;
                this.deviceDesc = DeviceDesc;
            }
        }

        private class GUIClassListItem
        {
            public string Name;
            public string Type;
            public GUIClassListItem(string Name, string Type)
            {
                this.Name = Name ;
                this.Type=Type;
            }
            public override string ToString()
            {
                return Name ;
            }
        }
        private List<GetGUIRow> Rows = new List<GetGUIRow>();
       // private List<DeviceHolders> Holders = new List<DeviceHolders>();
        private List<DeviceDesc> ListHardware = null;
        /// <summary>
        /// Mainform must point at the MDI host and is used to control all the other windows.
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="MainForm"></param>
        public HardwareSetup4(EasyCore eCore,ref  List<DeviceDesc> ChosenDevices)
        {

            ListHardware = ChosenDevices;
            ECore = eCore;
            InitializeComponent();

            //Try to clear out all records of the previous setup
            ECore.Core.unloadAllDevices();
            ECore.ClearDevices();


            //todo: this sometimes crashes when adding the camera when the camera was already started
            //need to just put in a try thing and catch these errors
            for (int i = 0; i < ChosenDevices.Count; i++)
            {
                if (ChosenDevices[i].Enabled && ChosenDevices[i].DeviceName != ""
                   && ChosenDevices[i].DeviceAdapter != "" && ChosenDevices[i].DeviceLib != "None" && ChosenDevices[i].DeviceLib != "Graphical Interfaces")
                {
                    ECore.Core.loadDevice(ChosenDevices[i].DeviceName, ChosenDevices[i].DeviceLib, ChosenDevices[i].DeviceAdapter);
                    try
                    {
                        ECore.Core.initializeDevice(ChosenDevices[i].DeviceName);
                    }
                    catch { }
                    ChosenDevices[i].DeviceType = eCore.Core.getDeviceType(ChosenDevices[i].DeviceName);
                }
            }


            int H = 32;
            int j = 0;
            foreach (DeviceDesc s in ChosenDevices)
            {
                GetGUIRow gUr=new GetGUIRow();

                gUr.Location = new System.Drawing.Point(3, H);
                gUr.Name = "getGUIRow1";
                gUr.Size = new System.Drawing.Size(656, 27);
                gUr.TabIndex = 0;
                gUr.ListIndex = j;
              
                gUr.Label = s.DeviceLib + " : " + s.DeviceAdapter + " : " + s.DeviceName  ;
                if (s.DeviceLib == "Graphical Interfaces")
                {
                    if (s.DeviceAdapter == "Image Viewer")
                        LoadImageViewers(gUr.GUIs);
                    else
                    {
                        if (s.DeviceAdapter == "Joystick Control")
                            gUr.GUIs.Items.Add(new GUIClassListItem(s.DeviceAdapter, "Micromanager_net.UI.StageJoystick"));
                        if (s.DeviceAdapter == "Image Capture and Record Tools")
                            gUr.GUIs.Items.Add(new GUIClassListItem(s.DeviceAdapter,"Micromanager_net.UI.MoreRecordTools" ));
                        if (s.DeviceAdapter == "Corelog Viewer")
                            gUr.GUIs.Items.Add(new GUIClassListItem(s.DeviceAdapter, "Micromanager_net.UI.CoreLogControl" ));    
                        if (s.DeviceAdapter == "Scripting Tools")
                            gUr.GUIs.Items.Add(new GUIClassListItem(s.DeviceAdapter, "MMUI_ScriptModules.IronPythonScriptModule" ));
                        gUr.GUIs.Text = s.DeviceAdapter;
                    }
                }
                else 
                    LoadGUIs(gUr.GUIs, s.DeviceType );
                panel2.Controls.Add(gUr);
                Rows.Add(gUr);
                H += 27;
                j++;
            }
            //figure out how to run the whole serial PortType thing 
            this.panel2.Height = H + 32;
           // DisplayGUI.Items.Clear();
           // DisplayGUI.Items.AddRange( eCore.GetViewerNames() );
        }

        
        /// <summary>
        /// Loads all the GUIs into the combo box.
        /// </summary>
        /// <param name="cb"></param>
        private void LoadGUIs(ComboBox cb, CWrapper.DeviceType  DeviceType)
        {
            cb.Items.Clear();
            cb.Items.Add ("Property List");
            string [] GUIs=  ECore.GetPossibleDeviceGUIsFromDeviceType(DeviceType);
            foreach (string s in GUIs)
            {
                ECore.GetDeviceGUI(s);
                string t = s.Replace("Micromanager_net.UI.", "");

                cb.Items.Add(new GUIClassListItem( t,s) );
            }
            cb.Text = "Property List";
        }
        /// <summary>
        /// Loads all the GUIs into the combo box.
        /// </summary>
        /// <param name="cb"></param>
        private void LoadImageViewers(ComboBox cb)
        {
            cb.Items.Clear();
           // cb.Items.Add("Property List");
            string[] GUIs = ECore.GetViewerNames();
            foreach (string s in GUIs)
            {
                string t="";
                if (s != "CoreDevices.IPictureView")
                {
                    t = s.Replace("CoreDevices.NI_Controls.", "");
                    t = t.Replace("Micromanager_net.", "");
                    cb.Items.Add(new GUIClassListItem(t, s));
                }
            }
            cb.Text = "Picture Board";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Get all the guis sorted out
            foreach (GetGUIRow  dh in Rows )
            {
                string GUIType = "";
                if (dh.GUIs.SelectedItem != null)
                {
                    try
                    {
                        GUIType = ((GUIClassListItem)dh.GUIs.SelectedItem).Type;
                    }
                    catch
                    {
                        GUIType = dh.GUIs.Text;
                    }
                }
                else
                    GUIType = dh.GUIs.Text;
                if (GUIType == "Picture Board") GUIType = "CoreDevices.NI_Controls.PictureBoard";
                if (GUIType == "Property List") GUIType = "Micromanager_net.UI.PropertiesList";
                ListHardware[dh.ListIndex ].DeviceGUI=GUIType;

            }

            this.Close();


        }
      
       
    }
    
}
