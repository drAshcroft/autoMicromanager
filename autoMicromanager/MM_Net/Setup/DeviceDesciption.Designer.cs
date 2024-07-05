namespace Micromanager_net.Setup
{
    partial class DeviceDesciption
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
            this.xDeviceName = new System.Windows.Forms.TextBox();
            this.XDeviceUI = new System.Windows.Forms.ComboBox();
            this.xDeviceLib = new System.Windows.Forms.ComboBox();
            this.XDeviceAdapter = new System.Windows.Forms.ComboBox();
            this.XDevEnabled = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // xDeviceName
            // 
            this.xDeviceName.Location = new System.Drawing.Point(43, 3);
            this.xDeviceName.Name = "xDeviceName";
            this.xDeviceName.Size = new System.Drawing.Size(103, 20);
            this.xDeviceName.TabIndex = 51;
            // 
            // XDeviceUI
            // 
            this.XDeviceUI.FormattingEnabled = true;
            this.XDeviceUI.Location = new System.Drawing.Point(487, 3);
            this.XDeviceUI.Name = "XDeviceUI";
            this.XDeviceUI.Size = new System.Drawing.Size(256, 21);
            this.XDeviceUI.TabIndex = 49;
            // 
            // xDeviceLib
            // 
            this.xDeviceLib.FormattingEnabled = true;
            this.xDeviceLib.Location = new System.Drawing.Point(152, 3);
            this.xDeviceLib.Name = "xDeviceLib";
            this.xDeviceLib.Size = new System.Drawing.Size(136, 21);
            this.xDeviceLib.TabIndex = 50;
            this.xDeviceLib.SelectedIndexChanged += new System.EventHandler(this.xDeviceLib_SelectedIndexChanged);
            // 
            // XDeviceAdapter
            // 
            this.XDeviceAdapter.FormattingEnabled = true;
            this.XDeviceAdapter.Location = new System.Drawing.Point(294, 3);
            this.XDeviceAdapter.Name = "XDeviceAdapter";
            this.XDeviceAdapter.Size = new System.Drawing.Size(187, 21);
            this.XDeviceAdapter.TabIndex = 48;
            // 
            // XDevEnabled
            // 
            this.XDevEnabled.AutoSize = true;
            this.XDevEnabled.Checked = true;
            this.XDevEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.XDevEnabled.Location = new System.Drawing.Point(2, 3);
            this.XDevEnabled.Name = "XDevEnabled";
            this.XDevEnabled.Size = new System.Drawing.Size(15, 14);
            this.XDevEnabled.TabIndex = 47;
            this.XDevEnabled.UseVisualStyleBackColor = true;
            // 
            // DeviceDesciption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xDeviceName);
            this.Controls.Add(this.XDeviceUI);
            this.Controls.Add(this.xDeviceLib);
            this.Controls.Add(this.XDeviceAdapter);
            this.Controls.Add(this.XDevEnabled);
            this.Name = "DeviceDesciption";
            this.Size = new System.Drawing.Size(747, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox xDeviceName;
        private System.Windows.Forms.ComboBox XDeviceUI;
        private System.Windows.Forms.ComboBox xDeviceLib;
        private System.Windows.Forms.ComboBox XDeviceAdapter;
        private System.Windows.Forms.CheckBox XDevEnabled;
    }
}
