/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.SystemLayer;
using System;
using System.Drawing;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage
{
    /// <summary>
    /// Provides a set of standard BinaryPixelOps.
    /// </summary>
    public sealed class BinaryPixelOps
    {
        private BinaryPixelOps()
        {
        }

        // This is provided solely for data file format compatibility
        [Obsolete("User UserBlendOps.NormalBlendOp instead", true)]
        [Serializable]
        public class AlphaBlend
            : BinaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase lhs, ColorPixelBase rhs)
            {
                return lhs;
            }
        }

        /// <summary>
        /// F(lhs, rhs) = rhs.A + lhs.R,g,b
        /// </summary>
        public class SetAlphaChannel
            : BinaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase lhs, ColorPixelBase rhs)
            {
                lhs.alpha  = rhs.alpha ;
                return lhs;
            }
        }

        /// <summary>
        /// F(lhs, rhs) = lhs.R,g,b + rhs.A
        /// </summary>
        public class SetColorChannels
            : BinaryPixelOp
        {
            
            public override ColorPixelBase Apply(ColorPixelBase lhs, ColorPixelBase rhs)
            {
                rhs.alpha  = lhs.alpha ;
                return rhs;
            }
        }

        /// <summary>
        /// result(lhs,rhs) = rhs
        /// </summary>
        [Serializable]
        public class AssignFromRhs
            : BinaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase lhs, ColorPixelBase rhs)
            {
                return rhs;
            }

            /*public unsafe override void Apply(ColorPixelBase* dst, ColorPixelBase* lhs, ColorPixelBase* rhs, int length)
            {
                Memory.Copy(dst, rhs, (ulong)length * (ulong)ColorPixelBase.SizeOf);
            }

            public unsafe override void Apply(ColorPixelBase* dst, ColorPixelBase* src, int length)
            {
                Memory.Copy(dst, src, (ulong)length * (ulong)ColorPixelBase.SizeOf);
            }*/
            
            public AssignFromRhs()
            {
            }
        }

        /// <summary>
        /// result(lhs,rhs) = lhs
        /// </summary>
        [Serializable]
        public class AssignFromLhs
            : BinaryPixelOp
        {
            public override ColorPixelBase Apply(ColorPixelBase lhs, ColorPixelBase rhs)
            {
                return lhs;
            }

            public AssignFromLhs()
            {
            }
        }

        [Serializable]
        public class Swap
            : BinaryPixelOp
        {
            BinaryPixelOp swapMyArgs;

            public override ColorPixelBase Apply(ColorPixelBase lhs, ColorPixelBase rhs)
            {
                return swapMyArgs.Apply(rhs, lhs);
            }

            public Swap(BinaryPixelOp swapMyArgs)
            {
                this.swapMyArgs = swapMyArgs;
            }
        }
    }
}
