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
    public sealed class OpenFileAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Open...";
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O);
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
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {

            return ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            string filePath;

            if (appWorkspace.ActiveDocumentWorkspace == null)
            {
                filePath = null;
            }
            else
            {
                // Default to the directory the active document came from
                string fileName;
                FileType fileType;
                SaveConfigToken saveConfigToken;
                appWorkspace.ActiveDocumentWorkspace.GetDocumentSaveOptions(out fileName, out fileType, out saveConfigToken);
                filePath = Path.GetDirectoryName(fileName);
            }

            string[] newFileNames;
            DialogResult result = DocumentWorkspace.ChooseFiles(appWorkspace, out newFileNames, true, filePath);

            if (result == DialogResult.OK)
            {
                appWorkspace.OpenFilesInNewWorkspace(newFileNames);
            }
            return true ;
        }

        public OpenFileAction()
        {
        }
    }
}
