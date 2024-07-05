using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SciImage;

namespace PaintDNetWindow.PaintForms
{
    public partial class MDIChildForm : Form 
    {
        public MDIChildForm()
        {
            InitializeComponent();
        }
        private UserControl MyControl;
        public UserControl  MainUserControl
        {
            get { return MyControl; }
            set
            {
                MyControl = value;
                this.SuspendLayout();
                // 
                // ToolsFormControl1
                // 
                MyControl.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
                MyControl.BackColor = System.Drawing.SystemColors.Control;
                MyControl.Location = new System.Drawing.Point(0, 0);
                //MyControl.MinimumSize = new System.Drawing.Size(30, 158);
                MyControl.Name = "ToolsFormControl1";
                //MyControl.Size = new System.Drawing.Size(this.ClientSize.Width , this.ClientSize.Height );
                MyControl.TabIndex = 0;
                MyControl.Dock = DockStyle.Fill;
                this.Width = MyControl.Width;
                this.Height = MyControl.Height;
                  // 
                // ToolsForm
                // 
                this.Controls.Add(MyControl);
                this.Refresh();



            }
        }

        private void DefaultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Controls.Remove(MyControl);
            //e.Cancel = true;
            //this.Hide();
            MyControl = null;
        }

        private void OnResize()
        {
            MyControl.Size = this.ClientSize;
        }
        private void DefaultForm_Resize(object sender, EventArgs e)
        {
            OnResize();
        }

        private void DefaultForm_ResizeEnd(object sender, EventArgs e)
        {
            OnResize();
        }

        private void DefaultForm_SizeChanged(object sender, EventArgs e)
        {
            OnResize();
        }
    }
}
