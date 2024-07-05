/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.Effects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SciImage.Menus
{
    public sealed class EffectsMenu
        : EffectMenuBase
    {
        public EffectsMenu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Name = "Menu.Effects";
            this.Text = "Plugins and Filters";// PdnResources.GetString("Menu.Effects.Text");
        }

        protected override bool EnableEffectShortcuts
        {
            get
            {
                return false;
            }
        }

        protected override bool EnableRepeatEffectMenuItem
        {
            get
            {
                return true;
            }
        }

        protected override bool FilterEffects(Effect effect)
        {
            return (effect.Category == EffectCategory.Effect);
        }

        public void AddSubMenuItems(PdnMenuItem MenuItems)
        {
            this.DropDown.Items.Add(new ToolStripSeparator());
            this.DropDown.Items.AddRange(MenuItems.DropDown.Items);
        }
    }
}
