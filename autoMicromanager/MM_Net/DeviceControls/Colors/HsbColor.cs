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
namespace CoreDevices.DeviceControls.Colors
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System.Diagnostics;
	using System.Drawing;
    using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Represents a HSV (=HSB) color space.
	/// http://en.wikipedia.org/wiki/HSV_color_space
	/// </summary>
	public sealed class HsbColor
	{
		#region Public static methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Creates from a given color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		public static HsbColor FromColor(
			Color color )
		{
			return ColorConverting.ColorToRgb( color ).ToHsbColor();
		}

		/// <summary>
		/// Creates from a given color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		public static HsbColor FromRgbColor(
			RgbColor color )
		{
			return color.ToHsbColor();
		}

		/// <summary>
		/// Creates from a given color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		public static HsbColor FromHsbColor(
			HsbColor color )
		{
			return new HsbColor( color.Hue, color.Saturation, color.Brightness );
		}

		/// <summary>
		/// Creates from a given color.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		public static HsbColor FromHslColor(
			HslColor color )
		{
			return FromRgbColor( color.ToRgbColor() );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="HsbColor"/> class.
		/// </summary>
		/// <param name="hue">The hue.</param>
		/// <param name="saturation">The saturation.</param>
		/// <param name="brightness">The brightness.</param>
		public HsbColor(
			int hue,
			int saturation,
			int brightness )
		{
			Hue = hue;
			Saturation = saturation;
			Brightness = brightness;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current 
		/// <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current
		///  <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				@"Hue: {0}; saturation: {1}; brightness: {2}.",
				Hue,
				Saturation,
				Brightness );
		}

		/// <summary>
		/// Returns the underlying .NET color.
		/// </summary>
		/// <returns></returns>
		public Color ToColor()
		{
			return ColorConverting.HsbToRgb( this ).ToColor();
		}

		/// <summary>
		/// Returns a RGB color.
		/// </summary>
		/// <returns></returns>
		public RgbColor ToRgbColor()
		{
			return ColorConverting.HsbToRgb( this );
		}

		/// <summary>
		/// Returns a HSB color.
		/// </summary>
		/// <returns></returns>
		public HsbColor ToHsbColor()
		{
			return this;
		}

		/// <summary>
		/// Returns a HSL color.
		/// </summary>
		/// <returns></returns>
		public HslColor ToHslColor()
		{
			return ColorConverting.RgbToHsl( ToRgbColor() );
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is 
		/// equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with 
		/// the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the 
		/// current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">The 
		/// <paramref name="obj"/> parameter is null.</exception>
		public override bool Equals(
			object obj )
		{
			var equal = false;

			if ( obj is HsbColor )
			{
				var hsb = (HsbColor)obj;

				if ( Hue == hsb.Hue && Saturation == hsb.Saturation &&
					Brightness == hsb.Brightness )
				{
					equal = true;
				}
			}

			return equal;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			Debug.Assert( 1 == 1 );
			return base.GetHashCode();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the hue. Values from 0 to 360.
		/// </summary>
		/// <value>The hue.</value>
		public int Hue
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the saturation. Values from 0 to 100.
		/// </summary>
		/// <value>The saturation.</value>
		public int Saturation
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the brightness. Values from 0 to 100.
		/// </summary>
		/// <value>The brightness.</value>
		public int Brightness
		{
			get;
			set;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Adapted from: 
    /// "A Primer on Building a Color Picker User Control with GDI+ in Visual Basic .NET or C#"
    /// http://www.msdnaa.net/Resources/display.aspx?ResID=2460
    /// </summary>
    public struct HsvColor
    {
        public int Hue; // 0-360
        public int Saturation; // 0-100
        public int Value; // 0-100

        public static bool operator ==(HsvColor lhs, HsvColor rhs)
        {
            if ((lhs.Hue == rhs.Hue) &&
                (lhs.Saturation == rhs.Saturation) &&
                (lhs.Value == rhs.Value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(HsvColor lhs, HsvColor rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return this == (HsvColor)obj;
        }

        public override int GetHashCode()
        {
            return (Hue + (Saturation << 8) + (Value << 16)).GetHashCode(); ;
        }

        public HsvColor(int hue, int saturation, int value)
        {
            if (hue < 0 || hue > 360)
            {
                throw new ArgumentOutOfRangeException("hue", "must be in the range [0, 360]");
            }

            if (saturation < 0 || saturation > 100)
            {
                throw new ArgumentOutOfRangeException("saturation", "must be in the range [0, 100]");
            }

            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException("value", "must be in the range [0, 100]");
            }

            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        public static HsvColor FromColor(Color color)
        {
            RgbColor rgb = new RgbColor(color.R, color.G, color.B);
            HsbColor l=rgb.ToHsbColor();
            return new HsvColor(l.Hue,l.Saturation , l.Brightness );// rgb.to();
        }

        public Color ToColor()
        {
            RgbColor rgb = ToRgb();
            return Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue);
        }

        public RgbColor ToRgb()
        {
            // HsvColor contains values scaled as in the color wheel:

            double h;
            double s;
            double v;

            double r = 0;
            double g = 0;
            double b = 0;

            // Scale Hue to be between 0 and 360. Saturation
            // and value scale to be between 0 and 1.
            h = (double)Hue % 360;
            s = (double)Saturation / 100;
            v = (double)Value / 100;

            if (s == 0)
            {
                // If s is 0, all colors are the same.
                // This is some flavor of gray.
                r = v;
                g = v;
                b = v;
            }
            else
            {
                double p;
                double q;
                double t;

                double fractionalSector;
                int sectorNumber;
                double sectorPos;

                // The color wheel consists of 6 sectors.
                // Figure out which sector you're in.
                sectorPos = h / 60;
                sectorNumber = (int)(Math.Floor(sectorPos));

                // get the fractional part of the sector.
                // That is, how many degrees into the sector
                // are you?
                fractionalSector = sectorPos - sectorNumber;

                // Calculate values for the three axes
                // of the color. 
                p = v * (1 - s);
                q = v * (1 - (s * fractionalSector));
                t = v * (1 - (s * (1 - fractionalSector)));

                // Assign the fractional colors to r, g, and b
                // based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }
            // return an RgbColor structure, with values scaled
            // to be between 0 and 255.
            return new RgbColor((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", Hue, Saturation, Value);
        }
    }
}