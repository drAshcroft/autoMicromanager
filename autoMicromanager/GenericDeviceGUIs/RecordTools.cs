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
    public partial class RecordTools : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private delegate void UpDateProgressEvent(int NewValue);
        private EasyCore ECore = null;
        private RecordTools  ThisControl=null;
        private CoreDevices.Devices.Camera MyCamera = null;
        private UpDateProgressEvent UpdateProgress;
        public string DeviceType() { return "GUIControl"; }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }// DESCRIPTION:   
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

        public RecordTools()
        {
            InitializeComponent();
            ThisControl = this;
            UpdateProgress += DoUpdateProgress;
        }
        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Record and Save Tools");
        }
        private CoreDevices.Devices.MMEmptyGuiDevice MyGuiDev;
        public void SetCore(EasyCore Ecore, string DeviceName)
        {
            if (DeviceName == "")
                DeviceName = "RecordTools";
            try
            {
                MyGuiDev = (CoreDevices.Devices.MMEmptyGuiDevice)Ecore.GetDevice(DeviceName);
            }
            catch
            {
                MyGuiDev = new CoreDevices.Devices.MMEmptyGuiDevice(Ecore, DeviceName);
                
            }
            try
            {
                if (DeviceName != "")
                {
                    MyCamera = (CoreDevices.Devices .Camera)Ecore.GetDevice(DeviceName);
                }
                else
                {
                    MyCamera = Ecore.MMCamera;
                }
            }
            catch { }
            if (MyCamera  != null)
            {
                this.ECore = Ecore;
                DataGridViewRow r = new DataGridViewRow();
                r.CreateCells(ChannelList );
                r.Cells[0].Value = "Add Group";
                ChannelList.CellClick += new DataGridViewCellEventHandler(ChannelList_CellClick);
                ChannelList.Rows.Add(r);
            }
            XDispTB.Text = Ecore.ScreenSize.ToString();
            YDispTB.Text = Ecore.ScreenSize.ToString();
            ZDispTB.Text = "1.0";

        }

        void ChannelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ChannelGroup cg=new ChannelGroup(ECore );
            ChannelGroupDefinition cgd = new ChannelGroupDefinition(ECore,ref cg );
            cgd.ShowDialog();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            SequenceFiles = null;
            saveFileDialog1.ShowDialog();
            Filename.Text = saveFileDialog1.FileName;
            Filename.Text = IncrementFilename(Filename.Text);
        }

        private void SnapShot_Click(object sender, EventArgs e)
        {
            CoreImage c=  ECore.MMCamera.SnapOneFrame(true );
            try
            {
                c.Save(Filename.Text, CBMultipage.Checked );
              
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message,ee.Message,MessageBoxButtons.OK );
            }

            Filename.Text = IncrementFilename(Filename.Text);
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
        private void RecordButton_Click(object sender, EventArgs e)
        {
            int nFrames =(int) FrameCount.Value;
            Position_indicator.Maximum = nFrames+5;
            FrameNum = 0;
            double Interval = 0;
            double.TryParse(IntervalBox.Text, out Interval);
            ECore.MMCamera.StartSequence(nFrames,Interval, true  );
            CoreImage cImage;
            for (int i = 0; i < nFrames; )
            {
               cImage= ECore.MMCamera.GetSequenceFrame();
               if (cImage != null)
               {
                   i++;
                   FrameNum++;
                   NoVidLoad = true;
                   try
                   {
                       Position_indicator.Value = FrameNum;
                   }
                   catch
                   { }
                   if (CBMultipage.Checked == true)
                   {
                       cImage.Save(Filename.Text,true );
                   }
                   else
                   {
                       cImage.Save(Filename.Text,false );
                       Filename.Text = IncrementFilename(Filename.Text);
                   }
                   NoVidLoad = false;
               }
               Application.DoEvents();
            }
            ECore.MMCamera.EndSequence();
            if (CBMultipage.Checked == true)
                Filename.Text = IncrementFilename(Filename.Text);
            List<string > ls= GetAllFiles(Filename.Text);
            SequenceFiles = new string[ls.Count]; 
            ls.CopyTo(SequenceFiles);
        }

       
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
                    List<string> ls = GetAllFiles(Filename.Text);
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
                    CoreImage c = new CoreImage(SequenceFiles[i]);
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
            ChannelList.Height = this.Height - ChannelList.Top;
        }

        private void TakeStackB_Click(object sender, EventArgs e)
        {
            double X0;
            double Y0;
            double Z0 = 0;

            int XRowsi = 0;
            int YRowsi = 0;
            int ZRowsi = 0;

            double ImageSizeX = ECore.ScreenSize;
            double ImageSizeY = ECore.ScreenSize;
            double ImageSizeZ = 1;

            string ffilename = FilenameStackTB.Text;
            if (ffilename == "")
            {
                saveFileDialog1.ShowDialog(this);
                ffilename = saveFileDialog1.FileName;
            }
            char[] sep = { '\\' };
            string[] parts = ffilename.Split(sep);
            string filename = parts[parts.Length - 1];
            string path1 = "";
            for (int i = 0; i < parts.Length - 1; i++)
            {
                path1 += parts[i] + "\\";
            }


            try { XRowsi = int.Parse(XRows.Text); }
            catch { }
            try { YRowsi = int.Parse(YRows.Text); }
            catch { }
            try { ZRowsi = int.Parse(ZRows.Text); }
            catch { }

            try { ImageSizeX = double.Parse(XDispTB.Text); }
            catch { }
            try { ImageSizeY = double.Parse(YDispTB.Text); }
            catch { }
            try { ImageSizeZ = double.Parse(ZDispTB.Text); }
            catch { }


            CoreDevices.Devices.XYStage xyStage = (CoreDevices.Devices.XYStage)ECore.GetDevice(TargetXYStageCB.Text);
            CoreDevices.Devices.ZStage zStage = (CoreDevices.Devices.ZStage)ECore.GetDevice(TargetZStageCB.Text);

            xyStage.GetStagePosition(out X0, out Y0);
            ECore.MMCamera.StopFocusMovie();
            if (zStage  != null) Z0 = zStage.CurrentPosition();
            long NumFrames = 6 * ZRowsi * XRowsi * YRowsi;
            long nFrames = 0;

            // Thread StageStackThread = new Thread(
            //       delegate()
            {

                for (int z = -1 * ZRowsi; z < ZRowsi; z++)
                {
                    if (zStage != null) zStage.SetPositionAbsolute(Z0 + z * ImageSizeZ);
                    for (int i = -1 * XRowsi; i < XRowsi; i++)
                    {
                        for (int j = -1 * YRowsi; j < YRowsi; j++)
                        {
                            bool ShowImage = StackShowCB.Checked;

                            if (xyStage != null) xyStage.MoveStageGuarantee(X0 + i * ImageSizeX, Y0 + ImageSizeY * j);
                            ECore.MMCamera.SnapOneFrame(ShowImage).Save(path1 + filename + "_" + i + "_" + j + "_" + z + ".tif", false);
                            //ECore.DoForcedSave(path1 + filename + "_"   + i + "_" + j + "_" + z + ".tif"); 
                            try
                            {

                                if (StackProgress.InvokeRequired)
                                {
                                    StackProgress.BeginInvoke(UpdateProgress, (int)((double)nFrames / (double)NumFrames * 100));
                                }
                                else
                                    DoUpdateProgress((int)((double)nFrames / (double)NumFrames * 100));
                            }
                            catch { }
                            nFrames++;
                            Application.DoEvents();
                        }
                    }
                }
                if (zStage != null) zStage.SetPositionAbsolute(Z0);
                if (xyStage != null) xyStage.MoveStageAbsolute(X0, Y0);
                MessageBox.Show("Stack Finished", "Stack");

                ECore.MMCamera.StartFocusMovie();
            }


            // );

            // StageStackThread.Start();
        }

        private void DoUpdateProgress(int NewValue)
        {
            StackProgress.Value = NewValue;
            // StackProgress.Update();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        
        
    }
}
