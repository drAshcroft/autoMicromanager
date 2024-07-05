using SciImage.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SciImage.Actions
{
    public sealed class RotateDocument90CCWAction
        : Actions.PluginAction
    {
        public override  string ActionName
        {
            get
            {
                return "Rotate CounterClockwise 90 Degrees ";
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
            get {
                return (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G);
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
            Actions.RotateDocumentHelper rdh = new SciImage.Actions.RotateDocumentHelper();
            HistoryMemento hm = rdh.OnExecute(appWorkspace.ActiveDocumentWorkspace, Actions.RotateDocumentHelper.RotateType.CounterClockwise90, TargetLayerIndex);
            if (OptionalHistoryRecord == null)
                appWorkspace.ActiveDocumentWorkspace.History.PushNewMemento(hm);
            else
                OptionalHistoryRecord.Add(hm);
            return true;
        }

        public RotateDocument90CCWAction()
            : base(ActionFlags.None)
        {
        }

    }
}
