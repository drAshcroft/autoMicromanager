namespace CoreDevices.DeviceControls
{
    partial class FocusSlider
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FocusSlider));
            this.MoveUpLots = new System.Windows.Forms.PictureBox();
            this.MoveUp = new System.Windows.Forms.PictureBox();
            this.MoveDown = new System.Windows.Forms.PictureBox();
            this.MoveDownLots = new System.Windows.Forms.PictureBox();
            this.tMUp = new System.Windows.Forms.Timer(this.components);
            this.tmDown = new System.Windows.Forms.Timer(this.components);
            this.tmUpFast = new System.Windows.Forms.Timer(this.components);
            this.tmDownFast = new System.Windows.Forms.Timer(this.components);
            this.colorSlider1 = new DeviceControls .ColorSlider();
            ((System.ComponentModel.ISupportInitialize)(this.MoveUpLots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveDownLots)).BeginInit();
            this.SuspendLayout();
            // 
            // MoveUpLots
            // 
            this.MoveUpLots.Image = ((System.Drawing.Image)(resources.GetObject("MoveUpLots.Image")));
            this.MoveUpLots.Location = new System.Drawing.Point(0, 0);
            this.MoveUpLots.Name = "MoveUpLots";
            this.MoveUpLots.Size = new System.Drawing.Size(30, 33);
            this.MoveUpLots.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MoveUpLots.TabIndex = 9;
            this.MoveUpLots.TabStop = false;
            this.MoveUpLots.Click += new System.EventHandler(this.pictureBox1_Click);
            this.MoveUpLots.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveUpLots_MouseDown);
            this.MoveUpLots.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveUpLots_MouseUp);
            // 
            // MoveUp
            // 
            this.MoveUp.Image = ((System.Drawing.Image)(resources.GetObject("MoveUp.Image")));
            this.MoveUp.Location = new System.Drawing.Point(-3, 39);
            this.MoveUp.Name = "MoveUp";
            this.MoveUp.Size = new System.Drawing.Size(30, 50);
            this.MoveUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MoveUp.TabIndex = 10;
            this.MoveUp.TabStop = false;
            this.MoveUp.Click += new System.EventHandler(this.MoveUp_Click);
            this.MoveUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveUp_MouseDown);
            this.MoveUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveUp_MouseUp);
            // 
            // MoveDown
            // 
            this.MoveDown.Image = ((System.Drawing.Image)(resources.GetObject("MoveDown.Image")));
            this.MoveDown.Location = new System.Drawing.Point(0, 311);
            this.MoveDown.Name = "MoveDown";
            this.MoveDown.Size = new System.Drawing.Size(30, 50);
            this.MoveDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MoveDown.TabIndex = 11;
            this.MoveDown.TabStop = false;
            this.MoveDown.Click += new System.EventHandler(this.MoveDown_Click);
            this.MoveDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveDown_MouseDown);
            this.MoveDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveDown_MouseUp);
            // 
            // MoveDownLots
            // 
            this.MoveDownLots.Image = ((System.Drawing.Image)(resources.GetObject("MoveDownLots.Image")));
            this.MoveDownLots.Location = new System.Drawing.Point(-3, 367);
            this.MoveDownLots.Name = "MoveDownLots";
            this.MoveDownLots.Size = new System.Drawing.Size(30, 33);
            this.MoveDownLots.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MoveDownLots.TabIndex = 12;
            this.MoveDownLots.TabStop = false;
            this.MoveDownLots.Click += new System.EventHandler(this.MoveDownLots_Click);
            this.MoveDownLots.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveDownLots_MouseDown);
            this.MoveDownLots.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveDownLots_MouseUp);
            // 
            // tMUp
            // 
            this.tMUp.Tick += new System.EventHandler(this.tMUp_Tick);
            // 
            // tmDown
            // 
            this.tmDown.Tick += new System.EventHandler(this.tmDown_Tick);
            // 
            // tmUpFast
            // 
            this.tmUpFast.Tick += new System.EventHandler(this.tmUpFast_Tick);
            // 
            // tmDownFast
            // 
            this.tmDownFast.Tick += new System.EventHandler(this.tmDownFast_Tick);
            // 
            // colorSlider1
            // 
            this.colorSlider1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.colorSlider1.BackColor = System.Drawing.Color.Transparent;
            this.colorSlider1.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.colorSlider1.LargeChange = ((uint)(5u));
            this.colorSlider1.Location = new System.Drawing.Point(1, 50);
            this.colorSlider1.Name = "colorSlider1";
            this.colorSlider1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.colorSlider1.Size = new System.Drawing.Size(26, 293);
            this.colorSlider1.SmallChange = ((uint)(1u));
            this.colorSlider1.TabIndex = 4;
            this.colorSlider1.Text = "colorSlider1";
            this.colorSlider1.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            this.colorSlider1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.colorSlider1_Scroll);
            // 
            // FocusSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MoveDownLots);
            this.Controls.Add(this.MoveDown);
            this.Controls.Add(this.MoveUp);
            this.Controls.Add(this.MoveUpLots);
            this.Controls.Add(this.colorSlider1);
            this.Name = "FocusSlider";
            this.Size = new System.Drawing.Size(30, 400);
            this.Resize += new System.EventHandler(this.FocusSlider_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.MoveUpLots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MoveDownLots)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ColorSlider colorSlider1;
        private System.Windows.Forms.PictureBox MoveUpLots;
        private System.Windows.Forms.PictureBox MoveUp;
        private System.Windows.Forms.PictureBox MoveDown;
        private System.Windows.Forms.PictureBox MoveDownLots;
        private System.Windows.Forms.Timer tMUp;
        private System.Windows.Forms.Timer tmDown;
        private System.Windows.Forms.Timer tmUpFast;
        private System.Windows.Forms.Timer tmDownFast;
    }
}
