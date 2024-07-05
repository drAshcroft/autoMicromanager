namespace Micromanager_net
{
    partial class MMDeviceHolder
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.allDeviceHolders1 = new CoreDevices.NI_Controls.AllDeviceHolders();
            this.SuspendLayout();
            // 
            // allDeviceHolders1
            // 
            this.allDeviceHolders1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allDeviceHolders1.Location = new System.Drawing.Point(0, 0);
            this.allDeviceHolders1.Name = "allDeviceHolders1";
            this.allDeviceHolders1.Size = new System.Drawing.Size(422, 583);
            this.allDeviceHolders1.TabIndex = 0;
            // 
            // MMDeviceHolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 583);
            this.Controls.Add(this.allDeviceHolders1);
            this.Name = "MMDeviceHolder";
            this.Text = "DeviceHolders";
            this.ResumeLayout(false);

        }

        #endregion

        private CoreDevices.NI_Controls.AllDeviceHolders allDeviceHolders1;

    }
}