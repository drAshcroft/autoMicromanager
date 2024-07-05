/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class OpenActiveLayerPropertiesAction
        : SciImage.Actions.PluginAction
    {
        public override string ActionName
        {
            get
            {
                return "Layer Properties...";
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
                return (System.Windows.Forms.Keys.F4 );
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 3; }
        }
        public override int OrderSuggestion
        {
            get { return 1; }
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null || documentWorkspace.ActiveLayer == null)
            {
                return ActionDisplayOptions.Visible;
            }
            return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;

        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            bool oldDirtyValue = documentWorkspace.Document.Dirty;
            int index = TargetLayerIndex;
            if (index == -1) index = documentWorkspace.ActiveLayerIndex;
            
            using (Form lpd =((Layer) documentWorkspace.Document.Layers[index]).CreateConfigDialog())
            {
                DialogResult result = Utility.ShowDialog(lpd, documentWorkspace.FindForm());

                if (result == DialogResult.Cancel)
                {
                    documentWorkspace.Document.Dirty = oldDirtyValue;
                }
            }

            return false ;
        }

        public OpenActiveLayerPropertiesAction()
            : base(ActionFlags.KeepToolActive)
        {
            // This action does not require that the current tool be deactivated.
        }
    }
}
