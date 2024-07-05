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

namespace SciImage
{
    public partial class DockingToolsBar : DockContent
    {
        public  void ResetSize()
        {
            this.ClientSize = new Size(ToolsFormControl1.Width+5, ToolsFormControl1.Height+5);
        }
        protected override void OnLoad(EventArgs e)
        {

            ToolsFormControl1.ForceResize();
            base.OnLoad(e);
            this.ClientSize = new Size(ToolsFormControl1.Width+5, ToolsFormControl1.Height+5);
        }

        public DockingToolsBar()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            if (ToolsControl!=null)
                this.Width = ToolsControl.Width+5;
           // this.Text = PdnResources.GetString("MainToolBarForm.Text");
        }
        public DockingToolsBar(SciImage.ToolsFormControl tfc)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            ToolsFormControl1 = tfc;
            this.Width = ToolsControl.Width + 5;
            this.ToolsControl = tfc;
            this.ClientSize = new System.Drawing.Size(tfc.Width, tfc.Height);
            Size s = SizeFromClientSize(new System.Drawing.Size(tfc.Width, tfc.Height));
            this.Size = s;
            this.Width = s.Width;
            this.Height = s.Height;
            this.MinimumSize = s;
            this.Text = "";
            // this.Text = PdnResources.GetString("MainToolBarForm.Text");
        }


        private SciImage.ToolsFormControl ToolsFormControl1;
        public SciImage.ToolsFormControl ToolsControl
        {
            get { return ToolsFormControl1; }
            set
            {
                this.ToolsFormControl1 = value ;
                this.SuspendLayout();
                // 
                // ToolsFormControl1
                // 
                this.ToolsFormControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
                this.ToolsFormControl1.BackColor = System.Drawing.SystemColors.Control;
                this.ToolsFormControl1.Location = new System.Drawing.Point(0, 0);
                this.ToolsFormControl1.MinimumSize = new System.Drawing.Size(30, 158);
                this.ToolsFormControl1.Name = "ToolsFormControl1";
                this.ToolsFormControl1.Size = new System.Drawing.Size(51, 219);
                this.ToolsFormControl1.TabIndex = 0;
                   // 
                // ToolsForm
                // 
                this.Controls.Add(this.ToolsFormControl1);
              
            }
        }
      

        private void ToolsBar_Load(object sender, EventArgs e)
        {
            ToolsFormControl1.ToolsControl .ForceVisible();
        }

        private void ToolsBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.Hide();
            Controls.Remove(ToolsFormControl1);
            ToolsFormControl1 = null;
            
           
        }

        private void ToolsBar_Resize(object sender, EventArgs e)
        {
            try
            {
                this.ClientSize = new Size(ToolsFormControl1.Width + 5, ToolsFormControl1.Height + 5);
            }
            catch { }
           
        }
    }
}
