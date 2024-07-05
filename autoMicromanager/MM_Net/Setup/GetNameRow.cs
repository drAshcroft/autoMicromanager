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
    public partial class GetNameRow : UserControl
    {
        public GetNameRow()
        {
            InitializeComponent();
        }
        public string Label
        {
            set { label1.Text = value; }
            get { return label1.Text; }
        }
        public string Text
        {
            set { textBox1.Text = value; }
            get { return textBox1.Text; }
        }
        private int listindex;
        public int ListIndex
        {
            get { return listindex; }
            set { listindex = value; }
        } 
        /*public ComboBox Guis
        {
            get { return comboBox1; }
        }*/
    }
}
