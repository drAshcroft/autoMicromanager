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
using System.Windows.Forms;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Tools
{
    public class ColorPickerTool : Tool
    {
        private bool mouseDown;
        private Cursor colorPickerToolCursor;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mouseDown)
            {
                PickColor(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseDown = true;
        
            PickColor(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseDown = false;

            switch (AppEnvironment.ColorPickerClickBehavior)
            {
                case SciImage.ColorPickers.ColorPickerClickBehavior.NoToolSwitch:
                    break;

                case SciImage.ColorPickers.ColorPickerClickBehavior.SwitchToLastTool:
                    DocumentWorkspace.SetToolFromType(DocumentWorkspace.PreviousActiveToolType);
                    break;

                case SciImage.ColorPickers.ColorPickerClickBehavior.SwitchToPencilTool:
                    DocumentWorkspace.SetToolFromType(typeof(PencilTool));
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }

        private ColorPixelBase LiftColor(int x, int y)
        {
            ColorPixelBase newColor;
            newColor = ((Layer)ActiveLayer).Surface.GetPoint(x, y);
            return newColor;
        }

        private void PickColor(MouseEventArgs e)
        {
            if (!Document.Bounds.Contains(e.X, e.Y))
            {
                return;
            }

            ColorPixelBase color;
            color = LiftColor(e.X, e.Y);

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.AppEnvironment.PrimaryColor = color;
            }
            else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {   
                this.AppEnvironment.SecondaryColor = color;
            }
        }

        protected override void OnActivate()
        {
            this.colorPickerToolCursor = new Cursor(PdnResources.GetResourceStream("Cursors.ColorPickerToolCursor.cur"));
            this.Cursor = this.colorPickerToolCursor;
            base.OnActivate();
        }

        protected override void OnDeactivate()
        {
            if (this.colorPickerToolCursor != null)
            {
                this.colorPickerToolCursor.Dispose();
                this.colorPickerToolCursor = null;
            }

            base.OnDeactivate();
        }

        public ColorPickerTool(DocumentWorkspace documentWorkspace)
            : base(documentWorkspace,
                   PdnResources.GetImageResource("Icons.ColorPickerToolIcon.png"),
                   "Color sampler",
                   "Left for copied primary color, right click for copying the secondary color",
                   'k',
                   true,
                   ToolBarConfigItems.ColorPickerBehavior)
        {
            // initialize any state information you need
            mouseDown = false;
        }
    }
}