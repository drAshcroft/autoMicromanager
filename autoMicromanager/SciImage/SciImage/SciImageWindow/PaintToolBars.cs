using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SciImage.Core.ColorsAndPixelOps;


namespace SciImage
{
    public partial class PaintToolBars : Form 
    {
        public PaintToolBars()
        {
            InitializeComponent();
            this.colorsFormControl1.UserPrimaryColor = ColorBgra.Black;// ().;// ((SciImage.ColorPixelBase)(resources.GetObject("colorsFormControl1.UserPrimaryColor")));
            this.colorsFormControl1.UserSecondaryColor = ColorBgra.White;  //)(resources.GetObject("colorsFormControl1.UserSecondaryColor")));
            this.colorsFormControl1.WhichUserColor = SciImage.WhichUserColor.Primary;

        }

       

        private void PaintToolBars_Resize(object sender, EventArgs e)
        {
           /* if (DockState.ToString() == "Document" || DockState.ToString() == "DockTop" || DockState.ToString() == "DockBottom")
            {
                historyFormControl1.Left = 0;
                historyFormControl1.Width = (int)(this.Width * .27);
                historyFormControl1.Height = this.Height;

                layerFormControl1.Left = (int)(this.Width * .27);
                layerFormControl1.Width = (int)(this.Width * .27);
                layerFormControl1.Height = this.Height;

                colorsFormControl1.Left = (int)(this.Width * .27 * 2);
                colorsFormControl1.Width = (int)(this.Width * .45);
                colorsFormControl1.Height = this.Height;

                historyFormControl1.Top = 0;
                layerFormControl1.Top = 0;
                colorsFormControl1.Top = 0;
            }
            else
            {*/
                historyFormControl1.Top = 0;
                historyFormControl1.Height = (int)(this.Height * .27);
                historyFormControl1.Width = this.Width;

                layerFormControl1.Top = (int)(this.Height * .27);
                layerFormControl1.Height = (int)(this.Height * .27);
                layerFormControl1.Width = this.Width;

                colorsFormControl1.Top = (int)(this.Height * .27 * 2);
                colorsFormControl1.Height = (int)(this.Height * .45);
                colorsFormControl1.Width = this.Width;

                historyFormControl1.Left = 0;
                layerFormControl1.Left = 0;
                colorsFormControl1.Left = 0;              
           // }
           
            
        }

      
      
    }
}
