/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.Actions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SciImage.Menus
{
    public sealed class ViewMenu
        : PdnMenuItem
    {
       
        private ToolStripSeparator menuViewSeparator1;
        private PdnMenuItem menuViewGrid;
        private PdnMenuItem menuViewRulers;
        private ToolStripSeparator menuViewSeparator2;
        private PdnMenuItem menuViewPixels;
        private PdnMenuItem menuViewInches;
        private PdnMenuItem menuViewCentimeters;

        public System.Windows.Forms.ToolStripItem[] ViewItems()
        {
           return ( new System.Windows.Forms.ToolStripItem[] 
                {
                    this.menuViewSeparator1,
                    this.menuViewGrid,
                    this.menuViewRulers,
                    this.menuViewSeparator2,
                    this.menuViewPixels,
                    this.menuViewInches,
                    this.menuViewCentimeters,
                });

        }

        public void PopulateMenu(PdnMenuItem FirstMenus)
        {
           
            
            this.DropDownItems.AddRange(
                new System.Windows.Forms.ToolStripItem[] 
                {
                    this.menuViewSeparator1,
                    this.menuViewGrid,
                    this.menuViewRulers,
                    this.menuViewSeparator2,
                    this.menuViewPixels,
                    this.menuViewInches,
                    this.menuViewCentimeters,
                });

            ToolStripItemCollection tsic = FirstMenus.DropDown.Items;
            for (int i = 0; i < tsic.Count; i++)
            {
                this.DropDownItems.Add(tsic[i]);
            }
            
        }


        private bool OnOemPlusShortcut(Keys keys)
        {
            AppWorkspace.PerformAction(new ZoomInAction());
           // this.menuViewZoomIn.PerformClick();
            return true;
        }

        private bool OnOemMinusShortcut(Keys keys)
        {
            AppWorkspace.PerformAction(new ZoomOutAction());
           // this.menuViewZoomOut.PerformClick();
            return true;
        }

        private bool OnCtrlAltZero(Keys keys)
        {
            AppWorkspace.PerformAction(new ActualSizeAction());
           // this.menuViewActualSize.PerformClick();
            return true;
        }

        public ViewMenu()
        {
            InitializeComponent();
            PdnBaseForm.RegisterFormHotKey(Keys.Control | Keys.OemMinus, OnOemMinusShortcut);
            PdnBaseForm.RegisterFormHotKey(Keys.Control | Keys.Oemplus, OnOemPlusShortcut);
            PdnBaseForm.RegisterFormHotKey(Keys.Control | Keys.Alt | Keys.D0, OnCtrlAltZero);
        }

        private void InitializeComponent()
        {
            
            this.menuViewSeparator1 = new ToolStripSeparator();
            this.menuViewGrid = new PdnMenuItem();
            this.menuViewRulers = new PdnMenuItem();
            this.menuViewSeparator2 = new ToolStripSeparator();
            this.menuViewPixels = new PdnMenuItem();
            this.menuViewInches = new PdnMenuItem();
            this.menuViewCentimeters = new PdnMenuItem();
            // 
            // menuView
            // 
            
            this.Name = "Menu.View";
            this.Text = "View";// PdnResources.GetString("Menu.View.Text"); 
            // 
            // menuViewGrid
            // 
            this.menuViewGrid.Name = "Grid";
            this.menuViewGrid.Click += new System.EventHandler(this.MenuViewGrid_Click);
            // 
            // menuViewRulers
            // 
            this.menuViewRulers.Name = "Rulers";
            this.menuViewRulers.Click += new System.EventHandler(this.MenuViewRulers_Click);
            //
            // menuViewPixels
            //
            this.menuViewPixels.Name = "Pixels";
            this.menuViewPixels.Click += new EventHandler(MenuViewPixels_Click);
            this.menuViewPixels.Text = "Pixels";
            //
            // menuViewInches
            //
            this.menuViewInches.Name = "Inches";
            this.menuViewInches.Text = "Micrometers";
            this.menuViewInches.Click += new EventHandler(MenuViewInches_Click);
            //
            // menuViewCentimeters
            //
            this.menuViewCentimeters.Name = "Centimeters";
            this.menuViewCentimeters.Click += new EventHandler(MenuViewCentimeters_Click);
            this.menuViewCentimeters.Text = "Nanometers";
        }

        protected override void OnDropDownOpening(EventArgs e)
        {
            this.menuViewPixels.Checked = false;
            this.menuViewInches.Checked = false;
            this.menuViewCentimeters.Checked = false;

            switch (AppWorkspace.Units)
            {
                case MeasurementUnit.Pixel:
                    this.menuViewPixels.Checked = true;
                    break;

                case MeasurementUnit.Inch:
                    this.menuViewInches.Checked = true;
                    break;

                case MeasurementUnit.Centimeter:
                    this.menuViewCentimeters.Checked = true;
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            if (AppWorkspace.ActiveDocumentWorkspace != null)
            {
               
                this.menuViewGrid.Enabled = true;
                this.menuViewRulers.Enabled = true;
                this.menuViewPixels.Enabled = true;
                this.menuViewInches.Enabled = true;
                this.menuViewCentimeters.Enabled = true;

                this.menuViewGrid.Checked = AppWorkspace.ActiveDocumentWorkspace.DrawGrid;
                this.menuViewRulers.Checked = AppWorkspace.ActiveDocumentWorkspace.RulersEnabled;
            }
            else
            {
                
                this.menuViewGrid.Enabled = false;
                this.menuViewRulers.Enabled = false;
                this.menuViewPixels.Enabled = true;
                this.menuViewInches.Enabled = true;
                this.menuViewCentimeters.Enabled = true;
            }

            base.OnDropDownOpening(e);
        }

        private void MenuViewZoomIn_Click(object sender, System.EventArgs e)
        {
            if (AppWorkspace.ActiveDocumentWorkspace != null)
            {
                AppWorkspace.ActiveDocumentWorkspace.PerformAction(new ZoomInAction());
            }
        }

       

        private void MenuViewZoomToWindow_Click(object sender, EventArgs e)
        {
            if (AppWorkspace.ActiveDocumentWorkspace != null)
            {
                AppWorkspace.ActiveDocumentWorkspace.PerformAction(new ZoomToWindowAction());
            }
        }

        private void MenuViewZoomToSelection_Click(object sender, EventArgs e)
        {
            if (AppWorkspace.ActiveDocumentWorkspace != null)
            {
                AppWorkspace.ActiveDocumentWorkspace.PerformAction(new ZoomToSelectionAction());
            }
        }

        private void MenuViewPixels_Click(object sender, EventArgs e)
        {
            AppWorkspace.Units = MeasurementUnit.Pixel;
        }

        private void MenuViewInches_Click(object sender, EventArgs e)
        {
            AppWorkspace.Units = MeasurementUnit.Inch;
        }

        private void MenuViewCentimeters_Click(object sender, EventArgs e)
        {
            AppWorkspace.Units = MeasurementUnit.Centimeter;
        }

        private void MenuViewRulers_Click(object sender, System.EventArgs e)
        {
            if (AppWorkspace.ActiveDocumentWorkspace != null)
            {
                AppWorkspace.ActiveDocumentWorkspace.RulersEnabled = !AppWorkspace.ActiveDocumentWorkspace.RulersEnabled;
            }
        }

        private void MenuViewGrid_Click(object sender, System.EventArgs e)
        {
            if (AppWorkspace.ActiveDocumentWorkspace != null)
            {
                AppWorkspace.ActiveDocumentWorkspace.DrawGrid = !AppWorkspace.ActiveDocumentWorkspace.DrawGrid;
            }
        }
    }
}
