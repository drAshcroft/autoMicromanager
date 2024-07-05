namespace Micromanager_net.UI
{
    partial class ScanningConfocal
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
            this.cBPreviewImage = new System.Windows.Forms.CheckBox();
            this.tbScanSizeY = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbScanSizeX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.GainS = new CoreDevices.DeviceControls.SlideAndTextl();
            this.ExposureS = new CoreDevices.DeviceControls.SlideAndTextl();
            this.lutGraph1 = new CoreDevices.DeviceControls.LUTGraph();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lbStages = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbCamera = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyList1 = new CoreDevices.DeviceControls.PropertyList();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(363, 600);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cBPreviewImage);
            this.tabPage1.Controls.Add(this.tbScanSizeY);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.tbScanSizeX);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.GainS);
            this.tabPage1.Controls.Add(this.ExposureS);
            this.tabPage1.Controls.Add(this.lutGraph1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(355, 574);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cBPreviewImage
            // 
            this.cBPreviewImage.AutoSize = true;
            this.cBPreviewImage.Checked = true;
            this.cBPreviewImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBPreviewImage.Location = new System.Drawing.Point(28, 423);
            this.cBPreviewImage.Name = "cBPreviewImage";
            this.cBPreviewImage.Size = new System.Drawing.Size(118, 17);
            this.cBPreviewImage.TabIndex = 21;
            this.cBPreviewImage.Text = "Do Preview Images";
            this.cBPreviewImage.UseVisualStyleBackColor = true;
            this.cBPreviewImage.CheckedChanged += new System.EventHandler(this.cBPreviewImage_CheckedChanged);
            // 
            // tbScanSizeY
            // 
            this.tbScanSizeY.Location = new System.Drawing.Point(133, 25);
            this.tbScanSizeY.Name = "tbScanSizeY";
            this.tbScanSizeY.Size = new System.Drawing.Size(74, 20);
            this.tbScanSizeY.TabIndex = 20;
            this.tbScanSizeY.Text = "140";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(130, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Scan Size Y(um)";
            // 
            // tbScanSizeX
            // 
            this.tbScanSizeX.Location = new System.Drawing.Point(6, 25);
            this.tbScanSizeX.Name = "tbScanSizeX";
            this.tbScanSizeX.Size = new System.Drawing.Size(74, 20);
            this.tbScanSizeX.TabIndex = 18;
            this.tbScanSizeX.Text = "140";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Scan Size X(um)";
            // 
            // GainS
            // 
            this.GainS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GainS.Location = new System.Drawing.Point(6, 132);
            this.GainS.LogScale = false;
            this.GainS.MaxValue = 100F;
            this.GainS.MinValue = 0F;
            this.GainS.Name = "GainS";
            this.GainS.Size = new System.Drawing.Size(340, 37);
            this.GainS.TabIndex = 16;
            this.GainS.Value = 0F;
            this.GainS.OnValueChanged += new CoreDevices.DeviceControls.OnValueChangedEvent(this.GainS_OnValueChanged_1);
            // 
            // ExposureS
            // 
            this.ExposureS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExposureS.Location = new System.Drawing.Point(6, 75);
            this.ExposureS.LogScale = false;
            this.ExposureS.MaxValue = 100F;
            this.ExposureS.MinValue = 0F;
            this.ExposureS.Name = "ExposureS";
            this.ExposureS.Size = new System.Drawing.Size(340, 38);
            this.ExposureS.TabIndex = 15;
            this.ExposureS.Value = 0F;
             this.ExposureS.OnValueChanged += new CoreDevices.DeviceControls.OnValueChangedEvent(this.ExposureS_OnValueChanged_1);
            // 
            // lutGraph1
            // 
            this.lutGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lutGraph1.AutoContrast = true;
            this.lutGraph1.Location = new System.Drawing.Point(3, 184);
            this.lutGraph1.Name = "lutGraph1";
            this.lutGraph1.Size = new System.Drawing.Size(349, 215);
            this.lutGraph1.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Contrast";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Gain";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Exposure";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.lbStages);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.lbCamera);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(355, 574);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Scanning Options";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 250);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(197, 20);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 234);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Resolution (Samples/um)";
            // 
            // lbStages
            // 
            this.lbStages.FormattingEnabled = true;
            this.lbStages.Location = new System.Drawing.Point(6, 138);
            this.lbStages.Name = "lbStages";
            this.lbStages.Size = new System.Drawing.Size(197, 82);
            this.lbStages.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "XY Stage";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Camera";
            // 
            // lbCamera
            // 
            this.lbCamera.FormattingEnabled = true;
            this.lbCamera.Location = new System.Drawing.Point(6, 36);
            this.lbCamera.Name = "lbCamera";
            this.lbCamera.Size = new System.Drawing.Size(197, 69);
            this.lbCamera.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyList1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(355, 574);
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
            this.propertyList1.Size = new System.Drawing.Size(349, 568);
            this.propertyList1.TabIndex = 0;
            // 
            // ScanningConfocal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ScanningConfocal";
            this.Size = new System.Drawing.Size(363, 600);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private CoreDevices.DeviceControls.SlideAndTextl GainS;
        private CoreDevices.DeviceControls.SlideAndTextl ExposureS;
        private CoreDevices.DeviceControls.LUTGraph lutGraph1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private CoreDevices.DeviceControls.PropertyList propertyList1;
        private System.Windows.Forms.TextBox tbScanSizeY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbScanSizeX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListBox lbStages;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox lbCamera;
        private System.Windows.Forms.CheckBox cBPreviewImage;
    }
}
