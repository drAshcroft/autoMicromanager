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
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class AddNewBlankLayerAction
        : Actions.PluginAction 
    {
        public override string ActionName
        {
            get
            {
                return "Add New Blank Layer";
            }
        }
        public override System.Drawing.Image MenuImage
        {
            get { return null; }
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift );
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
            DocumentWorkspace historyWorkspace = appWorkspace.ActiveDocumentWorkspace;
            int index = TargetLayerIndex;
            if (index == -1) index = appWorkspace.ActiveDocumentWorkspace.ActiveLayerIndex;
            BitmapLayer newLayer = null;
            ColorBgra clr = ColorBgra.White;
            clr.alpha = 0;
            newLayer = new BitmapLayer(historyWorkspace.Document.Width, historyWorkspace.Document.Height, clr  );
            string newLayerNameFormat = "Blank Layer {0}";
            newLayer.Name = string.Format(newLayerNameFormat, (1 + historyWorkspace.Document.Layers.Count).ToString());

            int newLayerIndex = index  + 1;

            NewLayerHistoryMemento ha = new NewLayerHistoryMemento(
                "New Layer",
                PdnResources.GetImageResource("Icons.MenuLayersAddNewLayerIcon.png"),
                historyWorkspace,
                newLayerIndex);


            historyWorkspace.Document.Layers.Insert(newLayerIndex, newLayer);
            if (OptionalHistoryRecord == null)
                historyWorkspace.History.PushNewMemento(ha);
            else
                OptionalHistoryRecord.Add(ha);
            return true;
        }

        public AddNewBlankLayerAction()
            : base(ActionFlags.None)
        {
        }
    }
}
