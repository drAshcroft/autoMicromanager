using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoreDevices.NI_Controls;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Security.Permissions;

namespace Micromanager_net
{
    [Guid("5A88092E-69DF-2227-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface _MMCOMViewer
    {
        bool Visible {  get;  set; }
        bool Enabled {  get;  set; }
        int ForegroundColor {  get;  set; }
        int BackgroundColor {  get;  set; }
        Image BackgroundImage {  get;  set; }
        void Refresh();
        void Show();
        void ShowImage(object  pic);
        PictureBoard GetPictureBoard();
    }

    [Guid("1514adf6-7cb1-2227-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class MMCOMViewer : Form, _MMCOMViewer 
    {

           #region VB6 Interop Code

#if COM_INTEROP_ENABLED

      

        #region "VB6 Events"

        //This section shows some examples of exposing a UserControl's events to VB6.  Typically, you just
        //1) Declare the event as you want it to be shown in VB6
        //2) Raise the event in the appropriate UserControl event.
        public delegate void ClickEventHandler();
        public delegate void DblClickEventHandler();
        public new event ClickEventHandler Click; //Event must be marked as new since .NET UserControls have the same name.
        public event DblClickEventHandler DblClick;

        private void InteropUserControl_Click(object sender, System.EventArgs e)
        {
            if (null != Click)
                Click();
        }

        // Handles Me.DoubleClick
        private void InteropUserControl_DblClick(object sender, System.EventArgs e)
        {
            if (null != DblClick)
                DblClick();
        }

        private void InteropUserControl_Load(object sender, System.EventArgs e)
        {
            base.Click += new System.EventHandler(InteropUserControl_Click);
            this.DoubleClick +=new System.EventHandler(InteropUserControl_DblClick);
        }

        #endregion

        #region "VB6 Properties"

        //The following are examples of how to expose typical form properties to VB6.  
        //You can also use these as examples on how to add additional properties.

        //Must Shadow this property as it exists in Windows.Forms and is not overridable
        public new bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        public new bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        public int ForegroundColor
        {
            get 
            {
                return ActiveXControlHelpers.GetOleColorFromColor(base.ForeColor);
            }
            set
            {
                base.ForeColor = ActiveXControlHelpers.GetColorFromOleColor(value);
            }
        }

        public int BackgroundColor
        {
            get
            {
                return ActiveXControlHelpers.GetOleColorFromColor(base.BackColor);
                }
            set
            {
                base.BackColor = ActiveXControlHelpers.GetColorFromOleColor(value);
            }
        }

        public override System.Drawing.Image BackgroundImage
        {
            get{return null;}
            set
            {
                if(null != value)
                {
                    MessageBox.Show("Setting the background image of an Interop UserControl is not supported, please use a PictureBox instead.", "Information");
                }
                base.BackgroundImage = null;
            }
        }

        #endregion

        #region "VB6 Methods"

            public override void Refresh()
            {
                base.Refresh();
            }

            //Ensures that tabbing across VB6 and .NET controls works as expected
            private void InteropUserControl_LostFocus(object sender, System.EventArgs e)
            {
                //ActiveXControlHelpers.HandleFocus(this);
            }

            public MMCOMViewer()
            {
                //This call is required by the Windows Form Designer.
                InitializeComponent();
                this.LostFocus += new System.EventHandler(InteropUserControl_LostFocus); 
                this.ControlAdded += new ControlEventHandler(InteropUserControl_ControlAdded);
                //' Add any initialization after the InitializeComponent() call.

                //'Raise Load event
                    this.OnCreateControl();
            }

            [SecurityPermission(SecurityAction.LinkDemand, Flags =SecurityPermissionFlag.UnmanagedCode)]
            protected override void WndProc(ref System.Windows.Forms.Message m)
            {

                const int WM_SETFOCUS = 0x7;
                const int WM_PARENTNOTIFY = 0x210;
                const int WM_DESTROY = 0x2;
                const int WM_LBUTTONDOWN = 0x201;
                const int WM_RBUTTONDOWN = 0x204;

                if (m.Msg == WM_SETFOCUS)
                {
                    //Raise Enter event
                    this.OnEnter(System.EventArgs.Empty);
                }
                else if( m.Msg == WM_PARENTNOTIFY && (m.WParam.ToInt32() == WM_LBUTTONDOWN || m.WParam.ToInt32() == WM_RBUTTONDOWN))
                {

                    if (!this.ContainsFocus)
                    {
                        //Raise Enter event
                        this.OnEnter(System.EventArgs.Empty);
                    }
                }
                else if (m.Msg == WM_DESTROY && !this.IsDisposed && !this.Disposing)
                {
                    //Used to ensure that VB6 will cleanup control properly
                    this.Dispose();
                }

                base.WndProc(ref m);
            }



            //This event will hook up the necessary handlers
            private void InteropUserControl_ControlAdded(object sender, ControlEventArgs e)
            {
                ActiveXControlHelpers.WireUpHandlers(e.Control, ValidationHandler);
            }

            //Ensures that the Validating and Validated events fire appropriately
            internal void ValidationHandler(object sender, System.EventArgs e)
            {
                if( this.ContainsFocus) return;

                //Raise Leave event
                this.OnLeave(e);

                if (this.CausesValidation)
                {
                    CancelEventArgs validationArgs = new CancelEventArgs();
                    this.OnValidating(validationArgs);

                    if(validationArgs.Cancel && this.ActiveControl != null)
                        this.ActiveControl.Focus();
                    else
                    {
                        //Raise Validated event
                        this.OnValidated(e);
                    }
                }

            }

        #endregion

       

#endif

        #endregion
      
        public void ShowImage(object  pic)
        {
            pictureBoard1.SendImage((CoreDevices.CoreImage)pic);
            DotNetDoEvents();
        }
        public PictureBoard GetPictureBoard()
        {
            return pictureBoard1 ;
        }
        public void DotNetDoEvents()
        {
            Application.DoEvents();
        }
    }
}
