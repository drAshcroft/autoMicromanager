/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage;
using SciImage.SystemLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SciImage.Actions
{
    public sealed class CloseAllWorkspacesAction
        : SciImage.Actions.PluginAction
    {
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {

            return ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
        }
        public override string ActionName
        {
            get
            {
                return "Close All Pictures";
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
                return (System.Windows.Forms.Keys.F9);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 4; }
        }
        public override int OrderSuggestion
        {
            get { return 1; }
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace originalDW = appWorkspace.ActiveDocumentWorkspace;

            int oldLatency = 10;

            try
            {
                oldLatency = appWorkspace.Widgets.DocumentStrip.ThumbnailUpdateLatency;
                appWorkspace.Widgets.DocumentStrip.ThumbnailUpdateLatency = 0;
            }

            catch (NullReferenceException)
            {
                // See bug #2544
            }

            List<DocumentWorkspace> unsavedDocs = new List<DocumentWorkspace>();
            foreach (DocumentWorkspace dw in appWorkspace.DocumentWorkspaces)
            {
                if (dw.Document != null && dw.Document.Dirty)
                {
                    unsavedDocs.Add(dw);
                }
            }

            if (unsavedDocs.Count == 1)
            {
                CloseWorkspaceAction cwa = new CloseWorkspaceAction();
                cwa.PerformAction(appWorkspace,OptionalHistoryRecord,TargetLayerIndex );
                
            }
            else if (unsavedDocs.Count > 1)
            {
                using (UnsavedChangesDialog dialog = new UnsavedChangesDialog())
                {
                    dialog.DocumentClicked += (s, e2) => { appWorkspace.ActiveDocumentWorkspace = e2.Data; };

                    dialog.Documents = unsavedDocs.ToArray();

                    if (appWorkspace.ActiveDocumentWorkspace.Document.Dirty)
                    {
                        dialog.SelectedDocument = appWorkspace.ActiveDocumentWorkspace;
                    }

                    Form mainForm = appWorkspace.FindForm();
                    if (mainForm != null)
                    {
                        PdnBaseForm asPDF = mainForm as PdnBaseForm;

                        if (asPDF != null)
                        {
                            asPDF.RestoreWindow();
                        }
                    }
                    

                    DialogResult dr = Utility.ShowDialog(dialog, appWorkspace);

                    switch (dr)
                    {
                        case DialogResult.Yes:
                            {
                                foreach (DocumentWorkspace dw in unsavedDocs)
                                {
                                    appWorkspace.ActiveDocumentWorkspace = dw;
                                    bool result = dw.DoSave();

                                    if (result)
                                    {
                                        appWorkspace.RemoveDocumentWorkspace(dw);
                                    }
                                    else
                                    {
                                        
                                        break;
                                    }
                                }
                            }
                            break;

                        case DialogResult.No:
                            
                            break;

                        case DialogResult.Cancel:
                            
                            break;

                        default:
                            throw new InvalidEnumArgumentException();
                    }
                }
            }

            try
            {
                appWorkspace.Widgets.DocumentStrip.ThumbnailUpdateLatency = oldLatency;
            }

            catch (NullReferenceException)
            {
                // See bug #2544
            }

           
                UI.SuspendControlPainting(appWorkspace);
                
                foreach (DocumentWorkspace dw in appWorkspace.DocumentWorkspaces)
                {
                    appWorkspace.RemoveDocumentWorkspace(dw);
                }

                UI.ResumeControlPainting(appWorkspace);
                appWorkspace.Invalidate(true);
            
            return true ;
        }

        public CloseAllWorkspacesAction()
        {
            
        }
    }
}
