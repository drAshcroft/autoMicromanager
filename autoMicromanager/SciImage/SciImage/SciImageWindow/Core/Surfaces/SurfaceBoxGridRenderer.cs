/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage
{
    public sealed class SurfaceBoxGridRenderer
        : SurfaceBoxRenderer
    {
        public override void OnDestinationSizeChanged()
        {
            if (this.Visible)
            {
                this.OwnerList.InvalidateLookups();
            }

            base.OnDestinationSizeChanged();
        }

        public override void OnSourceSizeChanged()
        {
            if (this.Visible)
            {
                this.OwnerList.InvalidateLookups();
            }

            base.OnSourceSizeChanged();
        }

        protected override void OnVisibleChanged()
        {
            if (this.Visible)
            {
                this.OwnerList.InvalidateLookups();
            }

            Invalidate();
        }

        public unsafe override void Render(Surface dst, System.Drawing.Point offset)
        {
            if (OwnerList.ScaleFactor < new ScaleFactor(2, 1))
            {
                return;
            }

            int[] d2SLookupX = OwnerList.Dst2SrcLookupX;
            int[] d2SLookupY = OwnerList.Dst2SrcLookupY;
            int[] s2DLookupX = OwnerList.Src2DstLookupX;
            int[] s2DLookupY = OwnerList.Src2DstLookupY;

            ColorPixelBase[] blackAndWhite = new ColorPixelBase[2] { dst.ColorPixelBase.WhiteColor(), dst.ColorPixelBase.BlackColor() };

            // draw horizontal lines
            int sTop = d2SLookupY[offset.Y];
            int sBottom = d2SLookupY[offset.Y + dst.Height];

            for (int srcY = sTop; srcY <= sBottom; ++srcY)
            {
                int dstY = s2DLookupY[srcY];
                int dstRow = dstY - offset.Y;

                if (dst.IsRowVisible(dstRow))
                {
                    //ColorPixelBase *dstRowPtr = dst.GetRowAddress(dstRow);
                    //ColorPixelBase *dstRowEndPtr = dstRowPtr + dst.Width;

                    //dstRowPtr += offset.X & 1;
                    for (int x = 0; x < dst.Width; x+=2)
                    {
                        dst[x, dstRow] = dst.ColorPixelBase.BlackColor().ToInt32();
                      
                    }
                }
            }

            // draw vertical lines
            int sLeft = d2SLookupX[offset.X];
            int sRight = d2SLookupX[offset.X + dst.Width];

            for (int srcX = sLeft; srcX <= sRight; ++srcX)
            {
                int dstX = s2DLookupX[srcX];
                int dstCol = dstX - offset.X;

                if (dst.IsColumnVisible(dstX - offset.X))
                {
                    //byte *dstColPtr = (byte *)dst.GetPointAddress(dstCol, 0);
                    //byte *dstColEndPtr = dstColPtr + dst.Stride * dst.Height;

                    //dstColPtr += (offset.Y & 1) * dst.Stride;

                    for (int y = 0; y < dst.Height ; y += 2)
                    {
                        dst[dstCol , y] = dst.ColorPixelBase.BlackColor().ToInt32();

                    }
                    
                }
            }
        }

        public SurfaceBoxGridRenderer(SurfaceBoxRendererList ownerList)
            : base(ownerList)
        {
        }
    }
}
