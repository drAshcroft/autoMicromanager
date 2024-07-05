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
using SciImage.Actions;

namespace SciImage.Actions
{
    public sealed class FlattenAction
        : Actions.PluginAction 
    {
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
            get { return "Image"; }
        }
        public override string SubMenuName
        {
            get { return ""; }
        }
        public override System.Windows.Forms.Keys ShortCutKeys
        {
            get
            {
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Shift );
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
        public static string StaticName
        {
            get
            {
                return "Flatten";
            }
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
            object savedSelection = null;
            List<HistoryMemento> actions = new List<HistoryMemento>();

            if (!historyWorkspace.Selection.IsEmpty)
            {
                savedSelection = historyWorkspace.Selection.Save();
                DeselectAction da = new DeselectAction();
                List<HistoryMemento> lhm = new List<HistoryMemento>();
                da.PerformAction(appWorkspace,lhm,TargetLayerIndex  );
                actions.AddRange(lhm);
            }

            ReplaceDocumentHistoryMemento rdha = new ReplaceDocumentHistoryMemento(null, null, historyWorkspace);
            actions.Add(rdha);
            CompoundHistoryMemento chm = null;
            if (OptionalHistoryRecord == null)
            {
                chm = new CompoundHistoryMemento(
                    StaticName,
                    PdnResources.GetImageResource("Icons.MenuImageFlattenIcon.png"),
                    actions);
            }
            else
                OptionalHistoryRecord.AddRange(actions);
            // TODO: we can save memory here by serializing, then flattening on to an existing blayer
            Document flat = historyWorkspace.Document.Flatten();

            //EnterCriticalRegion();
            historyWorkspace.Document = flat;

            if (savedSelection != null)
            {
                SelectionHistoryMemento shm = new SelectionHistoryMemento(null, null, historyWorkspace);
                historyWorkspace.Selection.Restore(savedSelection);
                if (OptionalHistoryRecord == null)
                    chm.PushNewAction(shm);
                else
                    OptionalHistoryRecord.Add(shm);
            }
            if (OptionalHistoryRecord == null)
                historyWorkspace.History.PushNewMemento(chm);
            
            return true ;
        }

        public FlattenAction()
            : base(ActionFlags.None)
        {
        }
    }
}
