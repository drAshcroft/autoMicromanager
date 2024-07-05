/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SciImage
{
    public class ToolsFormControl
        : UserControl 
    {
        private ToolsControl toolsControl = null;
        private SciImage.ColorPickers.ColorDisplayWidget colorDisplayWidget1;

        public SciImage.ColorPickers.ColorDisplayWidget ColorDisplay
        {
            get
            {
                return this.colorDisplayWidget1;
            }
        }

        public ToolsControl ToolsControl
        {
            
            get
            {
                return this.toolsControl;
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public event EventHandler OnResize;
        public void ForceResize()
        {
            this.ClientSize = new Size(toolsControl.Width, toolsControl.Height+colorDisplayWidget1.Height );
            this.Width =toolsControl.Width;
            this.Height = toolsControl.Height + colorDisplayWidget1.Height;
            colorDisplayWidget1.Top = toolsControl.Height;
            colorDisplayWidget1.Left = 0;
            if (OnResize != null) OnResize(this, EventArgs.Empty);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ClientSize = new Size(toolsControl.Width, toolsControl.Height);
            colorDisplayWidget1.Top = toolsControl.Height;
            colorDisplayWidget1.Left = 0;
        }

        public ToolsFormControl()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            colorDisplayWidget1.Top = toolsControl.Height;
            colorDisplayWidget1.Left = 0;
            this.Width = toolsControl.Width;
            this.Height = toolsControl.Height + colorDisplayWidget1.Height;
           // this.Text = PdnResources.GetString("MainToolBarForm.Text");
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolsFormControl));
            this.colorDisplayWidget1 = new SciImage.ColorPickers.ColorDisplayWidget();
            this.toolsControl = new SciImage.ToolsControl();
            this.SuspendLayout();
            // 
            // colorDisplayWidget1
            // 
            this.colorDisplayWidget1.Location = new System.Drawing.Point(0, 219);
            this.colorDisplayWidget1.Name = "colorDisplayWidget1";
            this.colorDisplayWidget1.Size = new System.Drawing.Size(50, 48);
            this.colorDisplayWidget1.TabIndex = 1;
              // 
            // toolsControl
            // 
            this.toolsControl.BackColor = System.Drawing.SystemColors.Control;
            //this.toolsControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolsControl.Location = new System.Drawing.Point(0, 0);
            this.toolsControl.Name = "toolsControl";
            this.toolsControl.Size = new System.Drawing.Size(50, 219);
            this.toolsControl.TabIndex = 0;
            this.toolsControl.RelinquishFocus += new System.EventHandler(this.ToolsControl_RelinquishFocus);
            this.toolsControl.OnResize += new EventHandler(toolsControl_OnResize);
            // 
            // ToolsFormControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.colorDisplayWidget1);
            this.Controls.Add(this.toolsControl);
            this.Name = "ToolsFormControl";
            this.Size = new System.Drawing.Size(50, 273);
            this.ResumeLayout(false);

        }

        void toolsControl_OnResize(object sender, EventArgs e)
        {
            ForceResize();
        }
        #endregion

        private void ToolsControl_RelinquishFocus(object sender, EventArgs e)
        {
            //OnRelinquishFocus();
        }
    }
}
