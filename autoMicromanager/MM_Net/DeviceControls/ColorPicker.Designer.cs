namespace CoreDevices.DeviceControls
{
    partial class ColorPicker
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
            this.tabColor = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SaTIntensity = new DeviceControls.SlideAndTextl();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.WavelengthShower = new System.Windows.Forms.PictureBox();
            this.SaTWavelength = new DeviceControls.SlideAndTextl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.colorAreaAndSliderUserControl1 = new DeviceControls.ColorAreaAndSliderUserControl();
            this.tabColor.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WavelengthShower)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabColor
            // 
            this.tabColor.Controls.Add(this.tabPage1);
            this.tabColor.Controls.Add(this.tabPage2);
            this.tabColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabColor.Location = new System.Drawing.Point(0, 0);
            this.tabColor.Name = "tabColor";
            this.tabColor.SelectedIndex = 0;
            this.tabColor.Size = new System.Drawing.Size(326, 315);
            this.tabColor.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SaTIntensity);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.WavelengthShower);
            this.tabPage1.Controls.Add(this.SaTWavelength);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(318, 289);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Wavelength";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SaTIntensity
            // 
            this.SaTIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SaTIntensity.Location = new System.Drawing.Point(21, 197);
            this.SaTIntensity.LogScale = false;
            this.SaTIntensity.MaxValue = 255F;
            this.SaTIntensity.MinValue = 0F;
            this.SaTIntensity.Name = "SaTIntensity";
            this.SaTIntensity.Size = new System.Drawing.Size(279, 33);
            this.SaTIntensity.TabIndex = 5;
            this.SaTIntensity.Value = 255F;
            this.SaTIntensity.OnValueChanged += new DeviceControls.OnValueChangedEvent(this.SaTIntensity_OnValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Intensity";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Wavelength";
            // 
            // WavelengthShower
            // 
            this.WavelengthShower.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.WavelengthShower.Location = new System.Drawing.Point(51, 18);
            this.WavelengthShower.Name = "WavelengthShower";
            this.WavelengthShower.Size = new System.Drawing.Size(212, 92);
            this.WavelengthShower.TabIndex = 2;
            this.WavelengthShower.TabStop = false;
            // 
            // SaTWavelength
            // 
            this.SaTWavelength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SaTWavelength.Location = new System.Drawing.Point(21, 134);
            this.SaTWavelength.LogScale = false;
            this.SaTWavelength.MaxValue = 780F;
            this.SaTWavelength.MinValue = 350F;
            this.SaTWavelength.Name = "SaTWavelength";
            this.SaTWavelength.Size = new System.Drawing.Size(279, 33);
            this.SaTWavelength.TabIndex = 1;
            this.SaTWavelength.Value = 399.45F;
            this.SaTWavelength.OnValueChanged += new DeviceControls.OnValueChangedEvent(this.slideAndTextl1_OnValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.colorAreaAndSliderUserControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(318, 289);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Color Picker";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // colorAreaAndSliderUserControl1
            // 
            this.colorAreaAndSliderUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorAreaAndSliderUserControl1.Location = new System.Drawing.Point(3, 3);
            this.colorAreaAndSliderUserControl1.Name = "colorAreaAndSliderUserControl1";
            this.colorAreaAndSliderUserControl1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorAreaAndSliderUserControl1.Size = new System.Drawing.Size(312, 283);
            this.colorAreaAndSliderUserControl1.TabIndex = 0;
            this.colorAreaAndSliderUserControl1.ColorChanged += new System.EventHandler(this.colorAreaAndSliderUserControl1_ColorChanged);
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabColor);
            this.Name = "ColorPicker";
            this.Size = new System.Drawing.Size(326, 315);
            this.tabColor.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WavelengthShower)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabColor;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox WavelengthShower;
        private DeviceControls.SlideAndTextl SaTWavelength;
        private System.Windows.Forms.Label label1;
        private DeviceControls.SlideAndTextl SaTIntensity;
        private System.Windows.Forms.Label label2;
        private DeviceControls.ColorAreaAndSliderUserControl colorAreaAndSliderUserControl1;
    }
}
