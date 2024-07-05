namespace Micromanager_net
{
    partial class FunctionGeneratorForm
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
            this.frequencyGenerator1 = new Micromanager_net.UI.FrequencyGenerator();
            this.SuspendLayout();
            // 
            // frequencyGenerator1
            // 
            this.frequencyGenerator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frequencyGenerator1.Location = new System.Drawing.Point(0, 0);
            this.frequencyGenerator1.Name = "frequencyGenerator1";
            this.frequencyGenerator1.Size = new System.Drawing.Size(292, 266);
            this.frequencyGenerator1.TabIndex = 0;
            // 
            // FunctionGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.frequencyGenerator1);
            this.Name = "FunctionGeneratorForm";
            this.TabText = "Function Generator";
            this.Text = "Function Generator";
            this.ResumeLayout(false);

        }

        #endregion

        private UI.FrequencyGenerator frequencyGenerator1;
    }
}