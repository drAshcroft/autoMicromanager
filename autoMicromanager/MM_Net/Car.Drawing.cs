using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Micromanager_net
{
	partial class Car
	{
		/* These are for laying out the text in the rectangles, they are static (shared)
		 * so that they are only created once per instance of the control. */
		private static readonly StringFormat sfLeft;
		private static readonly StringFormat sfCenter;
		private static readonly StringFormat sfRight;
		private Pen ThickPen = new Pen(Color.Blue, 4);

		static Car()
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


			// Draw the picture
			e.Graphics.DrawImage(Properties.Resources.car, this.picture);

			// Draw Total Box
			e.Graphics.FillRectangle(Brushes.RosyBrown, this.boxTotal);
			e.Graphics.DrawRectangle(Pens.Black, Rectangle.Truncate(this.boxTotal));

			// Draw other values
			e.Graphics.DrawString(this.Cross1.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxCross1, sfRight);
			e.Graphics.DrawString(this.Cross1.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxCross2, sfLeft);
			e.Graphics.DrawString(this.Front.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxFront, sfCenter);
			e.Graphics.DrawString(this.FrontL.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxFrontL, sfRight);
			e.Graphics.DrawString(this.FrontR.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxFrontR, sfLeft);
			e.Graphics.DrawString(this.RearL.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxRearL, sfRight);
			e.Graphics.DrawString(this.RearR.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxRearR, sfLeft);
			e.Graphics.DrawString(this.Back.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxBack, sfCenter);
			e.Graphics.DrawString(this.Total.ToString(this.FloatFormat), this.Font, Brushes.Black, this.boxTotal, sfCenter);

			// Draw Blue Lines
			e.Graphics.DrawLine(this.ThickPen, (this.picture.Left - (this.FontHeight * (2f / 3f))), (this.picture.Top + (this.picture.Height * 0.1509f) - this.FontHeight), this.picture.Left, (this.picture.Top + (this.picture.Height * 0.1509f)));
			e.Graphics.DrawLine(this.ThickPen, (this.picture.Right + (this.FontHeight * (2f / 3f))), (this.picture.Top + (this.picture.Height * 0.1509f) - this.FontHeight), this.picture.Right, (this.picture.Top + (this.picture.Height * 0.1509f)));
			e.Graphics.DrawLine(this.ThickPen, (this.picture.Left - (this.FontHeight * (2f / 3f))), this.boxTotal.Top, this.picture.Left, (this.picture.Bottom - (this.picture.Height * 0.0512f)));
			e.Graphics.DrawLine(this.ThickPen, (this.picture.Right + (this.FontHeight * (2f / 3f))), this.boxTotal.Top, this.picture.Right, (this.picture.Bottom - (this.picture.Height * 0.0512f)));
		}
	}
}