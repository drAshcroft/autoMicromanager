namespace Micromanager_net.Setup
{
    partial class SetProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetProperties));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.propertyList1 = new CoreDevices.DeviceControls.PropertyList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PropertyValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lSave = new System.Windows.Forms.Label();
            this.PropertyPanel = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(644, 34);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Device:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(580, 526);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "Next Device";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // propertyList1
            // 
            this.propertyList1.AutoScroll = true;
            this.propertyList1.Location = new System.Drawing.Point(15, 76);
            this.propertyList1.Name = "propertyList1";
            this.propertyList1.Size = new System.Drawing.Size(651, 434);
            this.propertyList1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.PropertyValue);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lSave);
            this.panel1.Controls.Add(this.PropertyPanel);
            this.panel1.Location = new System.Drawing.Point(1, 76);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 444);
            this.panel1.TabIndex = 4;
            // 
            // PropertyValue
            // 
            this.PropertyValue.AutoSize = true;
            this.PropertyValue.Location = new System.Drawing.Point(239, 0);
            this.PropertyValue.Name = "PropertyValue";
            this.PropertyValue.Size = new System.Drawing.Size(76, 13);
            this.PropertyValue.TabIndex = 8;
            this.PropertyValue.Text = "Property Value";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(83, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Property Name";
            // 
            // lSave
            // 
            this.lSave.AutoSize = true;
            this.lSave.Location = new System.Drawing.Point(3, 0);
            this.lSave.Name = "lSave";
            this.lSave.Size = new System.Drawing.Size(74, 13);
            this.lSave.TabIndex = 6;
            this.lSave.Text = "Save Property";
            // 
            // PropertyPanel
            // 
            this.PropertyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyPanel.Location = new System.Drawing.Point(3, 27);
            this.PropertyPanel.Name = "PropertyPanel";
            this.PropertyPanel.Size = new System.Drawing.Size(685, 382);
            this.PropertyPanel.TabIndex = 5;
            // 
            // SetProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 559);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.propertyList1);
            this.Controls.Add(this.label1);
            this.Name = "SetProperties";
            this.Text = "SetProperties";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CoreDevices.DeviceControls.PropertyList propertyList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PropertyPanel;
        private System.Windows.Forms.Label PropertyValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lSave;
    }
}