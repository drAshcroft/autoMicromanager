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

namespace SciImage.Actions
{
    public sealed class MoveActiveLayerDownAction
        : SciImage.Actions.PluginAction
    {

        public static string StaticName
        {
            get
            {
                return PdnResources.GetString("MoveLayerDown.HistoryMementoName");
            }
        }

        public static ImageResource StaticImage
        {
            get
            {
                return PdnResources.GetImageResource("Icons.MenuLayersMoveLayerDownIcon.png");
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
            get { return StaticImage.Reference; }
        }
        public override string MenuName
        {
            get { return ""; }
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
            get { return 0; }
        }
        public override int OrderSuggestion
        {
            get { return 0; }
        }

        private void SwapLayers(DocumentWorkspace docWorkspace, Layer layer1,Layer layer2,int layerIndex1,int layerIndex2)
        {
            
            int firstIndex = Math.Min(layerIndex1, layerIndex2);
            int secondIndex = Math.Max(layerIndex1, layerIndex2);

            if (secondIndex - firstIndex == 1)
            {
                docWorkspace.Document.Layers.RemoveAt(layerIndex1);
                docWorkspace.Document.Layers.Insert(layerIndex2, layer1);
            }
            else
            {
                // general version
                docWorkspace.Document.Layers[layerIndex1] = layer2;
                docWorkspace.Document.Layers[layerIndex2] = layer1;
            }

            ((Layer)docWorkspace.Document.Layers[layerIndex1]).Invalidate();
            ((Layer)docWorkspace.Document.Layers[layerIndex2]).Invalidate();



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

            int index = TargetLayerIndex;
            if (index == -1) index = documentWorkspace.ActiveLayerIndex;
            

            if (index != 0)
            {
                BitmapLayer L1 = ((BitmapLayer)documentWorkspace.ActiveLayer);
                BitmapLayer L2 = ((BitmapLayer)documentWorkspace.Document.Layers[index-1]);

                 HistoryMemento bh1 = new BitmapHistoryMemento
                ("",
                StaticImage ,
                documentWorkspace,
                index,
                documentWorkspace.Selection.CreateRegion(),
                L1.Surface );


                 HistoryMemento bh2 = new BitmapHistoryMemento
                ("",
                StaticImage ,
                documentWorkspace,
                index-1,
                documentWorkspace.Selection.CreateRegion(),
                L2.Surface);

                SwapLayers(documentWorkspace, L1 ,
                    L2, index, index - 1);
                if (OptionalHistoryRecord == null)
                {
                    CompoundHistoryMemento hm =
                        new CompoundHistoryMemento("Move Layer Down", StaticImage, new HistoryMemento[] { bh1, bh2 });
                    documentWorkspace.History.PushNewMemento(hm);
                }
                else
                {
                    OptionalHistoryRecord.Add(bh1);
                    OptionalHistoryRecord.Add(bh2);
                }
            }

            return true ;
        }

        public MoveActiveLayerDownAction()
            : base(ActionFlags.None)
        {
        }
    }
}
