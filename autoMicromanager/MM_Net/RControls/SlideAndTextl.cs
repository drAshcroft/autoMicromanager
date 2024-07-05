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

namespace Micromanager_net.RControls
{
    public delegate void  OnValueChangedEvent(object sender,float value);
    public partial class SlideAndTextl : UserControl
    {
        private float _MaxValue=100;
        private float _MinValue=0;
        private bool _LogScale = false;
        private float _Value;
        public event OnValueChangedEvent OnValueChanged;

        public SlideAndTextl()
        {
            InitializeComponent();
        }

        public float Value
        {
            get {return (_Value );}
            set {
                _Value = value;
                ValueBox.Text = _Value.ToString();
            
            }
        }
        override public string Text
        {
            get {return (_Value.ToString());}
            set {ValueBox.Text =value ;}

        }

        public bool LogScale
        {
            get { return (_LogScale); }
            set { _LogScale = value; }
        }
        public float MaxValue
        {
            get { return this._MaxValue; }
            set {
                label3.Text = value.ToString();
                _MaxValue = value;
                if (_LogScale)
                {
                    float mid = (float)(Math.Log(_MaxValue) + Math.Log(_MinValue+1))/2;
                    mid=(float)(Math.Exp(mid) );
                    mid=(float)Math.Round(mid, 2);
                    label2.Text = mid.ToString();
                }
                else
                {
                    float mid = (_MaxValue + _MinValue) / 2;
                    label2.Text = mid.ToString();
                }

            }

        }
        public float MinValue
        {
            get { return (this._MinValue); }
            set {
                _MinValue = value;
                label1.Text = value.ToString();
                if (_LogScale)
                {
                   
                    float mid =(float)Math.Round( Math.Exp( (Math.Log( _MaxValue) +Math.Log( _MinValue+1)) / 2),2);
                    label2.Text = mid.ToString();
                }
                else
                {
                    float mid=(_MaxValue + _MinValue) / 2;
                    label2.Text = mid.ToString();
                }
            }
        }

        private void SlideAndTextl_Resize(object sender, EventArgs e)
        {
            try
            {
                colorSlider1.Left = 0;
                colorSlider1.Top = 0;
                colorSlider1.Width = this.Width - ValueBox.Width;
                colorSlider1.Height = this.Height - label1.Height;

                label1.Left = 0;
                label1.Top = colorSlider1.Height;

                label2.Left = (colorSlider1.Width - label2.Width) / 2;
                label2.Top = colorSlider1.Height;

                label3.Left = colorSlider1.Width - label3.Width;
                label3.Top  = colorSlider1.Height;

                ValueBox.Left = this.Width - ValueBox.Width;
                ValueBox.Top = 0;
            }
            catch { }

        }

        private void colorSlider1_ValueChanged(object sender, EventArgs e)
        {
           
            double mid;
            if (_LogScale)
            {
                mid =UnMapExposure(colorSlider1.Value);
                
            }
            else
            {
                mid =Math.Round( ((double)colorSlider1.Value)/1000*(_MaxValue-_MinValue )+_MinValue ,3);
                
            }
            if (Math.Abs(mid-_Value )>.1 )
            {
                _Value =(float) mid;
                if (OnValueChanged!=null) OnValueChanged(this,_Value );
                ValueBox.Text = _Value.ToString();
            }
        }
        private int MapExposure(double e)
        {
            return ((int)(Math.Log(e + 1) / 6.9077552789821370520539743640531 * 1000));
        }
        private double UnMapExposure(int value)
        {
            Math.Round( Math.Exp(colorSlider1.Value *(Math.Log(_MaxValue )-Math.Log(_MinValue +1))*1000),3);
            return (Math.Round(Math.Exp(value * 6.9077552789821370520539743640531 / 1000) - 1, 1));
        }

        private int MapValuetoSlider(double f )
        {
            
            double max = _MaxValue ;
            double min = _MinValue ;
            if (max == min)
            {
                    max = 1000;
                    min = 0;
            }
            if (f > max) f = max;
            if (f < min) f = min;
            return ((int)((f - min) / (max - min) * colorSlider1.Maximum ));
        }

        private void ValueBox_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                int map=0;
                double f;
                if (_LogScale )
                {
                    f=double.Parse(ValueBox.Text );
                    map  =MapExposure(f);

                }
                else 
                {
                     f= double.Parse(ValueBox.Text );
                    map  = MapValuetoSlider(f);
                }
                if (Math.Abs(map-colorSlider1.Value )>5)
                {
                    _Value = (float)f;

                    colorSlider1.Value =map;
                    if (OnValueChanged!=null) OnValueChanged(this,_Value );
                }

            }
            catch {}
        
        }

    }
}
