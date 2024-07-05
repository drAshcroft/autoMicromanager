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
		// Retrieve these once, the dimensions of the picture stored in the resources.
		private static readonly float ImageWidth = Micromanager_App.Properties.Resources.car.Width;
		private static readonly float ImageHeight = Properties.Resources.car.Height;

		// Used to determine how much to scale the picture of the car by.
		private float scalingFactor = 1;
		// Stores the dimenions of the picture (when scaled and centred).
		private RectangleF picture;

		// Stores the dimensions of the rectangles to draw the text inside.
		private RectangleF boxFront;
		private RectangleF boxCross1;
		private RectangleF boxCross2;
		private RectangleF boxFrontL;
		private RectangleF boxFrontR;
		private RectangleF boxRearL;
		private RectangleF boxRearR;
		private RectangleF boxBack;
		private RectangleF boxTotal;

		// Fired when the control is resize, recalculates the position of everything.
		protected override void OnResize(EventArgs e)
		{
			// Calculate the scaling factor
			float sx = (base.DisplayRectangle.Width / ImageWidth);
			float sy = (base.DisplayRectangle.Height / (ImageHeight * 1.4125f));
			// Pick the smallest scaling factor to keep the picture inside the control
			this.scalingFactor = Math.Min(sx, sy);
			// Determine the coordinates of the picture
			float picWidth = ImageWidth * this.scalingFactor;
			float picHeight = ImageHeight * this.scalingFactor;
			float sideWidth = (base.DisplayRectangle.Width - picWidth) / 2;
			// This is the height of the front wheels for arguments sake.
			float boxHeight = (picHeight * 0.1375f);
			/*
			 * Calculates the font size, creates a Graphics canvas so that we can retrieve
			 * the current resolution of the display in the y direction (dpi).  Once that is
			 * done then font size (in points) = (height in pixels * ((72 points / inch) / DpiY))
			 */
			using (Graphics gfx = this.CreateGraphics())
			{
				this.Font = new Font(this.Font.FontFamily, boxHeight * (72 / gfx.DpiY));
			}
			// Display rectangle of the picture
			this.picture = new RectangleF(sideWidth, boxHeight, picWidth, picHeight);
			// Calculate coordinates of textboxes (percentages of car wheels, padding is FontHeight)
			this.boxCross1 = new RectangleF(this.FontHeight, 0, sideWidth - (this.FontHeight * 2), boxHeight);
			this.boxCross2 = new RectangleF(this.picture.Right + this.FontHeight, 0, sideWidth - (this.FontHeight * 2), boxHeight);
			this.boxFront = new RectangleF(this.picture.Left, 0, this.picture.Width, boxHeight);
			this.boxFrontL = new RectangleF(this.FontHeight, (0.2884f * picHeight), sideWidth - (this.FontHeight * 2), boxHeight);
			this.boxFrontR = new RectangleF(this.picture.Right + this.FontHeight, (0.2884f * picHeight), sideWidth - (this.FontHeight * 2), boxHeight);
			this.boxRearL = new RectangleF(this.FontHeight, (0.9434f * picHeight), sideWidth - (this.FontHeight * 2), (0.1429f * picHeight));
			this.boxRearR = new RectangleF(this.picture.Right + this.FontHeight, (0.9434f * picHeight), sideWidth - (this.FontHeight * 2), (0.1429f * picHeight));
			this.boxBack = new RectangleF(this.picture.Left - ((sideWidth - this.picture.Width) / 2), this.picture.Bottom, sideWidth, boxHeight);
			this.boxTotal = new RectangleF(this.picture.Left - ((sideWidth - this.picture.Width) / 2), this.picture.Bottom + boxHeight, sideWidth, boxHeight);
			// Fire the base resize event to continue the "bubbling" of event handles
			base.OnResize(e);
		}
	}
}