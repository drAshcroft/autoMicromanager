/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

// Original C++ implementation by Jason Waltman as part of "Filter Explorer," 
// http://www.jasonwaltman.com/thesis/index.html

using SciImage;
using SciImage.Core;
using SciImage.IndirectUI;
using SciImage.PropertySystem;
using SciImage.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    public sealed class FrostedGlassEffect
        : Effect
    {
        public static string StaticName
        {
            get
            {
                return "Frosted Glass";
            }
        }

        public static Image StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.FrostedGlassEffect.png").Reference;
            }
        }

        [ThreadStatic]
        private static Random threadRand;

        public FrostedGlassEffect() 
            : base(StaticName, 
                   StaticImage,
                   SubmenuNames.Distort,
                   EffectFlags.Configurable)
        {
        }

        public enum PropertyNames
        {
            MaxScatterRadius = 0,
            MinScatterRadius = 1,
            NumSamples = 2
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new DoubleProperty(PropertyNames.MaxScatterRadius, 3.0, 0.0, 200.0));
            props.Add(new DoubleProperty(PropertyNames.MinScatterRadius, 0.0, 0.0, 200.0));
            props.Add(new Int32Property(PropertyNames.NumSamples, 2, 1, 8));

            List<PropertyCollectionRule> propertyRules = new List<PropertyCollectionRule>();
            propertyRules.Add(new SoftMutuallyBoundMinMaxRule<double, DoubleProperty>(PropertyNames.MinScatterRadius, PropertyNames.MaxScatterRadius));

            PropertyCollection propertyCollection = new PropertyCollection(props, propertyRules);
            return propertyCollection;
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.MaxScatterRadius, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("FrostedGlassEffect.ConfigDialog.MaxScatterRadius.DisplayName"));
            configUI.FindControlForPropertyName(PropertyNames.MaxScatterRadius).ControlProperties["UseExponentialScale"].Value = true;
            configUI.SetPropertyControlValue(PropertyNames.MinScatterRadius, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("FrostedGlassEffect.ConfigDialog.MinScatterRadius.DisplayName"));
            configUI.FindControlForPropertyName(PropertyNames.MinScatterRadius).ControlProperties["UseExponentialScale"].Value = true;
            configUI.SetPropertyControlValue(PropertyNames.NumSamples, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("FrostedGlassEffect.ConfigDialog.NumSamples.DisplayName"));

            return configUI;
        }

        private double minRadius;
        private double maxRadius;
        private int sampleCount;

        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            double minRadiusP = newToken.GetProperty<DoubleProperty>(PropertyNames.MinScatterRadius).Value;

            this.minRadius = Math.Min(minRadiusP, Math.Min(srcArgs.Width, srcArgs.Height) / 2); 
            this.maxRadius = newToken.GetProperty<DoubleProperty>(PropertyNames.MaxScatterRadius).Value;
            this.sampleCount = newToken.GetProperty<Int32Property>(PropertyNames.NumSamples).Value;

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public override unsafe void Render(EffectConfigToken parameters, RenderArgs DstArgs, RenderArgs SrcArgs, Rectangle[] rois, int startIndex, int length)
        {
            double radiusDelta = this.maxRadius - this.minRadius;
            double effectiveRadiusDelta = radiusDelta;
            
            if (threadRand == null)
            {
                threadRand = new Random(unchecked(System.Threading.Thread.CurrentThread.GetHashCode() ^
                    unchecked((int)DateTime.Now.Ticks)));
            }

            ColorPixelBase[] samples = new ColorPixelBase[sampleCount];

            Random localRand = threadRand;

            for (int r = startIndex; r < startIndex + length; ++r)
            {
                Rectangle roi = rois[r];

                for (int y = roi.Top; y < roi.Bottom; ++y)
                {
                   // ColorPixelBase* dstPtr = DstArgs.Surface.GetPointAddressUnchecked(roi.Left, y);

                    for (int x = roi.Left; x < roi.Right; ++x)
                    {
                        for (int sampleIndex = 0; sampleIndex < sampleCount; ++sampleIndex)
                        {
                            double srcX;
                            double srcY;

                            while (true)
                            {
                                double angle = localRand.NextDouble() * Math.PI * 2.0;
                                double distanceDelta = localRand.NextDouble() * effectiveRadiusDelta;
                                double distance = distanceDelta + this.minRadius;

                                srcX = x + (Math.Cos(angle) * distance);
                                srcY = y + (Math.Sin(angle) * distance);

                                if (srcX >= 0 && srcX <= SrcArgs.Width && srcY >= 0 && srcY <= SrcArgs.Height)
                                {
                                    break;
                                }
                            }

                            samples[sampleIndex] = SrcArgs.Surface.GetBilinearSample((float)srcX, (float)srcY,SrcArgs.Surface.ColorPixelBase );
                        }

                        ColorPixelBase result = SrcArgs.Surface.ColorPixelBase.Blend(samples);

                        DstArgs.Surface.SetPoint(x, y, result);
                    }
                }
            }
        }
    }
}
