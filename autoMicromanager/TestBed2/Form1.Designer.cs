namespace TestBed2
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.niEasyCore1 = new CoreDevices.NI_Controls.NIEasyCore();
            this.lutGraph1 = new CoreDevices.DeviceControls.LUTGraph();
            this.pictureBoard1 = new CoreDevices.NI_Controls.PictureBoard();
            this.allDeviceHolders1 = new CoreDevices.NI_Controls.AllDeviceHolders();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1079, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 92);
            this.button1.TabIndex = 9;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(536, 445);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "label1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(933, 50);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 63);
            this.button2.TabIndex = 14;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // niEasyCore1
            // 
            this.niEasyCore1.Guis = null;
            this.niEasyCore1.Location = new System.Drawing.Point(807, 26);
            this.niEasyCore1.Name = "niEasyCore1";
            this.niEasyCore1.Size = new System.Drawing.Size(72, 68);
            this.niEasyCore1.TabIndex = 13;
            // 
            // lutGraph1
            // 
            this.lutGraph1.AutoContrast = true;
            this.lutGraph1.Location = new System.Drawing.Point(796, 120);
            this.lutGraph1.Name = "lutGraph1";
            this.lutGraph1.Size = new System.Drawing.Size(457, 401);
            this.lutGraph1.TabIndex = 10;
            // 
            // pictureBoard1
            // 
            this.pictureBoard1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pictureBoard1.Location = new System.Drawing.Point(24, 22);
            this.pictureBoard1.Name = "pictureBoard1";
            this.pictureBoard1.Size = new System.Drawing.Size(440, 391);
            this.pictureBoard1.TabIndex = 16;
            // 
            // allDeviceHolders1
            // 
            this.allDeviceHolders1.Location = new System.Drawing.Point(539, 40);
            this.allDeviceHolders1.Name = "allDeviceHolders1";
            this.allDeviceHolders1.Size = new System.Drawing.Size(277, 405);
            this.allDeviceHolders1.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 523);
            this.Controls.Add(this.allDeviceHolders1);
            this.Controls.Add(this.pictureBoard1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.niEasyCore1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lutGraph1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private CoreDevices.DeviceControls.LUTGraph lutGraph1;
        private System.Windows.Forms.Label label1;
        private CoreDevices.NI_Controls.NIEasyCore niEasyCore1;
        private System.Windows.Forms.Button button2;
        private CoreDevices.NI_Controls.PictureBoard pictureBoard1;
        private CoreDevices.NI_Controls.AllDeviceHolders allDeviceHolders1;
      
    }
}

