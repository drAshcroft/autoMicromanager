namespace Micromanager_net
{
    partial class FilterWheelForm
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
            this.filterWheelControl1 = new Micromanager_net.FilterWheelControl();
            this.SuspendLayout();
            // 
            // filterWheelControl1
            // 
            this.filterWheelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterWheelControl1.Location = new System.Drawing.Point(0, 0);
            this.filterWheelControl1.Name = "filterWheelControl1";
            this.filterWheelControl1.Size = new System.Drawing.Size(420, 404);
            this.filterWheelControl1.TabIndex = 0;
            // 
            // FilterWheelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 404);
            this.Controls.Add(this.filterWheelControl1);
            this.Name = "FilterWheelForm";
            this.Text = "FilterWheelForm";
            this.ResumeLayout(false);

        }

        #endregion

        private FilterWheelControl filterWheelControl1;
    }
}