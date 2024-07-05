namespace Micromanager_net
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.newHardwareConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadHardwareConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCurrentConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stageControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.focusControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joystickControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterWheelControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frequencyGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DockPanel
            // 
            this.DockPanel.ActiveAutoHideContent = null;
            this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.DockPanel.Location = new System.Drawing.Point(0, 0);
            this.DockPanel.Name = "DockPanel";
            this.DockPanel.ShowDocumentIcon = true;
            this.DockPanel.Size = new System.Drawing.Size(782, 405);
            this.DockPanel.TabIndex = 23;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(782, 55);
            this.toolStrip1.TabIndex = 29;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton3.Text = "Live Images";
            this.toolStripButton3.ToolTipText = "Slower Live Images From Camera";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(52, 52);
            this.toolStripButton4.Text = "Movie Sequence";
            this.toolStripButton4.ToolTipText = "Fast movie sequence";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(782, 24);
            this.menuStrip1.TabIndex = 32;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newUserToolStripMenuItem,
            this.deleteUserToolStripMenuItem,
            this.toolStripSeparator1,
            this.newHardwareConfigurationToolStripMenuItem,
            this.loadHardwareConfigurationToolStripMenuItem,
            this.saveCurrentConfigurationToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newUserToolStripMenuItem
            // 
            this.newUserToolStripMenuItem.Name = "newUserToolStripMenuItem";
            this.newUserToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.newUserToolStripMenuItem.Text = "New User";
            // 
            // deleteUserToolStripMenuItem
            // 
            this.deleteUserToolStripMenuItem.Name = "deleteUserToolStripMenuItem";
            this.deleteUserToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.deleteUserToolStripMenuItem.Text = "Delete User";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(212, 6);
            // 
            // newHardwareConfigurationToolStripMenuItem
            // 
            this.newHardwareConfigurationToolStripMenuItem.Name = "newHardwareConfigurationToolStripMenuItem";
            this.newHardwareConfigurationToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.newHardwareConfigurationToolStripMenuItem.Text = "New Hardware Configuration";
            this.newHardwareConfigurationToolStripMenuItem.Click += new System.EventHandler(this.newHardwareConfigurationToolStripMenuItem_Click);
            // 
            // loadHardwareConfigurationToolStripMenuItem
            // 
            this.loadHardwareConfigurationToolStripMenuItem.Name = "loadHardwareConfigurationToolStripMenuItem";
            this.loadHardwareConfigurationToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.loadHardwareConfigurationToolStripMenuItem.Text = "Load Hardware Configuration";
            // 
            // saveCurrentConfigurationToolStripMenuItem
            // 
            this.saveCurrentConfigurationToolStripMenuItem.Name = "saveCurrentConfigurationToolStripMenuItem";
            this.saveCurrentConfigurationToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.saveCurrentConfigurationToolStripMenuItem.Text = "Save Current Configuration";
            this.saveCurrentConfigurationToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentConfigurationToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.cameraPropertiesToolStripMenuItem,
            this.stageControlToolStripMenuItem,
            this.focusControlToolStripMenuItem,
            this.joystickControlToolStripMenuItem,
            this.filterWheelControlToolStripMenuItem,
            this.frequencyGeneratorToolStripMenuItem,
            this.scriptControlToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem1.Text = "View Form";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // cameraPropertiesToolStripMenuItem
            // 
            this.cameraPropertiesToolStripMenuItem.Name = "cameraPropertiesToolStripMenuItem";
            this.cameraPropertiesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.cameraPropertiesToolStripMenuItem.Text = "Camera Control";
            this.cameraPropertiesToolStripMenuItem.Click += new System.EventHandler(this.cameraPropertiesToolStripMenuItem_Click);
            // 
            // stageControlToolStripMenuItem
            // 
            this.stageControlToolStripMenuItem.Name = "stageControlToolStripMenuItem";
            this.stageControlToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.stageControlToolStripMenuItem.Text = "Stage Control";
            this.stageControlToolStripMenuItem.Click += new System.EventHandler(this.stageControlToolStripMenuItem_Click);
            // 
            // focusControlToolStripMenuItem
            // 
            this.focusControlToolStripMenuItem.Name = "focusControlToolStripMenuItem";
            this.focusControlToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.focusControlToolStripMenuItem.Text = "Focus Control";
            this.focusControlToolStripMenuItem.Click += new System.EventHandler(this.focusControlToolStripMenuItem_Click);
            // 
            // joystickControlToolStripMenuItem
            // 
            this.joystickControlToolStripMenuItem.Name = "joystickControlToolStripMenuItem";
            this.joystickControlToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.joystickControlToolStripMenuItem.Text = "Joystick Control";
            this.joystickControlToolStripMenuItem.Click += new System.EventHandler(this.joystickControlToolStripMenuItem_Click);
            // 
            // filterWheelControlToolStripMenuItem
            // 
            this.filterWheelControlToolStripMenuItem.Name = "filterWheelControlToolStripMenuItem";
            this.filterWheelControlToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.filterWheelControlToolStripMenuItem.Text = "Filter Wheel Control";
            this.filterWheelControlToolStripMenuItem.Click += new System.EventHandler(this.filterWheelControlToolStripMenuItem_Click);
            // 
            // frequencyGeneratorToolStripMenuItem
            // 
            this.frequencyGeneratorToolStripMenuItem.Name = "frequencyGeneratorToolStripMenuItem";
            this.frequencyGeneratorToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.frequencyGeneratorToolStripMenuItem.Text = "Function Generator";
            this.frequencyGeneratorToolStripMenuItem.Click += new System.EventHandler(this.frequencyGeneratorToolStripMenuItem_Click);
            // 
            // scriptControlToolStripMenuItem
            // 
            this.scriptControlToolStripMenuItem.Name = "scriptControlToolStripMenuItem";
            this.scriptControlToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.scriptControlToolStripMenuItem.Text = "Script Control";
            this.scriptControlToolStripMenuItem.Click += new System.EventHandler(this.scriptControlToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 405);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.DockPanel);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "autoMicroManager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel;

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem newHardwareConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadHardwareConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cameraPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stageControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem focusControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joystickControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterWheelControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frequencyGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveCurrentConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptControlToolStripMenuItem;
    }
}

