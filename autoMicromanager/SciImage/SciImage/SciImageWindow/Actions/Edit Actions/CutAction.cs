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
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class CutAction:Actions.PluginAction 
    {
        private static string StaticName
        {
            get
            {
                return "Cut";
            }
        }

        private static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuEditCutIcon.png");
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
            get { return StaticImage.Reference ; }
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X);
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
            HistoryMemento finalHM;

            if (documentWorkspace.Selection.IsEmpty)
            {
                finalHM = null;
            }
            else
            {
                CopyToClipboardAction ctca = new CopyToClipboardAction();
                ctca.PerformAction(appWorkspace,OptionalHistoryRecord,TargetLayerIndex   );

                
                    using (new PushNullToolMode(documentWorkspace))
                    {
                        EraseSelectionAction esa = new EraseSelectionAction();
                        esa.PerformAction(appWorkspace, OptionalHistoryRecord, TargetLayerIndex);
                    }
                
            }

           
            return true ;
        }

        public CutAction()
        {
            SystemLayer.Tracing.LogFeature("CutAction");
        }
    }
}
