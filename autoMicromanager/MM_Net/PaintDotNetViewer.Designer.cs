namespace SciImage
{
    partial class View
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
            this.PS = new SciImage.AppWorkspace();
            this.SuspendLayout();
            // 
            // PS
            // 
            this.PS.ActiveDocumentWorkspace = null;
            this.PS.DefaultToolType = typeof(SciImage.Tools.PaintBrushTool);
            this.PS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PS.GlobalToolTypeChoice = typeof(SciImage.Tools.PaintBrushTool);
            this.PS.Location = new System.Drawing.Point(0, 0);
            this.PS.MDIworkspace = false;
            this.PS.Name = "PS";
            this.PS.RulersEnabled = false;
            this.PS.Size = new System.Drawing.Size(1046, 651);
            this.PS.TabIndex = 1;
            this.PS.Units = SciImage.MeasurementUnit.Pixel;
            this.PS.MakeNewForm += new SciImage.MakeNewMDISubFormEvent(this.PS_MakeNewForm);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1046, 651);
            this.Controls.Add(this.PS);
            this.MinimumSize = new System.Drawing.Size(820, 685);
            this.Name = "View";
            this.TabText = "View";
            this.Text = "View";
            this.Load += new System.EventHandler(this.View_Load);
            this.SizeChanged += new System.EventHandler(this.View_SizeChanged);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.View_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.View_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.View_ResizeEnd);
            this.Resize += new System.EventHandler(this.View_Resize_1);
            this.ResumeLayout(false);

        }

        #endregion

        private SciImage.AppWorkspace PS;





    }
}