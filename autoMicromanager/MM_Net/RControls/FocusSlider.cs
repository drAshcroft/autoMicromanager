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

namespace Micromanager_net.RControls
{
    public partial class FocusSlider : UserControl
    {
        public int  Maximum
        {

            get { return colorSlider1.Maximum ; }
            set { colorSlider1.Maximum  = value; }
        }
        public int Value
        {
            get { return colorSlider1.Value; }
            set { 
                
                colorSlider1.Value = value;
                if (OnScroll != null) OnScroll(this, new ScrollEventArgs(ScrollEventType.Last, value));
    
            
            }
        }
        public int  Minimum
        {

            get { return colorSlider1.Minimum; }
            set { colorSlider1.Minimum = value; }
        }
        
        new public event ScrollEventHandler OnScroll;
        public FocusSlider()
        {
            InitializeComponent();
        }

        private void FocusSlider_Resize(object sender, EventArgs e)
        {
            int bWidth=(int)((double)this.Width *.75);
            int bHeight=(int)( .1*(double)this.Height );
            if (bHeight > 33) bHeight = 33;
            if (bWidth > 40) bWidth = 40;
            int w=(this.Width - bWidth) / 2;
            int h = 0;
            MoveUpLots.Top = h;
            MoveUpLots.Width = bWidth;
            MoveUpLots.Height =bHeight ;
            MoveUpLots.Left = w;

            h+=bHeight ;
            MoveUp.Top = h;
            MoveUp.Left = w;
            MoveUp.Height = bHeight;
            MoveUp.Width = bWidth;
            h += bHeight;

            int h2 = this.Height - bHeight;
            MoveDownLots.Top = h2;
            MoveDownLots.Left = w;
            MoveDownLots.Width = bWidth ;
            MoveDownLots.Height = bHeight;

            h2 -= bHeight;
            MoveDown.Top = h2;
            MoveDown.Left = w;
            MoveDown.Width = bWidth;
            MoveDown.Height = bHeight;

            
            colorSlider1.Top = h;
            colorSlider1.Left = 0;
            colorSlider1.Width = this.Width;
            colorSlider1.Height = h2 - h;

 
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
          //  Value -= (colorSlider1.Maximum - colorSlider1.Minimum) / 100; ;
        }

        private void MoveUp_Click(object sender, EventArgs e)
        {
           // Value -= 10;
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
           // Value += 9;
        }

        private void MoveDownLots_Click(object sender, EventArgs e)
        {
            //Value += (colorSlider1.Maximum-colorSlider1.Minimum )/100;
        }

        private void colorSlider1_Scroll(object sender, ScrollEventArgs e)
        {
            if (OnScroll!=null)  OnScroll(this,e);
            
        }

        private void MoveUp_MouseDown(object sender, MouseEventArgs e)
        {
            tMUp.Enabled = true;
        }

        private void MoveUp_MouseUp(object sender, MouseEventArgs e)
        {
            tMUp.Enabled = false;
        }

        private void tMUp_Tick(object sender, EventArgs e)
        {
            Value -= 2;
        }

        private void MoveUpLots_MouseDown(object sender, MouseEventArgs e)
        {
            tmUpFast.Enabled = true;
        }

        private void MoveUpLots_MouseUp(object sender, MouseEventArgs e)
        {
            tmUpFast.Enabled = false;
        }

        private void tmUpFast_Tick(object sender, EventArgs e)
        {
            Value -= 25;
        }

        private void MoveDown_MouseDown(object sender, MouseEventArgs e)
        {
            tmDown.Enabled = true;
        }

        private void MoveDown_MouseUp(object sender, MouseEventArgs e)
        {
            tmDown.Enabled = false;
        }

        private void tmDown_Tick(object sender, EventArgs e)
        {
            Value += 2;
        }

        private void MoveDownLots_MouseDown(object sender, MouseEventArgs e)
        {
            tmDownFast.Enabled = true;
        }

        private void MoveDownLots_MouseUp(object sender, MouseEventArgs e)
        {
            tmDownFast.Enabled = false;
        }

        private void tmDownFast_Tick(object sender, EventArgs e)
        {
            Value += 24;
        }

    }
}
