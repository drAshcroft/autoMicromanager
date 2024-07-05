using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Micromanager_net.UI
{
    public partial class StageProperties : UserControl, UI.GUIDeviceControl 
    {
        private delegate void UpDateProgressEvent(int NewValue);
        private UpDateProgressEvent UpdateProgress;
        CoreDevices.EasyCore ECore;
        CoreDevices.XYStage xyStage;

        public string DeviceType() { return "XYStage"; }
        
        public StageProperties()
        {
            InitializeComponent();
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
        public void SetCore(CoreDevices.EasyCore Ecore,string DeviceName)
        {
            try
            {
                if (DeviceName != "")
                {
                    xyStage   = (CoreDevices.XYStage  )Ecore.GetDevice(DeviceName);
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
                XDispTB.Text = ECore.ScreenSize.ToString();
                YDispTB.Text = ECore.ScreenSize.ToString();
                ZDispTB.Text = "1.0";
            }
        }

        
        private int  LocationCount=0;
        public void LoadLocationList()
        {
            dataGridView1.Rows.Clear();
            foreach (CoreDevices.XYStage.XYZlocation z in xyStage.Stops)
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
            xyStage.Stops.Add(new CoreDevices.XYStage.XYZlocation(lname,x,y,z));
        }

        private void button1_Click(object sender, EventArgs e)
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



            xyStage.GetStagePosition(out X0, out Y0);
            ECore.MMCamera.StopFocusMovie();
            if (ECore.MMFocusStage != null) Z0 = ECore.MMFocusStage.CurrentPosition();
            long NumFrames =6* ZRowsi * XRowsi * YRowsi;
            long nFrames = 0;
            
           // Thread StageStackThread = new Thread(
             //       delegate()
                    {

                        for (int z=-1*ZRowsi;z<ZRowsi ;z++)
                        {
                            if (ECore.MMFocusStage != null) ECore.MMFocusStage.SetPositionAbsolute(Z0+z* ImageSizeZ );
                            for (int i = -1*XRowsi ; i < XRowsi; i++)
                            {                   
                                for (int j = -1*YRowsi ; j < YRowsi; j++)
                                {
                                    bool ShowImage = StackShowCB.Checked;

                                    if (xyStage !=null) xyStage.MoveStageGuarantee (X0 + i * ImageSizeX, Y0 + ImageSizeY * j);
                                    ECore.MMCamera.SnapOneFrame(ShowImage  ).Save(path1 + filename + "_"   + i + "_" + j + "_" + z + ".tif",false );
                                    //ECore.DoForcedSave(path1 + filename + "_"   + i + "_" + j + "_" + z + ".tif"); 
                                    try
                                    {

                                        if (StackProgress.InvokeRequired)
                                        {
                                            StackProgress.BeginInvoke(UpdateProgress, (int) ((double)nFrames / (double)NumFrames * 100));
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
                        if (ECore.MMFocusStage != null) ECore.MMFocusStage.SetPositionAbsolute(Z0);
                        if (xyStage!=null) xyStage.MoveStageAbsolute(X0, Y0);
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

        private void StageProperties_Resize(object sender, EventArgs e)
        {
            
        }


    }
}
