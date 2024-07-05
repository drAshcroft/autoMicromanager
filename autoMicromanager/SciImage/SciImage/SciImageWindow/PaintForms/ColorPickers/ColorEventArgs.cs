/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.ColorPickers
{
    [Serializable]
    public class ColorEventArgs
        : System.EventArgs
    {
        private ColorPixelBase color;
        public ColorPixelBase Color
        {
            get
            {
                return color;
            }
        }

        public ColorEventArgs(ColorPixelBase color)
        {
            this.color = color;
        }
    }
}
