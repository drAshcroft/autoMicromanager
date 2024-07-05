namespace PaintDNetWindow.PaintForms
{
    partial class DefaultForm
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
            this.SuspendLayout();
            // 
            // DefaultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 314);
            this.Name = "DefaultForm";
            this.Text = "DefaultForm";
            this.SizeChanged += new System.EventHandler(this.DefaultForm_SizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DefaultForm_FormClosing);
            this.Resize += new System.EventHandler(this.DefaultForm_Resize);
            this.ResizeEnd += new System.EventHandler(this.DefaultForm_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion
    }
}