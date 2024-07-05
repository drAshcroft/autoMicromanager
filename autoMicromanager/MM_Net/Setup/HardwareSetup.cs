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
    public partial class HardwareSetup : Form
    {
        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);

        EasyCore ECore;
        
        Micromanager_net.Form1 MainForm;
        List<DeviceDesciption> ExtraDevices = new List<DeviceDesciption>();

        /// <summary>
        /// Mainform must point at the MDI host and is used to control all the other windows.
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="MainForm"></param>
        public HardwareSetup(EasyCore eCore, Form1 MainForm)
        {
            this.MainForm = MainForm;
            ECore = eCore;
            InitializeComponent();

            //Query the core to get all possible GUIs
            LoadGUIs(cameraGUI);
            LoadGUIs(xyGUI );
            LoadGUIs(focusGUI);
            LoadGUIs(filterGUI);

            string[] adapters=ECore.GetDeviceLibraries ;

            LoadAdapters(cameraLibs,adapters );
            LoadAdapters(xyLibs,adapters );
            LoadAdapters(focusLibs, adapters);
            LoadAdapters(filterLibs, adapters);
            deviceDesciption1.SetCore(ECore);
            deviceDesciption2.SetCore(ECore);
            deviceDesciption3.SetCore(ECore);
            deviceDesciption4.SetCore(ECore);
            deviceDesciption5.SetCore(ECore);
            deviceDesciption6.SetCore(ECore);
            deviceDesciption7.SetCore(ECore);

            ExtraDevices.Add(deviceDesciption1);
            ExtraDevices.Add(deviceDesciption2);
            ExtraDevices.Add(deviceDesciption3);
            ExtraDevices.Add(deviceDesciption4);
            ExtraDevices.Add(deviceDesciption5);
            ExtraDevices.Add(deviceDesciption6);
            ExtraDevices.Add(deviceDesciption7);

        }

        /// <summary>
        /// Shows all the devices in a library
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="LibraryName"></param>
        private void LoadDevices(ComboBox cb, string LibraryName)
        {
            try
            {
                if (LibraryName != "None")
                {
                    string[] Devices = ECore.GetDevicesFromLibrary(LibraryName);
                    cb.Items.Clear();
                    cb.Items.AddRange(Devices);

                    cb.Text = "Select";
                }
                else
                {
                    cb.Items.Clear();
                    cb.Text = "Cannot Open Library";
                }
            }
            catch 
            {
                cb.Items.Clear();
                cb.Text="Cannot Open Library";
            }
        }
       
        /// <summary>
        /// Shows all adapters
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="Adapters"></param>
        private void LoadAdapters(ComboBox cb,string[] Adapters)
        {
            cb.Items.Clear();
            cb.Items.Add("None");
            cb.Items.AddRange(Adapters);
            cb.Text = "None";
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
        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cameraAdapter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cameraLibs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDevices(cameraAdapter,(string) cameraLibs.SelectedItem);
        }

        private void xyLibs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDevices(xyAdapter,(string) xyLibs.SelectedItem );
        }

        private void focusLibs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDevices(FocusAdapter,(string) focusLibs.SelectedItem );
        }

        private void filterLibs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDevices(filterAdapter, (string)filterLibs.SelectedItem );
        }

        private void addXDevice_Click(object sender, EventArgs e)
        {
            DeviceDesciption dd = new DeviceDesciption();
            dd.SetCore(ECore);
            
            this.panel1.Controls.Add(dd);
            dd.Location = new System.Drawing.Point(12,deviceDesciption1.Top + (ExtraDevices.Count ) * deviceDesciption1.Height );
            dd.Name = "deviceDesciption1";
            dd.Size = new System.Drawing.Size(747, 29);
            dd.TabIndex = 48;
            
            ExtraDevices.Add(dd);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            List<DeviceDesc> Hardware = new List<DeviceDesc>();
            Hardware.Add(new DeviceDesc(cameraCB.Checked,"camera",cameraLibs.Text ,cameraAdapter.Text ,cameraGUI.Text  ));
           
            Hardware.Add(new DeviceDesc(xyCB.Checked, "xyStage", xyLibs.Text, xyAdapter.Text, xyGUI.Text));
            Hardware.Add(new DeviceDesc(focusCB.Checked, "zStage", focusLibs.Text, FocusAdapter.Text, focusGUI.Text));
            Hardware.Add(new DeviceDesc(filterCB.Checked, "filter", filterLibs.Text, filterAdapter.Text, filterGUI.Text));

            
            for (int i = 0; i < ExtraDevices.Count; i++)
            {
                DeviceDesciption dd = ExtraDevices[i];
                if (dd.DeviceName.Trim() !="")
                    Hardware.Add(new DeviceDesc(dd.DeviceEnabled  , dd.DeviceName ,dd.DeviceLib ,dd.DeviceAdapter ,dd.DeviceGUI ));
            }

            //Try to clear out all records of the previous setup
            ECore.Core.unloadAllDevices();
            ECore.ClearDevices();

           
            //todo: this sometimes crashes when adding the camera when the camera was already started
            //need to just put in a try thing and catch these errors
            for (int i = 0; i < Hardware.Count; i++)
            {
                if (Hardware[i].Enabled && Hardware[i].DeviceName != ""
                   && Hardware[i].DeviceAdapter != "" && Hardware[i].DeviceLib != "None" && Hardware[i].DeviceLib!="GUI Only")
                {
                    ECore.Core.loadDevice(Hardware[i].DeviceName, Hardware[i].DeviceLib, Hardware[i].DeviceAdapter);
                    try
                    {
                        ECore.Core.initializeDevice(Hardware[i].DeviceName);
                    }
                    catch { }
                    
                }
            }

           

            Dictionary<string, DeviceDesc> HardDict = new Dictionary<string, DeviceDesc>();
            foreach (DeviceDesc dd in Hardware)
            {
                HardDict.Add(dd.DeviceName, dd);

            }
            //todo: need to make a default interface here that is just a property list for those 
            //devices that do not a fancy gui made.  should be set in mmui_genericeDeviceGUIS

            //also need a default core device for unknown devices, so that they can be found with scripting
            //and from the other devices

           
            //Put in all the devices into each parts category
            List<string> AllDevices = new List<string>();
            for (int i = 0; i < Hardware.Count; i++)
                AllDevices.Add(Hardware[i].DeviceName );
            List<string> Cameras = new List<string>();
            Cameras.AddRange( ECore.ConvertStrVectortoArray(ECore.Core.getLoadedDevicesOfType(DeviceType.CameraDevice)));
            List<string> filterWheels = new List<string>();
            filterWheels.AddRange( ECore.ConvertStrVectortoArray(ECore.Core.getLoadedDevicesOfType(DeviceType.StateDevice )));
            List<string> DAQs = new List<string>();
            DAQs.AddRange( ECore.ConvertStrVectortoArray(ECore.Core.getLoadedDevicesOfType(DeviceType.SignalIODevice)));
            List<string> XYStages = new List<string>();
            XYStages.AddRange( ECore.ConvertStrVectortoArray(ECore.Core.getLoadedDevicesOfType(DeviceType.XYStageDevice )));
            List<string> ZStages = new List<string>();
            ZStages.AddRange ( ECore.ConvertStrVectortoArray(ECore.Core.getLoadedDevicesOfType(DeviceType.StageDevice )));

            List<string> Oddballs;
            Oddballs = GetOddBalls(AllDevices, Cameras);
            Oddballs = GetOddBalls(Oddballs, filterWheels);
            Oddballs = GetOddBalls(Oddballs, DAQs);
            Oddballs = GetOddBalls(Oddballs, XYStages);
            Oddballs = GetOddBalls(Oddballs, ZStages);

            //ECore.Core.unloadAllDevices();
            ///Start up each type of devices
            foreach (string s in Cameras)
            {
                CoreDevices.Devices.Camera c = new CoreDevices.Devices.NormalCamera(ECore, s, HardDict[s].DeviceLib, HardDict[s].DeviceAdapter);
                 if (s=="camera") c.MakeOffical();
            }
            foreach (string s in filterWheels)
            {
                CoreDevices.Devices.FilterWheel f = new CoreDevices.Devices.FilterWheel(ECore, s, HardDict[s].DeviceLib, HardDict[s].DeviceAdapter);
                if (s == "filter") f.MakeOffical();
            }
            foreach (string s in DAQs)
            {
                CoreDevices.Devices.FunctionGenerator f = new CoreDevices.Devices.FunctionGenerator(ECore, s, HardDict[s].DeviceLib, HardDict[s].DeviceAdapter);
            }
            foreach (string s in XYStages)
            {
                CoreDevices.Devices.XYStage x = new CoreDevices.Devices.XYStage(ECore, s, HardDict[s].DeviceLib, HardDict[s].DeviceAdapter);
                if (s == "xyStage") x.MakeOffical();

            }
            foreach (string s in ZStages)
            {
                CoreDevices.Devices.ZStage z = new CoreDevices.Devices.ZStage(ECore, s, HardDict[s].DeviceLib, HardDict[s].DeviceAdapter);
                if (s == "zStage") z.MakeOffical();
            }

            for (int i = 0; i < Hardware.Count; i++)
            {
                if (Hardware[i].Enabled && Hardware[i].DeviceName != ""
                   && Hardware[i].DeviceAdapter != "" && Hardware[i].DeviceLib != "None" && Hardware[i].DeviceLib != "GUI Only")
                {
                    SetProperties sp = new SetProperties();
                    sp.SetCore(Hardware[i].DeviceName, ECore);
                    sp.ShowDialog();
                }
            }


            for (int i = 0; i < Hardware.Count; i++)
            {
                if (Hardware[i].Enabled && Hardware[i].DeviceName != ""
                   && Hardware[i].DeviceAdapter != "" && Hardware[i].DeviceLib != "None" && Hardware[i].DeviceLib != "GUI Only")
                {
                    BuildGUI("Micromanager_net.UI.GUIDeviceForm", Hardware[i].DeviceGUI, Hardware[i].DeviceName);
                }
            }
            //ECore.Core.saveSystemConfiguration(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "MMDefaultConfig.xml"));
            //start the devices that are not supported by micromanager
            foreach (DeviceDesc dd in Hardware)
            {
                if (dd.DeviceLib=="GUI Only")
                    BuildGUI("Micromanager_net.UI.GUIDeviceForm", dd.DeviceGUI, "");
            }


            if (RecordToolsCB.Checked)
                BuildGUI("Micromanager_net.UI.GUIDeviceForm", "Micromanager_net.UI.MoreRecordTools", "");
                
                //MainForm.SetupBasic("Micromanager_net.UI.RecordToolsForm","", "");
            if (CorelogViewerCB.Checked == true)
                BuildGUI("Micromanager_net.UI.GUIDeviceForm", "Micromanager_net.UI.CoreLogControl", "");

            if (JoyStickCB.Checked)
                BuildGUI("Micromanager_net.UI.GUIDeviceForm","Micromanager_net.UI.StageJoystick", "");

            BuildGUI(DisplayGUI.Text, "", "Display");
            
             this.Close();
             if (MainForm != null)
             {
                 SaveProfiles spf = new SaveProfiles(ECore, MainForm);
                 spf.Show();
                 SetParent((int)spf.Handle, (int)MainForm.Handle);
             }
        }
        private List<string[]> _RequestedGUIs = new List<string[]>();
        public List<string[]> RequestedGUIs
        {
            get { return _RequestedGUIs; }
        }
        private void BuildGUI(string GUIFormType, string GUIControlType, string DeviceName)
        {
            if (MainForm != null)
                MainForm.SetupBasic(GUIFormType, GUIControlType, DeviceName);
            else
                _RequestedGUIs.Add(new string[] { GUIFormType, GUIControlType, DeviceName });
        }
        private List<string> GetOddBalls(List<string> BigList, List<string> DeviceList)
        {
            List<string> OddBalls=new List<string>();
            foreach (string s in BigList )
                if (!DeviceList.Contains(s))
                    OddBalls.Add(s);
            return OddBalls;
        }

        private void addXDevice_Click_1(object sender, EventArgs e)
        {

        }

       
    }
    
}
