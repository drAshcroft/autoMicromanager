/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.SystemLayer;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage
{
    /// <summary>
    /// Provides a set of standard UnaryPixelOps.
    /// </summary>
    public sealed class UnaryPixelOps
    {
        private UnaryPixelOps()
        {
        }

        /// <summary>
        /// Passes through the given color value.
        /// result(color) = color
        /// </summary>
        [Serializable]
        public class Identity
            : UnaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                return color;
            }

           
        }

        /// <summary>
        /// Always returns a constant color.
        /// </summary>
        [Serializable]
        public class Constant
            : UnaryPixelOp
        {
            private ColorPixelBase setColor;

            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                return setColor;
            }

            

            public Constant(ColorPixelBase setColor)
            {
                this.setColor = setColor;
            }
        }

        /// <summary>
        /// Blends pixels with the specified constant color.
        /// </summary>
        [Serializable]
        public class BlendConstant
            : UnaryPixelOp
        {
            private ColorPixelBase blendColor;

            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                int a = blendColor.alpha ;
                int invA = 255 - a;

                long [] ColorChannels = new long[color.NumChannels ];
                for (int i=0;i<ColorChannels.Length ;i++)
                    ColorChannels[i] = ((color.GetChannel(i) * invA) + (color.GetChannel(i) * a)) / 256;
              
                byte a2 = ComputeAlpha(color.alpha , blendColor.alpha );

                return color.FromArray(ColorChannels,a2 );
            }

            public BlendConstant(ColorPixelBase blendColor)
            {
                this.blendColor = blendColor;
            }
        }

        /// <summary>
        /// Used to set a given channel of a pixel to a given, predefined color.
        /// Useful if you want to set only the alpha value of a given region.
        /// </summary>
        [Serializable]
        public class SetChannel
            : UnaryPixelOp
        {
            private int channel;
            private byte setValue;

            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                color.SetChannel (channel,  setValue);
                return color;
            }

           
            public SetChannel(int channel, byte setValue)
            {
                this.channel = channel;
                this.setValue = setValue;
            }
        }

        /// <summary>
        /// Specialization of SetChannel that sets the alpha channel.
        /// </summary>
        /// <remarks>This class depends on the system being litte-endian with the alpha channel 
        /// occupying the 8 most-significant-bits of a ColorPixelBase instance.
        /// By the way, we use addition instead of bitwise-OR because an addition can be
        /// perform very fast (0.5 cycles) on a Pentium 4.</remarks>
        [Serializable]
        public class SetAlphaChannel
            : UnaryPixelOp
        {
            private byte addValue;

            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                ColorPixelBase c = color.AnotherPixel();
                c.alpha =(byte)( color.alpha + addValue);
                return   c ;
            }


            public SetAlphaChannel(byte alphaValue)
            {
                addValue = alphaValue ;
            }
        }

        /// <summary>
        /// Specialization of SetAlphaChannel that always sets alpha to 255.
        /// </summary>
        [Serializable]
        public class SetAlphaChannelTo255
            : UnaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                ColorPixelBase c = color.AnotherPixel();
                c.alpha = 255;
                return c;
            }

            
        }

        /// <summary>
        /// Inverts a pixel's color, and passes through the alpha component.
        /// </summary>
        [Serializable]
        public class Invert
            : UnaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                ColorPixelBase c = color.InvertColor(color);

                return c; //
            }
        }

        /// <summary>
        /// If the color is within the red tolerance, remove it
        /// </summary>
        [Serializable]
        public class RedEyeRemove
            : UnaryPixelOp
        {
            private int tolerence;
            private double setSaturation;

            public RedEyeRemove(int tol, int sat)
            {
                tolerence = tol;
                setSaturation = (double)sat / 100;
            }

            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                // The higher the saturation, the more red it is
                int saturation = GetSaturation(color);

                // The higher the difference between the other colors, the more red it is
                int difference = color[2]  - Math.Max(color[0] ,color[1] );

                // If it is within tolerence, and the saturation is high
                if ((difference > tolerence) && (saturation > 100)) 
                {
                    double i = 255.0 * color.GetIntensity();
                    byte ib = (byte)(i * setSaturation); // adjust the red color for user inputted saturation
                    return color.FromBgra((byte)color[0] ,(byte)color[1] , ib, color.alpha );
                }
                else
                {
                    return color;
                }
            }

            //Saturation formula from RgbColor.cs, public HsvColor ToHsv()
            private int GetSaturation(ColorPixelBase color)
            {
                double min;
                double max;
                double delta;

                double r = (double) color[2]  / 255;
                double g = (double) color[1]  / 255;
                double b = (double) color[0]  / 255;

                double s;

                min = Math.Min(Math.Min(r, g), b);
                max = Math.Max(Math.Max(r, g), b);
                delta = max - min;

                if (max == 0 || delta == 0) 
                {
                    // R, G, and B must be 0, or all the same.
                    // In this case, S is 0, and H is undefined.
                    // Using H = 0 is as good as any...
                    s = 0;
                } 
                else
                {
                    s = delta / max;
                }

                return (int)(s * 255);
            }               
        }

        /// <summary>
        /// Inverts a pixel's color and its alpha component.
        /// </summary>
        [Serializable]
        public class InvertWithAlpha
            : UnaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                return color.InvertColorAndAlpha(color);
                    
            }
        }

        /// <summary>
        /// Averages the input color's red, green, and blue channels. The alpha component
        /// is unaffected.
        /// </summary>
        [Serializable]
        public class AverageChannels
            : UnaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                return color.AverageChannels(color);
            }
        }

        [Serializable]
        public class Desaturate
            : UnaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                byte i = color.GetIntensityByte();
                return color.FromBgra(i, i, i, color.alpha );
            }
        }

        [Serializable]
        public class LuminosityCurve
            : UnaryPixelOp
        {
            public byte[] Curve = new byte[256];

            public LuminosityCurve()
            {    
                for (int i = 0; i < 256; ++i)
                {
                    Curve[i] = (byte)i;
                }
            }

            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                byte lumi = color.GetIntensityByte();
                int diff = Curve[lumi] - lumi;

                return color.FromBgraClamped(
                    color[0]  + diff,
                    color[1]  + diff,
                    color[2]  + diff,
                    color.alpha );
            }
        }

        [Serializable]
        public class ChannelCurve
            : UnaryPixelOp
        {
            public byte[] CurveB = new byte[256];
            public byte[] CurveG = new byte[256];
            public byte[] CurveR = new byte[256];

            public ChannelCurve()
            {
                for (int i = 0; i < 256; ++i)
                {
                    CurveB[i] = (byte)i;
                    CurveG[i] = (byte)i;
                    CurveR[i] = (byte)i;
                }
            }
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                return color.FromBgra(CurveB[color[0] ], CurveG[color[1] ], CurveR[color[2] ], color.alpha );
            }

            public override void Apply(Surface dst, Point dstOffset, Surface src, Point srcOffset, int scanLength)
            {
                base.Apply (dst, dstOffset, src, srcOffset, scanLength);
            }
        }

        [Serializable]
        public class Level
            : ChannelCurve,
              ICloneable
        {
            private ColorPixelBase colorInLow;
            public ColorPixelBase ColorInLow 
            {
                get 
                {
                    return colorInLow; 
                }

                set 
                {
                    if (value[2]  == 255) 
                    {
                        value[2]  = 254;
                    }

                    if (value[1]  == 255)
                    {
                        value[1]  = 254;
                    }

                    if (value[0]  == 255)
                    {
                        value[0]  = 254;
                    }

                    if (colorInHigh[2]  < value[2]  + 1) 
                    {
                        colorInHigh[2]  = (byte)(value[2]  + 1);
                    }

                    if (colorInHigh[1]  < value[1]  + 1) 
                    {
                        colorInHigh[1]  = (byte)(value[2]  + 1);
                    }

                    if (colorInHigh[0]  < value[0]  + 1) 
                    {
                        colorInHigh[0]  = (byte)(value[2]  + 1);
                    }

                    colorInLow = value;
                    UpdateLookupTable();
                }
            }

            private ColorPixelBase colorInHigh;
            public ColorPixelBase ColorInHigh 
            {
                get 
                {
                    return colorInHigh;
                }

                set 
                {
                    if (value[2]  == 0) 
                    {
                        value[2]  = 1;
                    }

                    if (value[1]  == 0)
                    { 
                        value[1]  = 1;
                    }

                    if (value[0]  == 0)
                    {
                        value[0] = 1;
                    }

                    if (colorInLow[2] > value[2] - 1) 
                    {
                        colorInLow[2] = (byte)(value[2] - 1);
                    }

                    if (colorInLow[1] > value[1] - 1) 
                    {
                        colorInLow[1] = (byte)(value[2] - 1);
                    }

                    if (colorInLow[0] > value[0] - 1) 
                    {
                        colorInLow[0] = (byte)(value[2] - 1);
                    }

                    colorInHigh = value;
                    UpdateLookupTable();
                }
            }

            private ColorPixelBase colorOutLow;
            public ColorPixelBase ColorOutLow 
            {
                get 
                {
                    return colorOutLow;
                }

                set 
                {
                    if (value[2] == 255) 
                    {
                        value[2] = 254;
                    }

                    if (value[1] == 255)
                    {
                        value[1] = 254;
                    }

                    if (value[0] == 255)
                    {
                        value[0] = 254;
                    }

                    if (colorOutHigh[2] < value[2] + 1) 
                    {
                        colorOutHigh[2] = (byte)(value[2] + 1);
                    }

                    if (colorOutHigh[1] < value[1] + 1) 
                    {
                        colorOutHigh[1] = (byte)(value[1] + 1);
                    }

                    if (colorOutHigh[0] < value[0] + 1) 
                    {
                        colorOutHigh[0] = (byte)(value[0] + 1);
                    }

                    colorOutLow = value;
                    UpdateLookupTable();
                }
            }

            private ColorPixelBase colorOutHigh;
            public ColorPixelBase ColorOutHigh 
            {
                get 
                {
                    return colorOutHigh;
                }

                set 
                {
                    if (value[2] == 0) 
                    {
                        value[2] = 1;
                    }

                    if (value[1] == 0)
                    { 
                        value[1] = 1;
                    }

                    if (value[0] == 0)
                    {
                        value[0] = 1;
                    }

                    if (colorOutLow[2] > value[2] - 1) 
                    {
                        colorOutLow[2] = (byte)(value[2] - 1);
                    }

                    if (colorOutLow[1] > value[1] - 1) 
                    {
                        colorOutLow[1] = (byte)(value[1] - 1);
                    }

                    if (colorOutLow[0] > value[0] - 1) 
                    {
                        colorOutLow[0] = (byte)(value[0] - 1);
                    }

                    colorOutHigh = value;
                    UpdateLookupTable();
                }       
            }               
                        
            private float[] gamma = new float[3];
            public float GetGamma(int index) 
            {               
                if (index < 0 || index >= 3) 
                {
                    throw new ArgumentOutOfRangeException("index", index, "Index must be between 0 and 2");
                }

                return gamma[index];
            }

            public void SetGamma(int index, float val) 
            {
                if (index < 0 || index >= 3) 
                {
                    throw new ArgumentOutOfRangeException("index", index, "Index must be between 0 and 2");
                }

                gamma[index] = Utility.Clamp(val, 0.1f, 10.0f);
                UpdateLookupTable();
            }

            public bool isValid = true;

            public static Level AutoFromLoMdHi(ColorPixelBase lo, ColorPixelBase md, ColorPixelBase hi) 
            {
                float[] gamma = new float[3];

                for (int i = 0; i < 3; i++)
                {
                    if (lo[i] < md[i] && md[i] < hi[i])
                    {
                        gamma[i] = (float)Utility.Clamp(Math.Log(0.5, (float)(md[i] - lo[i]) / (float)(hi[i] - lo[i])), 0.1, 10.0);
                    }
                    else
                    {
                        gamma[i] = 1.0f;
                    }
                }

                return new Level(lo, hi, gamma, lo.FromColor(Color.Black), lo.FromColor(Color.White));
            }

            private void UpdateLookupTable() 
            {
                for (int i = 0; i < 3; i++) 
                {
                    if (colorOutHigh[i] < colorOutLow[i] ||
                        colorInHigh[i] <= colorInLow[i] ||
                        gamma[i] < 0)
                    {
                        isValid = false;
                        return;
                    }

                    for (int j = 0; j < 256; j++) 
                    {
                        ColorPixelBase col = Apply(j, j, j,colorOutHigh );
                        CurveB[j] = (byte)col[0] ;
                        CurveG[j] =(byte) col[1] ;
                        CurveR[j] = (byte)col[2] ;
                    }
                }
            }

           /* public Level() 
                : this(ColorPixelBase.FromColor(Color.Black),
                       ColorPixelBase.FromColor(Color.White),
                       new float[] { 1, 1, 1 },
                       ColorPixelBase.FromColor(Color.Black),
                       ColorPixelBase.FromColor(Color.White))
            {
            }*/

            public Level(ColorPixelBase in_lo, ColorPixelBase in_hi, float[] gamma, ColorPixelBase out_lo, ColorPixelBase out_hi)
            {
                colorInLow = in_lo;
                colorInHigh = in_hi;
                colorOutLow = out_lo;
                colorOutHigh = out_hi;

                if (gamma.Length != 3) 
                {
                    throw new ArgumentException("gamma", "gamma must be a float[3]");
                }

                this.gamma = gamma;
                UpdateLookupTable();
            }

            public ColorPixelBase Apply(float r, float g, float b, ColorPixelBase RequiredFormat) 
            {
                ColorPixelBase ret = RequiredFormat.AnotherPixel();
                float[] input = new float[] { b, g, r };

                for (int i = 0; i < 3; i++) 
                {
                    float v = (input[i] - colorInLow[i]);

                    if (v < 0)
                    {
                        ret[i] = colorOutLow[i];
                    }
                    else if (v + colorInLow[i] >= colorInHigh[i])
                    {
                        ret[i] = colorOutHigh[i];
                    }
                    else
                    {
                        ret[i] = (byte)Utility.Clamp(
                            colorOutLow[i] + (colorOutHigh[i] - colorOutLow[i]) * Math.Pow(v / (colorInHigh[i] - colorInLow[i]), gamma[i]),
                            0.0f,
                            255.0f);
                    }
                }

                return ret;
            }

            public void UnApply(ColorPixelBase after, float[] beforeOut, float[] slopesOut) 
            {
                if (beforeOut.Length != 3) 
                {
                    throw new ArgumentException("before must be a float[3]", "before");
                }

                if (slopesOut.Length != 3) 
                {
                    throw new ArgumentException("slopes must be a float[3]", "slopes");
                }

                for (int i = 0; i < 3; i++) 
                {
                    beforeOut[i] = colorInLow[i] + (colorInHigh[i] - colorInLow[i]) *
                        (float)Math.Pow((float)(after[i] - colorOutLow[i]) / (colorOutHigh[i] - colorOutLow[i]), 1 / gamma[i]);

                    slopesOut[i] = (float)(colorInHigh[i] - colorInLow[i]) / ((colorOutHigh[i] - colorOutLow[i]) * gamma[i]) *
                        (float)Math.Pow((float)(after[i] - colorOutLow[i]) / (colorOutHigh[i] - colorOutLow[i]), 1 / gamma[i] - 1);

                    if (float.IsInfinity(slopesOut[i]) || float.IsNaN(slopesOut[i])) 
                    {
                        slopesOut[i] = 0;
                    }
                }
            }

            public object Clone()
            {
                Level copy = new Level(colorInLow, colorInHigh, (float[])gamma.Clone(), colorOutLow, colorOutHigh);

                copy.CurveB = (byte[])this.CurveB.Clone();
                copy.CurveG = (byte[])this.CurveG.Clone();
                copy.CurveR = (byte[])this.CurveR.Clone();

                return copy;
            }
        }

        [Serializable]
        public class HueSaturationLightness
            : UnaryPixelOp
        {
            private int hueDelta;
            private int satFactor;
            private UnaryPixelOp blendOp;

            public HueSaturationLightness(int hueDelta, int satDelta, int lightness, ColorPixelBase RequestedColor)
            {
                this.hueDelta = hueDelta;
                this.satFactor = (satDelta * 1024) / 100;

                if (lightness == 0)
                {
                    blendOp = new UnaryPixelOps.Identity();
                }
                else if (lightness > 0)
                {
                    blendOp = new UnaryPixelOps.BlendConstant(RequestedColor.FromBgra(255, 255, 255, (byte)((lightness * 255) / 100)));
                }
                else // if (lightness < 0)
                {
                    blendOp = new UnaryPixelOps.BlendConstant(RequestedColor.FromBgra(0, 0, 0, (byte)((-lightness * 255) / 100)));
                }
            }
            
            public override ColorPixelBase Apply(ColorPixelBase color)
            {
                //adjust saturation
                byte intensity = color.GetIntensityByte();
                color[2]  = Utility.ClampToByte((intensity * 1024 + (color[2]  - intensity) * satFactor) >> 10);
                color[1]  = Utility.ClampToByte((intensity * 1024 + (color[1]  - intensity) * satFactor) >> 10);
                color[0]  = Utility.ClampToByte((intensity * 1024 + (color[0]  - intensity) * satFactor) >> 10);

                HsvColor hsvColor = HsvColor.FromColor(color.ToColor());
                int hue = hsvColor.Hue;

                hue += hueDelta;

                while (hue < 0)
                {
                    hue += 360;
                }
                        
                while (hue > 360)
                {
                    hue -= 360;
                }

                hsvColor.Hue = hue;

                ColorPixelBase newColor = color.FromColor(hsvColor.ToColor());
                newColor = blendOp.Apply(newColor);
                newColor.alpha = color.alpha;
                
                return newColor;
            }
        }
    }
}
