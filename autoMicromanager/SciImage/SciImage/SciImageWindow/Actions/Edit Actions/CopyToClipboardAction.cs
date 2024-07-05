/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class CopyToClipboardAction:Actions.PluginAction 
    {
       
        public override string ActionName
        {
            get
            {
                return "Copy";
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 2; }
        }
        public override int OrderSuggestion
        {
            get { return 2; }
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null)
            {
                return ActionDisplayOptions.Visible;
            }
            if (documentWorkspace.ActiveLayer == null)
            {
                return ActionDisplayOptions.Visible ;
            }
            if (documentWorkspace.ActiveLayer.Surface  != null)
            {
                return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
            }
            return ActionDisplayOptions.Hidden;
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            DocumentWorkspace documentWorkspace = appWorkspace.ActiveDocumentWorkspace;
            int index = TargetLayerIndex;
            if (index == -1) index = appWorkspace.ActiveDocumentWorkspace.ActiveLayerIndex;

            
            bool success = true;

            if (documentWorkspace.Selection.IsEmpty ||
                (   ( (Layer) documentWorkspace.Document.Layers[index]).Surface ==null ))
            {
                return false ;
            }

            try
            {
                using (new WaitCursorChanger(documentWorkspace))
                {
                    Utility.GCFullCollect();
                    PdnRegion selectionRegion = documentWorkspace.Selection.CreateRegion();
                    PdnGraphicsPath selectionOutline = documentWorkspace.Selection.CreatePath();
                    success= documentWorkspace.ActiveLayer.CopyAction(selectionRegion, selectionOutline);
                    selectionRegion.Dispose();
                    selectionOutline.Dispose();
                    
                }
            }

            catch (OutOfMemoryException)
            {
                success = false;
                Utility.ErrorBox(documentWorkspace, PdnResources.GetString("CopyAction.Error.OutOfMemory"));
            }

            catch (Exception)
            {
                success = false;
                Utility.ErrorBox(documentWorkspace, PdnResources.GetString("CopyAction.Error.Generic"));
            }

            Utility.GCFullCollect();
            return success  ;
        }

        public CopyToClipboardAction()
        {
        }
    }
}
