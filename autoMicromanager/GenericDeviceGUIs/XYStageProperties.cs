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
using System.Threading;
using CoreDevices;

namespace Micromanager_net.UI
{
    /// <summary>
    /// GUI for a standard XY stage.   Includes a location list in order to lable locations
    /// </summary>
    public partial class XYStageProperties : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        EasyCore ECore;
        CoreDevices.Devices.XYStage xyStage;

        public string DeviceType() { return CWrapper.DeviceType.XYStageDevice.ToString(); }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public XYStageProperties()
        {
            InitializeComponent();
            
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("XY Stage Control");
        }
        /// <summary>
        /// Starts the GUI, with information of which micromanager GUI is required
        /// </summary>
        /// <param name="Ecore"></param>
        /// <param name="DeviceName"></param>
        public void SetCore(EasyCore Ecore,string DeviceName)
        {
            try
            {
                if (DeviceName != "")
                {
                    xyStage = (CoreDevices.Devices.XYStage)Ecore.GetDevice(DeviceName);
                }
                else
                {
                    xyStage  = Ecore.MMXYStage ;
                }
            }
            catch { }
            if (xyStage != null)
            {

                this.ECore = Ecore;
                xyStage.SetPropUI((IPropertyList) propertyList1);
                LoadLocationList();
                
            }
        }

        
        private int  LocationCount=0;
        /// <summary>
        /// Pulls the location list from the device and displays it on the screen
        /// </summary>
        public void LoadLocationList()
        {
            dataGridView1.Rows.Clear();
            foreach (CoreDevices.Devices.XYStage.XYZlocation z in xyStage.Stops)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dataGridView1);
                r.Cells[1].Value = z.LocationName;
                LocationCount += 1;
                r.Cells[2].Value = z.x.ToString() + ", " + z.y.ToString() + ", " +z.z.ToString() ;
                dataGridView1.Rows.Add(r);
            }
        }

        private void AddLocationButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(dataGridView1);
            string lname = "Location " + LocationCount;
            r.Cells[1].Value = lname;
            LocationCount += 1;
            double x;
            double y;
            double z=0;

            xyStage.GetStagePosition(out x,out  y);
            x = Math.Round(x, 3);
            y = Math.Round(y, 3);

            if (ECore.MMFocusStage != null) z = ECore.MMFocusStage.CurrentPosition();
            z = Math.Round(z, 3);
            r.Cells[2].Value = x.ToString() + ", " + y.ToString() + ", " + z.ToString();
            dataGridView1.Rows.Add(r);
            xyStage.Stops.Add(new CoreDevices.Devices.XYStage.XYZlocation(lname, x, y, z));
        }

        

        

        private void StageProperties_Resize(object sender, EventArgs e)
        {
            
        }


    }
}
