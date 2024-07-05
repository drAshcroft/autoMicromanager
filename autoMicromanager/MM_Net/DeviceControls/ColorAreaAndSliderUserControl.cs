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
namespace CoreDevices.DeviceControls
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using Colors;

	// ----------------------------------------------------------------------
	#endregion

	/// <summary>
	/// 
	/// </summary>
	public partial class ColorAreaAndSliderUserControl : 
		UserControl
	{

      

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="ColorAreaAndSliderUserControl"/> class.
		/// </summary>
		public ColorAreaAndSliderUserControl()
		{
			InitializeComponent();
		}

		private void colorAreaControl_HueSaturationChanged( 
			object sender, 
			EventArgs e )
		{
			double h, s;
			colorAreaControl.GetHueSaturation( out h, out s );

			colorSliderControl.SetHueSaturation( h, s );
			notifyColorChanged();
		}

		/// <summary>
		/// Occurs when the user changed the color.
		/// </summary>
		public event EventHandler ColorChanged;

		/// <summary>
		/// Occurs when a value has been changed.
		/// </summary>
		public event EventHandler ValueChangedByUser;

        public Colors.HsvColor HsvColor
        {
            get
            {

                return  HsvColor.FromColor ( colorSliderControl.GetSelectedColor());
            }

            set
            {
                Color c = value.ToColor();
                if (colorSliderControl.GetSelectedColor() != c)
                {
                    SelectedColor = c;
                }
            }
        }

		private void notifyColorChanged()
		{
			if ( ColorChanged != null )
			{
				ColorChanged( this, EventArgs.Empty );
			}
		}

		private void notifyValueChangedByUser()
		{
			if ( ValueChangedByUser != null )
			{
				ValueChangedByUser( this, EventArgs.Empty );
			}
		}

		/// <summary>
		/// Gets or sets the color of the selected.
		/// </summary>
		/// <value>The color of the selected.</value>
		public Color SelectedColor
		{
			get
			{
				return colorSliderControl.GetSelectedColor();
			}
			set
			{
                DeviceControls.Colors.HslColor hslColor = DeviceControls.Colors.HslColor.FromColor(value);

				colorAreaControl.SetHueSaturation(
					hslColor.PreciseHue,
					hslColor.PreciseSaturation );

				colorSliderControl.SetHueSaturation(
					hslColor.PreciseHue,
					hslColor.PreciseSaturation );

				colorSliderControl.SetLight(
					hslColor.PreciseLight );

				notifyColorChanged();
			}
		}

		private void colorSliderControl_BrightnessChanged( 
			object sender, 
			EventArgs e )
		{
			notifyColorChanged();
		}

		private void colorAreaControl_ValueChangedByUser( object sender, EventArgs e )
		{
			notifyColorChanged();
			notifyValueChangedByUser();
		}

		private void colorSliderControl_ValueChangedByUser( object sender, EventArgs e )
		{
			notifyColorChanged();
			notifyValueChangedByUser();
		}
	}
}