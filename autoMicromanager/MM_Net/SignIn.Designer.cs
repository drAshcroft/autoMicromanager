namespace Micromanager_net
{
    partial class SignIn
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
            this.lbConfigs = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bLoadConfig = new System.Windows.Forms.Button();
            this.bCreateConfig = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbExperimentFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lbConfigs
            // 
            this.lbConfigs.FormattingEnabled = true;
            this.lbConfigs.Location = new System.Drawing.Point(12, 25);
            this.lbConfigs.Name = "lbConfigs";
            this.lbConfigs.Size = new System.Drawing.Size(363, 147);
            this.lbConfigs.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Config File";
            // 
            // bLoadConfig
            // 
            this.bLoadConfig.Location = new System.Drawing.Point(381, 25);
            this.bLoadConfig.Name = "bLoadConfig";
            this.bLoadConfig.Size = new System.Drawing.Size(85, 37);
            this.bLoadConfig.TabIndex = 2;
            this.bLoadConfig.Text = "Load External Config";
            this.bLoadConfig.UseVisualStyleBackColor = true;
            this.bLoadConfig.Click += new System.EventHandler(this.bLoadConfig_Click);
            // 
            // bCreateConfig
            // 
            this.bCreateConfig.Location = new System.Drawing.Point(381, 68);
            this.bCreateConfig.Name = "bCreateConfig";
            this.bCreateConfig.Size = new System.Drawing.Size(85, 26);
            this.bCreateConfig.TabIndex = 3;
            this.bCreateConfig.Text = "Create Config";
            this.bCreateConfig.UseVisualStyleBackColor = true;
            this.bCreateConfig.Click += new System.EventHandler(this.bCreateConfig_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Experiment Folder";
            // 
            // tbExperimentFolder
            // 
            this.tbExperimentFolder.Location = new System.Drawing.Point(12, 201);
            this.tbExperimentFolder.Name = "tbExperimentFolder";
            this.tbExperimentFolder.Size = new System.Drawing.Size(363, 20);
            this.tbExperimentFolder.TabIndex = 5;
            this.tbExperimentFolder.TextChanged += new System.EventHandler(this.tbExperimentFolder_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(381, 201);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 20);
            this.button1.TabIndex = 6;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(372, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(272, 227);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "OK";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 258);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbExperimentFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bCreateConfig);
            this.Controls.Add(this.bLoadConfig);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbConfigs);
            this.Name = "SignIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sign In";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbConfigs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bLoadConfig;
        private System.Windows.Forms.Button bCreateConfig;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbExperimentFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}