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
using System.Drawing;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class DeselectAction
        : Actions.PluginAction 
    {
        public static string StaticName
        {
            get
            {
                return "Deselect"; 
            }
        }

        public static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuEditDeselectIcon.png");
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 3; }
        }
        public override int OrderSuggestion
        {
            get { return 5; }
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
            DocumentWorkspace docWorkspace = appWorkspace.ActiveDocumentWorkspace;
            if (docWorkspace.Selection.IsEmpty)
            {
                return false ;
            }
            else
            {
                SelectionHistoryMemento sha = new SelectionHistoryMemento(StaticName, StaticImage, docWorkspace);
                if (OptionalHistoryRecord == null)
                {
                    docWorkspace.History.PushNewMemento(sha);
                }
                else
                    OptionalHistoryRecord.Add(sha);
                //EnterCriticalRegion();
                docWorkspace.Selection.Reset();

                return true ;
            }
        }

        public DeselectAction()
            : base(ActionFlags.None)
        {
        }
    }
}
