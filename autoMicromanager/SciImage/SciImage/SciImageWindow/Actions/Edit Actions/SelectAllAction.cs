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
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class SelectAllAction
        : Actions.PluginAction 
    {
        public static string StaticName
        {
            get
            {
                return "Select All";
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 3; }
        }
        public override int OrderSuggestion
        {
            get { return 4; }
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
            
            SelectionHistoryMemento sha = new SelectionHistoryMemento(
                StaticName, 
                PdnResources.GetImageResource("Icons.MenuEditSelectAllIcon.png"),
                historyWorkspace);

           // EnterCriticalRegion();
            historyWorkspace.Selection.PerformChanging();
            historyWorkspace.Selection.Reset();
            historyWorkspace.Selection.SetContinuation(historyWorkspace.Document.Bounds, CombineMode.Replace);
            historyWorkspace.Selection.CommitContinuation();
            historyWorkspace.Selection.PerformChanged();
            if (OptionalHistoryRecord == null)
                historyWorkspace.History.PushNewMemento(sha);
            else
                OptionalHistoryRecord.Add(sha);
            return true ;
        }

        public SelectAllAction()
            : base(ActionFlags.None)
        {
        }
    }
}
