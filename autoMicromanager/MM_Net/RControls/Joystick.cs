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
    public delegate void JoystickMovedEvent(object sender,double x,double y,double Mag);
    public delegate void JoystickReleasedEvent(object sender);

    public partial class Joystick : UserControl
    {
        public event JoystickMovedEvent OnJoystickMoved;
        public event JoystickReleasedEvent OnJoyStickReleased;
        public Joystick()
        {
            InitializeComponent();
        }
        private bool MouseCaptureJoystick = false;
        private int LastX;
        private int LastY;
        private void Joystick_MouseDown(object sender, MouseEventArgs e)
        {
            MouseCaptureJoystick = true;
            LastX = e.X;
            LastY = e.Y;
            timer1.Enabled=true ;
        }

        private void Joystick_MouseMove(object sender, MouseEventArgs e)
        {
            LastX = e.X;
            LastY = e.Y;
            if (MouseCaptureJoystick)
            {
                double x=((double)e.X-(double)this.Width/2)/((double)this.Width/2) ;
                double y=((double)e.Y-(double)this.Height/2)/((double)this.Height/2) ;
                double m=x*x+y*y;
                if (OnJoystickMoved != null) OnJoystickMoved(this, x, y, m);
                pictureBox1.Left = (int)(e.X - (pictureBox1.Width) / 2);// +this.Width / 2;
                pictureBox1.Top = (int)(e.Y - (pictureBox1.Height) / 2);// +this.Height / 2;
             
            }
        }

        private void Joystick_MouseUp(object sender, MouseEventArgs e)
        {
            MouseCaptureJoystick = false;
            pictureBox1.Left = (this.Width - pictureBox1.Height) / 2;
            pictureBox1.Top = (this.Height - pictureBox1.Height) / 2;
            timer1.Enabled = false;
            if (OnJoyStickReleased != null) OnJoyStickReleased(this);
            LastX = 0;
            LastY = 0;
        }

        private void Joystick_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = (this.Width-pictureBox1.Height ) / 2;
            pictureBox1.Top = (this.Height - pictureBox1.Height) / 2;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Joystick_MouseMove(this,new MouseEventArgs(e.Button,e.Clicks,pictureBox1.Left+e.X ,pictureBox1.Top+e.Y,e.Delta ));
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Joystick_MouseDown(this,new MouseEventArgs(e.Button,e.Clicks,pictureBox1.Left+e.X ,pictureBox1.Top+e.Y,e.Delta ));
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Joystick_MouseUp(this,new MouseEventArgs(e.Button,e.Clicks,pictureBox1.Left+e.X ,pictureBox1.Top+e.Y,e.Delta ));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Joystick_MouseMove(this, new MouseEventArgs(MouseButtons.Left,1,LastX,LastY,0));
        }

        public void ForceJoyStickStart(double x, double y)
        {
            if (this.Width != 0)
            {
                int xi = (int)(this.Width * (.5 + x / 2));
                int yi = (int)(this.Height * (.5 + y / 2));
                pictureBox1.Left = (int)(xi - (pictureBox1.Width) / 2);// +this.Width / 2;
                pictureBox1.Top = (int)(yi - (pictureBox1.Height) / 2);// +this.Height / 2;
                
            }
            double m = x * x + y * y;
            if (OnJoystickMoved != null) OnJoystickMoved(this, x, y, m);
            
        }
        //todo:  lastx and lasty should be in relative coords to make the joystick make more sense to labview
        public void ForceJoyStickStop()
        {
            pictureBox1.Left = (this.Width - pictureBox1.Height) / 2;
            pictureBox1.Top = (this.Height - pictureBox1.Height) / 2;
            if (OnJoyStickReleased != null) OnJoyStickReleased(this);
            LastX = 0;
            LastY = 0;
        }

        public void GetJoyStickCoords(out double x, out double y)
        {
            x = LastX;
            y = LastY;

        }

       

       

       
    }
}
