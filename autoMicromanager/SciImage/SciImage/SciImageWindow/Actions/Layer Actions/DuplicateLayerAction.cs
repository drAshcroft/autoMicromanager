/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.HistoryMementos;
using System;
using SciImage.Actions;

namespace SciImage.Actions
{
    public sealed class DuplicateLayerAction
        : Actions.PluginAction 
    {
        public static string StaticName
        {
            get
            {
                return "Duplicate Layer";
            }
        }

        public static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuLayersDuplicateLayerIcon.png");
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
            get { return "Layers"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }

        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D | System.Windows.Forms.Keys.Shift );
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 1; }
        }
        public override int OrderSuggestion
        {
            get { return 3; }
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null || documentWorkspace.ActiveLayer == null)
            {
                return ActionDisplayOptions.Visible;
            }
            return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;

        }
        public override bool PerformAction(AppWorkspace appWorkspace, System.Collections.Generic.List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace historyWorkspace = appWorkspace.ActiveDocumentWorkspace;
            int index = TargetLayerIndex;
            if (index == -1) index = appWorkspace.ActiveDocumentWorkspace.ActiveLayerIndex;
            int layerIndex = index;
            if (layerIndex < 0 || layerIndex >= historyWorkspace.Document.Layers.Count)
            {
                throw new ArgumentOutOfRangeException("layerIndex = " + layerIndex + ", expected [0, " + historyWorkspace.Document.Layers.Count + ")");
            }

            Layer newLayer = null;

            newLayer = (Layer)historyWorkspace.ActiveLayer.Clone();
            newLayer.IsBackground = false;
            int newIndex = 1 + layerIndex;

            HistoryMemento ha = new NewLayerHistoryMemento(
                StaticName,
                StaticImage,
                historyWorkspace,
                newIndex);

            //EnterCriticalRegion();
            historyWorkspace.Document.Layers.Insert(newIndex, newLayer);
            newLayer.Invalidate();
            if (OptionalHistoryRecord == null)
                historyWorkspace.History.PushNewMemento(ha);
            else
                OptionalHistoryRecord.Add(ha);

            return true ;
        }

        public DuplicateLayerAction()
            : base(ActionFlags.None)
        {
           
        }
    }
}
