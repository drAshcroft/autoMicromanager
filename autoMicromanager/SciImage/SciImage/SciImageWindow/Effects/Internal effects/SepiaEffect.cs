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
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    [EffectCategory(EffectCategory.Adjustment)]
    public sealed class SepiaEffect
        : Effect 
    {
        private UnaryPixelOp levels;
        private UnaryPixelOp desaturate;

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return PropertyCollection.CreateEmpty();
        }

        public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            this.desaturate.Apply(dstArgs.Surface, srcArgs.Surface, rois, startIndex, length);
            this.levels.Apply(dstArgs.Surface, dstArgs.Surface, rois, startIndex, length);
        }

        public SepiaEffect()
            : base("Sepia",
                   PdnResources.GetImageResource("Icons.SepiaEffect.png").Reference,
                   null,
                   EffectFlags.None)
        {
            this.desaturate = new UnaryPixelOps.Desaturate();

            this.levels = new UnaryPixelOps.Level(
                ColorBgra.Black,
                ColorBgra.White,
                new float[] { 1.2f, 1.0f, 0.8f },
                ColorBgra.Black,
                ColorBgra.White);
        }
    }
}
