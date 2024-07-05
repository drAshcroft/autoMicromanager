/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.PropertySystem;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SciImage.Effects
{
    [EffectCategory(EffectCategory.Adjustment)]
    public sealed class InvertColorsEffect
        : Effect 
    {
        private UnaryPixelOps.Invert invertOp;

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return PropertyCollection.CreateEmpty();
        }

        public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            this.invertOp.Apply(dstArgs.Surface, srcArgs.Surface, rois, startIndex, length);
        }

        public InvertColorsEffect()
            : base("Invert Colors",
                   PdnResources.GetImageResource("Icons.InvertColorsEffect.png").Reference,
                   null,
                   EffectFlags.None)
        {
            this.invertOp = new UnaryPixelOps.Invert();
        }
    }
}
