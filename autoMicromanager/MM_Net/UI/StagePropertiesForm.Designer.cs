namespace Micromanager_net
{
    partial class StagePropertiesForm
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
            this.stageProperties1 = new Micromanager_net.UI.StageProperties();
            this.SuspendLayout();
            // 
            // stageProperties1
            // 
            this.stageProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stageProperties1.Location = new System.Drawing.Point(0, 0);
            this.stageProperties1.Name = "stageProperties1";
            this.stageProperties1.Size = new System.Drawing.Size(363, 502);
            this.stageProperties1.TabIndex = 0;
            // 
            // StagePropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 502);
            this.Controls.Add(this.stageProperties1);
            this.Name = "StagePropertiesForm";
            this.TabText = "Stage Control";
            this.Text = "Stage Properties";
            this.ResumeLayout(false);

        }

        #endregion

        private Micromanager_net.UI.StageProperties stageProperties1;
    }
}