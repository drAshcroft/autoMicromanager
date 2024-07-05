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
using SciImage.HistoryMementos;
using System.Collections.Generic;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage.Actions
{
    public class FlipLayerVerticalAction
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
                return (System.Windows.Forms.Keys.F9);
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
        public static string StaticName
        {
            get
            {
                return "Flip Layer Vertical";
            }
        }

        private void Flip(Surface surface)
        {
            for (int x = 0; x < surface.Width; ++x)
            {
                for (int y = 0; y < surface.Height / 2; ++y)
                {
                    ColorPixelBase temp = surface.GetPoint(x, y);
                    surface[x, y] = surface[x, surface.Height - y - 1];
                    surface.SetPoint(x, surface.Height - y - 1, temp);
                }
            }
        }
        public override PluginAction.ActionDisplayOptions CheckIfEnabled(DocumentWorkspace documentWorkspace)
        {
            if (documentWorkspace == null)
            {
                return ActionDisplayOptions.Visible;
            }
            if (documentWorkspace.ActiveLayer.Surface!=null)
            {
                return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
            }
            return ActionDisplayOptions.Hidden;
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            int index = TargetLayerIndex;
            if (index == -1) index = appWorkspace.ActiveDocumentWorkspace.ActiveLayerIndex;

            Layer layer = (Layer)(appWorkspace.ActiveDocumentWorkspace.Document.Layers[index]);

            HistoryMemento bha = new BitmapHistoryMemento
                ("Flip Layer Vertical",
                PdnResources.GetImageResource("Icons.MenuLayersFlipVerticalIcon.png"),
                appWorkspace.ActiveDocumentWorkspace,
                index,
                appWorkspace.ActiveDocumentWorkspace.Selection.CreateRegion(),
                layer.Surface);
            if (OptionalHistoryRecord == null)
                appWorkspace.ActiveDocumentWorkspace.History.PushNewMemento(bha);
            else
                OptionalHistoryRecord.Add(bha);
            Flip(layer.Surface);
            layer.Invalidate();
            //appWorkspace.ActiveDocumentWorkspace.Update();
            return true;
        }

        public FlipLayerVerticalAction()
/*            : base(StaticName,
                   PdnResources.GetImageResource("Icons.MenuLayersFlipVerticalIcon.png"), 
                   FlipType.Vertical,
                   layerIndex)*/
        {
        }
    }
}
