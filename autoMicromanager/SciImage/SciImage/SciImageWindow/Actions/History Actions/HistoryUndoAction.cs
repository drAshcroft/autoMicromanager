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
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class HistoryUndoAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Undo";
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 1; }
        }
        public override int OrderSuggestion
        {
            get { return 0; }
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
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (documentWorkspace.History.UndoStack.Count > 0)
            {
                if (!(documentWorkspace.History.UndoStack[documentWorkspace.History.UndoStack.Count - 1] is NullHistoryMemento))
                {
                    using (new WaitCursorChanger(documentWorkspace.FindForm()))
                    {
                        documentWorkspace.History.StepBackward();
                        documentWorkspace.Update();
                    }
                }

                Utility.GCFullCollect();
            }

            return true ;
        }

        public HistoryUndoAction()
            : base(ActionFlags.KeepToolActive)
        {
            // We use ActionFlags.KeepToolActive because the process of undo/redo has its own
            // set of protocols for determine whether to keep the tool active, or to refresh it
        }
    }
}
