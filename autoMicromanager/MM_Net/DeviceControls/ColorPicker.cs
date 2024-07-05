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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoreDevices.DeviceControls
{
    public delegate void OnColorPickedEvent(object sender,Color newColor);
    public partial class ColorPicker : UserControl
    {
        public event OnColorPickedEvent OnColorPicked;

        public Color SelectedColor
        {
            get { return WavelengthShower.BackColor; }
            set
            {
                colorAreaAndSliderUserControl1.SelectedColor = value;
                WavelengthShower.BackColor = value;
            }
        }

        public void SetColor(Color value)
        {
            colorAreaAndSliderUserControl1.SelectedColor=value ;
            WavelengthShower.BackColor = value;

        }

        public ColorPicker()
        {
            InitializeComponent();
        }

        private void slideAndTextl1_OnValueChanged(object sender, float value)
        {
            UpdateColor();
        }

        private void SaTIntensity_OnValueChanged(object sender, float value)
        {
            UpdateColor();
        }
        private void UpdateColor()
        {
            int Wavelength =(int) SaTWavelength.Value ;
            int Intensity = (int) SaTIntensity.Value ;

            WavelengthShower.BackColor = Generals.ColorConversions.getColorFromWaveLength(Wavelength,Intensity );
            if (WavelengthShower.BackColor != colorAreaAndSliderUserControl1.SelectedColor)
                colorAreaAndSliderUserControl1.SelectedColor = WavelengthShower.BackColor;
        }

        private void colorAreaAndSliderUserControl1_ColorChanged(object sender, EventArgs e)
        {
            WavelengthShower.BackColor = colorAreaAndSliderUserControl1.SelectedColor;
            if (OnColorPicked != null) OnColorPicked(this, WavelengthShower.BackColor);
        }

       
    }
}
