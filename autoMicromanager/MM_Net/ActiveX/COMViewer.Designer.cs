namespace Micromanager_net
{
    partial class MMCOMViewer
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
            this.pictureBoard1 = new CoreDevices.NI_Controls.PictureBoard();
            this.SuspendLayout();
            // 
            // pictureBoard1
            // 
            this.pictureBoard1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pictureBoard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoard1.Location = new System.Drawing.Point(0, 0);
            this.pictureBoard1.Name = "pictureBoard1";
            this.pictureBoard1.Size = new System.Drawing.Size(711, 583);
            this.pictureBoard1.TabIndex = 0;
            // 
            // MMCOMViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 583);
            this.Controls.Add(this.pictureBoard1);
            this.Name = "MMCOMViewer";
            this.Text = "MM_NETViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private CoreDevices.NI_Controls.PictureBoard pictureBoard1;
    }
}