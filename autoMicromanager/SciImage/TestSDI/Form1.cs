using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Test
{
    public partial class Form1 : Form
    {
        private void CreateBasicLayout()
         {

             //appWorkspace1.ShowToolsbars();
             
             ToolBar tb = new ToolBar();
             tb.AddUserControl(appWorkspace1.Widgets.ToolsForm);
             ToolBar cb = new ToolBar();
             cb.AddUserControl(appWorkspace1.Widgets.ColorsForm );
             tb.Show(this  );
             cb.Show(this );
            
             

         }
      
        public Form1()
        {
            InitializeComponent();


            CreateBasicLayout();
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            appWorkspace1.SetLayerWithImage(0,new Bitmap("TestImage.bmp"));

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            appWorkspace1.Dispose();  
        }

        private Form appWorkspace1_MakeFormFromUsercontrol(UserControl Control)
        {
            ToolBar tb = new ToolBar();
            tb.AddUserControl(Control);
            
            
            tb.Show(this);
            
            //tb.SetBounds(0, 0, s.Width, s.Height);
            return tb;
        }

        private void appWorkspace1_MakeNewForm(object sender, Form NewForm)
        {
            System.Diagnostics.Debug.Print(NewForm.GetType().ToString());
        }

       
     
    }
}
