namespace SciImage
{
    partial class PaintToolBars
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaintToolBars));
            this.colorsFormControl1 = new SciImage.ColorPickers.ColorsFormControl();
            this.layerFormControl1 = new SciImage.LayerFormControl();
            this.historyFormControl1 = new SciImage.HistoryFormControl();
            this.SuspendLayout();
            // 
            // colorsFormControl1
            // 
            this.colorsFormControl1.BackColor = System.Drawing.SystemColors.Control;
            this.colorsFormControl1.Location = new System.Drawing.Point(2, 330);
            this.colorsFormControl1.Name = "colorsFormControl1";
            this.colorsFormControl1.PaletteCollection = null;
            this.colorsFormControl1.Size = new System.Drawing.Size(209, 217);
            this.colorsFormControl1.TabIndex = 4;
            
            this.colorsFormControl1.WhichUserColor = SciImage.WhichUserColor.Primary;
            // 
            // layerFormControl1
            // 
            this.layerFormControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.layerFormControl1.BackColor = System.Drawing.Color.White;
            this.layerFormControl1.Location = new System.Drawing.Point(1, 2);
            this.layerFormControl1.MinimumSize = new System.Drawing.Size(165, 158);
            this.layerFormControl1.Name = "layerFormControl1";
            this.layerFormControl1.Size = new System.Drawing.Size(210, 158);
            this.layerFormControl1.TabIndex = 3;
            // 
            // historyFormControl1
            // 
            this.historyFormControl1.BackColor = System.Drawing.Color.White;
            this.historyFormControl1.Location = new System.Drawing.Point(1, 166);
            this.historyFormControl1.MinimumSize = new System.Drawing.Size(165, 158);
            this.historyFormControl1.Name = "historyFormControl1";
            this.historyFormControl1.Size = new System.Drawing.Size(210, 158);
            this.historyFormControl1.TabIndex = 2;
            // 
            // PaintToolBars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 577);
            this.Controls.Add(this.colorsFormControl1);
            this.Controls.Add(this.layerFormControl1);
            this.Controls.Add(this.historyFormControl1);
            this.Name = "PaintToolBars";
            
            this.Text = "Drawing Tools";
          
            this.Resize += new System.EventHandler(this.PaintToolBars_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        public  SciImage.HistoryFormControl historyFormControl1;
        public SciImage.ColorPickers.ColorsFormControl colorsFormControl1;
        public SciImage.LayerFormControl layerFormControl1;
    }
}