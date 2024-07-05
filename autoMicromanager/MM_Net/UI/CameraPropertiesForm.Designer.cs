namespace Micromanager_net
{
    partial class CameraPropertiesForm
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
            this.cameraProperties1 = new Micromanager_net.CameraProperties();
            this.SuspendLayout();
            // 
            // cameraProperties1
            // 
            this.cameraProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraProperties1.Location = new System.Drawing.Point(0, 0);
            this.cameraProperties1.Name = "cameraProperties1";
            this.cameraProperties1.Size = new System.Drawing.Size(436, 759);
            this.cameraProperties1.TabIndex = 0;
            this.cameraProperties1.Load += new System.EventHandler(this.cameraProperties1_Load);
            // 
            // CameraPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 759);
            this.Controls.Add(this.cameraProperties1);
            this.Name = "CameraPropertiesForm";
            this.TabText = "Camera Properties";
            this.Text = "Camera Properties";
            this.ResumeLayout(false);

        }

        #endregion

        private CameraProperties cameraProperties1;
    }
}