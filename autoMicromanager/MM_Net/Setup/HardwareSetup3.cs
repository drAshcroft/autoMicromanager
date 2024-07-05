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
    public partial class HardwareSetup3 : Form
    {
        [DllImport("user32")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);

        EasyCore ECore;

        private List<GetNameRow> Rows = new List<GetNameRow>();

        private List<DeviceDesc> HardwareList=null ;
        /// <summary>
        /// Takes information from hardware setup 2 and gets names and GUIs.
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="MainForm"></param>
        public HardwareSetup3(EasyCore eCore,ref  List<DeviceDesc> ChosenDevices)
        {
            HardwareList = ChosenDevices;
            ECore = eCore;
            InitializeComponent();

            int H = 32;
            int i = 0;
            foreach (DeviceDesc  s in ChosenDevices)
            {
                GetNameRow gnr=new GetNameRow();
                gnr.Location = new System.Drawing.Point(3, H );
                gnr.Name = "getNameRow1";
                gnr.Label = s.DeviceLib + " : " + s.DeviceAdapter ;
                if (s.DeviceLib == "Graphical Interfaces")
                    gnr.Text = s.DeviceAdapter;
                gnr.Size = new System.Drawing.Size(729, 32);
               // LoadGUIs(gnr.Guis);
                gnr.TabIndex = 0;
                gnr.ListIndex = i;

                this.panel2.Controls.Add(gnr);

                Rows.Add(gnr);
                H += 32;
                i++;
                
            }
            this.panel2.Height = H + 32;
            //DisplayGUI.Items.Clear();
            //DisplayGUI.Items.AddRange( eCore.GetViewerNames() );
        }

        
        /// <summary>
        /// Loads all the GUIs into the combo box.
        /// </summary>
        /// <param name="cb"></param>
        private void LoadGUIs(ComboBox cb)
        {
            cb.Items.Clear();
            cb.Items.Add ("Property List");
            foreach (string s in ECore.GetAllDeviceGUIs())
            {
                string t = s.Replace("Micromanager_net.UI.", "");

                cb.Items.Add(t);
            }
            cb.Text = "None";
        }
        

      
        private void button1_Click(object sender, EventArgs e)
        {
            //List <DeviceDesc > FinalizedDevices = new List<DeviceDesc>();

            foreach (GetNameRow gnr in Rows)
            {
                 //string lab=gnr.Label ;
                 //string s = lab.Substring(0, lab.IndexOf(':')-1).Trim();
                 //string s2 = lab.Substring(lab.IndexOf(':')+1 ).Trim();
                 //DeviceDesc dd = new DeviceDesc(true, gnr.Text, s, s2, "");
                 //FinalizedDevices.Add(dd);
                 HardwareList[gnr.ListIndex].DeviceName = gnr.Text;
            }
            HardwareSetup4 hs4 = new HardwareSetup4(ECore, ref HardwareList );
            hs4.ShowDialog();
            this.Close();
        }
      
      
       
    }
    
}
