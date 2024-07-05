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
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System;

namespace CoreDevices.DeviceControls
{
    public delegate void OnContrastChangeEvent(object sender, double     MinValue, double     MaxValue);

    [Serializable]
    public partial class LUTGraph : UserControl
    {
        private delegate void ShowImageD(Bitmap  BackBuffer);
        private ShowImageD ShowImage;
        public double  minValue = 0;
        public double  maxValue = .5 ;
        //public double  MaxPossible=1;
        public OnContrastChangeEvent OnContrastChange;
        private bool QuietEvents = false;
        public bool AutoContrast
        {
            get { return (AutoContrastCB.Checked); }
            set { AutoContrastCB.Checked = value; }

        }
        Bitmap BackBuffer = null;
        public LUTGraph()
        {
            InitializeComponent();
            Contrast.ValueLower = (int)( Contrast.Maximum * minValue );
            Contrast.ValueUpper =(int)( Contrast.Maximum* maxValue) ;
            ShowImage += DoShowImage;
            BackBuffer = new Bitmap(Graph.Width,Graph.Height,PixelFormat.Format24bppRgb  );
        }
        private void DoShowImage(Bitmap  BackBuffer)
        {
            Graph.Image =(Image) BackBuffer;
            Application.DoEvents();
        }

        public CoreImage ProcessImage(Devices.Camera camera, CoreImage cImage)
        {
            ProcessImage(cImage);
            
            double length = maxValue - minValue;

            camera.SetContrast(minValue + length * .1, minValue + length * .9);
            return cImage;
        }
        public CoreImage  ProcessImage(CoreImage cImage)
        {
            if (cImage != null)
            {
                int[] Values;
                long MaxValue;
                long MinValue;
                long MaxPossible;
                cImage.GetHistogram(out Values, out MaxValue, out MinValue, out MaxPossible);

                double dMaxValue;
                double dMinValue;
                dMaxValue = (double)MaxValue / (double)MaxPossible;
                dMinValue = (double)MinValue / (double)MaxPossible;
                try
                {
                    ShowGraph(Values, dMaxValue, dMinValue);
                }
                catch { }
                if (!AutoContrast)
                {
                    cImage.MaxContrast = (long)(maxValue * MaxPossible);
                    cImage.MinContrast = (long)(minValue * MaxPossible);
                }
                else
                {
                    cImage.MaxContrast = (long)(MaxValue );
                    cImage.MinContrast = (long)(MinValue );
                }
            }
            return cImage;

        }
        public void ShowGraph(int[] data,double  MaxData,double  MinData)
        {
           
            int MinIndex = -1;
            int MaxIndex = -1;
            int MaxColor = 0;
            int MinColor = 100000;
           // double BinSize = (double)MaxPossible / (double)data.Length;
            for (int i=0;i<data.Length ;i++)
            {
                if (data[i] > MaxColor) MaxColor = data[i];
                if (data[i] < MinColor) MinColor = data[i];
                if (data[i] != 0) MaxIndex = i;
                if ((data[i]!=0) && (MinIndex ==-1)) MinIndex =i;
            }
            if (MinIndex == -1) MinIndex = 0;
            if (MaxIndex == -1) MaxIndex = data.Length;

            float cX = ((float)this.Width) /((float) data.Length);
            float GraphBottom = this.Height - Contrast.Height - 5;
            
            if (MaxData != MinData)
            {
                float cY = (GraphBottom ) / ((float)(MaxColor - MinColor));
                //Graphics g = Graph.CreateGraphics();
                //Image b = new Image();
                
                Graphics g = Graphics.FromImage(BackBuffer );
              
                g.Clear(Color.White);
                int h =(int) GraphBottom ;
                Point p1 = new Point(0, h);
                int stepSize = data.Length / 256;
                if (stepSize <= 0) stepSize = 1;
                try
                {
                    for (int i = 1; i < data.Length; i += stepSize)
                    {
                        Point p2 = new Point((int)(i * cX), (int)(h - (data[i] - MinData) * cY));
                        g.DrawLine(Pens.Black, p1, p2);
                        p1 = p2;


                    }
                }
                catch { }
                if (Graph.InvokeRequired )
                    Graph.Invoke( ShowImage , BackBuffer );
                else
                    Graph.Image = (Image)BackBuffer;

                if (AutoContrastCB.Checked)
                {
                    QuietEvents = true;
                    minValue = MinData ;
                    Contrast.ValueLower =(int)( minValue * Contrast.Maximum);
                    maxValue = MaxData;
                    if (maxValue > 1) maxValue = 1;
                    Contrast.ValueUpper =(int)( maxValue * Contrast.Maximum);
                    minValue = MinData;
                    maxValue = MaxData;
                    QuietEvents = false;
                }
            }

        }

