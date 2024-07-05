namespace Micromanager_net.UI
{
    partial class CoreLog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.refreshB = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.CoreLogRTB = new System.Windows.Forms.RichTextBox();
            this.coreErrors = new System.Windows.Forms.RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Experiment Log";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(571, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Current Errors";
            // 
            // refreshB
            // 
            this.refreshB.Location = new System.Drawing.Point(264, 3);
            this.refreshB.Name = "refreshB";
            this.refreshB.Size = new System.Drawing.Size(148, 29);
            this.refreshB.TabIndex = 4;
            this.refreshB.Text = "Refresh Corelog View";
            this.refreshB.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 40);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.CoreLogRTB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.coreErrors);
            this.splitContainer1.Size = new System.Drawing.Size(804, 210);
            this.splitContainer1.SplitterDistance = 529;
            this.splitContainer1.TabIndex = 5;
            // 
            // CoreLogRTB
            // 
            this.CoreLogRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CoreLogRTB.Location = new System.Drawing.Point(0, 0);
            this.CoreLogRTB.Name = "CoreLogRTB";
            this.CoreLogRTB.Size = new System.Drawing.Size(529, 210);
            this.CoreLogRTB.TabIndex = 1;
            this.CoreLogRTB.Text = "";
            // 
            // coreErrors
            // 
            this.coreErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.coreErrors.Location = new System.Drawing.Point(0, 0);
            this.coreErrors.Name = "coreErrors";
            this.coreErrors.Size = new System.Drawing.Size(271, 210);
            this.coreErrors.TabIndex = 4;
            this.coreErrors.Text = "";
            // 
            // CoreLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 262);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.refreshB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CoreLog";
            this.TabText = "CoreLog";
            this.Text = "CoreLog";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button refreshB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox CoreLogRTB;
        private System.Windows.Forms.RichTextBox coreErrors;
    }
}