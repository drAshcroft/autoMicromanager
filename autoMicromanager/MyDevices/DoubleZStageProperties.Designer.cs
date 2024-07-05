namespace Micromanager_net.UI
{
    partial class DoubleZStageProperties
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
            this.LinkSlidersCB = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SecFineFocus = new CoreDevices.DeviceControls.FocusSlider();
            this.SecCourseFocus = new CoreDevices.DeviceControls.FocusSlider();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.FineFocus = new CoreDevices.DeviceControls.FocusSlider();
            this.DoAutofocus = new System.Windows.Forms.Button();
            this.lPosition = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CourseFocus = new CoreDevices.DeviceControls.FocusSlider();
            this.AddLocationButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyList1 = new CoreDevices.DeviceControls.PropertyList();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.propertyList2 = new CoreDevices.DeviceControls.PropertyList();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(428, 660);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.LinkSlidersCB);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.SecFineFocus);
            this.tabPage1.Controls.Add(this.SecCourseFocus);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.FineFocus);
            this.tabPage1.Controls.Add(this.DoAutofocus);
            this.tabPage1.Controls.Add(this.lPosition);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.CourseFocus);
            this.tabPage1.Controls.Add(this.AddLocationButton);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(420, 634);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Focus Control";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Resize += new System.EventHandler(this.tabPage1_Resize);
            // 
            // LinkSlidersCB
            // 
            this.LinkSlidersCB.AutoSize = true;
            this.LinkSlidersCB.Checked = true;
            this.LinkSlidersCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LinkSlidersCB.Location = new System.Drawing.Point(83, 346);
            this.LinkSlidersCB.Name = "LinkSlidersCB";
            this.LinkSlidersCB.Size = new System.Drawing.Size(80, 17);
            this.LinkSlidersCB.TabIndex = 17;
            this.LinkSlidersCB.Text = "Link Sliders";
            this.LinkSlidersCB.UseVisualStyleBackColor = true;
            this.LinkSlidersCB.CheckedChanged += new System.EventHandler(this.LinkSlidersCB_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(365, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Fine";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(311, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Course";
            // 
            // SecFineFocus
            // 
            this.SecFineFocus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.SecFineFocus.Location = new System.Drawing.Point(362, 19);
            this.SecFineFocus.Maximum = 100;
            this.SecFineFocus.Minimum = 0;
            this.SecFineFocus.Name = "SecFineFocus";
            this.SecFineFocus.Size = new System.Drawing.Size(29, 609);
            this.SecFineFocus.TabIndex = 14;
            this.SecFineFocus.Value = 50;
            this.SecFineFocus.OnScroll += new System.Windows.Forms.ScrollEventHandler(this.SecFineFocus_OnScroll);
            // 
            // SecCourseFocus
            // 
            this.SecCourseFocus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.SecCourseFocus.Location = new System.Drawing.Point(315, 19);
            this.SecCourseFocus.Maximum = 100;
            this.SecCourseFocus.Minimum = 0;
            this.SecCourseFocus.Name = "SecCourseFocus";
            this.SecCourseFocus.Size = new System.Drawing.Size(29, 609);
            this.SecCourseFocus.TabIndex = 13;
            this.SecCourseFocus.Value = 50;
            this.SecCourseFocus.OnScroll += new System.Windows.Forms.ScrollEventHandler(this.SecCourseFocus_OnScroll);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(83, 305);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 35);
            this.button1.TabIndex = 12;
            this.button1.Text = "Zero Sliders";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Fine";
            // 
            // FineFocus
            // 
            this.FineFocus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.FineFocus.Location = new System.Drawing.Point(53, 19);
            this.FineFocus.Maximum = 100;
            this.FineFocus.Minimum = 0;
            this.FineFocus.Name = "FineFocus";
            this.FineFocus.Size = new System.Drawing.Size(29, 609);
            this.FineFocus.TabIndex = 10;
            this.FineFocus.Value = 50;
            this.FineFocus.OnScroll += new System.Windows.Forms.ScrollEventHandler(this.FineFocus_OnScroll);
            // 
            // DoAutofocus
            // 
            this.DoAutofocus.Location = new System.Drawing.Point(84, 42);
            this.DoAutofocus.Name = "DoAutofocus";
            this.DoAutofocus.Size = new System.Drawing.Size(67, 36);
            this.DoAutofocus.TabIndex = 8;
            this.DoAutofocus.Text = "Auto Focus";
            this.DoAutofocus.UseVisualStyleBackColor = true;
            this.DoAutofocus.Click += new System.EventHandler(this.DoAutofocus_Click);
            // 
            // lPosition
            // 
            this.lPosition.AutoSize = true;
            this.lPosition.Location = new System.Drawing.Point(88, 16);
            this.lPosition.Name = "lPosition";
            this.lPosition.Size = new System.Drawing.Size(13, 13);
            this.lPosition.TabIndex = 7;
            this.lPosition.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Position";
            // 
            // CourseFocus
            // 
            this.CourseFocus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.CourseFocus.Location = new System.Drawing.Point(6, 19);
            this.CourseFocus.Maximum = 100;
            this.CourseFocus.Minimum = 0;
            this.CourseFocus.Name = "CourseFocus";
            this.CourseFocus.Size = new System.Drawing.Size(29, 609);
            this.CourseFocus.TabIndex = 1;
            this.CourseFocus.Value = 50;
            this.CourseFocus.OnScroll += new System.Windows.Forms.ScrollEventHandler(this.CourseFocus_OnScroll);
            // 
            // AddLocationButton
            // 
            this.AddLocationButton.Location = new System.Drawing.Point(189, 602);
            this.AddLocationButton.Name = "AddLocationButton";
            this.AddLocationButton.Size = new System.Drawing.Size(103, 26);
            this.AddLocationButton.TabIndex = 5;
            this.AddLocationButton.Text = "Add Plane";
            this.AddLocationButton.UseVisualStyleBackColor = true;
            this.AddLocationButton.Click += new System.EventHandler(this.AddLocationButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this._Height});
            this.dataGridView1.Location = new System.Drawing.Point(162, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(143, 590);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.FillWeight = 50F;
            this.Column1.HeaderText = "Move To:";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Location Name";
            this.Column2.Name = "Column2";
            // 
            // _Height
            // 
            this._Height.HeaderText = "_Height";
            this._Height.Name = "_Height";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Course";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyList1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(420, 634);
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
            this.propertyList1.Size = new System.Drawing.Size(414, 628);
            this.propertyList1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.propertyList2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(420, 634);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // propertyList2
            // 
            this.propertyList2.AutoScroll = true;
            this.propertyList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyList2.Location = new System.Drawing.Point(3, 3);
            this.propertyList2.Name = "propertyList2";
            this.propertyList2.Size = new System.Drawing.Size(414, 628);
            this.propertyList2.TabIndex = 1;
            // 
            // DoubleZStageProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "DoubleZStageProperties";
            this.Size = new System.Drawing.Size(428, 660);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private CoreDevices.DeviceControls.PropertyList propertyList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button AddLocationButton;
        private CoreDevices.DeviceControls.FocusSlider CourseFocus;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lPosition;
        private System.Windows.Forms.Button DoAutofocus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.Label label3;
        private CoreDevices.DeviceControls.FocusSlider FineFocus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Height;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private CoreDevices.DeviceControls.FocusSlider SecFineFocus;
        private CoreDevices.DeviceControls.FocusSlider SecCourseFocus;
        private System.Windows.Forms.TabPage tabPage3;
        private CoreDevices.DeviceControls.PropertyList propertyList2;
        private System.Windows.Forms.CheckBox LinkSlidersCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn _Height;
    }
}
