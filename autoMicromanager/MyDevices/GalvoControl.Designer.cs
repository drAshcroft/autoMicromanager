namespace Micromanager_net.UI
{
    partial class GalvoControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.AmplitudeTB = new System.Windows.Forms.TextBox();
            this.StopB = new System.Windows.Forms.Button();
            this.StartGenerationB = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyList1 = new Micromanager_net.DeviceControls .PropertyList();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(341, 494);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.AmplitudeTB);
            this.tabPage1.Controls.Add(this.StopB);
            this.tabPage1.Controls.Add(this.StartGenerationB);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(333, 468);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Function Generator";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Amplitude";
            // 
            // AmplitudeTB
            // 
            this.AmplitudeTB.Location = new System.Drawing.Point(18, 158);
            this.AmplitudeTB.Name = "AmplitudeTB";
            this.AmplitudeTB.Size = new System.Drawing.Size(89, 20);
            this.AmplitudeTB.TabIndex = 8;
            this.AmplitudeTB.Text = "0.08";
            this.AmplitudeTB.TextChanged += new System.EventHandler(this.AmplitudeTB_TextChanged);
            // 
            // StopB
            // 
            this.StopB.Location = new System.Drawing.Point(18, 90);
            this.StopB.Name = "StopB";
            this.StopB.Size = new System.Drawing.Size(100, 23);
            this.StopB.TabIndex = 7;
            this.StopB.Text = "Stop Generation";
            this.StopB.UseVisualStyleBackColor = true;
            this.StopB.Click += new System.EventHandler(this.StopB_Click);
            // 
            // StartGenerationB
            // 
            this.StartGenerationB.Location = new System.Drawing.Point(18, 61);
            this.StartGenerationB.Name = "StartGenerationB";
            this.StartGenerationB.Size = new System.Drawing.Size(100, 23);
            this.StartGenerationB.TabIndex = 6;
            this.StartGenerationB.Text = "Start Generation";
            this.StartGenerationB.UseVisualStyleBackColor = true;
            this.StartGenerationB.Click += new System.EventHandler(this.StartGenerationB_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Update Frequency";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyList1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(333, 468);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Properties";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // propertyList1
            // 
            this.propertyList1.AutoScroll = true;
            this.propertyList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyList1.Location = new System.Drawing.Point(3, 3);
            this.propertyList1.Name = "propertyList1";
            this.propertyList1.Size = new System.Drawing.Size(327, 462);
            this.propertyList1.TabIndex = 0;
            // 
            // FrequencyGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "FrequencyGenerator";
            this.Size = new System.Drawing.Size(341, 494);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button StopB;
        private System.Windows.Forms.Button StartGenerationB;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private Micromanager_net.DeviceControls.PropertyList propertyList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AmplitudeTB;

    }
}
