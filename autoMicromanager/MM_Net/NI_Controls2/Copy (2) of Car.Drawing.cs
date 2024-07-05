using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Micromanager_net
{
	partial class PictureBox
	{
		/* These are for laying out the text in the rectangles, they are static (shared)
		 * so that they are only created once per instance of the control. */
		private static readonly StringFormat sfLeft;
		private static readonly StringFormat sfCenter;
		private static readonly StringFormat sfRight;
		private Pen ThickPen = new Pen(Color.Blue, 4);

		static PictureBox()
		{
			sfLeft = new StringFormat();
			sfCenter = new StringFormat();
			sfRight = new StringFormat();
			// Line Alignment
			sfLeft.LineAlignment = StringAlignment.Center;
			sfCenter.LineAlignment = StringAlignment.Center;
			sfRight.LineAlignment = StringAlignment.Center;
			// Alignment
			sfLeft.Alignment = StringAlignment.Near;
			sfCenter.Alignment = StringAlignment.Center;
			sfRight.Alignment = StringAlignment.Far;
		}
       
        
		protected override void OnPaint(PaintEventArgs e)
		{
           
			// NOTE: Ordering here is important because some things are drawn on top of each other
			// Call the Paint routine in the base class (for clearing background etc).
			base.OnPaint(e);
            
            e.Graphics.Clear(Color.Red);
            if (MyImage != null)
            {
                // Draw the picture
                e.Graphics.DrawImage(MyImage, new Point(0, 0));
            }
			
		}
	}
}