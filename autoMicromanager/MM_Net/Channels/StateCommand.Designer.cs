namespace CoreDevices.Channels
{
    partial class StateCommand
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
            this.ValueTB = new System.Windows.Forms.TextBox();
            this.MethodName = new System.Windows.Forms.ComboBox();
            this.DeviceName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ValueTB
            // 
            this.ValueTB.Location = new System.Drawing.Point(330, 0);
            this.ValueTB.Name = "ValueTB";
            this.ValueTB.Size = new System.Drawing.Size(128, 20);
            this.ValueTB.TabIndex = 14;
            // 
            // MethodName
            // 
            this.MethodName.FormattingEnabled = true;
            this.MethodName.Location = new System.Drawing.Point(158, 0);
            this.MethodName.Name = "MethodName";
            this.MethodName.Size = new System.Drawing.Size(171, 21);
            this.MethodName.TabIndex = 13;
            // 
            // DeviceName
            // 
            this.DeviceName.FormattingEnabled = true;
            this.DeviceName.Location = new System.Drawing.Point(0, 0);
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Size = new System.Drawing.Size(157, 21);
            this.DeviceName.TabIndex = 12;
            this.DeviceName.SelectedIndexChanged += new System.EventHandler(this.DeviceName_SelectedIndexChanged);
            // 
            // StateCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ValueTB);
            this.Controls.Add(this.MethodName);
            this.Controls.Add(this.DeviceName);
            this.Name = "StateCommand";
            this.Size = new System.Drawing.Size(460, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ValueTB;
        private System.Windows.Forms.ComboBox MethodName;
        private System.Windows.Forms.ComboBox DeviceName;
    }
}
