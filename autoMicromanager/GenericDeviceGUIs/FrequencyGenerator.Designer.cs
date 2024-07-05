namespace Micromanager_net.UI
{
    partial class FrequencyGenerator
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ChannelPanel = new System.Windows.Forms.Panel();
            this.RemoveChannelB = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.WaveFormFilename0 = new System.Windows.Forms.TextBox();
            this.WaveformFilenameLabel0 = new System.Windows.Forms.Label();
            this.WaveformFormula0 = new System.Windows.Forms.TextBox();
            this.WaveformFormulaLabel0 = new System.Windows.Forms.Label();
            this.WaveformPhaseTB0 = new System.Windows.Forms.TextBox();
            this.WaveformPhaselabel0 = new System.Windows.Forms.Label();
            this.WaveFormTypeLabel = new System.Windows.Forms.Label();
            this.WaveFormTypeLB0 = new System.Windows.Forms.ListBox();
            this.Waveformlabel2 = new System.Windows.Forms.Label();
            this.WaveFormAmplitudeTB0 = new System.Windows.Forms.TextBox();
            this.WaveFormFreqTB0 = new System.Windows.Forms.TextBox();
            this.WaveFormlabel1 = new System.Windows.Forms.Label();
            this.AddChannelB = new System.Windows.Forms.Button();
            this.WaveformOutputTime0 = new System.Windows.Forms.TextBox();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.label1 = new System.Windows.Forms.Label();
            this.StopB = new System.Windows.Forms.Button();
            this.StartGenerationB = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyList1 = new CoreDevices.DeviceControls.PropertyList();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ChannelPanel.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(349, 679);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.ChannelPanel);
            this.tabPage1.Controls.Add(this.WaveformOutputTime0);
            this.tabPage1.Controls.Add(this.zedGraphControl1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.StopB);
            this.tabPage1.Controls.Add(this.StartGenerationB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(341, 653);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Function Generator";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ChannelPanel
            // 
            this.ChannelPanel.AutoScroll = true;
            this.ChannelPanel.Controls.Add(this.RemoveChannelB);
            this.ChannelPanel.Controls.Add(this.tabControl2);
            this.ChannelPanel.Controls.Add(this.AddChannelB);
            this.ChannelPanel.Location = new System.Drawing.Point(8, 329);
            this.ChannelPanel.Name = "ChannelPanel";
            this.ChannelPanel.Size = new System.Drawing.Size(317, 289);
            this.ChannelPanel.TabIndex = 19;
            // 
            // RemoveChannelB
            // 
            this.RemoveChannelB.Location = new System.Drawing.Point(150, 264);
            this.RemoveChannelB.Name = "RemoveChannelB";
            this.RemoveChannelB.Size = new System.Drawing.Size(110, 22);
            this.RemoveChannelB.TabIndex = 19;
            this.RemoveChannelB.Text = "Remove Channel";
            this.RemoveChannelB.UseVisualStyleBackColor = true;
            this.RemoveChannelB.Click += new System.EventHandler(this.RemoveChannelB_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(311, 255);
            this.tabControl2.TabIndex = 18;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.WaveFormFilename0);
            this.tabPage3.Controls.Add(this.WaveformFilenameLabel0);
            this.tabPage3.Controls.Add(this.WaveformFormula0);
            this.tabPage3.Controls.Add(this.WaveformFormulaLabel0);
            this.tabPage3.Controls.Add(this.WaveformPhaseTB0);
            this.tabPage3.Controls.Add(this.WaveformPhaselabel0);
            this.tabPage3.Controls.Add(this.WaveFormTypeLabel);
            this.tabPage3.Controls.Add(this.WaveFormTypeLB0);
            this.tabPage3.Controls.Add(this.Waveformlabel2);
            this.tabPage3.Controls.Add(this.WaveFormAmplitudeTB0);
            this.tabPage3.Controls.Add(this.WaveFormFreqTB0);
            this.tabPage3.Controls.Add(this.WaveFormlabel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(303, 229);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Channel 0";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // WaveFormFilename0
            // 
            this.WaveFormFilename0.Enabled = false;
            this.WaveFormFilename0.Location = new System.Drawing.Point(164, 173);
            this.WaveFormFilename0.Name = "WaveFormFilename0";
            this.WaveFormFilename0.Size = new System.Drawing.Size(89, 20);
            this.WaveFormFilename0.TabIndex = 26;
            this.WaveFormFilename0.TextChanged += new System.EventHandler(this.WaveFormFilename0_TextChanged);
            // 
            // WaveformFilenameLabel0
            // 
            this.WaveformFilenameLabel0.AutoSize = true;
            this.WaveformFilenameLabel0.Location = new System.Drawing.Point(161, 157);
            this.WaveformFilenameLabel0.Name = "WaveformFilenameLabel0";
            this.WaveformFilenameLabel0.Size = new System.Drawing.Size(49, 13);
            this.WaveformFilenameLabel0.TabIndex = 25;
            this.WaveformFilenameLabel0.Text = "Filename";
            // 
            // WaveformFormula0
            // 
            this.WaveformFormula0.Enabled = false;
            this.WaveformFormula0.Location = new System.Drawing.Point(164, 134);
            this.WaveformFormula0.Name = "WaveformFormula0";
            this.WaveformFormula0.Size = new System.Drawing.Size(89, 20);
            this.WaveformFormula0.TabIndex = 24;
            this.WaveformFormula0.TextChanged += new System.EventHandler(this.WaveformFormula0_TextChanged);
            // 
            // WaveformFormulaLabel0
            // 
            this.WaveformFormulaLabel0.AutoSize = true;
            this.WaveformFormulaLabel0.Location = new System.Drawing.Point(161, 118);
            this.WaveformFormulaLabel0.Name = "WaveformFormulaLabel0";
            this.WaveformFormulaLabel0.Size = new System.Drawing.Size(44, 13);
            this.WaveformFormulaLabel0.TabIndex = 23;
            this.WaveformFormulaLabel0.Text = "Formula";
            // 
            // WaveformPhaseTB0
            // 
            this.WaveformPhaseTB0.Location = new System.Drawing.Point(164, 95);
            this.WaveformPhaseTB0.Name = "WaveformPhaseTB0";
            this.WaveformPhaseTB0.Size = new System.Drawing.Size(89, 20);
            this.WaveformPhaseTB0.TabIndex = 22;
            this.WaveformPhaseTB0.TextChanged += new System.EventHandler(this.WaveformPhaseTB0_TextChanged);
            // 
            // WaveformPhaselabel0
            // 
            this.WaveformPhaselabel0.AutoSize = true;
            this.WaveformPhaselabel0.Location = new System.Drawing.Point(161, 79);
            this.WaveformPhaselabel0.Name = "WaveformPhaselabel0";
            this.WaveformPhaselabel0.Size = new System.Drawing.Size(37, 13);
            this.WaveformPhaselabel0.TabIndex = 21;
            this.WaveformPhaselabel0.Text = "Phase";
            // 
            // WaveFormTypeLabel
            // 
            this.WaveFormTypeLabel.AutoSize = true;
            this.WaveFormTypeLabel.Location = new System.Drawing.Point(6, 1);
            this.WaveFormTypeLabel.Name = "WaveFormTypeLabel";
            this.WaveFormTypeLabel.Size = new System.Drawing.Size(66, 13);
            this.WaveFormTypeLabel.TabIndex = 20;
            this.WaveFormTypeLabel.Text = "Output Type";
            // 
            // WaveFormTypeLB0
            // 
            this.WaveFormTypeLB0.FormattingEnabled = true;
            this.WaveFormTypeLB0.Items.AddRange(new object[] {
            "Gaussian Noise",
            "Pseudo Noise",
            "MersenneTwister Noise",
            "Sine",
            "Cosine",
            "Triangle",
            "Square",
            "SawTooth",
            "Constant Value",
            "Formula",
            "UserDefined"});
            this.WaveFormTypeLB0.Location = new System.Drawing.Point(9, 17);
            this.WaveFormTypeLB0.Name = "WaveFormTypeLB0";
            this.WaveFormTypeLB0.Size = new System.Drawing.Size(128, 160);
            this.WaveFormTypeLB0.TabIndex = 19;
            this.WaveFormTypeLB0.SelectedIndexChanged += new System.EventHandler(this.WaveFormTypeLB_SelectedIndexChanged);
            // 
            // Waveformlabel2
            // 
            this.Waveformlabel2.AutoSize = true;
            this.Waveformlabel2.Location = new System.Drawing.Point(161, 1);
            this.Waveformlabel2.Name = "Waveformlabel2";
            this.Waveformlabel2.Size = new System.Drawing.Size(53, 13);
            this.Waveformlabel2.TabIndex = 18;
            this.Waveformlabel2.Text = "Amplitude";
            // 
            // WaveFormAmplitudeTB0
            // 
            this.WaveFormAmplitudeTB0.Location = new System.Drawing.Point(164, 17);
            this.WaveFormAmplitudeTB0.Name = "WaveFormAmplitudeTB0";
            this.WaveFormAmplitudeTB0.Size = new System.Drawing.Size(89, 20);
            this.WaveFormAmplitudeTB0.TabIndex = 17;
            this.WaveFormAmplitudeTB0.TextChanged += new System.EventHandler(this.WaveFormAmplitudeTB_TextChanged);
            // 
            // WaveFormFreqTB0
            // 
            this.WaveFormFreqTB0.Location = new System.Drawing.Point(164, 56);
            this.WaveFormFreqTB0.Name = "WaveFormFreqTB0";
            this.WaveFormFreqTB0.Size = new System.Drawing.Size(89, 20);
            this.WaveFormFreqTB0.TabIndex = 16;
            this.WaveFormFreqTB0.TextChanged += new System.EventHandler(this.WaveFormFreqTB0_TextChanged);
            // 
            // WaveFormlabel1
            // 
            this.WaveFormlabel1.AutoSize = true;
            this.WaveFormlabel1.Location = new System.Drawing.Point(161, 40);
            this.WaveFormlabel1.Name = "WaveFormlabel1";
            this.WaveFormlabel1.Size = new System.Drawing.Size(57, 13);
            this.WaveFormlabel1.TabIndex = 15;
            this.WaveFormlabel1.Text = "Frequency";
            // 
            // AddChannelB
            // 
            this.AddChannelB.Location = new System.Drawing.Point(44, 264);
            this.AddChannelB.Name = "AddChannelB";
            this.AddChannelB.Size = new System.Drawing.Size(100, 22);
            this.AddChannelB.TabIndex = 17;
            this.AddChannelB.Text = "Add Channel";
            this.AddChannelB.UseVisualStyleBackColor = true;
            this.AddChannelB.Click += new System.EventHandler(this.AddChannelB_Click);
            // 
            // WaveformOutputTime0
            // 
            this.WaveformOutputTime0.Location = new System.Drawing.Point(11, 297);
            this.WaveformOutputTime0.Name = "WaveformOutputTime0";
            this.WaveformOutputTime0.Size = new System.Drawing.Size(101, 20);
            this.WaveformOutputTime0.TabIndex = 28;
            this.WaveformOutputTime0.Text = "1";
            this.WaveformOutputTime0.TextChanged += new System.EventHandler(this.WaveformOutputTime0_TextChanged);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zedGraphControl1.Location = new System.Drawing.Point(8, 7);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0;
            this.zedGraphControl1.ScrollMaxX = 0;
            this.zedGraphControl1.ScrollMaxY = 0;
            this.zedGraphControl1.ScrollMaxY2 = 0;
            this.zedGraphControl1.ScrollMinX = 0;
            this.zedGraphControl1.ScrollMinY = 0;
            this.zedGraphControl1.ScrollMinY2 = 0;
            this.zedGraphControl1.Size = new System.Drawing.Size(321, 228);
            this.zedGraphControl1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 281);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Output Time";
            // 
            // StopB
            // 
            this.StopB.Location = new System.Drawing.Point(148, 242);
            this.StopB.Name = "StopB";
            this.StopB.Size = new System.Drawing.Size(100, 23);
            this.StopB.TabIndex = 7;
            this.StopB.Text = "Stop Generation";
            this.StopB.UseVisualStyleBackColor = true;
            this.StopB.Click += new System.EventHandler(this.StopB_Click);
            // 
            // StartGenerationB
            // 
            this.StartGenerationB.Location = new System.Drawing.Point(42, 242);
            this.StartGenerationB.Name = "StartGenerationB";
            this.StartGenerationB.Size = new System.Drawing.Size(100, 23);
            this.StartGenerationB.TabIndex = 6;
            this.StartGenerationB.Text = "Start Generation";
            this.StartGenerationB.UseVisualStyleBackColor = true;
            this.StartGenerationB.Click += new System.EventHandler(this.StartGenerationB_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyList1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(341, 653);
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
            this.propertyList1.Size = new System.Drawing.Size(335, 647);
            this.propertyList1.TabIndex = 0;
            // 
            // FrequencyGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "FrequencyGenerator";
            this.Size = new System.Drawing.Size(349, 679);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ChannelPanel.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button StopB;
        private System.Windows.Forms.Button StartGenerationB;
        private System.Windows.Forms.TabPage tabPage2;
        private CoreDevices.DeviceControls.PropertyList propertyList1;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Button AddChannelB;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox WaveformPhaseTB0;
        private System.Windows.Forms.Label WaveformPhaselabel0;
        private System.Windows.Forms.Label WaveFormTypeLabel;
        private System.Windows.Forms.ListBox WaveFormTypeLB0;
        private System.Windows.Forms.Label Waveformlabel2;
        private System.Windows.Forms.TextBox WaveFormAmplitudeTB0;
        private System.Windows.Forms.TextBox WaveFormFreqTB0;
        private System.Windows.Forms.Label WaveFormlabel1;
        private System.Windows.Forms.Panel ChannelPanel;
        private System.Windows.Forms.TextBox WaveFormFilename0;
        private System.Windows.Forms.Label WaveformFilenameLabel0;
        private System.Windows.Forms.TextBox WaveformFormula0;
        private System.Windows.Forms.Label WaveformFormulaLabel0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox WaveformOutputTime0;
        private System.Windows.Forms.Button RemoveChannelB;

    }
}
