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
    public sealed class DeleteLayerAction
        : Actions.PluginAction 
    {

        private static string StaticName
        {
            get
            {
                return "Delete Layer";
            }
        }

        private static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuLayersDeleteLayerIcon.png");
            }
        }
        public override string ActionName
        {
            get
            {
                return StaticName;
            }
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
            get { return 2; }
        }

        public override System.Drawing.Image MenuImage
        {
            get { return StaticImage.Reference; }
        }
        public override string MenuName
        {
            get { return "Layers"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }

        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null || documentWorkspace.ActiveLayer==null)
            {
                return ActionDisplayOptions.Visible;
            }
            return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;

        }
        public override bool  PerformAction(AppWorkspace appWorkspace, System.Collections.Generic.List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace historyWorkspace = appWorkspace.ActiveDocumentWorkspace;
            int index = TargetLayerIndex;
            if (index == -1) index = appWorkspace.ActiveDocumentWorkspace.ActiveLayerIndex;
            int layerIndex=index ;
            if (layerIndex < 0 || layerIndex >= historyWorkspace.Document.Layers.Count)
            {
                throw new ArgumentOutOfRangeException("layerIndex = " + layerIndex + 
                    ", expected [0, " + historyWorkspace.Document.Layers.Count + ")");
            }

            HistoryMemento hm = new DeleteLayerHistoryMemento(StaticName, StaticImage, historyWorkspace, historyWorkspace.Document.Layers.GetAt(layerIndex));

            
            historyWorkspace.Document.Layers.RemoveAt(layerIndex);
            if (OptionalHistoryRecord == null)
                historyWorkspace.History.PushNewMemento(hm);
            else
                OptionalHistoryRecord.Add(hm);
            return true ;
        }

        public DeleteLayerAction()
            : base(ActionFlags.None)
        {
            
        }
    }
}
