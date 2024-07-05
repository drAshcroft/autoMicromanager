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
    /// <summary>
    /// These are a number of GUIs that I use my lab.  Their use is pretty straight forward and are intended as an example of the customization you can perform.
    /// Here I have two stages that needed to run in unison for imaging, but apart for focusing.  This is just a copy of the Z stage control
    /// </summary>
    public partial class DoubleZStageProperties : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private CoreDevices.Devices.ZStage zStage;
        private CoreDevices.Devices.ZStage SecStage;
        private EasyCore Ecore;
        private double SlideOffsets = 0;
        private const double CourseRange = 1000;
        private const double FineRange = 100;

        public string DeviceType() { return CWrapper.DeviceType.StageDevice.ToString(); }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
        public DoubleZStageProperties()
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
            try 
            {
                SecStage = (CoreDevices.Devices.ZStage)Ecore.GetDevice("SecStage");
            }
            catch 
            {
                Ecore.StartCoreOnlyDevice("SecStage", "NiMotionStage", "NI_Motion_ZStage");
                SecStage = new CoreDevices.Devices.ZStage(Ecore, "SecStage", "NiMotionStage", "NI_Motion_ZStage");
            }

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
                    
                    SecStage.SetPropUI((IPropertyList)propertyList2);
                }
                catch { }

                try
                {
                    CenterStages(zStage, CourseFocus,  CourseRange );
                   /* int d = zStage.CurrentPositionSteps();
                   
                    int l = (Math.Abs(zStage.BottomLimit / 10) + Math.Abs(zStage.TopLimit / 10)) / 2;
                    CourseFocus.Minimum = d - l;
                    CourseFocus.Maximum = d + l;
                    CourseFocus.Value = (d);*/
                }
                catch { }
                try
                {
                    CenterStages(SecStage, SecCourseFocus, FineRange );
                    
                }
                catch { }
                SlideOffsets = -1 * (zStage.CurrentPosition() - SecStage.CurrentPosition());
                try
                {
                    LoadLocationList();
                }
                catch { }
            }
        }
        public void CenterStages(CoreDevices.Devices.ZStage stage, CoreDevices.DeviceControls.FocusSlider slider, double RangeMicron)
        {
            int d = stage.CurrentPositionSteps();
            double stepsize = stage.StepSize_um;
            int l =(int)( RangeMicron  / stepsize / 2);//Make the range 1 cm
            slider.Minimum = d - l;
            slider.Maximum = d + l;
            slider.Value = (d);

        }
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
        private void CourseFocus_OnScroll(object sender, ScrollEventArgs e)
        {
            int d2 = e.NewValue;
            FineFocus.Value = (d2);
            CenterStages(zStage, FineFocus, FineRange );
            
           
        }
        private void SecCourseFocus_OnScroll(object sender, ScrollEventArgs e)
        {
            int d2 = e.NewValue;// zStage.CurrentPositionSteps();
            
            SecFineFocus.Value = (d2);
            CenterStages(SecStage, SecFineFocus, FineRange );
          /*  
            SecFineFocus.Minimum = d2 - l;
            SecFineFocus.Maximum = d2 + l;
            SecFineFocus.Value = (d2);*/
        }
       
        private int LocationCount = 0;
        private void AddLocationButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(dataGridView1 );
            string lname = "Location " + LocationCount;
            r.Cells[1].Value = lname;
            LocationCount += 1;
            r.Cells[2].Value = zStage.CurrentPosition();
            dataGridView1.Rows.Add(r);

            zStage.Stops.Add(new CoreDevices.Devices.ZStage.Zlocation(lname, zStage.CurrentPosition()));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex>-1)
            {
                try
                {
                    zStage.GoToLocation((string) dataGridView1.Rows[e.RowIndex].Cells[1].Value);// Value
                    if (LinkSlidersCB.Checked == true)
                        SecStage.SetPositionAbsolute(zStage.CurrentPosition() + SlideOffsets);
                    CourseFocus.Value  = zStage.CurrentPositionSteps();
                    SecCourseFocus.Value = SecStage.CurrentPositionSteps();
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
            if (LinkSlidersCB.Checked == true)
            {
                SecStage.SetPositionAbsolute(d + SlideOffsets);
                //SecFineFocus.Value = SecStage.CurrentPositionSteps();
            }
        }
        private void SecFineFocus_OnScroll(object sender, ScrollEventArgs e)
        {
            double d = e.NewValue * SecStage.StepSize_um;
            SecStage.SetPositionAbsolute(d);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CenterStages(zStage, CourseFocus, CourseRange  );
            }
            catch { }
            try
            {
                CenterStages(SecStage, SecCourseFocus, CourseRange  );

            }
            catch { } 
        }

        private void LinkSlidersCB_CheckedChanged(object sender, EventArgs e)
        {
            if (LinkSlidersCB.Checked)
            {
                SlideOffsets =-1*( zStage.CurrentPosition() - SecStage.CurrentPosition());
            }
        }
      
    }
}
