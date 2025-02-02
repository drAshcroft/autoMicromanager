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
using System.Drawing;
using System.Collections.Generic;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    public sealed class MedianEffect
        : LocalHistogramEffect
    {
        public static string StaticName
        {
            get
            {
                return "Median";
            }
        }

        public static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MedianEffectIcon.png");
            }
        }

        private int radius;
	    private int percentile;

        public MedianEffect() 
            : base(StaticName, 
                   StaticImage.Reference, 
                   SubmenuNames.Noise,
                   EffectFlags.Configurable)
        {
        }

        public enum PropertyNames
        {
            Radius = 0,
            Percentile = 1
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new Int32Property(PropertyNames.Radius, 10, 1, 200));
            props.Add(new Int32Property(PropertyNames.Percentile, 50, 0, 100));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Radius, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("MedianEffect.ConfigDialog.RadiusLabel"));
            configUI.SetPropertyControlValue(PropertyNames.Percentile, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("MedianEffect.ConfigDialog.PercentileLabel"));

            return configUI;
        }

        public unsafe override ColorPixelBase Apply(ColorPixelBase src, int area, int* hb, int* hg, int* hr, int* ha)
        {
	        ColorPixelBase c = GetPercentile(this.percentile, area, hb, hg, hr, ha,src);
            return c;
        }

        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.radius = newToken.GetProperty<Int32Property>(PropertyNames.Radius).Value;
            this.percentile = newToken.GetProperty<Int32Property>(PropertyNames.Percentile).Value;

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public override unsafe void Render(EffectConfigToken parameters, RenderArgs DstArgs, RenderArgs SrcArgs, Rectangle[] rois, int startIndex, int length)
        {
	        foreach (Rectangle rect in rois)
	        {
		        RenderRect(this.radius, SrcArgs.Surface, DstArgs.Surface, rect);
	        }
        }
    }
}
