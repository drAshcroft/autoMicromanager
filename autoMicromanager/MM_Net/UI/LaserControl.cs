using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using System.Threading;
using ZedGraph;

namespace Micromanager_net.UI
{

    public partial class LaserControl : UserControl, UI.GUIDeviceControl 
    {
        private delegate void UpdateFreqReadoutEvent(string NewFreq);

        private event UpdateFreqReadoutEvent UpdateFreq;

        public string DeviceType() { return "Laser"; }
        

        private Capture _capture;
        //private int DeltaY = 0;
        //private int Delta800 = 0;
        private double PixelSize = 29.450754468697575;// 33.433455076724378;

        //29.450754468697575
        private Point ZeroOrder = new Point(435, 439);
        private Point  FirstOrder=new Point(447,45);

        //f 447,45
        //z 435,439

        private double Frequency=0;
        private double DistToScreen = 28.5;
        private double GratingDistance = 300;//lines per mm
        private Thread _captureThread;

        public LaserControl()
        {
            InitializeComponent();
            UpdateFreq += new UpdateFreqReadoutEvent(FrequencyUpdate_UpdateFreq);
        }

        void FrequencyUpdate_UpdateFreq(string NewFreq)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke(UpdateFreq ,NewFreq);
            }
            else 
            {
                label1.Text = NewFreq;
                label1.Refresh();
                }
            //throw new NotImplementedException();
        }
        private delegate void ShowCaptureImageEvent(Image image);
        private event ShowCaptureImageEvent ShowCaptureImage;
        private bool Running=false ;
        private void StartUpdates()
        {

            if (_captureThread == null) // if currently there is no capture thread running
            {
                if (_capture == null)
                {
                    try
                    {
                        Image<Bgr, Byte> imaget = new Image<Bgr, Byte>(100, 100);
                        imaget.Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);
                        Image<Gray, Byte> image2 = imaget.Convert<Gray, Byte>();
                        _capture = new Capture();
                        ShowCaptureImage += new ShowCaptureImageEvent(FrequencyUpdate_ShowCaptureImage);
                    }
                    catch (PrioritizedException excpt)
                    {
                        excpt.Alert(true);
                        return;
                    }
                }
                Running = !Running;
                button2.Text = "Stop";
              //  _captureThread = new Thread(
                //    delegate()
                 //   {
                        while (Running && pictureBox1!=null  )
                        {
                            Image<Bgr, Byte> frame = _capture.QueryFrame();

                            Image<Gray, Byte> grayFrame = frame.Convert<Gray, Byte>();
                            frame= frame.Flip(Emgu.CV.CvEnum.FLIP.VERTICAL );
                            if (ShowCaptureImage!=null) ShowCaptureImage((Image) frame.Bitmap);
                            //pictureBox1.Image = ;
                            try
                            {
                                if (ZeroOrder.X != 0 && FirstOrder.X != 0) Frequency = CalcFrequency(frame.Bitmap );
                            }
                            catch (Exception e)
                            {
                               // Running = false;
                            }
                            UpdateFreq( Math.Round(Frequency).ToString());
                            label1.Text = Math.Round(Frequency, 1).ToString();
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }
               //     }
                 //  );

              // _captureThread.Start();

            }
            else // if currently capturing
            {
                button2.Text = "Start";
                if (_captureThread != null)
                {
                    _captureThread.Abort();
                    _captureThread = null;
                }

               
            }
        }

        void FrequencyUpdate_ShowCaptureImage(Image image)
        {
            if (pictureBox1.InvokeRequired == true)
            {
                pictureBox1.Invoke(ShowCaptureImage, image);
            }
            else
            {
                pictureBox1.Image = image;
            }
        }

        private double CalcFrequency(Bitmap  mImage)
        {

            //Bitmap mImage = (Bitmap)pictureBox1.Image;
            Graphics g = pictureBox1.CreateGraphics();
            float[] Profile = new float[mImage.Height];
            double uY = (double)FirstOrder.Y - (double)ZeroOrder.Y;
            double uX = (double)FirstOrder.X - (double)ZeroOrder.X;

            double m = (uY/uX);
            double b = -1*(double)FirstOrder.X/m;
            for (int i = 0; i < mImage.Height ; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                    float I = mImage.GetPixel((int)(m*j+b) + j, i).GetBrightness();
                    Profile[i] += I/10;
                }
            }
 
            long ISum = 0;
            int[] Profile2 = new int[Profile.Length];
            for (int i = 0; i < mImage.Height; i++)
            {
                ISum = ISum +(long) Profile[i];
                if (Profile[i] > .75)
                    Profile2[i] = 1;
                else
                    Profile2[i] = 0;

            }
            label3.Text = Math.Round((double)ISum / (double)mImage.Height * 100).ToString();

            List< long> SPeaks = new List<long>();
            List<long> EPeaks = new List<long>();
            long LastPeak = -100;
            long StartPeak=-100;
            long PeakWidth = 10;
            for (int i = 1; i < mImage.Height; i++)
            {
                if (Profile2[i] == 1)
                {
                    if (i > (LastPeak + PeakWidth))
                    {
                        StartPeak = i;
                        LastPeak = i;
                        SPeaks.Add(i);
                    }
                    else
                    {
                        LastPeak = i;
                    }
                }
                else
                {
                    if (Profile2[i - 1] == 1 && Profile2[i] == 0)
                    {
                        EPeaks.Add(i);
                    }
                }
            }
            int  ZeroPeak = 1;
            double Length = 0;
            if (SPeaks.Count == 0) return 0;
            if ((SPeaks.Count % 2) == 0)
            {
                ZeroPeak =(int ) Math.Floor((double)SPeaks.Count / 2)-1;
                if (SPeaks.Count > 2)
                {
                    double ZLength = (EPeaks[ZeroPeak] + SPeaks[ZeroPeak]) / 2;
                    double PLength = (EPeaks[ZeroPeak + 1] + SPeaks[ZeroPeak + 1]) / 2;
                    double MLength = (EPeaks[ZeroPeak - 1] + SPeaks[ZeroPeak - 1]) / 2;
                    Length = Math.Abs(PLength - MLength) / 2;
                }
                else
                {
                    double ZLength = (EPeaks[ZeroPeak] + SPeaks[ZeroPeak]) / 2;
                    double PLength = (EPeaks[ZeroPeak + 1] + SPeaks[ZeroPeak + 1]) / 2;
                    Length = Math.Abs(ZLength - PLength);
                }

            }
            else
            {
                ZeroPeak = (int )Math.Floor((double)SPeaks.Count / 2+.5)-1;
                double ZLength = (EPeaks[ZeroPeak] + SPeaks[ZeroPeak]) / 2;
                double PLength = (EPeaks[ZeroPeak + 1] + SPeaks[ZeroPeak + 1]) / 2;
                double MLength = (EPeaks[ZeroPeak - 1] + SPeaks[ZeroPeak - 1]) / 2;
                Length = Math.Abs(PLength - MLength) / 2;
            }

            
            Length = Length / PixelSize/DistToScreen ;
            double wavelength=800*(Length * 1 / GratingDistance * 1000000)/780;
            GraphPane myPane = zg1.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Peak Locations";
            myPane.XAxis.Title.Text = "Pixel";
            myPane.YAxis.Title.Text = "Intensity";
            
            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            myPane.CurveList.Clear();
            for (int i = 0; i < Profile2.Length ; i++)
            {
                double x = (double)i;
                double y = (double)Profile2[i];
                list.Add(x, y);
            }

            // Generate a red curve with diamond symbols, and "Alpha" in the legend
            LineItem myCurve = myPane.AddCurve("Alpha",
                list, Color.Red, SymbolType.Diamond);

            zg1.AxisChange();
            // Make sure the Graph gets redrawn
            zg1.Invalidate();
           
            double yp = uY * uY + ZeroOrder.Y;
            return (wavelength );
        }

        private void ReleaseData()
        {
            if (_captureThread != null)
                _captureThread.Abort();

            if (_capture != null)
                _capture.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartUpdates();
        }
        private int SMouseX=0;
        private int SMouseY=0;
        private int EMouseX = 0;
        private int EMouseY = 0;
        
        
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //MouseCaptured = true;
            SMouseX = e.X;
            SMouseY = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                ZeroOrder = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //MouseCaptured = false;
            EMouseX = e.X;
            EMouseY = e.Y;
            if (e.Button==MouseButtons.Right )
            {
                double x = ((double)EMouseX - (double)SMouseX);
                double y=((double)EMouseY - (double)SMouseY);
                double d =Math.Sqrt(x  *x  + y*y );
                PixelSize = d / 3.5;// Pixels/CM 

            }
            else 
            {
                FirstOrder = e.Location;                
            }

        }


        public Control GetControl()
        {
            return this;
        }
        public string Caption()
        {
            return ("Laser Control");
        }
        public void SetCore(CoreDevices.EasyCore Ecore, string DeviceName)
        {
           
        }







    }
}
