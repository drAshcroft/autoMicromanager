namespace Micromanager_net.UI
{
    partial class CameraProperties
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ROIH = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ROIW = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ROIY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ROIX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BSaveSpecialImage = new System.Windows.Forms.Button();
            this.nUDNumFrames = new System.Windows.Forms.NumericUpDown();
            this.AverageMacro = new System.Windows.Forms.Button();
            this.pBBackgroundImage = new System.Windows.Forms.PictureBox();
            this.cbBackgroundSubtract = new System.Windows.Forms.CheckBox();
            this.MovingBackground = new System.Windows.Forms.Button();
            this.DoBackgroundImage = new System.Windows.Forms.Button();
            this.filterWheelControl1 = new Micromanager_net.UI.FilterWheelControl();
            this.GainS = new CoreDevices.DeviceControls.SlideAndTextl();
            this.ExposureS = new CoreDevices.DeviceControls.SlideAndTextl();
            this.lutGraph1 = new CoreDevices.DeviceControls.LUTGraph();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyList1 = new CoreDevices.DeviceControls.PropertyList();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDNumFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBBackgroundImage)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(430, 891);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.ROIH);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.ROIW);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.ROIY);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.ROIX);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.BSaveSpecialImage);
            this.tabPage1.Controls.Add(this.nUDNumFrames);
            this.tabPage1.Controls.Add(this.AverageMacro);
            this.tabPage1.Controls.Add(this.pBBackgroundImage);
            this.tabPage1.Controls.Add(this.cbBackgroundSubtract);
            this.tabPage1.Controls.Add(this.MovingBackground);
            this.tabPage1.Controls.Add(this.DoBackgroundImage);
            this.tabPage1.Controls.Add(this.filterWheelControl1);
            this.tabPage1.Controls.Add(this.GainS);
            this.tabPage1.Controls.Add(this.ExposureS);
            this.tabPage1.Controls.Add(this.lutGraph1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(422, 865);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(267, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 35;
            this.button1.Text = "Set ROI";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(17, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 16);
            this.label9.TabIndex = 34;
            this.label9.Text = "Camera ROI";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(299, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Height";
            // 
            // ROIH
            // 
            this.ROIH.Location = new System.Drawing.Point(343, 30);
            this.ROIH.Name = "ROIH";
            this.ROIH.Size = new System.Drawing.Size(41, 20);
            this.ROIH.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(199, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Width";
            // 
            // ROIW
            // 
            this.ROIW.Location = new System.Drawing.Point(240, 30);
            this.ROIW.Name = "ROIW";
            this.ROIW.Size = new System.Drawing.Size(41, 20);
            this.ROIW.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(109, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Top";
            // 
            // ROIY
            // 
            this.ROIY.Location = new System.Drawing.Point(141, 30);
            this.ROIY.Name = "ROIY";
            this.ROIY.Size = new System.Drawing.Size(41, 20);
            this.ROIY.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Left";
            // 
            // ROIX
            // 
            this.ROIX.Location = new System.Drawing.Point(55, 30);
            this.ROIX.Name = "ROIX";
            this.ROIX.Size = new System.Drawing.Size(41, 20);
            this.ROIX.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 603);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Frames";
            // 
            // BSaveSpecialImage
            // 
            this.BSaveSpecialImage.Location = new System.Drawing.Point(341, 612);
            this.BSaveSpecialImage.Name = "BSaveSpecialImage";
            this.BSaveSpecialImage.Size = new System.Drawing.Size(57, 27);
            this.BSaveSpecialImage.TabIndex = 24;
            this.BSaveSpecialImage.Text = "Save";
            this.BSaveSpecialImage.UseVisualStyleBackColor = true;
            this.BSaveSpecialImage.Click += new System.EventHandler(this.BSaveSpecialImage_Click);
            // 
            // nUDNumFrames
            // 
            this.nUDNumFrames.Location = new System.Drawing.Point(9, 619);
            this.nUDNumFrames.Name = "nUDNumFrames";
            this.nUDNumFrames.Size = new System.Drawing.Size(78, 20);
            this.nUDNumFrames.TabIndex = 23;
            this.nUDNumFrames.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // AverageMacro
            // 
            this.AverageMacro.Location = new System.Drawing.Point(9, 552);
            this.AverageMacro.Name = "AverageMacro";
            this.AverageMacro.Size = new System.Drawing.Size(88, 43);
            this.AverageMacro.TabIndex = 22;
            this.AverageMacro.Text = "Create Average Image";
            this.AverageMacro.UseVisualStyleBackColor = true;
            this.AverageMacro.Click += new System.EventHandler(this.AverageMacro_Click);
            // 
            // pBBackgroundImage
            // 
            this.pBBackgroundImage.Location = new System.Drawing.Point(234, 458);
            this.pBBackgroundImage.Name = "pBBackgroundImage";
            this.pBBackgroundImage.Size = new System.Drawing.Size(164, 148);
            this.pBBackgroundImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBBackgroundImage.TabIndex = 21;
            this.pBBackgroundImage.TabStop = false;
            // 
            // cbBackgroundSubtract
            // 
            this.cbBackgroundSubtract.AutoSize = true;
            this.cbBackgroundSubtract.Location = new System.Drawing.Point(8, 517);
            this.cbBackgroundSubtract.Name = "cbBackgroundSubtract";
            this.cbBackgroundSubtract.Size = new System.Drawing.Size(127, 17);
            this.cbBackgroundSubtract.TabIndex = 20;
            this.cbBackgroundSubtract.Text = "Subtract Background";
            this.cbBackgroundSubtract.UseVisualStyleBackColor = true;
            this.cbBackgroundSubtract.CheckedChanged += new System.EventHandler(this.cbBackgroundSubtract_CheckedChanged);
            // 
            // MovingBackground
            // 
            this.MovingBackground.Location = new System.Drawing.Point(102, 458);
            this.MovingBackground.Name = "MovingBackground";
            this.MovingBackground.Size = new System.Drawing.Size(89, 53);
            this.MovingBackground.TabIndex = 19;
            this.MovingBackground.Text = "Moving Background Image";
            this.MovingBackground.UseVisualStyleBackColor = true;
            this.MovingBackground.Click += new System.EventHandler(this.MovingBackground_Click);
            // 
            // DoBackgroundImage
            // 
            this.DoBackgroundImage.Location = new System.Drawing.Point(8, 458);
            this.DoBackgroundImage.Name = "DoBackgroundImage";
            this.DoBackgroundImage.Size = new System.Drawing.Size(88, 53);
            this.DoBackgroundImage.TabIndex = 18;
            this.DoBackgroundImage.Text = "Collect Background Image";
            this.DoBackgroundImage.UseVisualStyleBackColor = true;
            this.DoBackgroundImage.Click += new System.EventHandler(this.DoBackgroundImage_Click);
            // 
            // filterWheelControl1
            // 
            this.filterWheelControl1.ExtraInformation = "";
            this.filterWheelControl1.Location = new System.Drawing.Point(5, 823);
            this.filterWheelControl1.Name = "filterWheelControl1";
            this.filterWheelControl1.Size = new System.Drawing.Size(414, 81);
            this.filterWheelControl1.TabIndex = 17;
            this.filterWheelControl1.Visible = false;
            // 
            // GainS
            // 
            this.GainS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GainS.Location = new System.Drawing.Point(8, 157);
            this.GainS.LogScale = false;
            this.GainS.MaxValue = 100F;
            this.GainS.MinValue = 0F;
            this.GainS.Name = "GainS";
            this.GainS.Size = new System.Drawing.Size(384, 37);
            this.GainS.TabIndex = 16;
            this.GainS.Value = 0F;
            this.GainS.OnValueChanged += new CoreDevices.DeviceControls.OnValueChangedEvent(this.GainS_OnValueChanged);
            // 
            // ExposureS
            // 
            this.ExposureS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExposureS.Location = new System.Drawing.Point(8, 100);
            this.ExposureS.LogScale = false;
            this.ExposureS.MaxValue = 100F;
            this.ExposureS.MinValue = 0F;
            this.ExposureS.Name = "ExposureS";
            this.ExposureS.Size = new System.Drawing.Size(384, 38);
            this.ExposureS.TabIndex = 15;
            this.ExposureS.Value = 0F;
            
            this.ExposureS.OnValueChanged += new CoreDevices.DeviceControls.OnValueChangedEvent(this.ExposureS_OnValueChanged);
            // 
            // lutGraph1
            // 
            this.lutGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lutGraph1.AutoContrast = true;
            this.lutGraph1.Location = new System.Drawing.Point(3, 229);
            this.lutGraph1.Name = "lutGraph1";
            this.lutGraph1.Size = new System.Drawing.Size(390, 215);
            this.lutGraph1.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Contrast";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Gain";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Exposure";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyList1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(422, 865);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // propertyList1
            // 
            this.propertyList1.AutoScroll = true;
            this.propertyList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyList1.Location = new System.Drawing.Point(3, 3);
            this.propertyList1.Name = "propertyList1";
            this.propertyList1.Size = new System.Drawing.Size(416, 859);
            this.propertyList1.TabIndex = 0;
            // 
            // CameraProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CameraProperties";
            this.Size = new System.Drawing.Size(430, 891);
            this.Resize += new System.EventHandler(this.CameraProperties_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDNumFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBBackgroundImage)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private CoreDevices.DeviceControls.PropertyList propertyList1;
        private System.Windows.Forms.Label label4;
        private CoreDevices.DeviceControls.LUTGraph lutGraph1;
        private CoreDevices.DeviceControls.SlideAndTextl GainS;
        private CoreDevices.DeviceControls.SlideAndTextl ExposureS;
        private System.Windows.Forms.CheckBox cbBackgroundSubtract;
        private System.Windows.Forms.Button MovingBackground;
        private System.Windows.Forms.Button DoBackgroundImage;
        private FilterWheelControl filterWheelControl1;
        private System.Windows.Forms.PictureBox pBBackgroundImage;
        private System.Windows.Forms.NumericUpDown nUDNumFrames;
        private System.Windows.Forms.Button AverageMacro;
        private System.Windows.Forms.Button BSaveSpecialImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ROIH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ROIW;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ROIY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ROIX;
        
        
        
    }
}
