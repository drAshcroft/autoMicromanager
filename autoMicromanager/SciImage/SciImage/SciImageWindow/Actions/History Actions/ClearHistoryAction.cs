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
using System.Windows.Forms;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class ClearHistoryAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Clear History";
            }
        }
        public override System.Drawing.Image MenuImage
        {
            get { return null; }
        }
        public override string MenuName
        {
            get { return ""; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }
        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.F9);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 0; }
        }
        public override int OrderSuggestion
        {
            get { return 0; }
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {

            return ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (DialogResult.Yes == Utility.AskYesNo(documentWorkspace, 
                PdnResources.GetString("ClearHistory.Confirmation")))
            {
                documentWorkspace.History.ClearAll();

                documentWorkspace.History.PushNewMemento(new NullHistoryMemento(
                    PdnResources.GetString("ClearHistory.HistoryMementoName"),
                    PdnResources.GetImageResource("Icons.MenuLayersDeleteLayerIcon.png")));
            }

            return true ;
        }

        public ClearHistoryAction()
            : base(ActionFlags.None)
        {
        }
    }
}
