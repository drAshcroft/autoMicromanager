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
using System.ComponentModel;
using System.Windows.Forms;

namespace SciImage.IndirectUI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyControlPropertyAttribute
        : Attribute
    {
        private object defaultValue;
        private string defaultValueResourceName;

        public object DefaultValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.defaultValueResourceName))
                {
                    return this.defaultValue;
                }
                else
                {
                    return PdnResources.GetString(this.defaultValueResourceName);
                }
            }

            set
            {
                this.defaultValue = value;
            }
        }

        public string DefaultValueResourceName
        {
            get
            {
                return this.defaultValueResourceName;
            }

            set
            {
                this.defaultValueResourceName = value;
            }
        }

        public PropertyControlPropertyAttribute()
        {
        }
    }
}