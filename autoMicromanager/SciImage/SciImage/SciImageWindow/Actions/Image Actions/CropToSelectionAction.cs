/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.HistoryMementos;
using SciImage.Actions;
using System;
using System.Drawing;
using System.Collections.Generic;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Actions
{
    /// <summary>
    /// Crops the image to the currently selected region.
    /// </summary>
    public sealed class CropToSelectionAction
        : Actions.PluginAction 
    {
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null)
            {
                return ActionDisplayOptions.Visible;
            }
            if (documentWorkspace.ActiveLayer.GetType() == typeof(BitmapLayer))
            {
                return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
            }
            return ActionDisplayOptions.Hidden;
        }
        private static string StaticName
        {
            get
            {
                return "Crop To Selection";
            }
        }

        public override string ActionName
        {
            get
            {
                return StaticName ;
            }
        }
        public override System.Drawing.Image MenuImage
        {
            get { return null; }
        }
        public override string MenuName
        {
            get { return "Image"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }
        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Shift );
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 1; }
        }
        public override int OrderSuggestion
        {
            get { return 1; }
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace docWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (docWorkspace.Selection.IsEmpty)
            {
                return false ;
            }
            else
            {
                PdnRegion selectionRegion = docWorkspace.Selection.CreateRegion();

                if (selectionRegion.GetArea() == 0)
                {
                    selectionRegion.Dispose();
                    return false ;
                }

                SelectionHistoryMemento sha = new SelectionHistoryMemento(StaticName, null, docWorkspace);
                ReplaceDocumentHistoryMemento rdha = new ReplaceDocumentHistoryMemento(StaticName, null, docWorkspace);
                Rectangle boundingBox;
                Rectangle[] inverseRegionRects = null;

                boundingBox = Utility.GetRegionBounds(selectionRegion);

                using (PdnRegion inverseRegion = new PdnRegion(boundingBox))
                {
                    inverseRegion.Exclude(selectionRegion);

                    inverseRegionRects = Utility.TranslateRectangles(
                        inverseRegion.GetRegionScansReadOnlyInt(),
                        -boundingBox.X,
                        -boundingBox.Y);
                }

                selectionRegion.Dispose();
                selectionRegion = null;

                Document oldDocument = docWorkspace.Document; // TODO: serialize this to disk so we don't *have* to store the full thing
                Document newDocument = new Document(boundingBox.Width, boundingBox.Height);

                // copy the document's meta data over
                newDocument.ReplaceMetaDataFrom(oldDocument);

                foreach (Layer layer in oldDocument.Layers)
                {
                    if (layer is BitmapLayer)
                    {
                        BitmapLayer oldLayer = (BitmapLayer)layer;
                        Surface croppedSurface = oldLayer.Surface.CreateWindow(boundingBox );
                        BitmapLayer newLayer = new BitmapLayer(croppedSurface);

                        ColorPixelBase clearWhite = layer.Surface.ColorPixelBase.WhiteColor();
                        clearWhite.alpha = 0;

                        foreach (Rectangle rect in inverseRegionRects)
                        {
                            newLayer.Surface.Clear(rect, clearWhite);
                        }

                        newLayer.LoadProperties(oldLayer.SaveProperties());
                        newDocument.Layers.Add(newLayer);
                    }
                    else
                    {
                        throw new InvalidOperationException("Crop does not support Layers that are not BitmapLayers");
                    }
                }

               

                //EnterCriticalRegion();
                docWorkspace.Document = newDocument;
                if (OptionalHistoryRecord == null)
                {
                    CompoundHistoryMemento cha = new CompoundHistoryMemento(
                       StaticName,
                       PdnResources.GetImageResource("Icons.MenuImageCropIcon.png"),
                       new HistoryMemento[] { sha, rdha });
                    docWorkspace.History.PushNewMemento(cha);
                }
                else
                {
                    OptionalHistoryRecord.Add(sha);
                    OptionalHistoryRecord.Add(rdha);
                }
                return true ;
            }
        }

        public CropToSelectionAction()
            : base(ActionFlags.None)
        {
        }
    }
}
