using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Micromanager_net.Setup
{
    public partial class GetGUIRow : UserControl
    {
        public GetGUIRow()
        {
            InitializeComponent();
        }
        public string Label
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public ComboBox GUIs
        {
            get { return comboBox1; }
        }
        private int listindex;
        public int ListIndex
        {
            get { return listindex; }
            set { listindex = value; }
        } 

    }
}
