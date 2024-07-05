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
using CoreDevices;

namespace Micromanager_net.UI
{
    public partial class ZStageProperties : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private CoreDevices.Devices.ZStage zStage;
        private EasyCore Ecore;

        public string DeviceType() { return CWrapper.DeviceType.StageDevice.ToString() ; }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public ZStageProperties()
        {
            InitializeComponent();
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Focus Stage Properties");
        }

        /// <summary>
        /// Device name referes the the micromanager or physical device name
        /// </summary>
        /// <param name="Ecore"></param>
        /// <param name="DeviceName"></param>
        public void SetCore(EasyCore Ecore,string DeviceName)
        {
            try
            {
                if (DeviceName != "")
                {
                    zStage = (CoreDevices.Devices.ZStage)Ecore.GetDevice(DeviceName);
                }
                else
                {
                    zStage  = Ecore.MMFocusStage ;
                }
            }
            catch { }
            if (zStage != null)
            {
                try
                {
                    this.Ecore = Ecore;
                    zStage.SetPropUI((IPropertyList) propertyList1);
                }
                catch { }
                try
                {
                    int d = zStage.CurrentPositionSteps();
                    int l = (int)((Math.Abs(zStage.BottomLimit_um / 10) + Math.Abs(zStage.TopLimit_um / 10)) / 2);
                    CourseFocus.Minimum = d - l;
                    CourseFocus.Maximum = d + l;
                    CourseFocus.Value = (d);
                }
                catch { }
                try
                {
                    LoadLocationList();
                }
                catch { }
            }
        }
        /// <summary>
        /// Gets the location list from the device and then shows it on the screen.
        /// </summary>
        public void LoadLocationList()
        {
            dataGridView1.Rows.Clear();
            foreach (CoreDevices.Devices.ZStage.Zlocation z in zStage.Stops)
            {
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(dataGridView1);
                r.Cells[1].Value = z.LocationName;
                LocationCount += 1;
                r.Cells[2].Value = z.Location ;
                dataGridView1.Rows.Add(r);
            }
        }
        private void focusSlider1_OnScroll(object sender, ScrollEventArgs e)
        {
           
                    
                    int d2 = e.NewValue;// zStage.CurrentPositionSteps();
                    int l = (int)((Math.Abs(zStage.BottomLimit_um / 100) + Math.Abs(zStage.TopLimit_um / 100)) / 2);
                    FineFocus.Value = (d2);
                    FineFocus.Minimum = d2 -l ;
                    FineFocus.Maximum = d2 + l;
                    FineFocus.Value = (d2);

                    
            
        }

        private int LocationCount = 0;
        private void AddLocationButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(dataGridView1 );
            string lname = "Location " + LocationCount;
            r.Cells[1].Value = lname;
            LocationCount += 1;
            r.Cells[2].Value = CourseFocus.Value * zStage.StepSize_um;
            dataGridView1.Rows.Add(r);
            zStage.Stops.Add(new CoreDevices.Devices.ZStage.Zlocation(lname, CourseFocus.Value * zStage.StepSize_um));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex>-1)
            {
                try
                {
                    zStage.GoToLocation((string) dataGridView1.Rows[e.RowIndex].Cells[1].Value);// Value
                    CourseFocus.Value  = zStage.CurrentPositionSteps();
                }
                catch { };

            }
        }

        private void tabPage1_Resize(object sender, EventArgs e)
        {
            AddLocationButton.Top = tabPage1.Height - AddLocationButton.Height;
            dataGridView1.Height = tabPage1.Height - dataGridView1.Top - AddLocationButton.Height;

        }

        private void DoAutofocus_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is not yet implemented");
        }
        private string OldLocationName = "";
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            OldLocationName=(string) dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string NewLocationName = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            zStage.RenameStop(OldLocationName, NewLocationName);
        }

        private void FineFocus_OnScroll(object sender, ScrollEventArgs e)
        {
            double d = e.NewValue * zStage.StepSize_um;
            lPosition.Text = d.ToString();
            zStage.SetPositionAbsolute(d);
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int d = zStage.CurrentPositionSteps();
            int l = (int)((Math.Abs(zStage.BottomLimit_um / 10) + Math.Abs(zStage.TopLimit_um / 10)) / 2);
            CourseFocus.Minimum = d - l;
            CourseFocus.Maximum = d + l;
            CourseFocus.Value = (d);
            
        }
       /* private bool bHighRes = true ;
        private void HighRes_Click(object sender, EventArgs e)
        {
            if (bHighRes)
            {
                HighRes.Text = "Change to Large Range";
                int d = zStage.CurrentPositionSteps();
                CourseFocus.Minimum =d+ zStage.BottomLimit / 100;
                CourseFocus.Maximum =d+ zStage.TopLimit / 100;
                CourseFocus.Value = (d);
            }
            else
            {
                HighRes.Text = "Change to Small Range";
                int d = zStage.CurrentPositionSteps();
                CourseFocus.Minimum = zStage.BottomLimit / 10;
                CourseFocus.Maximum = zStage.TopLimit / 10;
                CourseFocus.Value = (d);
               
            }
            bHighRes = (!bHighRes);

        }
        */
       
    }
}
