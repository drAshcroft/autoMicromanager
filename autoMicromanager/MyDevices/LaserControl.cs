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
using Emgu.CV;
using Emgu.Util;
using System.Threading;
using ZedGraph;
using CoreDevices;

namespace Micromanager_net.UI
{

    /// <summary>
    /// A class to get the frequency of light that is run through transmission grating.  Needs a lot of work
    /// </summary>
    public partial class LaserControl : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        private delegate void UpdateFreqReadoutEvent(string NewFreq);

        private event UpdateFreqReadoutEvent UpdateFreq;

        public string DeviceType() { return "Laser"; }
        public string ExtraInformation
        {
            get { return ""; }
            set { }
        }
       
        private Capture _capture;
        //private int DeltaY = 0;
        //private int Delta800 = 0;
        private double PixelSize = 40.81;// 33.433455076724378;

        //29.450754468697575
        private Point ZeroOrder = new Point(435, 439);
        private Point  FirstOrder=new Point(447,45);

        //f 447,45
        //z 435,439

        private double Frequency=0;
        private double DistToScreen = 23.3;
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
                        Image<Emgu.CV.Structure.Bgr, Byte> imaget = new Image<Emgu.CV.Structure.Bgr, Byte>(100, 100);
                        imaget.Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);

                        Image<Emgu.CV.Structure.Gray, Byte> image2 = imaget.Convert<Emgu.CV.Structure.Gray, Byte>();
                        _capture = new Capture();
                      
                     
                        ShowCaptureImage += new ShowCaptureImageEvent(FrequencyUpdate_ShowCaptureImage);
                    }
                    catch (Exception excpt)
                    {
                       // excpt.Alert(true);
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
                            Image<Emgu.CV.Structure. Bgr, Byte> frame = _capture.QueryFrame();

                            Image<Emgu.CV.Structure.Gray, Byte> grayFrame = frame.Convert<Emgu.CV.Structure.Gray, Byte>();
                            frame= frame.Flip(Emgu.CV.CvEnum.FLIP.VERTICAL );
                            
                            if (ShowCaptureImage!=null) ShowCaptureImage((Image) frame.Bitmap);
                            //pictureBox1.Image = ;
                            try
                            {
                                if (ZeroOrder.X != 0 && FirstOrder.X != 0) Frequency = CalcFrequency(frame.Bitmap,frame.Data  );
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.Print(e.Message);
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

        private double CalcFrequency(Bitmap  mImage,byte [,,] mData)
        {

            //Bitmap mImage = (Bitmap)pictureBox1.Image;
            Graphics g = pictureBox1.CreateGraphics();
            
            float[] Profile = new float[mImage.Height];
            double uY = (double)FirstOrder.Y - (double)ZeroOrder.Y;
            double uX = (double)FirstOrder.X - (double)ZeroOrder.X;

            g.DrawLine(Pens.Blue, new Point(0, FirstOrder.Y), new Point((int)pictureBox1.Width / 10, FirstOrder.Y));
            g.DrawLine(Pens.Red , new Point(0, ZeroOrder.Y), new Point((int)pictureBox1.Width / 10, ZeroOrder.Y));

            double m = (uX/uY);
            double b =   (double)FirstOrder.X-m*(double)FirstOrder.Y ;
            double MaxProfile = 0;
            for (int i = 0; i < mImage.Height ; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                 //   try
                    {
                       // System.Diagnostics.Debug.Print((m * i + b).ToString());
                        int x=(int)(m * i + b) + j;
                        int y=i;
                        float I = mData[y,x, 0];// mImage.GetPixel(x, i).GetBrightness();
                        Profile[i] += I ;
                       // Point p1=new Point ((int)(m * i + b) + j, i);
                       // Point p2 = new Point ((int)(m * 0 + b) + j, 0);
                       // g.DrawLine(Pens.Green, p1, p2);
                    }
                  //  catch { }
                    
                }
                if (Profile[i] > MaxProfile) MaxProfile = Profile[i];
            }
 
            long ISum = 0;
            int[] Profile2 = new int[Profile.Length];
            for (int i = 0; i < mImage.Height; i++)
            {
                ISum = ISum +(long) Profile[i];
                if (Profile[i] > .5 * MaxProfile )
                    Profile2[i] = 1;
                else
                    Profile2[i] = 0;

            }
            label3.Text = Math.Round((double)ISum / (double)mImage.Height * 100).ToString();

            List<long> Centers = new List<long>();
            double PeakSum = 0;
            double PeakCount = 0;
            int LastPeak = 0;
            long PeakWidth = 10;

            for (int i = 1; i < mImage.Height; i++)
            {
                if (Profile2[i] == 1 && ((i-PeakWidth)>LastPeak ) )
                {
                    PeakSum += i*Profile[i];
                    PeakCount += Profile[i];

                }
                else if (Profile2[i]==0 && PeakCount!=0)
                {
                    LastPeak = i - 1;
                    Centers.Add((long)(PeakSum / (double)PeakCount));
                    PeakSum = 0;
                    PeakCount = 0;
                }
            }
            List<long> SPeaks = Centers ;
           
            int  ZeroPeak = 1;
            double Length = 0;
            if (SPeaks.Count == 0) return 0;

            for (int i = 0; i < SPeaks.Count; i++)
            {
                if (Math.Abs(SPeaks[i] - ZeroOrder.Y) < 20) ZeroPeak = i;
            }

            long PixelDifference = 0;
            if (ZeroPeak == 0)
            {
                PixelDifference  = (SPeaks[ZeroPeak + 1] - SPeaks[ZeroPeak]);
            }
            else
            {
                double ZLength = (SPeaks[ZeroPeak ] - SPeaks[ZeroPeak-1]);
                double PLength = (SPeaks[ZeroPeak + 1] - SPeaks[ZeroPeak]);
               
                PixelDifference  =(long)( Math.Abs(ZLength + PLength) / 2);
            }
            
     

            double o = PixelDifference  / PixelSize;
            double h =Math.Sqrt( DistToScreen * DistToScreen + o * o);
            //Length = Length / PixelSize/DistToScreen ;
            double Sine= o / h;
            double wavelength =2* Sine  * 1 / GratingDistance * 1000000 * 800/813;
            //double wavelength=800*(Length * 1 / GratingDistance * 1000000);
            GraphPane myPane = zg1.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Peak Locations";
            myPane.XAxis.Title.Text = "Pixel";
            myPane.YAxis.Title.Text = "Intensity";
            
            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            myPane.CurveList.Clear();
            for (int i = 0; i < Profile2.Length ; i++)
            {
                double x = (double)i;
                double y = (double)Profile2[i];
                list.Add(x, y);
                list2.Add (x,(double)Profile[i]/MaxProfile );
            }

            // Generate a red curve with diamond symbols, and "Alpha" in the legend
            LineItem myCurve = myPane.AddCurve("Thresholded",
                list, Color.Red, SymbolType.Diamond);
            myPane.AddCurve("LineProfile", list2, Color.Blue, SymbolType.Circle);
            zg1.AxisChange();
            // Make sure the Graph gets redrawn
            zg1.Invalidate();
           
            double yp = uY * uY + ZeroOrder.Y;
            g.DrawLine(Pens.Yellow, FirstOrder, ZeroOrder);
            return (wavelength );
        }
        /*        private double CalcFrequency(Bitmap  mImage,byte [,,] mData)
        {

            //Bitmap mImage = (Bitmap)pictureBox1.Image;
            Graphics g = pictureBox1.CreateGraphics();
            
            float[] Profile = new float[mImage._Height];
            double uY = (double)FirstOrder.Y - (double)ZeroOrder.Y;
            double uX = (double)FirstOrder.X - (double)ZeroOrder.X;

            g.DrawLine(Pens.Blue, new Point(0, FirstOrder.Y), new Point((int)pictureBox1._Width / 10, FirstOrder.Y));
            g.DrawLine(Pens.Red , new Point(0, ZeroOrder.Y), new Point((int)pictureBox1._Width / 10, ZeroOrder.Y));

            double m = (uX/uY);
            double b =   (double)FirstOrder.X-m*(double)FirstOrder.Y ;
            double MaxProfile = 0;
            for (int i = 0; i < mImage._Height ; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                 //   try
                    {
                       // System.Diagnostics.Debug.Print((m * i + b).ToString());
                        int x=(int)(m * i + b) + j;
                        int y=i;
                        float I = mData[y,x, 0];// mImage.GetPixel(x, i).GetBrightness();
                        Profile[i] += I ;
                       // Point p1=new Point ((int)(m * i + b) + j, i);
                       // Point p2 = new Point ((int)(m * 0 + b) + j, 0);
                       // g.DrawLine(Pens.Green, p1, p2);
                    }
                  //  catch { }
                    
                }
                if (Profile[i] > MaxProfile) MaxProfile = Profile[i];
            }
 
            long ISum = 0;
            int[] Profile2 = new int[Profile.Length];
            for (int i = 0; i < mImage._Height; i++)
            {
                ISum = ISum +(long) Profile[i];
                if (Profile[i] > .25 * MaxProfile )
                    Profile2[i] = 1;
                else
                    Profile2[i] = 0;

            }
            label3.Text = Math.Round((double)ISum / (double)mImage._Height * 100).ToString();

            List< long> SPeaks = new List<long>();
            List<long> EPeaks = new List<long>();
            long LastPeak = -100;
            long StartPeak=-100;
            long PeakWidth = 10;
            for (int i = 1; i < mImage._Height; i++)
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

            double o = Length / PixelSize;
            double h =Math.Sqrt( DistToScreen * DistToScreen + o * o);
            //Length = Length / PixelSize/DistToScreen ;
            double Sine= o / h;
            double wavelength = Sine  * 1 / GratingDistance * 1000000;
            //double wavelength=800*(Length * 1 / GratingDistance * 1000000);
            GraphPane myPane = zg1.GraphPane;

            // Set the titles and axis labels
            myPane.Title.Text = "Peak Locations";
            myPane.XAxis.Title.Text = "Pixel";
            myPane.YAxis.Title.Text = "Intensity";
            
            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            PointPairList list2 = new PointPairList();
            myPane.CurveList.Clear();
            for (int i = 0; i < Profile2.Length ; i++)
            {
                double x = (double)i;
                double y = (double)Profile2[i];
                list.Add(x, y);
                list2.Add (x,(double)Profile[i]/MaxProfile );
            }

            // Generate a red curve with diamond symbols, and "Alpha" in the legend
            LineItem myCurve = myPane.AddCurve("Thresholded",
                list, Color.Red, SymbolType.Diamond);
            myPane.AddCurve("LineProfile", list2, Color.Blue, SymbolType.Circle);
            zg1.AxisChange();
            // Make sure the Graph gets redrawn
            zg1.Invalidate();
           
            double yp = uY * uY + ZeroOrder.Y;
            g.DrawLine(Pens.Yellow, FirstOrder, ZeroOrder);
            return (wavelength );
        }*/
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
        public void SetCore(EasyCore Ecore, string DeviceName)
        {
            System.Diagnostics.Debug.Print(DeviceName);
        }







    }
}