        public void ShowGraph(int[] data, int MaxY, int MinY, double MaxData, double MinData)
        {

            int MinIndex = -1;
            int MaxIndex = -1;
            int MaxColor = MaxY ;
            int MinColor = MinY ;
            // double BinSize = (double)MaxPossible / (double)data.Length;
            for (int i = 0; i < data.Length; i++)
            {
                //if (data[i] > MaxColor) MaxColor = data[i];
                //if (data[i] < MinColor) MinColor = data[i];
                if (data[i] != 0) MaxIndex = i;
                if ((data[i] != 0) && (MinIndex == -1)) MinIndex = i;
            }
            if (MinIndex == -1) MinIndex = 0;
            if (MaxIndex == -1) MaxIndex = data.Length;

            float cX = ((float)this.Width) / ((float)data.Length);
            float GraphBottom = this.Height - Contrast.Height - 5;

            if (MaxData != MinData)
            {
                float cY = (GraphBottom) / ((float)(MaxColor - MinColor));
                //Graphics g = Graph.CreateGraphics();
                //Image b = new Image();

                Graphics g = Graphics.FromImage(BackBuffer);

                g.Clear(Color.White);
                int h = (int)GraphBottom;
                Point p1 = new Point(0, h);
                int stepSize = data.Length / 256;
                if (stepSize <= 0) stepSize = 1;
                try
                {
                    for (int i = 1; i < data.Length; i += stepSize)
                    {
                        Point p2 = new Point((int)(i * cX), (int)(h - (data[i] - MinData) * cY));
                        g.DrawLine(Pens.Black, p1, p2);
                        p1 = p2;


                    }
                }
                catch { }
                if (Graph.InvokeRequired)
                    Graph.Invoke(ShowImage, BackBuffer);
                else
                    Graph.Image = (Image)BackBuffer;

                if (AutoContrastCB.Checked)
                {
                    QuietEvents = true;
                    minValue = MinData;
                    Contrast.ValueLower = (int)(minValue * Contrast.Maximum);
                    maxValue = MaxData;
                    if (maxValue > 1) maxValue = 1;
                    Contrast.ValueUpper = (int)(maxValue * Contrast.Maximum);
                    minValue = MinData;
                    maxValue = MaxData;
                    QuietEvents = false;
                }
            }

        }
       // delegate void ShowImage(Image BackBuffer);
                        
        private void Graph_Resize(object sender, EventArgs e)
        {
            Contrast.Left = 1;
            Contrast.Width = this.Width - 3;
            Contrast.Top = this.Height - Contrast.Height;
            AutoContrastCB.Left = this.Width - AutoContrastCB.Width;
            AutoContrastCB.Top = this.Height -Contrast.Height -AutoContrastCB.Height -2;
            try
            {
                BackBuffer = new Bitmap(Graph.Width, Graph.Height, PixelFormat.Format24bppRgb);
            }
            catch { }
        }

        private void Contrast_ValueChanged(object sender, EventArgs e)
        {
            if (!QuietEvents)
            {
                minValue = (float)Contrast.ValueLower / (float)Contrast.Maximum;
                maxValue = (float)Contrast.ValueUpper / (float)Contrast.Maximum;

                if (OnContrastChange != null) OnContrastChange(this, minValue, maxValue);
            }
        }

        /// <summary>
        /// This allows the LUT to automatically connect to one certain camera device.  There is no other notification and contrasting action is performed before
        /// the imageprocessing stack
        /// </summary>
        /// <param name="TargetCamera"></param>
        public void AttachLUTToCamera(EasyCore EasyCore, string CameraName)
        {
            try
            {
                AttachLUTToCamera((CoreDevices.Devices.Camera)EasyCore.GetDevice(CameraName));
            }
            catch(Exception ex)
            {
                throw new Exception("Camera name probably not correct.\n" + ex.Message);
            }
        }
        /// <summary>
        /// This allows the LUT to automatically connect to one certain camera device.  There is no other notification and contrasting action is performed before
        /// the imageprocessing stack
        /// </summary>
        /// <param name="TargetCamera"></param>
        public void AttachLUTToCamera(CoreDevices.Devices.Camera TargetCamera)
        {
            TargetCamera.OnHistogram += new CoreDevices.Devices.OnHistogramMadeEvent(TargetCamera_OnHistogram);
        }

        /// <summary>
        /// for those situations where the LUT graph does not need a monitor.  This will just perform all the functions of contrast and brightness automatically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Values"></param>
        /// <param name="MaxValue"></param>
        /// <param name="MinValue"></param>
        /// <param name="MaxPossible"></param>
        /// <param name="CI"></param>
        void TargetCamera_OnHistogram(object sender, int[] Values, double MaxValue, double MinValue, long MaxPossible, CoreImage CI)
        {
            try
            {
                ShowGraph(Values, MaxValue, MinValue);
            }
            catch { }
            //if (lutGraph1.AutoContrast )  core.MMCamera.SetContrast(MinValue, MaxValue);
            CoreDevices.Devices.Camera c = (CoreDevices.Devices.Camera)sender;
            double length = maxValue - minValue;

            c.SetContrast(minValue + length * .1, minValue + length * .9);
            if (!AutoContrast)
            {
                CI.MaxContrast = (long)(maxValue * MaxPossible);
                CI.MinContrast = (long)(minValue * MaxPossible);
            }
        }
    }
}
