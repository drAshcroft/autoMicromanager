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
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Effects
{
    public sealed class PixelateEffect
        : Effect 
    {
        public static string StaticName
        {
            get
            {
                return "Pixelate";
            }
        }

        public PixelateEffect() 
            : base(StaticName,
                   PdnResources.GetImageResource("Icons.PixelateEffect.png").Reference,
                   SubmenuNames.Distort,
                   EffectFlags.Configurable)
        {
        }

        public enum PropertyNames
        {
            CellSize = 0
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            List<Property> props = new List<Property>();

            props.Add(new Int32Property(PropertyNames.CellSize, 2, 1, 100));

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.CellSize, ControlInfoPropertyNames.DisplayName, PdnResources.GetString("PixelateEffect.ConfigDialog.SliderLabel"));
            // TODO: units label?
            //aecg.SliderUnitsName = PdnResources.GetString("PixelateEffect.ConfigDialog.SliderUnitsName");

            return configUI;
        }

        private ColorPixelBase ComputeCellColor(int x, int y, RenderArgs src, int cellSize)
        {
            Rectangle cell = GetCellBox(x, y, cellSize);
            cell.Intersect(src.Bounds);
            
            int left = cell.Left;
            int right = cell.Right - 1;
            int bottom = cell.Bottom - 1;
            int top = cell.Top;
 
            ColorPixelBase colorTopLeft = src.Surface[left, top,src.Surface.ColorPixelBase ];
            ColorPixelBase colorTopRight = src.Surface[right, top, src.Surface.ColorPixelBase];
            ColorPixelBase colorBottomLeft = src.Surface[left, bottom, src.Surface.ColorPixelBase];
            ColorPixelBase colorBottomRight = src.Surface[right, bottom, src.Surface.ColorPixelBase];

            ColorPixelBase c = src.Surface.ColorPixelBase.BlendColors4W16IP(colorTopLeft, 16384, colorTopRight, 16384, colorBottomLeft, 16384, colorBottomRight, 16384);

            return c;
        }

        private Rectangle GetCellBox(int x, int y, int cellSize)
        {
            int widthBoxNum = x % cellSize;
            int heightBoxNum = y % cellSize;
            Point leftUpper = new Point(x - widthBoxNum, y - heightBoxNum);
            Rectangle returnMe = new Rectangle(leftUpper, new Size(cellSize, cellSize));
            return returnMe;
        }

        private int cellSize;
        protected override void OnSetRenderInfo(EffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            this.cellSize = newToken.GetProperty<Int32Property>(PropertyNames.CellSize).Value;
            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        public unsafe override void Render(EffectConfigToken  parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            for (int i = startIndex; i < startIndex + length; ++i)
            {
                Rectangle rect = rois[i];

                for (int y = rect.Top; y < rect.Bottom; ++y)
                {
                    int yEnd = y + 1;

                    for (int x = rect.Left; x < rect.Right; ++x)
                    {
                        Rectangle cellRect = GetCellBox(x, y, this.cellSize);
                        cellRect.Intersect(dstArgs.Bounds);
                        ColorPixelBase color = ComputeCellColor(x, y, srcArgs, this.cellSize);

                        int xEnd = Math.Min(rect.Right, cellRect.Right);
                        yEnd = Math.Min(rect.Bottom, cellRect.Bottom);

                        for (int y2 = y; y2 < yEnd; ++y2)
                        {
                            

                            for (int x2 = x; x2 < xEnd; ++x2)
                            {
                                //ColorPixelBase ptr = dstArgs.Surface.GetPoint(x2, y2);
                                dstArgs.Surface[x2,y2 ]= color.ToInt32();
                            }
                        }

                        x = xEnd - 1;
                    }

                    y = yEnd - 1;
                }
            }
        }
    }
}
