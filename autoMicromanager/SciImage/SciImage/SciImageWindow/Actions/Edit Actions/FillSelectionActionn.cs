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
    public sealed class FillSelectionAction
        : Actions.PluginAction 
    {
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null)
            {
                return ActionDisplayOptions.Visible;
            }
            if (documentWorkspace.ActiveLayer.Surface !=null)
            {
                return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
            }
            return ActionDisplayOptions.Hidden;
        }
        private static string StaticName
        {
            get
            {
                return "Fill Selection";
            }
        }

        private static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuEditFillSelectionIcon.png");
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
                return (System.Windows.Forms.Keys.F9 );
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 3; }
        }
        public override int OrderSuggestion
        {
            get { return 2; }
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace docWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (docWorkspace.Selection.IsEmpty)
            {
                return false ;
            }

            PdnRegion region = docWorkspace.Selection.CreateRegion();

            int index = TargetLayerIndex;
            if (index == -1) index = appWorkspace.ActiveDocumentWorkspace.ActiveLayerIndex;

            BitmapLayer layer = (BitmapLayer)(appWorkspace.ActiveDocumentWorkspace.Document.Layers[index]);

            PdnRegion simplifiedRegion = Utility.SimplifyAndInflateRegion(region);

            HistoryMemento hm = new BitmapHistoryMemento(
                StaticName,
                StaticImage,
                docWorkspace,
                docWorkspace.ActiveLayerIndex,
                simplifiedRegion);
            if (OptionalHistoryRecord == null)
                docWorkspace.History.PushNewMemento(hm);
            else
                OptionalHistoryRecord.Add(hm);
            //EnterCriticalRegion();
            ColorPixelBase c = appWorkspace.AppEnvironment.PrimaryColor;
            layer.Surface.Clear(region, c);
            layer.Invalidate(simplifiedRegion);

            simplifiedRegion.Dispose();
            region.Dispose();

            return true;
        }

        public FillSelectionAction()
            : base(ActionFlags.None)
        {
            
        }
    }
}
