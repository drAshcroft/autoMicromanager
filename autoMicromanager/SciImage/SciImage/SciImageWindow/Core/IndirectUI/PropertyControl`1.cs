/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) Rick Brewster, Tom Jackson, and past contributors.            //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.PropertySystem;
using System;
using System.Windows.Forms;

namespace SciImage.IndirectUI
{
    public abstract class PropertyControl<TValue, TProperty>
        : PropertyControl
          where TProperty : Property<TValue>
    {
        public new TProperty Property
        {
            get
            {
                return (TProperty)base.Property;
            }
        }

        public PropertyControl(PropertyControlInfo propInfo)
            : base(propInfo)
        {
        }
    }
}
