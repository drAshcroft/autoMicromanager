/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.Actions;
using SciImage.HistoryMementos;
using System;
using System.Drawing;
using System.Collections.Generic;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Actions
{
    public sealed class EraseSelectionAction
        : Actions.PluginAction 
    {
        private static string StaticName
        {
            get
            {
                return "Erase Selection";
            }
        }

        private static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuEditEraseSelectionIcon.png");
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
            get { return StaticImage.Reference ; }
        }
        public override string MenuName
        {
            get { return "Edit"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }

        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.Delete );
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 3; }
        }
        public override int OrderSuggestion
        {
            get { return 1; }
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null)
            {
                return ActionDisplayOptions.Visible;
            }
            return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;

        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace docWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (docWorkspace.Selection.IsEmpty)
            {
                return false;
            }

            SelectionHistoryMemento shm = new SelectionHistoryMemento(string.Empty, null, docWorkspace);

            PdnRegion region = docWorkspace.Selection.CreateRegion();

            int index = TargetLayerIndex;
            if (index == -1) index = appWorkspace.ActiveDocumentWorkspace.ActiveLayerIndex;

            Layer layer = (Layer)(appWorkspace.ActiveDocumentWorkspace.Document.Layers[index]);

            PdnRegion simplifiedRegion = Utility.SimplifyAndInflateRegion(region);

            HistoryMemento hm = new BitmapHistoryMemento(
                null, 
                null,
                docWorkspace,
                docWorkspace.ActiveLayerIndex, 
                simplifiedRegion);

            if (OptionalHistoryRecord == null)
            {
                HistoryMemento chm = new CompoundHistoryMemento(
                    StaticName,
                    StaticImage,
                    new HistoryMemento[] { shm, hm });

                //EnterCriticalRegion();
                docWorkspace.History.PushNewMemento(chm);
            }
            else
            {
                OptionalHistoryRecord.Add(shm);
                OptionalHistoryRecord.Add(hm);
            }
            layer.Surface.Clear(region, layer.Surface.ColorPixelBase.FromBgra(255, 255, 255, 0));

            layer.Invalidate(simplifiedRegion);
            docWorkspace.Document.Invalidate(simplifiedRegion);
            simplifiedRegion.Dispose();
            region.Dispose();
            docWorkspace.Selection.Reset();

            return true ;
        }

        public EraseSelectionAction()
            : base(ActionFlags.None)
        {
        }
    }
}
