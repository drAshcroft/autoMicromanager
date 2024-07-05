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
using System.Runtime.InteropServices;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    public sealed class TileEffect
        : Effect 
    {
        public static Image StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.TileEffect.png").Reference;
            }
        }

        public static string StaticName
        {
            get
            {
                return "Tile Reflection";
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
            Rotation = 0,
            SquareSize = 1,
            Curvature = 2,
            Quality = 3
        }

        public TileEffect()
            : base(StaticName, StaticImage, StaticSubMenuName, EffectFlags.Configurable)
        {
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new DoubleProperty(PropertyNames.Rotation, 30, -180, +180));
            props.Add(new DoubleProperty(PropertyNames.SquareSize, 40, 1, 800));
            props.Add(new DoubleProperty(PropertyNames.Curvature, 8, -100, 100));
            props.Add(new Int32Property(PropertyNames.Quality, 2, 1, 5));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Rotation, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("TileEffect.Rotation.Text"));
            configUI.SetPropertyControlType(PropertyNames.Rotation, PropertyControlType.AngleChooser);

            configUI.SetPropertyControlValue(PropertyNames.SquareSize, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("TileEffect.SquareSize.Text"));
            configUI.SetPropertyControlValue(PropertyNames.SquareSize, ControlInfoPropertyNames.UseExponentialScale, true);

            configUI.SetPropertyControlValue(PropertyNames.Curvature, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("TileEffect.Intensity.Text"));

            configUI.SetPropertyControlValue(PropertyNames.Quality, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("TileEffect.Quality.Text"));

            return configUI;
        }

        private double rotation;
        private double squareSize;
        private double curvature;

        private int quality;
        private float sin;
        private float cos;
        private float scale;
        private float intensity;
        //private PointF[] aaPointsArray;

        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.rotation = -newToken.GetProperty<DoubleProperty>(PropertyNames.Rotation).Value;
            this.squareSize = newToken.GetProperty<DoubleProperty>(PropertyNames.SquareSize).Value;
            this.curvature = newToken.GetProperty<DoubleProperty>(PropertyNames.Curvature).Value;
            
            this.sin = (float)Math.Sin(this.rotation * Math.PI / 180.0);
            this.cos = (float)Math.Cos(this.rotation * Math.PI / 180.0);
            this.scale = (float)(Math.PI / this.squareSize);
            this.intensity = (float)(this.curvature * this.curvature / 10.0 * Math.Sign(this.curvature));

            this.quality = newToken.GetProperty<Int32Property>(PropertyNames.Quality).Value;

            if (this.quality != 1)
            {
                ++this.quality;
            }

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public unsafe override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            Surface dst = dstArgs.Surface;
            Surface src = srcArgs.Surface;
            int width = dst.Width;
            int height = dst.Height;
            float hw = width / 2.0f;
            float hh = height / 2.0f;

            int aaSampleCount = this.quality * this.quality;
            PointF* aaPointsArray = stackalloc PointF[aaSampleCount];
            Utility.GetRgssOffsets(aaPointsArray, aaSampleCount, this.quality);
            ColorPixelBase[] samples = new ColorPixelBase[aaSampleCount];

            for (int n = startIndex; n < startIndex + length; ++n)
            {
                Rectangle rect = rois[n];

                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    float j = y - hh;
                    //ColorPixelBase* dstPtr = dst.GetPointAddressUnchecked(rect.Left, y);

                    for (int x = rect.Left; x < rect.Right; x++)
                    {
                        float i = x - hw;

                        for (int p = 0; p < aaSampleCount; ++p)
                        {
                            PointF pt = aaPointsArray[p];

                            float u1 = i + pt.X;
                            float v1 = j - pt.Y;

                            float s1 =  cos * u1 + sin * v1;
                            float t1 = -sin * u1 + cos * v1;

                            float s2 = s1 + this.intensity * (float)Math.Tan(s1 * this.scale);
                            float t2 = t1 + this.intensity * (float)Math.Tan(t1 * this.scale);

                            float u2 = cos * s2 - sin * t2;
                            float v2 = sin * s2 + cos * t2;

                            float xSample = hw + u2;
                            float ySample = hh + v2;

                            samples[p] = src.GetBilinearSampleWrapped(xSample, ySample,src.ColorPixelBase );

                            
                        }
                        dst.SetPoint(x,y,src.ColorPixelBase.Blend(samples) );
                      
                    }
                }
            }
        }
    }
}
