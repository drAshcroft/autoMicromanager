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
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace Micromanager_net.RControls
{
    public delegate void OnContrastChangeEvent(object sender, double     MinValue, double     MaxValue);

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
                    Graph.BeginInvoke( ShowImage , BackBuffer );
                //Graph.Image =(Image) BackBuffer;//.Clone();
                if (AutoContrastCB.Checked)
                {
                    QuietEvents = true;
                    minValue = MinData ;//(double)MinIndex /(double)data.Length ;
                    Contrast.ValueLower =(int)( minValue * Contrast.Maximum);
                    maxValue = MaxData;// (double)MaxIndex / (double)data.Length;
                    if (maxValue > 1) maxValue = 1;
                    Contrast.ValueUpper =(int)( maxValue * Contrast.Maximum);
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

    }
}
