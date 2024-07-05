namespace CoreDevices.DeviceControls
{
    partial class LUTGraph
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
            this.Graph = new System.Windows.Forms.PictureBox();
            this.Contrast = new CoreDevices.DeviceControls.DoubleSlider();
            this.AutoContrastCB = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Graph)).BeginInit();
            this.SuspendLayout();
            // 
            // Graph
            // 
            this.Graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Graph.BackColor = System.Drawing.Color.White;
            this.Graph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Graph.Location = new System.Drawing.Point(0, 0);
            this.Graph.Name = "Graph";
            this.Graph.Size = new System.Drawing.Size(493, 448);
            this.Graph.TabIndex = 0;
            this.Graph.TabStop = false;
            this.Graph.Resize += new System.EventHandler(this.Graph_Resize);
            // 
            // Contrast
            // 
            this.Contrast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Contrast.BackColor = System.Drawing.Color.Transparent;
            this.Contrast.BarInnerColor = System.Drawing.Color.Lime;
            this.Contrast.BarOuterColor = System.Drawing.Color.Green;
            this.Contrast.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.Contrast.ElapsedInnerColor = System.Drawing.Color.Navy;
            this.Contrast.LargeChange = ((uint)(5u));
            this.Contrast.Location = new System.Drawing.Point(3, 451);
            this.Contrast.Maximum = 100000;
            this.Contrast.Name = "Contrast";
            this.Contrast.Size = new System.Drawing.Size(492, 30);
            this.Contrast.SmallChange = ((uint)(1u));
            this.Contrast.TabIndex = 13;
            this.Contrast.Text = "doubleSlider1";
            this.Contrast.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            this.Contrast.ValueLower = 25;
            this.Contrast.ValueUpper = 75;
            this.Contrast.ValueChanged += new System.EventHandler(this.Contrast_ValueChanged);
            // 
            // AutoContrastCB
            // 
            this.AutoContrastCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoContrastCB.AutoSize = true;
            this.AutoContrastCB.Checked = true;
            this.AutoContrastCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoContrastCB.Location = new System.Drawing.Point(406, 487);
            this.AutoContrastCB.Name = "AutoContrastCB";
            this.AutoContrastCB.Size = new System.Drawing.Size(87, 17);
            this.AutoContrastCB.TabIndex = 14;
            this.AutoContrastCB.Text = "AutoContrast";
            this.AutoContrastCB.UseVisualStyleBackColor = true;
            // 
            // LUTGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AutoContrastCB);
            this.Controls.Add(this.Contrast);
            this.Controls.Add(this.Graph);
            this.Name = "LUTGraph";
            this.Size = new System.Drawing.Size(498, 509);
            ((System.ComponentModel.ISupportInitialize)(this.Graph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Graph;
        private DoubleSlider Contrast;
        private System.Windows.Forms.CheckBox AutoContrastCB;
    }
}
