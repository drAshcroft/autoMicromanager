/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.Actions;
using System;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class PasteInToNewLayerAction  : Actions.PluginAction 
    {
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null)
            {
                return ActionDisplayOptions.Visible;
            }
            return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;

        }
        public override string ActionName
        {
            get
            {
                return "Paste in New Layer";
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Shift );
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 2; }
        }
        public override int OrderSuggestion
        {
            get { return 5; }
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            bool hfr = new AddNewBlankLayerAction().PerformAction(appWorkspace, OptionalHistoryRecord, TargetLayerIndex);
            
            if (hfr == true )
            {
                PasteAction pa = new PasteAction();
                bool result = pa.PerformAction(documentWorkspace.AppWorkspace,OptionalHistoryRecord,TargetLayerIndex  );

                if (!result)
                {
                    using (new WaitCursorChanger(documentWorkspace))
                    {
                        if (OptionalHistoryRecord ==null)
                            documentWorkspace.History.StepBackward();
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public PasteInToNewLayerAction()
        {
        }
    }
}
