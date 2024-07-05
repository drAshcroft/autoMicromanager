namespace Test
{
    partial class Form1
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
            this.appWorkspace1 = new SciImage.AppWorkspace();
            this.SuspendLayout();
            // 
            // appWorkspace1
            // 
            this.appWorkspace1.ActiveDocumentWorkspace = null;
            this.appWorkspace1.DefaultToolType = typeof(SciImage.Tools.PaintBrushTool);
            this.appWorkspace1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appWorkspace1.GlobalToolTypeChoice = typeof(SciImage.Tools.PaintBrushTool);
            this.appWorkspace1.Location = new System.Drawing.Point(0, 0);
            this.appWorkspace1.MDIworkspace = false;
            this.appWorkspace1.Name = "appWorkspace1";
            this.appWorkspace1.RulersEnabled = true;
            this.appWorkspace1.Size = new System.Drawing.Size(660, 350);
            this.appWorkspace1.TabIndex = 27;
            this.appWorkspace1.Units = SciImage.MeasurementUnit.Pixel;
            this.appWorkspace1.MakeNewForm += new SciImage.MakeNewMDISubFormEvent(this.appWorkspace1_MakeNewForm);
            this.appWorkspace1.MakeFormFromUsercontrol += new SciImage.MakeFormFromUserControl(this.appWorkspace1_MakeFormFromUsercontrol);
           
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 350);
            this.Controls.Add(this.appWorkspace1);
            this.IsMdiContainer = true;
            this.Name = "Form1";
            this.Text = "SciImage";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion


        private SciImage.AppWorkspace appWorkspace1;
    }
}

