/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class SaveAsFileAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Save As...";
            }
        }
        public override System.Drawing.Image MenuImage
        {
            get { return null; }
        }
        public override string MenuName
        {
            get { return "File"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }
        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.Control | Keys.Shift  | System.Windows.Forms.Keys.S);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 2; }
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
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (documentWorkspace != null)
            {
                documentWorkspace.DoSaveAs();
            }
            return true ;
        }

        public SaveAsFileAction()
        {
        }
    }
}
