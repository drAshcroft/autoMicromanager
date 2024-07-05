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
using CoreDevices;
using CoreDevices.UI;

namespace Micromanager_net.UI
{
    /// <summary>
    /// A container form for the GUIDevice control plugins.
    /// </summary>
    [Serializable]
    public partial class GUIDeviceForm : DockContent 
    {
        GUIDeviceControl  MyGUI = null;
        public GUIDeviceForm()
        {
            InitializeComponent();
        }
        public GUIDeviceForm(GUIDeviceControl TheGui)
        {
            MyGUI = TheGui;
        }
        public GUIDeviceControl GuiDevice
        {
            set { MyGUI = value; }
            get { return MyGUI; }

        }
        public void SetCore(EasyCore EasyCore,string DeviceName)
        {
            MyGUI.SetCore(EasyCore, DeviceName);
            ShowControl(MyGUI);
        }

        public void ShowControl(GUIDeviceControl GUIDC)
        {
            
            this.SuspendLayout();
            // 
            // cameraProperties1
            // 
            Control c = GUIDC.GetControl();
            c.Dock = System.Windows.Forms.DockStyle.Fill;
            c.Location = new System.Drawing.Point(0, 0);
            c.Name = "GUI_Control";
            c.Size = new System.Drawing.Size(436, 759);
            c.TabIndex = 0;
            
            // 
            // CameraPropertiesForm
            // 
            this.Name = "Gui_ControlForm";            
            this.TabText = GUIDC.Caption();
            this.Text = GUIDC.Caption();
            this.Controls.Add(c);
            c.Visible = true;
            this.ResumeLayout(false);
        }
    }
}
