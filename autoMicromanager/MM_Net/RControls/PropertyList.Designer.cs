namespace Micromanager_net.RControls
{
    partial class PropertyListO
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PropertyPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // PropertyPanel
            // 
            this.PropertyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyPanel.Location = new System.Drawing.Point(3, 16);
            this.PropertyPanel.Name = "PropertyPanel";
            this.PropertyPanel.Size = new System.Drawing.Size(282, 288);
            this.PropertyPanel.TabIndex = 2;
            // 
            // PropertyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.PropertyPanel);
            this.Name = "PropertyList";
            this.Size = new System.Drawing.Size(337, 337);
            this.Resize += new System.EventHandler(this.PropertyList_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PropertyPanel;
    }
}
