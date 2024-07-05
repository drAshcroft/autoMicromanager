using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;


namespace axCOMContainer
{
    /*[Guid("5A88092E-69DF-4bb8-AD8D-8FA8F3552F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPanelHolder
    {
        void AddControl(System.Windows.Forms.Control control);
        Control GetControl();
        Control GetTestControl();
    }


    [Guid("1514adf6-7cb1-4561-9fbb-b75c0467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]*/
    [ProgId ("axCOMContainer")]
	public partial class PanelEX : System.Windows.Forms.Panel //,IPanelHolder 
	{
        static PanelEX()
        {

        }
       /* private EasyCore Ecore;
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
		/*public PanelEx()
		{
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.UserPaint, true);
		}*/
	}
}
