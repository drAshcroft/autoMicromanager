/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;

namespace SciImage.Effects
{
    [Obsolete]
    public sealed class EffectTypeHintAttribute
        : Attribute
    {
        private EffectTypeHint effectTypeHint;
        public EffectTypeHint EffectTypeHint
        {
            get
            {
                return effectTypeHint;
            }
        }

        public EffectTypeHintAttribute(EffectTypeHint effectTypeHint)
        {
            this.effectTypeHint = effectTypeHint;
        }
    }
}
