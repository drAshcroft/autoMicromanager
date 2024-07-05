using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Security.Permissions;
using System.IO;
using System.Drawing;
using CoreDevices;
using CoreDevices.NI_Controls;
using System.Collections.Generic;


namespace Micromanager_net
{
    // Nested Types
    [ComVisible(true), Guid(MMToolKitComService.EventsId), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface __MMToolKitComService
    {
        [DispId(1)]
        void Click();
        [DispId(2)]
        void DblClick();
       
        
    }

    [Guid(MMToolKitComService.InterfaceId), ComVisible(true)]
    public interface _MMToolKitComService
    {
        [DispId(1)]
        bool Visible { [DispId(1)] get; [DispId(1)] set; }
        [DispId(2)]
        bool Enabled { [DispId(2)] get; [DispId(2)] set; }
        [DispId(3)]
        int ForegroundColor { [DispId(3)] get; [DispId(3)] set; }
        [DispId(4)]
        int BackgroundColor { [DispId(4)] get; [DispId(4)] set; }
        [DispId(5)]
        Image BackgroundImage { [DispId(5)] get; [DispId(5)] set; }
        [DispId(6)]
        void Refresh();
        [DispId(7)]
        EasyCore StartSuperForm( string ConfigFile);
        [DispId(8)]
        EasyCore  StartEcore( string ConfigFile);
        [DispId(9)]
        NiHelpers GetHelpers();
        [DispId(10)]
        UserControl[] AllGuis { [DispId(10)]get; [DispId(10)] set; }
        [DispId(11)]
        void NewHardwareConfig(string ConfigFilePath, string ConfigFileName);
        [DispId(12)]
        MMDeviceHolder CreateImageViewer();
    }

    [Guid(MMToolKitComService.ClassId), ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces("Micromanager_net.__MMToolKitComService")]
    [ComClass(MMToolKitComService.ClassId, MMToolKitComService.InterfaceId, MMToolKitComService.EventsId)]
    public partial class MMToolKitComService : CoreDevices.NI_Controls.NIEasyCore , _MMToolKitComService 
    {
        #region VB6 Interop Code

#if COM_INTEROP_ENABLED

        #region "COM Registration"

        //These  GUIDs provide the COM identity for this class 
        //and its COM interfaces. If you change them, existing 
        //clients will no longer be able to access the class.

        public const string ClassId = "63024889-97e6-4ed3-8642-d5bb985ce309";
        public const string InterfaceId = "b1f18a77-417a-431a-9aa7-b34fa02eaf79";
        public const string EventsId = "d984e327-d1df-4276-a37b-c60d4b950a49";

        //These routines perform the additional COM registration needed by ActiveX controls
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction]
        private static void Register(System.Type t)
        {
            ComRegistration.RegisterControl(t, "102");
            //MessageBox.Show("Registered");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction]
        private static void Unregister(System.Type t)
        {
            ComRegistration.UnregisterControl(t);
        }


        #endregion

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
                ActiveXControlHelpers.HandleFocus(this);
            }

            public MMToolKitComService()
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
            public UserControl[] AllGuis
            {
                get { return Guis.ToArray(); }
                set { Guis.AddRange(value); }

            }
            public MMDeviceHolder CreateImageViewer()
            {
                MMDeviceHolder c=new MMDeviceHolder ();
                c.Show();
                return c;

            }
        //Please enter any new code here, below the Interop code
    }

}
