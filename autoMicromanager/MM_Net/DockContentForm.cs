// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
//                License text is included with the source distribution.
//
//                This file is distributed in the hope that it will be useful,
//                but WITHOUT ANY WARRANTY; without even the implied warranty
//                of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//                IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//                CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace Micromanager_net
{
    /// <summary>
    /// Provides a carrier class for forms outputed by the viewer.  Very simple performance
    /// </summary>
    public partial class DockContentForm : DockContent 
    {
        public DockContentForm()
        {
            InitializeComponent();
        }
        public DockContentForm(UserControl NewControl)
        {
            InitializeComponent();
            this.ToolsControl = NewControl;
        }
        private UserControl  ToolsFormControl1;
        public UserControl ToolsControl
        {
            get { return ToolsFormControl1; }
            set
            {
                this.ToolsFormControl1 = value;
                UserControl uc = value;
                ExtraInformation = uc.GetType().ToString();
                this.SuspendLayout();
                // 
                // button1
                // 
                uc.Location = new System.Drawing.Point(0, 0);
                uc.TabIndex = 0;
                // 
                // ToolBar
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(uc.Width, uc.Height);
                Size s = SizeFromClientSize(new System.Drawing.Size(uc.Width, uc.Height));
                this.Size = s;
                this.Width = s.Width;
                this.Height = s.Height;

                this.MinimumSize = new Size(s.Width, s.Height);
                
                this.ControlBox = false;
                this.Controls.Add(uc);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                this.Name = uc.Name;
                this.ShowInTaskbar = false;
                this.Text = uc.Name;
                this.ResumeLayout(false);
                this.Text = "";
            }
        }

        private void DockContentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            Controls.Remove(ToolsFormControl1);
            ToolsFormControl1 = null;
        }
    }
}
