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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CoreDevices;
using CoreDevices.Channels;

namespace Micromanager_net.UI
{
    public partial class AimsScriptModule : UserControl, CoreDevices.UI.GUIDeviceControl
    {
       
        private EasyCore ECore = null;
        
        
       
        public string DeviceType() { return "GUIControl"; }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public AimsScriptModule()
        {
            InitializeComponent();
           
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("AIMS ScriptControl");
        }
        private CoreDevices.Devices.MMEmptyGuiDevice MyGuiDev;
        ImageContainer ic = new ImageContainer();
        public void SetCore(EasyCore Ecore, string DeviceName)
        {
            if (DeviceName == "")
                DeviceName = "AimsScript";
            try
            {
                MyGuiDev = (CoreDevices.Devices.MMEmptyGuiDevice)Ecore.GetDevice(DeviceName);
            }
            catch
            {
                MyGuiDev = new CoreDevices.Devices.MMEmptyGuiDevice(Ecore, DeviceName);
                
            }
            ECore = Ecore;
            
           
            scriptControl1.AddObject("EasyCore",Ecore  );
            scriptControl1.AddObject("ScriptHost", this);
            scriptControl1.AddObject("ButtonPage", panel1);
            scriptControl1.StartEditor();

            scriptControl1.Execute += new EventHandler(scriptControl1_Execute);
        }
        private bool _StopRequested = false;
        public bool StopRequested
        {
            get { return _StopRequested; }
        }
        public void DoEvents()
        {
            Application.DoEvents();
            System.Threading.Thread.Sleep(25);
        }
        public CoreDevices.CoreImage TestImage;
        /*try
           {
               _StopRequested = false;
               engine1.OutputAssemblyName = scriptControl1.OutputAssemblyName;
               engine1.StartMethodName = scriptControl1.StartMethodName;
               engine1.DefaultNameSpace = scriptControl1.DefaultNameSpace;
               engine1.RemoteVariables = scriptControl1.RemoteVariables;
               engine1.DefaultClassName = scriptControl1.DefaultClassName;
               object ret = engine1.Execute(null);

               MessageBox.Show("Execution stopped");
           }
           catch (Exception ex)
           {
               System.Diagnostics.Debug.Print(ex.Message);
               System.Diagnostics.Debug.Print(ex.InnerException.ToString());
           }*/
        void scriptControl1_Execute(object sender, EventArgs e)
        {
            try
            {
                _StopRequested = false;
                engine1.OutputAssemblyName = scriptControl1.OutputAssemblyName;
                engine1.StartMethodName = scriptControl1.StartMethodName;
                engine1.DefaultNameSpace = scriptControl1.DefaultNameSpace;
                engine1.RemoteVariables = scriptControl1.RemoteVariables;
                engine1.DefaultClassName = scriptControl1.DefaultClassName;
                object ret = engine1.Execute(null);

                MessageBox.Show("Execution stopped");
            }
            catch (Exception ex)
            {
                engine1.KillScript();
                System.Diagnostics.Debug.Print(ex.Message);
                System.Diagnostics.Debug.Print(ex.InnerException.ToString());
            }
        }
        private CoreDevices.CoreImage ProcessImage(CoreDevices.CoreImage cImage)
        {

            return cImage;
        }

      
    }

    
}
