namespace Micromanager_net.UI
{
    partial class StageProperties
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
            try
            {
                xyStage.StopStageThread();
            }
            catch { } 
            xyStage = null;
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddLocationButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.StackShowCB = new System.Windows.Forms.CheckBox();
            this.StackProgress = new System.Windows.Forms.ProgressBar();
            this.FilenameStackTB = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ZDispTB = new System.Windows.Forms.TextBox();
            this.YDispTB = new System.Windows.Forms.TextBox();
            this.XDispTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.TakeStack = new System.Windows.Forms.Button();
            this.ZRows = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.YRows = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.XRows = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyList1 = new Micromanager_net.RControls.PropertyList();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.tabControl1.Size = new System.Drawing.Size(332, 633);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(324, 607);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stage Controls";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 392);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Joystick";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Named Locations";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.AddLocationButton);
            this.panel1.Location = new System.Drawing.Point(3, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 365);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.colHeight});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(315, 315);
            this.dataGridView1.TabIndex = 9;
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
            // Height
            // 
            this.colHeight.HeaderText = "Height";
            this.colHeight.Name = "Height";
            // 
            // AddLocationButton
            // 
            this.AddLocationButton.Location = new System.Drawing.Point(28, 324);
            this.AddLocationButton.Name = "AddLocationButton";
            this.AddLocationButton.Size = new System.Drawing.Size(182, 26);
            this.AddLocationButton.TabIndex = 10;
            this.AddLocationButton.Text = "Add X,Y,Z Location";
            this.AddLocationButton.UseVisualStyleBackColor = true;
            this.AddLocationButton.Click += new System.EventHandler(this.AddLocationButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.StackShowCB);
            this.tabPage3.Controls.Add(this.StackProgress);
            this.tabPage3.Controls.Add(this.FilenameStackTB);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.ZDispTB);
            this.tabPage3.Controls.Add(this.YDispTB);
            this.tabPage3.Controls.Add(this.XDispTB);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.checkBox1);
            this.tabPage3.Controls.Add(this.TakeStack);
            this.tabPage3.Controls.Add(this.ZRows);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.YRows);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.XRows);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(324, 607);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Stacks and Montage";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // StackShowCB
            // 
            this.StackShowCB.AutoSize = true;
            this.StackShowCB.Location = new System.Drawing.Point(153, 167);
            this.StackShowCB.Name = "StackShowCB";
            this.StackShowCB.Size = new System.Drawing.Size(90, 17);
            this.StackShowCB.TabIndex = 17;
            this.StackShowCB.Text = "Show Images";
            this.StackShowCB.UseVisualStyleBackColor = true;
            // 
            // StackProgress
            // 
            this.StackProgress.Location = new System.Drawing.Point(11, 232);
            this.StackProgress.Name = "StackProgress";
            this.StackProgress.Size = new System.Drawing.Size(214, 18);
            this.StackProgress.TabIndex = 16;
            // 
            // FilenameStackTB
            // 
            this.FilenameStackTB.Location = new System.Drawing.Point(11, 141);
            this.FilenameStackTB.Name = "FilenameStackTB";
            this.FilenameStackTB.Size = new System.Drawing.Size(214, 20);
            this.FilenameStackTB.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Filename";
            // 
            // ZDispTB
            // 
            this.ZDispTB.Location = new System.Drawing.Point(125, 102);
            this.ZDispTB.Name = "ZDispTB";
            this.ZDispTB.Size = new System.Drawing.Size(100, 20);
            this.ZDispTB.TabIndex = 13;
            // 
            // YDispTB
            // 
            this.YDispTB.Location = new System.Drawing.Point(125, 63);
            this.YDispTB.Name = "YDispTB";
            this.YDispTB.Size = new System.Drawing.Size(100, 20);
            this.YDispTB.TabIndex = 12;
            // 
            // XDispTB
            // 
            this.XDispTB.Location = new System.Drawing.Point(125, 24);
            this.XDispTB.Name = "XDispTB";
            this.XDispTB.Size = new System.Drawing.Size(100, 20);
            this.XDispTB.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(122, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Displacement (um)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(122, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Displacement (um)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(122, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Displacement (um)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(11, 167);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(136, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Save Individual Images";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // TakeStack
            // 
            this.TakeStack.Location = new System.Drawing.Point(11, 190);
            this.TakeStack.Name = "TakeStack";
            this.TakeStack.Size = new System.Drawing.Size(75, 23);
            this.TakeStack.TabIndex = 6;
            this.TakeStack.Text = "Take Stack";
            this.TakeStack.UseVisualStyleBackColor = true;
            this.TakeStack.Click += new System.EventHandler(this.button1_Click);
            // 
            // ZRows
            // 
            this.ZRows.Location = new System.Drawing.Point(11, 102);
            this.ZRows.Name = "ZRows";
            this.ZRows.Size = new System.Drawing.Size(100, 20);
            this.ZRows.TabIndex = 5;
            this.ZRows.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Number Of Z Rows";
            // 
            // YRows
            // 
            this.YRows.Location = new System.Drawing.Point(11, 63);
            this.YRows.Name = "YRows";
            this.YRows.Size = new System.Drawing.Size(100, 20);
            this.YRows.TabIndex = 3;
            this.YRows.Text = "5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Number Of Y Rows";
            // 
            // XRows
            // 
            this.XRows.Location = new System.Drawing.Point(11, 24);
            this.XRows.Name = "XRows";
            this.XRows.Size = new System.Drawing.Size(100, 20);
            this.XRows.TabIndex = 1;
            this.XRows.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Number of X Rows";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyList1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(324, 607);
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
            this.propertyList1.Size = new System.Drawing.Size(318, 601);
            this.propertyList1.TabIndex = 0;
            // 
            // StageProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "StageProperties";
            this.Size = new System.Drawing.Size(332, 633);
            this.Resize += new System.EventHandler(this.StageProperties_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPage2;
        private Micromanager_net.RControls.PropertyList propertyList1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
       // private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.Button AddLocationButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button TakeStack;
        private System.Windows.Forms.TextBox ZRows;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox YRows;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox XRows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        //private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeight;
        private System.Windows.Forms.TextBox FilenameStackTB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ZDispTB;
        private System.Windows.Forms.TextBox YDispTB;
        private System.Windows.Forms.TextBox XDispTB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar StackProgress;
        private System.Windows.Forms.CheckBox StackShowCB;
    }
}
