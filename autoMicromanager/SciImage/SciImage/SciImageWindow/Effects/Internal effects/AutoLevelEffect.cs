/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage;
using SciImage.PropertySystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SciImage.Effects
{
    [EffectCategory(EffectCategory.Adjustment)]
    public sealed class AutoLevelEffect
        : Effect 
    {
        private UnaryPixelOps.Level levels = null;

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return PropertyCollection.CreateEmpty();
        }

        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            HistogramRgb histogram = new HistogramRgb();
            histogram.UpdateHistogram(srcArgs.Surface, this.EnvironmentParameters.GetSelection(dstArgs.Bounds));
            this.levels = histogram.MakeLevelsAuto();

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            if (this.levels.isValid)
            {
                this.levels.Apply(dstArgs.Surface, srcArgs.Surface, rois, startIndex, length);
            }
        }

        public AutoLevelEffect()
            : base("Auto-Level",
                   PdnResources.GetImageResource("Icons.AutoLevel.png").Reference,
                   null,
                   EffectFlags.None)
        {
        }
    }
}
