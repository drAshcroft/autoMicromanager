/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class HistoryRewindAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "History Rewind";
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
            DateTime lastUpdate = DateTime.Now;

            documentWorkspace.History.BeginStepGroup();

            using (new WaitCursorChanger(documentWorkspace))
            {
                documentWorkspace.SuspendToolCursorChanges();

                while (documentWorkspace.History.UndoStack.Count > 1)
                {
                    documentWorkspace.History.StepBackward();

                    if ((DateTime.Now - lastUpdate).TotalMilliseconds >= 500)
                    {
                        documentWorkspace.History.EndStepGroup();
                        documentWorkspace.Update();
                        lastUpdate = DateTime.Now;
                        documentWorkspace.History.BeginStepGroup();
                    }
                }

                documentWorkspace.ResumeToolCursorChanges();
            }

            documentWorkspace.History.EndStepGroup();

            Utility.GCFullCollect();
            documentWorkspace.Document.Invalidate();
            documentWorkspace.Update();

            return true ;
        }

        public HistoryRewindAction()
            : base(ActionFlags.KeepToolActive)
        {
            // We use ActionFlags.KeepToolActive because the process of undo/redo has its own
            // set of protocols for determining whether to keep the tool active, or to refresh it
        }
    }
}
