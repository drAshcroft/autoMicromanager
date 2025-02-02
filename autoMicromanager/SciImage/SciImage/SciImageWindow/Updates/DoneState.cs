/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace SciImage.Updates
{
    public class DoneState
        : UpdatesState
    {
        public override void OnEnteredState()
        {
            base.OnEnteredState();
        }

        public override void ProcessInput(object input, out State newState)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DoneState()
            : base(true, false, MarqueeStyle.None)
        {
        }
    }
}
