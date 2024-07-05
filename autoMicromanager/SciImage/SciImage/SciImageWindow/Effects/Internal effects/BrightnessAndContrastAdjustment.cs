/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.IndirectUI;
using SciImage.PropertySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    [EffectCategory(EffectCategory.Adjustment)]
    public sealed class BrightnessAndContrastAdjustment
        : Effect
    {
        public enum PropertyNames
        {
            Brightness = 0,
            Contrast = 1
        }

        public static string StaticName
        {
            get
            {
                return "Brightness / Contrast";
            }
        }

        public static Image StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.BrightnessAndContrastAdjustment.png").Reference;
            }
        }

        private int brightness;
        private int contrast;
        private int multiply;
        private int divide;
        private byte[] rgbTable;

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new Int32Property(PropertyNames.Brightness, 0, -100, +100));
            props.Add(new Int32Property(PropertyNames.Contrast, 0, -100, +100));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Brightness, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("BrightnessAndContrastAdjustment.Brightness"));
            configUI.SetPropertyControlValue(PropertyNames.Contrast, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("BrightnessAndContrastAdjustmnet.Contrast"));

            return configUI;
        }

        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.brightness = newToken.GetProperty<Int32Property>(PropertyNames.Brightness).Value;
            this.contrast = newToken.GetProperty<Int32Property>(PropertyNames.Contrast).Value;

            if (this.contrast < 0)
            {
                this.multiply = this.contrast + 100;
                this.divide = 100;
            }
            else if (this.contrast > 0)
            {
                this.multiply = 100;
                this.divide = 100 - this.contrast;
            }
            else
            {
                this.multiply = 1;
                this.divide = 1;
            }

            if (this.rgbTable == null)
            {
                this.rgbTable = new byte[65536];
            }

            if (this.divide == 0)
            {
                for (int intensity = 0; intensity < 256; ++intensity)
                {
                    if (intensity + this.brightness < 128)
                    {
                        this.rgbTable[intensity] = 0;
                    }
                    else
                    {
                        this.rgbTable[intensity] = 255;
                    }
                }
            }
            else if (this.divide == 100)
            {
                for (int intensity = 0; intensity < 256; ++intensity)
                {
                    int shift = (intensity - 127) * this.multiply / this.divide + 127 - intensity + this.brightness;

                    for (int col = 0; col < 256; ++col)
                    {
                        int index = (intensity * 256) + col;
                        this.rgbTable[index] = Utility.ClampToByte(col + shift);
                    }
                }
            }
            else
            {
                for (int intensity = 0; intensity < 256; ++intensity)
                {
                    int shift = (intensity - 127 + this.brightness) * this.multiply / this.divide + 127 - intensity;

                    for (int col = 0; col < 256; ++col)
                    {
                        int index = (intensity * 256) + col;
                        this.rgbTable[index] = Utility.ClampToByte(col + shift);
                    }
                }
            }

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public unsafe override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            for (int r = startIndex; r < startIndex + length; ++r)
            {
                Rectangle rect = rois[r];

                for (int y = rect.Top; y < rect.Bottom; ++y)
                {
                    //ColorPixelBase* srcRowPtr = srcArgs.Surface.GetPointAddressUnchecked(rect.Left, y);
                    //ColorPixelBase* dstRowPtr = dstArgs.Surface.GetPointAddressUnchecked(rect.Left, y);
                    //ColorPixelBase *dstRowEndPtr = dstRowPtr + rect.Width;

                    if (divide == 0)
                    {
                        for (int x = rect.Left ;x<rect.Right;x++)// (dstRowPtr < dstRowEndPtr)
                        {
                            ColorPixelBase col = srcArgs.Surface.GetPoint(x, y);
                            int i = col.GetIntensityByte();
                            uint c = this.rgbTable[i];
                            col.alpha =255;
                            col[2] =(byte) c;
                            col[0] =(byte)c;
                            col[1] =(byte)c; //((Int32)col.ToInt64() & 0xff000000) | c | (c << 8) | (c << 16))
                            dstArgs.Surface.SetPoint(x, y, col);
                        }
                    }
                    else
                    {
                        for (int x = rect.Left; x < rect.Right; x++)// (dstRowPtr < dstRowEndPtr)
                        {
                            ColorPixelBase col = srcArgs.Surface.GetPoint(x, y);
                            int i = col.GetIntensityByte();
                            int shiftIndex = i * 256;

                            col[2] = this.rgbTable[shiftIndex + col[2] ];
                            col[1]  = this.rgbTable[shiftIndex + col[1] ];
                            col[0]  = this.rgbTable[shiftIndex + col[0] ];

                            dstArgs.Surface.SetPoint(x, y, col);
                        }
                    }
                }
            }
            
            return;
        }

        public BrightnessAndContrastAdjustment()
            : base(StaticName,
                   StaticImage,
                   null,
                   EffectFlags.Configurable)
        {
        }
    }
}
