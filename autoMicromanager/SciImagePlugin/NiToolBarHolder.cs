using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SciImage
{
    /// <summary>
    /// A stationary usercontrol that can be used from labview to show a toolbar
    /// </summary>
    public partial class NiToolBarHolder : UserControl
    {
        public NiToolBarHolder()
        {
            InitializeComponent();
        }

        private UserControl ToolsFormControl1;
        /// <summary>
        /// SciImage Toolbar that should be shown
        /// </summary>
        public UserControl ToolsControl
        {
            get { return ToolsFormControl1; }
            set
            {
                if (value !=null)
                {
                    this.ToolsFormControl1 = value;
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
                    this.ToolsFormControl1.Dock = DockStyle.Fill;
                    // 
                    // ToolsForm
                    // 
                    this.Controls.Add(this.ToolsFormControl1);
                }

            }
        }
    }
}
