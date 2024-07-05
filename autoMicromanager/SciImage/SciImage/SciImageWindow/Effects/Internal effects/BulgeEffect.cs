/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage;
using SciImage.IndirectUI;
using SciImage.PropertySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    public sealed class BulgeEffect
        : Effect 
    {
        public static Image StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.BulgeEffect.png").Reference;
            }
        }

        public static string StaticName
        {
            get
            {
                return "Bulge";
            }
        }

        public static string StaticSubMenuName
        {
            get
            {
                return SubmenuNames.Distort;
            }
        }

        public enum PropertyNames
        {
            Amount = 0,
            Offset = 1
        }

        public BulgeEffect()
            : base(StaticName, StaticImage, StaticSubMenuName, EffectFlags.Configurable)
        {
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new Int32Property(PropertyNames.Amount, 45, -200, 100));

            props.Add(new DoubleVectorProperty(
                PropertyNames.Offset,
                Pair.Create(0.0, 0.0),
                Pair.Create(-1.0, -1.0),
                Pair.Create(1.0, 1.0)));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Amount, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("BulgeEffect.BulgeAmount.Text"));

            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("BulgeEffect.Offset.Text"));
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderSmallChangeX, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderLargeChangeX, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.UpDownIncrementX, 0.01);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderSmallChangeY, 0.05);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.SliderLargeChangeY, 0.25);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.UpDownIncrementY, 0.01);

            Surface sourceSurface = this.EnvironmentParameters.SourceSurface;
            Bitmap bitmap = sourceSurface.CreateAliasedBitmap();
            ImageResource imageResource = ImageResource.FromImage(bitmap);
            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.StaticImageUnderlay, imageResource);

            return configUI;
        }

        private int amount;
        private float offsetX;
        private float offsetY;

        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.amount = newToken.GetProperty<Int32Property>(PropertyNames.Amount).Value;

            this.offsetX = (float)newToken.GetProperty<DoubleVectorProperty>(PropertyNames.Offset).ValueX;
            this.offsetY = (float)newToken.GetProperty<DoubleVectorProperty>(PropertyNames.Offset).ValueY;

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public unsafe override void Render(EffectConfigToken  parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            float bulge = this.amount;
            Surface dst = dstArgs.Surface;
            Surface src = srcArgs.Surface;

            float hw = dst.Width / 2.0f;
            float hh = dst.Height / 2.0f;
            float maxrad = Math.Min(hw, hh);
            float maxrad2 = maxrad * maxrad;
            float amt = this.amount / 100.0f;

            hh = hh + this.offsetY * hh;
            hw = hw + this.offsetX * hw;

            for (int n = startIndex; n < startIndex + length; ++n)
            {
                Rectangle rect = rois[n];
                
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    //ColorPixelBase* dstPtr = dst.GetPointAddressUnchecked(rect.Left, y);
                    
                    float v = y - hh;

                    for (int x = rect.Left; x < rect.Right; x++)
                    {
                        ColorPixelBase srcPtr = src.GetPoint(x, y);
                        float u = x - hw;
                        float r = (float)Math.Sqrt(u * u + v * v);
                        float rscale1 = (1.0f - (r / maxrad));

                        if (rscale1 > 0)
                        {
                            float rscale2 = 1 - amt * rscale1 * rscale1;

                            float xp = u * rscale2;
                            float yp = v * rscale2;

                            dst.SetPoint(x,y,  src.GetBilinearSampleClamped(xp + hw, yp + hh,src.ColorPixelBase ) );
                        }
                        else
                        {
                            dst.SetPoint(x,y, srcPtr);
                        }
                    }
                }
            }
        }
    }
}
