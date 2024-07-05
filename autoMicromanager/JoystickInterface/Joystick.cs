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

using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace JoystickInterface
{
    public delegate void JoystickMovedEvent(object sender,double x,double y,double Mag);
    public delegate void JoystickReleasedEvent(object sender);
    public delegate void JoystickButtonPushedEvent(object sender,bool[] ButtonStates);

    public partial class Joystick : UserControl
    {
        [DllImport("USER32.DLL")]
        static extern IntPtr GetShellWindow();

        JoystickInterface.DPJoystickInterface jst;
        public event JoystickMovedEvent OnJoystickMoved;
        public event JoystickReleasedEvent OnJoyStickReleased;
        public event JoystickButtonPushedEvent OnJoyStickButtonPushed;

        /// <summary>
        /// A procedure to get the joystick properly attached. This is for a labview environment where they just want to start the joystick and go.  
        /// </summary>
        /// <param name="SearchForPhysicalJoystick"></param>
        public void BeginJoystickAction(bool SearchForPhysicalJoystick)
        {
            if (SearchForPhysicalJoystick)
            {
                string[] Joys = this.AvailableJoysticks();
                if (Joys[0] != null && Joys[0] != "")
                    this.StartJoystickDirectPlay(Joys[0]);
            }
        }

        public string[] AvailableJoysticks()
        {
            try
            {
                
                if (jst == null)
                {
                   
                    if (this.ParentForm == null)
                        jst = new JoystickInterface.DPJoystickInterface(GetShellWindow() );
                    else 
                        jst = new JoystickInterface.DPJoystickInterface(this.ParentForm.Handle);
                    
                }

                string[] joys = jst.FindJoysticks();
                
                //convience for labview which does not like nulls returned
                if (joys == null)
                {
                    return new string[]{"",""};
                }
                
                for (int i = 0; i < joys.Length; i++)
                    if (joys[i] == null) joys[i] = "";
                

                return joys;
            }
            catch(Exception ex )
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public string JoystickName
        {
            get {
                if (jst == null)
                {

                    if (this.ParentForm == null)
                        jst = new JoystickInterface.DPJoystickInterface(GetShellWindow());
                    else
                        jst = new JoystickInterface.DPJoystickInterface(this.ParentForm.Handle);

                }
                return jst.CurrentAcquiredJoystick; }
        }
        public void StartJoystickDirectPlay(string Joystickname)
        {
            if (jst == null)
            {

                if (this.ParentForm == null)
                    jst = new JoystickInterface.DPJoystickInterface(GetShellWindow());
                else
                    jst = new JoystickInterface.DPJoystickInterface(this.ParentForm.Handle);

            }
            
            bool Acquired= jst.AcquireJoystick(Joystickname );

            tmrUpdateStick.Enabled = Acquired ;
        }
        public Joystick()
        {
            InitializeComponent();
            tmrUpdateStick.Enabled = false;
        }
        private bool MouseCaptureJoystick = false;
        private double LastX;
        private double LastY;
        private void Joystick_MouseDown(object sender, MouseEventArgs e)
        {
            MouseCaptureJoystick = true;
            timer1.Enabled=true ;
            Joystick_MouseMove(sender, e);
        }

        private void Joystick_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (MouseCaptureJoystick)
            {
               
                double x=((double)e.X-(double)this.Width/2)/((double)this.Width/2) ;
                double y=((double)e.Y-(double)this.Height/2)/((double)this.Height/2) ;
                double m=x*x+y*y;
                LastX = x;
                LastY = y;
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
            int xi = (int)(this.Width * (.5 + LastX  / 2));
            int yi = (int)(this.Height * (.5 + LastY  / 2));
            Joystick_MouseMove(this, new MouseEventArgs(MouseButtons.Left,1,xi,yi,0));
        }

        public void ForceJoyStickStart(double x, double y)
        {
            if (this.Width != 0)
            {
                int xi = (int)(this.Width * (.5 + x / 2));
                int yi = (int)(this.Height * (.5 + y / 2));
                pictureBox1.Left = (int)(xi - (pictureBox1.Width) / 2);// +this.Width / 2;
                pictureBox1.Top = (int)(yi - (pictureBox1.Height) / 2);// +this.Height / 2;
                LastX = x;
                LastY = y;
            }
            double m = x * x + y * y;
            if (OnJoystickMoved != null) OnJoystickMoved(this, x, y, m);
            
        }
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
            x = LastX;// ((double)LastX - (double)this.Width / 2) / ((double)this.Width / 2);
            y = LastY;// ((double)LastY - (double)this.Height / 2) / ((double)this.Height / 2);
            
        }
        public void GetJoyStickButtonState(out bool[] Buttons)
        {
            Buttons = JoyButtons;
        }
        private bool JoyStickDown=false ;
        private bool[] JoyButtons=null;
        private void tmrUpdateStick_Tick(object sender, EventArgs e)
        {
            // get status
            jst.UpdateStatus();

            if (Math.Abs(jst.AxisC - 32767) > 100 || Math.Abs(jst.AxisD - 32767) > 100)
            {
                JoyStickDown = true;
                double dx =2* ((double)(jst.AxisC) - 32767) / 65534;
                double dy =-2* (32767 - (double)jst.AxisD) / 65534;
              
                ForceJoyStickStart(dx, dy);

            }
            else if (JoyStickDown)
            {
                JoyStickDown = false;
                ForceJoyStickStop();
            }

            JoyButtons = jst.Buttons;
            for (int i = 0; i < JoyButtons.Length; i++)
            {
                if (JoyButtons[i] == true)
                {
                    if (OnJoyStickButtonPushed != null) OnJoyStickButtonPushed(this, JoyButtons);
                    i = JoyButtons.Length + 1;
                }

            }
            
        }






    }
}
