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
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace SciImage.Actions
{
    public sealed class InvertSelectionAction
        : Actions.PluginAction 
    {
        public static string StaticName
        {
            get
            {
                return "Invert Selection";
            }
        }

        public static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuEditInvertSelectionIcon.png");
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
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I);
            }
        }
        public override int MenuSubGroupIndex
        {
            get { return 3; }
        }
        public override int OrderSuggestion
        {
            get { return 3; }
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
            if (historyWorkspace.Selection.IsEmpty)
            {
                return false ;
            }
            else
            {
                SelectionHistoryMemento sha = new SelectionHistoryMemento(
                    StaticName,
                    StaticImage,
                    historyWorkspace);

                //PdnGraphicsPath selectedPath = historyWorkspace.Selection.GetPathReadOnly();
                PdnGraphicsPath selectedPath = historyWorkspace.Selection.CreatePath();

                PdnGraphicsPath boundsOutline = new PdnGraphicsPath();
                boundsOutline.AddRectangle(historyWorkspace.Document.Bounds);

                PdnGraphicsPath clippedPath = PdnGraphicsPath.Combine(selectedPath, CombineMode.Intersect, boundsOutline);
                PdnGraphicsPath invertedPath = PdnGraphicsPath.Combine(clippedPath, CombineMode.Xor, boundsOutline);

                selectedPath.Dispose();
                selectedPath = null;

                clippedPath.Dispose();
                clippedPath = null;

                //EnterCriticalRegion();
                historyWorkspace.Selection.PerformChanging();
                historyWorkspace.Selection.Reset();
                historyWorkspace.Selection.SetContinuation(invertedPath, CombineMode.Replace, true);
                historyWorkspace.Selection.CommitContinuation();
                historyWorkspace.Selection.PerformChanged();

                boundsOutline.Dispose();
                boundsOutline = null;
                if (OptionalHistoryRecord == null)
                    historyWorkspace.History.PushNewMemento(sha);
                else
                    OptionalHistoryRecord.Add(sha);
                return true ;
            }
        }
 
        public InvertSelectionAction()
            : base(ActionFlags.None)
        {
        }
    }
}
