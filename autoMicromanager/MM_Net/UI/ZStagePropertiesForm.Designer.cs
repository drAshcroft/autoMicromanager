namespace Micromanager_net
{
    partial class ZStagePropertiesForm
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
            this.zStageProperties1 = new Micromanager_net.UI.ZStageProperties();
            this.SuspendLayout();
            // 
            // zStageProperties1
            // 
            this.zStageProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zStageProperties1.Location = new System.Drawing.Point(0, 0);
            this.zStageProperties1.Name = "zStageProperties1";
            this.zStageProperties1.Size = new System.Drawing.Size(292, 266);
            this.zStageProperties1.TabIndex = 0;
            // 
            // ZStagePropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.zStageProperties1);
            this.Name = "ZStagePropertiesForm";
            this.TabText = "Focus Control";
            this.Text = "Focus Control";
            this.ResumeLayout(false);

        }

        #endregion

        private Micromanager_net.UI.ZStageProperties zStageProperties1;
    }
}