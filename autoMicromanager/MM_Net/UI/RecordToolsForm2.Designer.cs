namespace Micromanager_net.UI
{
    partial class RecordToolsForm
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
            this.recordTools1 = new Micromanager_net.UI.MoreRecordTools();
            this.SuspendLayout();
            // 
            // recordTools1
            // 
            this.recordTools1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recordTools1.Location = new System.Drawing.Point(0, 0);
            this.recordTools1.Name = "recordTools1";
            this.recordTools1.Size = new System.Drawing.Size(427, 306);
            this.recordTools1.TabIndex = 0;
            // 
            // RecordToolsForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 306);
            this.Controls.Add(this.recordTools1);
            this.Name = "RecordToolsForm2";
            this.Text = "RecordToolsForm2";
            this.ResumeLayout(false);

        }

        #endregion

        private MoreRecordTools recordTools1;
    }
}