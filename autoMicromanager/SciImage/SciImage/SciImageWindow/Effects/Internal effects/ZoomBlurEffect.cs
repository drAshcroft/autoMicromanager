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
using System.Windows.Forms;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    public sealed class ZoomBlurEffect
        : Effect 
    {
        public static string StaticName
        {
            get
            {
                return "Zoom Blur";
            }
        }

        public static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.ZoomBlurEffect.png");
            }
        }

        public ZoomBlurEffect()
            : base(StaticName,
                   StaticImage.Reference,
                   SubmenuNames.Blurs,
                   EffectFlags.Configurable)
        {
        }

        public enum PropertyNames
        {
            Amount = 0,
            Offset = 1
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new Int32Property(PropertyNames.Amount, 10, 0, 100));

            props.Add(new DoubleVectorProperty(
                PropertyNames.Offset,
                Pair.Create(0.0, 0.0),
                Pair.Create(-2.0, -2.0),
                Pair.Create(+2.0, +2.0)));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Amount, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("ZoomBlurEffect.ConfigDialog.AmountLabel"));

            configUI.SetPropertyControlValue(PropertyNames.Offset, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("ZoomBlurEffect.ConfigDialog.Offset"));
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
        private Pair<double, double> offset;

        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.amount = newToken.GetProperty<Int32Property>(PropertyNames.Amount).Value;
            this.offset = newToken.GetProperty<DoubleVectorProperty>(PropertyNames.Offset).Value;

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public unsafe override void Render(EffectConfigToken  parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            Surface dst = dstArgs.Surface;
            Surface src = srcArgs.Surface;
            long w = dst.Width;
            long h = dst.Height;
            long fox = (long)(dst.Width * this.offset.First * 32768.0);
            long foy = (long)(dst.Height * this.offset.Second * 32768.0);
            long fcx = fox + (w << 15);
            long fcy = foy + (h << 15);
            long fz = this.amount;

            const int n = 64;
            
            for (int r = startIndex; r < startIndex + length; ++r)
            {
                Rectangle rect = rois[r];

                for (int y = rect.Top; y < rect.Bottom; ++y)
                {
                    for (int x = rect.Left; x < rect.Right; ++x)
                    {
                        //ColorPixelBase* dstPtr = dst.GetPointAddressUnchecked(rect.Left, y);
                        //ColorPixelBase* srcPtr = src.GetPointAddressUnchecked(rect.Left, y);
                        ColorPixelBase srcP = src.GetPoint(x, y);

                        long fx = (x << 16) - fcx;
                        long fy = (y << 16) - fcy;

                        int sr = 0;
                        int sg = 0;
                        int sb = 0;
                        int sa = 0;
                        int sc = 0;

                        sr += srcP[2]  * srcP.alpha ;
                        sg += srcP[1]  * srcP.alpha ;
                        sb += srcP[0]  * srcP.alpha ;
                        sa += srcP.alpha ;
                        ++sc;

                        for (int i = 0; i < n; ++i)
                        {
                            fx -= ((fx >> 4) * fz) >> 10;
                            fy -= ((fy >> 4) * fz) >> 10;

                            int u = (int)(fx + fcx + 32768 >> 16);
                            int v = (int)(fy + fcy + 32768 >> 16);

                            if (src.IsVisible(u, v))
                            {
                                ColorPixelBase srcPtr2 = src.GetPoint(u, v);

                                sr += srcPtr2[2]  * srcPtr2.alpha ;
                                sg += srcPtr2[1]  * srcPtr2.alpha ;
                                sb += srcPtr2[0]  * srcPtr2.alpha ;
                                sa += srcPtr2.alpha ;
                                ++sc;
                            }
                        }
                 
                        if (sa != 0)
                        {
                            dst.SetPoint(x,y, src.ColorPixelBase.FromBgra(
                                Utility.ClampToByte(sb / sa),
                                Utility.ClampToByte(sg / sa),
                                Utility.ClampToByte(sr / sa),
                                Utility.ClampToByte(sa / sc)));
                        }
                        else
                        {
                            dst[x,y]=0;
                        }

                    }
                }
            }                       
        }
    }
}
