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

namespace SciImage.Actions
{
    public class FlipImageHorizontalAction
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
                return (System.Windows.Forms.Keys.F9);
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
            if (documentWorkspace.ActiveLayer.Surface !=null)
            {
                return PluginAction.ActionDisplayOptions.Visible | ActionDisplayOptions.Enabled;
            }
            return ActionDisplayOptions.Hidden;
        }
        public override bool PerformAction(AppWorkspace appWorkspace, List<HistoryMemento> OptionalHistoryRecord, int TargetLayerIndex)
        {
            List<HistoryMemento > mementos=new List<HistoryMemento>();
            FlipLayerHorizontalAction flh=new FlipLayerHorizontalAction();
            for (int i=0;i<appWorkspace.ActiveDocumentWorkspace.Document.Layers.Count ;i++)
            {
                flh.PerformAction(appWorkspace,mementos,i);

            }
            
            if (OptionalHistoryRecord == null)
            {
               HistoryMemento hm=new CompoundHistoryMemento(this.ActionName,null,mementos );
               appWorkspace.ActiveDocumentWorkspace.History.PushNewMemento(hm);
            }
            else
               OptionalHistoryRecord.AddRange(mementos );
          
            return true;
        }


   
        public static string StaticName
        {
            get
            {
                return "Flip Image Horizontal";
            }
        }

        public FlipImageHorizontalAction()
            /*: base(StaticName,
                   PdnResources.GetImageResource("Icons.MenuLayersFlipHorizontalIcon.png"), 
                   FlipType.Horizontal,
                   layerIndex)*/
        {
        }
    }
}
