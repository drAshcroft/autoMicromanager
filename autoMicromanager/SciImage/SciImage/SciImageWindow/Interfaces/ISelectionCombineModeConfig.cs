/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing.Drawing2D;

namespace SciImage
{
    public interface ISelectionCombineModeConfig
    {
        event EventHandler SelectionCombineModeChanged;

        CombineMode SelectionCombineMode
        {
            get;
            set;
        }

        void PerformSelectionCombineModeChanged();
    }
}
