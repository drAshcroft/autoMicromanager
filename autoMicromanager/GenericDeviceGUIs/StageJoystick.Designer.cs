namespace Micromanager_net.UI
{
    partial class StageJoystick
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.joystick1 = new JoystickInterface.Joystick();
            this.SuspendLayout();
            // 
            // joystick1
            // 
            this.joystick1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.joystick1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.joystick1.Location = new System.Drawing.Point(0, 0);
            this.joystick1.Name = "joystick1";
            this.joystick1.Size = new System.Drawing.Size(438, 388);
            this.joystick1.TabIndex = 2;
            this.joystick1.OnJoystickMoved += new JoystickInterface.JoystickMovedEvent(this.joystick1_OnJoystickMoved_1);
            this.joystick1.OnJoyStickButtonPushed += new JoystickInterface.JoystickButtonPushedEvent(this.joystick1_OnJoyStickButtonPushed);
            this.joystick1.OnJoyStickReleased += new JoystickInterface.JoystickReleasedEvent(this.joystick1_OnJoyStickReleased);
            // 
            // StageJoystick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 388);
            this.Controls.Add(this.joystick1);
            this.Name = "StageJoystick";
           
            this.Text = "Joystick";
            this.ResumeLayout(false);

        }

        #endregion

        private JoystickInterface.Joystick joystick1;
    }
}
