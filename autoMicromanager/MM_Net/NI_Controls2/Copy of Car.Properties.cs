using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using CoreDevices;
using CoreDevices.NI_Controls;

namespace Micromanager_net
{
    partial class COMEasyCore
	{
        private CoreDevices.EasyCore ECore;
        public CoreDevices.EasyCore CreateEasyCore()
        {
            ECore = new CoreDevices.EasyCore();
            return ECore;
        }
        /*
        private EasyCore Ecore;
        public EasyCore StartEcore(string CoreFilePath, string ConfigFile)
        {
            Ecore = new EasyCore();
            Ecore.StartCore(CoreFilePath, ConfigFile);
            return Ecore;
        }
        public NiHelpers GetHelpers()
        {
            return new NiHelpers(Ecore);
        }*/
        public void Redraw()
        {
            this.Invalidate();
           
            this.Refresh();
        }
	}
}