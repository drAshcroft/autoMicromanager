namespace CoreDevices.Channels
{
    partial class ChannelSetup
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
            this.channelSetupControl1 = new ChannelSetupControl();
            this.SuspendLayout();
            // 
            // channelSetupControl1
            // 
            this.channelSetupControl1.Location = new System.Drawing.Point(-1, 5);
            this.channelSetupControl1.Name = "channelSetupControl1";
            this.channelSetupControl1.Size = new System.Drawing.Size(760, 353);
            this.channelSetupControl1.TabIndex = 0;
            // 
            // ChannelSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 370);
            this.Controls.Add(this.channelSetupControl1);
            this.Name = "ChannelSetup";
            this.Text = "ChannelSetup";
            this.ResumeLayout(false);

        }

        #endregion

        private ChannelSetupControl channelSetupControl1;

    }
}