namespace Micromanager_net.UI
{
    partial class FreqCheck
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
            this.frequencyUpdate1 = new Micromanager_net.UI.LaserControl();
            this.SuspendLayout();
            // 
            // frequencyUpdate1
            // 
            this.frequencyUpdate1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frequencyUpdate1.Location = new System.Drawing.Point(0, 0);
            this.frequencyUpdate1.Name = "frequencyUpdate1";
            this.frequencyUpdate1.Size = new System.Drawing.Size(575, 533);
            this.frequencyUpdate1.TabIndex = 0;
            this.frequencyUpdate1.Load += new System.EventHandler(this.frequencyUpdate1_Load);
            // 
            // FreqCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 533);
            this.Controls.Add(this.frequencyUpdate1);
            this.Name = "FreqCheck";
            this.Text = "FreqCheck";
            this.ResumeLayout(false);

        }

        #endregion

        private LaserControl frequencyUpdate1;
    }
}