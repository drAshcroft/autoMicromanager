namespace Micromanager_net
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
            this.filterWheelControl1 = new Micromanager_net.FilterWheelControl();
            this.GainS = new Micromanager_net.RControls.SlideAndTextl();
            this.ExposureS = new Micromanager_net.RControls.SlideAndTextl();
            this.lutGraph1 = new Micromanager_net.RControls.LUTGraph();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.recordTools1 = new Micromanager_net.UI.RecordTools();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyList1 = new Micromanager_net.RControls.PropertyList();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(430, 891);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.filterWheelControl1);
            this.tabPage1.Controls.Add(this.GainS);
            this.tabPage1.Controls.Add(this.ExposureS);
            this.tabPage1.Controls.Add(this.lutGraph1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.panel1);
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
            // filterWheelControl1
            // 
            this.filterWheelControl1.Location = new System.Drawing.Point(5, 547);
            this.filterWheelControl1.Name = "filterWheelControl1";
            this.filterWheelControl1.Size = new System.Drawing.Size(414, 357);
            this.filterWheelControl1.TabIndex = 17;
            this.filterWheelControl1.Visible = false;
            // 
            // GainS
            // 
            this.GainS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GainS.Location = new System.Drawing.Point(9, 265);
            this.GainS.LogScale = false;
            this.GainS.MaxValue = 100F;
            this.GainS.MinValue = 0F;
            this.GainS.Name = "GainS";
            this.GainS.Size = new System.Drawing.Size(407, 37);
            this.GainS.TabIndex = 16;
            this.GainS.Value = 0F;
            this.GainS.OnValueChanged += new Micromanager_net.RControls.OnValueChangedEvent(this.GainS_OnValueChanged);
            // 
            // ExposureS
            // 
            this.ExposureS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExposureS.Location = new System.Drawing.Point(9, 208);
            this.ExposureS.LogScale = false;
            this.ExposureS.MaxValue = 100F;
            this.ExposureS.MinValue = 0F;
            this.ExposureS.Name = "ExposureS";
            this.ExposureS.Size = new System.Drawing.Size(407, 38);
            this.ExposureS.TabIndex = 15;
            this.ExposureS.Value = 0F;
           
            this.ExposureS.OnValueChanged += new Micromanager_net.RControls.OnValueChangedEvent(this.ExposureS_OnValueChanged);
            // 
            // lutGraph1
            // 
            this.lutGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lutGraph1.AutoContrast = true;
            this.lutGraph1.Location = new System.Drawing.Point(6, 326);
            this.lutGraph1.Name = "lutGraph1";
            this.lutGraph1.Size = new System.Drawing.Size(416, 215);
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
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 183);
            this.panel1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "ROIs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 249);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Gain";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Exposure";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.recordTools1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(422, 865);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Recording";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // recordTools1
            // 
            this.recordTools1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordTools1.Location = new System.Drawing.Point(3, 3);
            this.recordTools1.Name = "recordTools1";
            this.recordTools1.Size = new System.Drawing.Size(416, 859);
            this.recordTools1.TabIndex = 0;
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Micromanager_net.RControls.PropertyList propertyList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Micromanager_net.RControls.LUTGraph lutGraph1;
        private Micromanager_net.RControls.SlideAndTextl GainS;
        private Micromanager_net.RControls.SlideAndTextl ExposureS;
        private System.Windows.Forms.TabPage tabPage3;
        private Micromanager_net.UI.RecordTools recordTools1;
        private FilterWheelControl filterWheelControl1;
        
        
        
    }
}
