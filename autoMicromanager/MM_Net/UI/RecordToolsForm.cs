﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Micromanager_net.UI
{
    public partial class RecordToolsForm : DockContent  
    {
        public RecordToolsForm()
        {
            InitializeComponent();
            ThisControl = this;
            UpdateProgress += DoUpdateProgress;
        }
        private delegate void UpDateProgressEvent(int NewValue);
        private CoreDevices.EasyCore ECore = null;
        private RecordToolsForm  ThisControl=null;
        private CoreDevices.Camera MyCamera = null;
        private UpDateProgressEvent UpdateProgress;
        public string DeviceType() { return "Camera"; }

        private int FocusMode = 0;

        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Record and Save Tools");
        }
        public void SetCore(CoreDevices.EasyCore Ecore,string DeviceName)
        {
            try
            {
                if (DeviceName != "")
                {
                    MyCamera  = (CoreDevices.Camera )Ecore.GetDevice(DeviceName);
                }
                else
                {
                    MyCamera = Ecore.MMCamera;
                }
            }
            catch { }
            this.ECore = Ecore;
            if (MyCamera  != null)
            {
                string[] Groups = Ecore.GetAllGroupNames();
                foreach (string g in Groups )
                    AddGroupListing(g);
                AddGroupListing("");
            }
            XDispTB.Text = Ecore.ScreenSize.ToString();
            YDispTB.Text = Ecore.ScreenSize.ToString();
            ZDispTB.Text = "1.0";
            UpdateTimer.Enabled = true;

        }

        #region GroupHandling
        
        private void AddGroupListing(string Groupname)
        {
            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(GroupList);
            if (Groupname == "")
                r.Cells[0].Value = "Add Group";
            else
            {
                r.Cells[0].Value = "Edit Group";
                r.Cells[1].Value = Groupname;
            }
            //Groups.Add(new Micromanager_net.CoreDevices.ChannelGroup(ECore ));
            GroupList.CellClick += new DataGridViewCellEventHandler(GroupList_CellClick);

            GroupList.Rows.Add(r);
        }
        void GroupList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                CoreDevices.ChannelGroup cg = null;
                try
                {
                    cg = ECore.GetChannelGroup((string)GroupList[1, e.RowIndex].Value);
                }
                catch
                {
                    cg = new Micromanager_net.CoreDevices.ChannelGroup(ECore );
                }
                
                Channels.ChannelGroupDefinition cgd = new Micromanager_net.Channels.ChannelGroupDefinition(ECore, ref cg);
                cgd.ShowDialog();
                if (cg != null && cg.GroupName !=null && cg.GroupName != "")
                {

                    GroupList[1, e.RowIndex].Value = cg.GroupName;
                    GroupList[0, e.RowIndex].Value = "Edit Group";
                    
                    ECore.AddGroup(cg);
                    AddGroupListing("");
                }
            }
            else if (e.ColumnIndex == 2)
            {
                for (int i = 0; i <GroupList.RowCount; i++)
                {
                    if (GroupList[2, i].Value != null)
                    {
                        if (i != e.RowIndex)
                            GroupList[2, i].Value = false;
                        else
                        {
                            GroupList[2, i].Value = (!(bool)GroupList[2, i].Value);
                            if ((bool)GroupList[2, i].Value == true)
                            {
                                try
                                {
                                    ActiveGroup = ECore.GetChannelGroup((string)GroupList[1, e.RowIndex].Value);
                                }
                                catch
                                {
                                    ActiveGroup = null;
                                }
                            }
                        }
                    }
                }

            }
        }
        #endregion
        

        private void browse_Click(object sender, EventArgs e)
        {
            SequenceFiles = null;
            saveFileDialog1.ShowDialog();
            ExperimentPath.Text = saveFileDialog1.FileName;
            ExperimentPath.Text = IncrementFilename(ExperimentPath.Text);
        }

        private void SnapShot_Click(object sender, EventArgs e)
        {
          

            ExperimentPath.Text = IncrementFilename(ExperimentPath.Text);
        }
        #region Filehandling
        private List<string> GetAllFiles(string filename)
        {
            char[] sep = { '\\' };
            string[] parts = filename.Split(sep);
            string outfilename = parts[parts.Length - 1];
            string path1 = "";
            for (int i = 0; i < parts.Length - 1; i++)
            {
                path1 += parts[i] + "\\";
            }
            char[] sep2 = { '.', '_' };

            parts = outfilename.Split(sep2);
            DirectoryInfo di;
            FileInfo[] rgFiles;
            if (path1.Trim() == "")
            {
                di = new DirectoryInfo(System.IO.Path.GetDirectoryName(Application.ExecutablePath));
                rgFiles = di.GetFiles(parts[0] + "*.tif");
            }
            else
            {
                di = new DirectoryInfo(path1);
                rgFiles = di.GetFiles(parts[0] + "*." + parts[parts.Length - 1]);
            }
            List<string> fileList = new List<string>();
            foreach (FileInfo fi in rgFiles)
            {
                fileList.Add(fi.FullName);
            }
            return fileList;
        }
        private string IncrementFilename(string filename)
        {
           
            char[] sep = { '\\' };
            string[] parts = filename.Split(sep);
            string outfilename = parts[parts.Length - 1];
            string path1 = "";
            for (int i = 0; i < parts.Length - 1; i++)
            {
                path1 += parts[i] + "\\";
            }
            char[] sep2 = { '.' };
            char[] sep3= {'_'};

            parts = outfilename.Split(sep2);
            if (parts.Length == 1)
            {
                outfilename += ".tif";
                parts = outfilename.Split(sep2);
            }
            
            
                if (parts[0].Contains(sep3[0]))
                {
                    string[] parts2 = parts[0].Split(sep3);
                    int i = 0;
                    int.TryParse(parts2[parts2.Length - 1], out i);
                    i++;
                    outfilename = path1 ;
                    for (int j = 0; j < parts2.Length - 1; j++)
                        outfilename += parts2[j];
                    string inc = "00000" + i.ToString();

                    outfilename += "_" + inc.Substring(inc.Length - 4) + "." + parts[parts.Length - 1];

                }
                else
                {
                    outfilename = path1  + parts[0] + "_0000." + parts[parts.Length - 1];
                }
            
            return outfilename;
        }
        #endregion

        private int FrameNum = 0;
        
        private bool NoVidLoad = false ;
       

        private void StopButton_Click(object sender, EventArgs e)
        {
            bStop = true;
        }
       
        private string[] SequenceFiles;
        int MaxIndex = 0;
        bool bStop = false;
        private int PlayPosition = 0;
        private void Play_Click(object sender, EventArgs e)
        {
            PlayPosition = 0;
            try
            {
                if (SequenceFiles == null)
                {
                    List<string> ls = GetAllFiles(ExperimentPath.Text);
                    SequenceFiles = new string[ls.Count];
                    ls.CopyTo(SequenceFiles);
                    Position_indicator.Maximum = ls.Count;
                    Position_indicator.Value = 0;
                }
            }
            catch
            { }

            int StartIndex=Position_indicator.Value ;
            MaxIndex = Position_indicator.Maximum;
            if (StartIndex >= MaxIndex-2)
            {
                StartIndex = 0;
            }
            bStop = false; 
            for (int i = StartIndex; (i < MaxIndex) && (!bStop); i++)
            {
                Position_indicator.Value = i;
            }
             
             
             
        }

        
        private void Position_indicator_ValueChanged(object sender, EventArgs e)
        {
            if (NoVidLoad == false)
            {
                try
                {
                    int i = Position_indicator.Value;
                    CoreDevices.CoreImage c = new CoreDevices.CoreImage(SequenceFiles[i]);
                //    Bitmap B = c.ImageNet ;
                //    B.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath),
                 //       String.Format("{0:000}", PlayPosition) + ".bmp"));
                    //c.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath),
                    //    String.Format("{0:0000}", PlayPosition) + ".jpg"),false );
                    PlayPosition++;
                    ECore.UpdatePaintSurface(c);
                   // ECore.DoForcedSave(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath),
                    //    String.Format("{0:000}", PlayPosition) + ".jpg"));
                    Application.DoEvents();
                    
                }
                catch
                { }
            }
        }

        private void FastForward_Click(object sender, EventArgs e)
        {
            
            int i =Position_indicator.Value +  10;
            if (i < Position_indicator.Maximum)
                Position_indicator.Value = i;
        }

        private void Rewind_Click(object sender, EventArgs e)
        {
            int i = Position_indicator.Value - 10;
            if (i > 0)
                Position_indicator.Value = i;
        }

        //private bool
        private void IntervalBox_KeyPress(object sender, KeyPressEventArgs e)
        {
          //  IntervalEntered = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ECore.MMCamera.EndSequence();
        }

        private void RecordTools_Resize(object sender, EventArgs e)
        {
            GroupList.Height = this.Height - GroupList.Top;
        }

        private void TakeStackB_Click(object sender, EventArgs e)
        {
           

           
        }

        private void DoUpdateProgress(int NewValue)
        {
            StackProgress.Value = NewValue;
            // StackProgress.Update();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            double  exposuretime= ECore.MMCamera.GetExposure();
            double AcqTime=0;
            string[] Props= ECore.MMCamera.GetDevicePropertyNames();
            foreach (string s in Props )
                if (s.ToLower().Contains("actualinterval") == true)
                {
                    string Value = ECore.MMCamera.GetDeviceProperty(s);
                    Double.TryParse(Value, out AcqTime);
                }
            double FrameTime =1/( (exposuretime + AcqTime) / 1000);
            MaximumFrameRateLabel.Text = "Maximum FrameRate: " + Math.Round(FrameTime, 2).ToString();

            double GivenFrameTime = 0;
            Double.TryParse(FrameRateTB.Text, out GivenFrameTime);
            if (GivenFrameTime > FrameTime || GivenFrameTime ==0)
                FrameRateTB.Text = Math.Round(FrameTime, 2).ToString();

            if ( AutosetFrameRateCB.Checked ==true )
                FrameRateTB.Text = Math.Round(FrameTime, 2).ToString();

        }

        private bool RunningAcq = false;
        private CoreDevices.ChannelGroup ActiveGroup=null;
        private void DoFocusModeB_Click(object sender, EventArgs e)
        {
            FocusMode = 0;
            long FrameRate =0;
            double dFrameRate;
            double.TryParse(FrameRateTB.Text, out dFrameRate);
            FrameRate =(long)(1000/ dFrameRate );


            double exposuretime = ECore.MMCamera.GetExposure();
            double AcqTime = 0;
            string[] Props = ECore.MMCamera.GetDevicePropertyNames();
            foreach (string s in Props)
                if (s.ToLower().Contains("acquisition") == true)
                {
                    Double.TryParse(ECore.MMCamera.GetDeviceProperty(s), out AcqTime);
                }
            if (AcqTime == 0) AcqTime = 50;
            long MaxFrameRate = (long)(exposuretime + AcqTime);

            if (FrameRate < MaxFrameRate) FrameRate = MaxFrameRate;

            if (ActiveGroup == null)
            {
                for (int i = 0; i < GroupList.RowCount; i++)
                {
                    if (GroupList[2,i].Value !=null)
                        if ((bool)GroupList[2,i].Value ==true )
                            ActiveGroup = ECore.GetChannelGroup((string)GroupList[1, i].Value);
                }

            }
            RunningAcq = true;
            ECore.RunChannelAcquisition(ActiveGroup , "camera", FrameRate,false,"","");
        }

        private void FrameRateTB_TextChanged(object sender, EventArgs e)
        {
            if (RunningAcq)
            {
                ECore.StopAcquisition();
                if (FocusMode == 0)
                    DoFocusModeB_Click(this, EventArgs.Empty);
                else
                    DoBurstModeB_Click(this, EventArgs.Empty);
            }
        }

        private void StopFocusB_Click(object sender, EventArgs e)
        {
            RunningAcq = false;
            ECore.StopAcquisition();
        }

        private void DoBurstModeB_Click(object sender, EventArgs e)
        {

        }

        private void GroupList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TakeStackBtn_Click(object sender, EventArgs e)
        {
            
            long FrameRate =0;
            double dFrameRate;
            double.TryParse(FrameRateTB.Text, out dFrameRate);
            FrameRate =(long)(1000/ dFrameRate );


            double exposuretime = ECore.MMCamera.GetExposure();
            double AcqTime = 0;
            string[] Props = ECore.MMCamera.GetDevicePropertyNames();
            foreach (string s in Props)
                if (s.ToLower().Contains("acquisition") == true)
                {
                    Double.TryParse(ECore.MMCamera.GetDeviceProperty(s), out AcqTime);
                }
            if (AcqTime == 0) AcqTime = 50;
            long MaxFrameRate = (long)(exposuretime + AcqTime);

            if (FrameRate < MaxFrameRate) FrameRate = MaxFrameRate;

            if (ActiveGroup == null)
            {
                for (int i = 0; i < GroupList.RowCount; i++)
                {
                    if (GroupList[2,i].Value !=null)
                        if ((bool)GroupList[2,i].Value ==true )
                            ActiveGroup = ECore.GetChannelGroup((string)GroupList[1, i].Value);
                }

            }

            int numSlices=0;
            double SliceDistance=0;
            int.TryParse( numSlicesTB.Text,out numSlices);
            double.TryParse(sliceDistanceTB.Text,out SliceDistance);
            ECore.RunZStackAcquisition(ActiveGroup, "camera", "zstage",(long) AcqTime, FirstStackCB.Checked, ShowSlices.Checked, CurrentIsMiddle.Checked, numSlices, SliceDistance,SaveStackCB.Checked,ExperimentPath.Text,StackFilenameTB.Text );
        }

        private void StopStackBtn_Click(object sender, EventArgs e)
        {
            ECore.StopAcquisition();
        }

        private void browse_Click_1(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.Description = "Choose Experiment Folder";
            folderBrowserDialog1.ShowDialog();
            ExperimentPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void RecordButton_Click_1(object sender, EventArgs e)
        {
            FocusMode = 0;
            long FrameRate = 0;
            double dFrameRate;
            double.TryParse(FrameRateTB.Text, out dFrameRate);
            FrameRate = (long)(1000 / dFrameRate);


            double exposuretime = ECore.MMCamera.GetExposure();
            double AcqTime = 0;
            string[] Props = ECore.MMCamera.GetDevicePropertyNames();
            foreach (string s in Props)
                if (s.ToLower().Contains("acquisition") == true)
                {
                    Double.TryParse(ECore.MMCamera.GetDeviceProperty(s), out AcqTime);
                }
            if (AcqTime == 0) AcqTime = 50;
            long MaxFrameRate = (long)(exposuretime + AcqTime);

            if (FrameRate < MaxFrameRate) FrameRate = MaxFrameRate;

            if (ActiveGroup == null)
            {
                for (int i = 0; i < GroupList.RowCount; i++)
                {
                    if (GroupList[2, i].Value != null)
                        if ((bool)GroupList[2, i].Value == true)
                            ActiveGroup = ECore.GetChannelGroup((string)GroupList[1, i].Value);
                }

            }
            RunningAcq = true;
            ECore.RunChannelAcquisition(ActiveGroup, "camera", FrameRate,true , ExperimentPath.Text,SaveFileNameTB.Text );
        }

      

        

       

        
        
        
    }
}