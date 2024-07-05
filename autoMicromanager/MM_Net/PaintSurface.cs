/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using PaintDotNet.Actions;
using PaintDotNet.Effects;
using PaintDotNet.Menus;
using PaintDotNet.SystemLayer;
using PaintDotNet.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace PaintDotNet
{
    
    public partial class PaintSurface : UserControl, ISnapManagerHost//,ISnapObstacleHost
    {

        public event MoveLeftEvent MoveLeft;
        public event MoveRightEvent MoveRight;
        public event MoveDownEvent MoveDown;
        public event MoveUpEvent MoveUp;


        private AppWorkspace appWorkspace;
        private Button defaultButton;

        
        private FloatingToolForm[] floaters;
        private System.Windows.Forms.Timer floaterOpacityTimer;
        private System.Windows.Forms.Timer deferredInitializationTimer;
       // private System.ComponentModel.IContainer components;
        private bool killAfterInit = false;
        private SplashForm splashForm = null;
        private List<string> queuedInstanceMessages = new List<string>();

        public event PauseMovieUpdatesEvent PauseMovieUpdates;
        private bool BlockMovieUpdates=false ;
        public  void SetLayerWithImage(int  LayerIndex, Image image)
        {
            if (BlockMovieUpdates ==false )
            if (appWorkspace.ActiveDocumentWorkspace != null)
            {
                if (appWorkspace.ActiveDocumentWorkspace.ActiveLayer.IsBackground != true)
                {
                    appWorkspace.ActiveDocumentWorkspace.MovieRunningOnBackGroundLayer = true;
                    PaintDotNet.Document doc = appWorkspace.ActiveDocumentWorkspace.Document;
                    //todo: need to make sure that the image is only sent to an apporiate document that is the right size and width.



                    Layer newLayer = Layer.FromImage(image);
                    if (LayerIndex == 0) newLayer.IsBackground = true;


                    doc.Layers.SetAtQuietly(0, newLayer).Invalidate();

                    // we invalidate each layer so that the layer previews refresh themselves
                    foreach (Layer layer in doc.Layers)
                    {
                        layer.Invalidate();
                    }

                    bool oldDirty = doc.Dirty;
                    doc.Invalidate();
                    doc.Dirty = oldDirty;
                    appWorkspace.ActiveDocumentWorkspace.Update(true);
                }
            }
            //appWorkspace.DocumentWorkspaces[0].Invalidate();
            // appWorkspace.DocumentWorkspaces[0].Update();
            // appWorkspace.DocumentWorkspaces[0].Refresh();
        }
        public void SetBackgroundImage(Image image)
        {
            BlockMovieUpdates = false;

            //appWorkspace.OpenPictureInNewWorkspace(image, "frame.tiff");
            appWorkspace.CreateImageDocumentInNewWorkspace(image, true);
            appWorkspace.ActiveDocumentWorkspace.RequestMovieStop +=new RequestMovieStopEvent(ActiveDocumentWorkspace_RequestMovieStop);
 //           public delegate void RequestMovieStopEvent(object sender);
        }

        void  ActiveDocumentWorkspace_RequestMovieStop(object  sender)
        {
            BlockMovieUpdates = true;
            if (PauseMovieUpdates != null) PauseMovieUpdates(this);
 	        
        }


        private SnapManager snapManager=new SnapManager();

        public SnapManager SnapManager
        {
            get {return (snapManager) ;}
        }
        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);
        }

        private enum ArgumentAction
        {
            Open,
            OpenUntitled,
            Print,
            NoOp
        }

        private bool SplitMessage(string message, out ArgumentAction action, out string actionParm)
        {
            if (message.Length == 0)
            {
                action = ArgumentAction.NoOp;
                actionParm = null;
                return false;
            }

            const string printPrefix = "print:";

            if (message.IndexOf(printPrefix) == 0)
            {
                action = ArgumentAction.Print;
                actionParm = message.Substring(printPrefix.Length);
                return true;
            }

            const string untitledPrefix = "untitled:";

            if (message.IndexOf(untitledPrefix) == 0)
            {
                action = ArgumentAction.OpenUntitled;
                actionParm = message.Substring(untitledPrefix.Length);
                return true;
            }

            action = ArgumentAction.Open;
            actionParm = message;
            return true;
        }

        private bool ProcessMessage(string message)
        {
            if (IsDisposed)
            {
                return false;
            }

            ArgumentAction action;
            string actionParm;
            bool result;
            
            result = SplitMessage(message, out action, out actionParm);

            if (!result)
            {
                return true;
            }

            switch (action)
            {
                case ArgumentAction.NoOp:
                    result = true;
                    break;

                case ArgumentAction.Open:
                    //Activate();

                    if ( Enabled)
                    {
                        result = this.appWorkspace.OpenFileInNewWorkspace(actionParm);
                    }

                    break;

                case ArgumentAction.OpenUntitled:
                   // Activate();

                    if (!string.IsNullOrEmpty(actionParm) &&  Enabled)
                    {
                        result = this.appWorkspace.OpenFileInNewWorkspace(actionParm, false);

                        if (result)
                        {
                            this.appWorkspace.ActiveDocumentWorkspace.SetDocumentSaveOptions(null, null, null);
                            this.appWorkspace.ActiveDocumentWorkspace.Document.Dirty = true;
                        }
                    }

                    break;

                case ArgumentAction.Print:
                    //Activate();

                    if (!string.IsNullOrEmpty(actionParm)  && Enabled)
                    {
                        result = this.appWorkspace.OpenFileInNewWorkspace(actionParm);

                        if (result)
                        {
                            DocumentWorkspace dw = this.appWorkspace.ActiveDocumentWorkspace;
                            PrintAction pa = new PrintAction();
                            dw.PerformAction(pa);
                            CloseWorkspaceAction cwa = new CloseWorkspaceAction(dw);
                            this.appWorkspace.PerformAction(cwa);

                            if (this.appWorkspace.DocumentWorkspaces.Length == 0)
                            {
                                //Startup.CloseApplication();
                            }
                        }
                    }
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            return result;
        }

        private void ProcessQueuedInstanceMessages()
        {
            if (IsDisposed)
            {
                return;
            }

            if (this.splashForm != null)
            {
                this.splashForm.Close();
                this.splashForm.Dispose();
                this.splashForm = null;
            }

            if (IsHandleCreated &&
                !PdnInfo.IsExpired )
            {
                //string[] messages1 = this.singleInstanceManager.GetPendingInstanceMessages();
                string[] messages2 = this.queuedInstanceMessages.ToArray();
                this.queuedInstanceMessages.Clear();

                string[] messages = messages2;// new string[ messages2.Length];
                

                foreach (string message in messages)
                {
                    bool result = ProcessMessage(message);

                    if (!result)
                    {
                        break;
                    }
                }
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (!this.IsDisposed && 
                (this.queuedInstanceMessages.Count > 0 ))
            {
                ProcessQueuedInstanceMessages();
            }
        }
        RichTextBox errorPanel ;
        public PaintSurface()
        {
            try
            {
                string[] args = new string[1];
                args[0] = "";
                StartPaintSurface(args);
            }
            catch (Exception e)
            {
                errorPanel = new RichTextBox();
                errorPanel.Dock = DockStyle.Fill;
                this.Controls.Add(errorPanel);
                errorPanel.Size = this.Size;
                errorPanel.Text = e.Message  ;


            }

        }
        public PaintSurface(  string[] args)
        {
            StartPaintSurface(args);
        }

        private void StartPaintSurface(string[] args)
        {

            Startup Start = new Startup();

            bool canSetCurrentDir = true;

            bool splash = false;
            List<string> fileNames = new List<string>();

            // Parse command line arguments
            foreach (string argument in args)
            {
                if (0 == string.Compare(argument, "/dontForceGC"))
                {
                    Utility.AllowGCFullCollect = false;
                }
                else if (0 == string.Compare(argument, "/splash", true))
                {
                    splash = true;
                }
                else if (0 == string.Compare(argument, "/test", true))
                {
                    // This lets us use an alternate update manifest on the web server so that
                    // we can test manifests on a small scale before "deploying" them to everybody
                    PdnInfo.IsTestMode = true;
                }
                else if (0 == string.Compare(argument, "/profileStartupTimed", true))
                {
                    // profileStartupTimed and profileStartupWorkingSet compete, which
                    // ever is last in the args list wins.
                    PdnInfo.StartupTest = StartupTestType.Timed;
                }
                else if (0 == string.Compare(argument, "/profileStartupWorkingSet", true))
                {
                    // profileStartupTimed and profileStartupWorkingSet compete, which
                    // ever is last in the args list wins.
                    PdnInfo.StartupTest = StartupTestType.WorkingSet;
                }
                else if (argument.Length > 0 && argument[0] != '/')
                {
                    try
                    {
                        string fullPath = Path.GetFullPath(argument);
                        fileNames.Add(fullPath);
                    }

                    catch (Exception)
                    {
                        fileNames.Add(argument);
                        canSetCurrentDir = false;
                    }

                    splash = true;
                }
            }

            if (canSetCurrentDir)
            {
                try
                {
                    Environment.CurrentDirectory = PdnInfo.GetApplicationDir();
                }

                catch (Exception ex)
                {
                    Tracing.Ping("Exception while trying to set Environment.CurrentDirectory: " + ex.ToString());
                }
            }

            // make splash, if warranted
            if (splash)
            {
                this.splashForm = new SplashForm();
                this.splashForm.TopMost = true;
                this.splashForm.Show();
                this.splashForm.Update();
            }

            InitializeComponent();


            // Does not load window location/state
            LoadSettings();

            foreach (string fileName in fileNames)
            {
                this.queuedInstanceMessages.Add(fileName);
            }

            // no file specified? create a blank image
            if (fileNames.Count == 0)
            {
                MeasurementUnit units = Document.DefaultDpuUnit;
                double dpu = Document.GetDefaultDpu(units);
                Size newSize = this.appWorkspace.GetNewDocumentSize();
                this.appWorkspace.CreateBlankDocumentInNewWorkspace(newSize, units, dpu, true);
                this.appWorkspace.ActiveDocumentWorkspace.IncrementJustPaintWhite();
                this.appWorkspace.ActiveDocumentWorkspace.Document.Dirty = false;
            }

            //LoadWindowState();

            deferredInitializationTimer.Enabled = true;

            Application.Idle += new EventHandler(Application_Idle);

        }
        protected void OnShown()
        {
           // base.OnShown(e);

            if (PdnInfo.IsExpired)
            {
                foreach (Form form in Application.OpenForms)
                {
                    form.Enabled = false;
                }

                TaskButton checkForUpdatesTB = new TaskButton(
                    PdnResources.GetImageResource("Icons.MenuHelpCheckForUpdatesIcon.png").Reference,
                    PdnResources.GetString("ExpiredTaskDialog.CheckForUpdatesTB.ActionText"),
                    PdnResources.GetString("ExpiredTaskDialog.CheckForUpdatesTB.ExplanationText"));

                TaskButton goToWebSiteTB = new TaskButton(
                    PdnResources.GetImageResource("Icons.MenuHelpPdnWebsiteIcon.png").Reference,
                    PdnResources.GetString("ExpiredTaskDialog.GoToWebSiteTB.ActionText"),
                    PdnResources.GetString("ExpiredTaskDialog.GoToWebSiteTB.ExplanationText"));

                TaskButton doNotCheckForUpdatesTB = new TaskButton(
                    PdnResources.GetImageResource("Icons.CancelIcon.png").Reference,
                    PdnResources.GetString("ExpiredTaskDialog.DoNotCheckForUpdatesTB.ActionText"),
                    PdnResources.GetString("ExpiredTaskDialog.DoNotCheckForUpdatesTB.ExplanationText"));

                TaskButton[] taskButtons =
                    new TaskButton[]
                    {
                        checkForUpdatesTB,
                        goToWebSiteTB,
                        doNotCheckForUpdatesTB
                    };

                TaskButton clickedTB = TaskDialog.Show(
                    this,null,
                    PdnInfo.GetFullAppName(),
                    PdnResources.GetImageResource("Icons.WarningIcon.png").Reference,
                    true,
                    PdnResources.GetString("ExpiredTaskDialog.InfoText"),
                    taskButtons,
                    checkForUpdatesTB,
                    doNotCheckForUpdatesTB,
                    450);

                if (clickedTB == checkForUpdatesTB)
                {
                    this.appWorkspace.CheckForUpdates();
                }
                else if (clickedTB == goToWebSiteTB)
                {
                    PdnInfo.LaunchWebSite(this, InvariantStrings.ExpiredPage);
                }


                //Close();
            }
        }

        private void LoadSettings()
        {
            try
            {
                PdnBaseForm.EnableOpacity = Settings.CurrentUser.GetBoolean(SettingNames.TranslucentWindows, true);
            }

            catch (Exception ex)
            {
                Tracing.Ping("Exception in MainForm.LoadSettings:" + ex.ToString());

                try
                {
                    Settings.CurrentUser.Delete(
                        new string[] 
                        { 
                            SettingNames.TranslucentWindows
                        });
                }

                catch
                {
                }
            }
        }

        private void SaveSettings()
        {
            Settings.CurrentUser.SetInt32(SettingNames.Width, this.Width);
            Settings.CurrentUser.SetInt32(SettingNames.Height, this.Height);
            Settings.CurrentUser.SetInt32(SettingNames.Top, this.Top);
            Settings.CurrentUser.SetInt32(SettingNames.Left, this.Left);
     
            Settings.CurrentUser.SetBoolean(SettingNames.TranslucentWindows, PdnBaseForm.EnableOpacity);

     
            SnapManager.Save(Settings.CurrentUser);
            this.appWorkspace.SaveSettings();

        }


        public void OnQueryEndSession(CancelEventArgs e)
        {
                foreach (Form form in Application.OpenForms)
                {
                    PdnBaseForm asPDF = form as PdnBaseForm;

                    if (asPDF != null)
                    {
                        asPDF.Flash();
                    }
                }

                e.Cancel = true;
         
        }

         
        public  void OnClosing(CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                if (this.appWorkspace != null)
                {
                    CloseAllWorkspacesAction cawa = new CloseAllWorkspacesAction();
                    this.appWorkspace.PerformAction(cawa);
                    e.Cancel = cawa.Cancelled;
                }
            }

            if (!e.Cancel)
            {
                SaveSettings();

                if (this.floaters != null)
                {
                    foreach (Form hideMe in this.floaters)
                    {
                        hideMe.Hide();
                    }
                }

                this.Hide();

                if (this.queuedInstanceMessages != null)
                {
                    this.queuedInstanceMessages.Clear();
                }

            }

        }

        public void OnClosed(EventArgs e)
        {
            if (this.appWorkspace.ActiveDocumentWorkspace != null)
            {
                appWorkspace.ActiveDocumentWorkspace.SetTool(null);
            }
        }


       
        private void AppWorkspace_ActiveDocumentWorkspaceChanging(object sender, EventArgs e)
        {
            if (this.appWorkspace.ActiveDocumentWorkspace != null)
            {
                //this.appWorkspace.ActiveDocumentWorkspace.ScaleFactorChanged -= DocumentWorkspace_ScaleFactorChanged;
                this.appWorkspace.ActiveDocumentWorkspace.DocumentChanged -= DocumentWorkspace_DocumentChanged;
                this.appWorkspace.ActiveDocumentWorkspace.SaveOptionsChanged -= DocumentWorkspace_SaveOptionsChanged;
            }
        }

        private void AppWorkspace_ActiveDocumentWorkspaceChanged(object sender, EventArgs e)
        {
            if (this.appWorkspace.ActiveDocumentWorkspace != null)
            {
                //this.appWorkspace.ActiveDocumentWorkspace.ScaleFactorChanged += DocumentWorkspace_ScaleFactorChanged;
                this.appWorkspace.ActiveDocumentWorkspace.DocumentChanged += DocumentWorkspace_DocumentChanged;
                this.appWorkspace.ActiveDocumentWorkspace.SaveOptionsChanged += DocumentWorkspace_SaveOptionsChanged;
            }

            //SetTitleText();
        }

        private void DocumentWorkspace_SaveOptionsChanged(object sender, EventArgs e)
        {
            //SetTitleText();
        }

        private Keys CharToKeys(char c)
        {
            Keys keys = Keys.None;
            c = Char.ToLower(c);

            if (c >= 'a' && c <= 'z')
            {
                keys = (Keys)((int)Keys.A + (int)c - (int)'a');
            }

            return keys;
        }

        private Keys GetMenuCmdKey(string text)
        {
            Keys keys = Keys.None;

            for (int i = 0; i < text.Length - 1; ++i)
            {
                if (text[i] == '&')
                {
                    keys = Keys.Alt | CharToKeys(text[i + 1]);
                    break;
                }
            }

            return keys;
        }

        protected override void OnLoad(EventArgs e)
        {
           // EnsureFormIsOnScreen();
            try
            {
                this.AllowDrop = true;
            }

            catch (InvalidOperationException)
            {
                // Discard error. See bug #2605.
            }
            try
            {
                if (killAfterInit)
                {
                    Application.Exit();
                }

                this.floaters = new FloatingToolForm[] { 
                                                       appWorkspace.Widgets.ToolsForm,
                                                       appWorkspace.Widgets.ColorsForm,
                                                       appWorkspace.Widgets.HistoryForm,
                                                       appWorkspace.Widgets.LayerForm
                                                   };

                foreach (FloatingToolForm ftf in floaters)
                {
                    ftf.Closing += this.HideInsteadOfCloseHandler;
                }


                PositionFloatingForms();

                base.OnLoad(e);



                switch (PdnInfo.StartupTest)
                {
                    case StartupTestType.Timed:
                        Application.DoEvents();
                        Application.Exit();
                        break;

                    case StartupTestType.WorkingSet:
                        const int waitPeriodForVadumpSnapshot = 20000;
                        Application.DoEvents();
                        Thread.Sleep(waitPeriodForVadumpSnapshot);
                        Application.Exit();
                        break;
                }
            }
            catch (Exception ee)
            {
                if (errorPanel == null)
                {
                    errorPanel = new RichTextBox();
                    errorPanel.Dock = DockStyle.Fill;
                    this.Controls.Add(errorPanel);
                    errorPanel.Size = this.Size;
                }
                errorPanel.Text += ee.Message;

            }
        }

        public void PositionFloatingForms()
        {
            this.appWorkspace.ResetFloatingForms();

            try
            {
                SnapManager.Load(Settings.CurrentUser);
            }

            catch
            {
                this.appWorkspace.ResetFloatingForms();
            } 
            
            foreach (FloatingToolForm ftf in floaters)
            {
                this.ParentForm.AddOwnedForm(ftf);
              //todo: not sure what this does  this.AddOwnedForm(ftf);
            }

            if (Settings.CurrentUser.GetBoolean(SettingNames.ToolsFormVisible, true))
            {
                this.appWorkspace.Widgets.ToolsForm.Show();
            }

            if (Settings.CurrentUser.GetBoolean(SettingNames.ColorsFormVisible, true))
            {
                this.appWorkspace.Widgets.ColorsForm.Show();
            }

            if (Settings.CurrentUser.GetBoolean(SettingNames.HistoryFormVisible, true))
            {
                this.appWorkspace.Widgets.HistoryForm.Show();
            }

            if (Settings.CurrentUser.GetBoolean(SettingNames.LayersFormVisible, true))
            {
                this.appWorkspace.Widgets.LayerForm.Show();
            }

            // If the floating form is off screen somehow, reset it
            // We've been getting a lot of reports where people say their Colors window has disappeared
            Screen[] allScreens = Screen.AllScreens;

            foreach (FloatingToolForm ftf in this.floaters)
            {
                if (!ftf.Visible)
                {
                    continue;
                }

                bool reset = false;

                try
                {
                    bool foundAScreen = false;

                    foreach (Screen screen in allScreens)
                    {
                        Rectangle intersect = Rectangle.Intersect(screen.Bounds, ftf.Bounds);

                        if (intersect.Width > 0 && intersect.Height > 0)
                        {
                            foundAScreen = true;
                            break;
                        }
                    }

                    if (!foundAScreen)
                    {
                        reset = true;
                    }
                }

                catch (Exception)
                {
                    reset = true;
                }

                if (reset)
                {
                    this.appWorkspace.ResetFloatingForm(ftf);
                }
            }

            this.floaterOpacityTimer.Enabled = true;
        }

        protected override void OnResize(EventArgs e)
        {
            if (floaterOpacityTimer != null)
            {
                if (this.ParentForm!=null && this.ParentForm.WindowState == FormWindowState.Minimized)
                {
                    if (this.floaterOpacityTimer.Enabled)
                    {
                        this.floaterOpacityTimer.Enabled = false;
                    }
                }
                else
                {
                    if (!this.floaterOpacityTimer.Enabled)
                    {
                        this.floaterOpacityTimer.Enabled = true;
                    }

                    this.FloaterOpacityTimer_Tick(this, EventArgs.Empty);
                }
            }

            base.OnResize (e);
        }

        private void DocumentWorkspace_DocumentChanged(object sender, System.EventArgs e)
        {
            //SetTitleText();
            OnResize(EventArgs.Empty);
        }


        // For the menus where we dynamically enable menu items (e.g. Copy only enabled when there's a selection),
        // we have to make sure to re-enable all the items when the menu goes way.
        // This is important for cases where, for example: Edit menu is opened, "Deselect" is disabled because
        // there is no selection. User then clicks on Select All. The menu then goes away. However, since Deselect
        // was disabled, the Ctrl+D shortcut will not be honored even though there is a selection.
        // So the disabling of menu items should only be temporary for the duration of the menu's visibility.
        private void OnMenuDropDownClosed(object sender, System.EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            foreach (ToolStripItem tsi in menu.DropDownItems)
            {
                tsi.Enabled = true;
            }
        }

        private void HideInsteadOfCloseHandler(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ((Form)sender).Hide();
        }

        // TODO: refactor into FloatingToolForm class somehow
        private void FloaterOpacityTimer_Tick(object sender, System.EventArgs e)
        {
            if (this.ParentForm == null) return;

            if ( this.ParentForm.WindowState == FormWindowState.Minimized ||
                this.floaters == null ||
                !PdnBaseForm.EnableOpacity ||
                this.appWorkspace.ActiveDocumentWorkspace == null)
            {
                return;
            }

            // Here's the behavior we want for our floaters:
            // 1. If the mouse is within a floaters rectangle, it should transition to fully opaque
            // 2. If the mouse is outside the floater's rectangle, it should transition to partially
            //    opaque
            // 3. However, if the floater is outside where the document is visible on screen, it
            //    should always be fully opaque.
            Rectangle screenDocRect;
                
            try
            {
                screenDocRect = this.appWorkspace.ActiveDocumentWorkspace.VisibleDocumentBounds;
            }

            catch (ObjectDisposedException)
            {
                return; // do nothing, we are probably in the process of shutting down the app
            }

            for (int i = 0; i < floaters.Length; ++i)
            {
                FloatingToolForm ftf = floaters[i];

                Rectangle intersect = Rectangle.Intersect(screenDocRect, ftf.Bounds);
                double opacity = -1.0;

                try
                {
                    if (intersect.Width == 0 ||
                        intersect.Height == 0 ||
                        (ftf.Bounds.Contains(Control.MousePosition) &&
                            !appWorkspace.ActiveDocumentWorkspace.IsMouseCaptured()) ||
                        Utility.DoesControlHaveMouseCaptured(ftf))
                    {
                        opacity = Math.Min(1.0, ftf.Opacity + 0.125);
                    }
                    else
                    {
                        opacity = Math.Max(0.75, ftf.Opacity - 0.0625);
                    }

                    if (opacity != ftf.Opacity)
                    {
                        ftf.Opacity = opacity;
                    }
                }

                catch (System.ComponentModel.Win32Exception)
                {
                    // We just eat the exception. Chris Strahl was having some problem where opacity was 0.7
                    // and we were trying to set it to 0.7 and it said "the parameter is incorrect"
                    // ... which is stupid. Bad NVIDIA drivers for his GeForce Go?
                }
            }
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (Enabled && drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])drgevent.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files) 
                {
                    try
                    {
                        FileAttributes fa = File.GetAttributes(file);

                        if ((fa & FileAttributes.Directory) == 0)
                        {
                            drgevent.Effect = DragDropEffects.Copy;
                        }
                    }

                    catch
                    {
                    }
                }
            }

            base.OnDragEnter(drgevent);
        }

        private string[] PruneDirectories(string[] fileNames)
        {
            List<string> result = new List<string>();

            foreach (string fileName in fileNames)
            {
                try
                {
                    FileAttributes fa = File.GetAttributes(fileName);

                    if ((fa & FileAttributes.Directory) == 0)
                    {
                        result.Add(fileName);
                    }
                }

                catch
                {
                }
            }

            return result.ToArray();
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
           // Activate();

            if ( !Enabled)
            {
                // do nothing
            }
            else if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] allFiles = (string[])drgevent.Data.GetData(DataFormats.FileDrop);

                if (allFiles == null)
                {
                    return;
                }

                string[] files = PruneDirectories(allFiles);

                bool importAsLayers = true;

                if (files.Length == 0)
                {
                    return;
                }
                else
                {
                    Icon formIcon = Utility.ImageToIcon(PdnResources.GetImageResource("Icons.DragDrop.OpenOrImport.FormIcon.png").Reference);
                    string title = PdnResources.GetString("DragDrop.OpenOrImport.Title");
                    string infoText = PdnResources.GetString("DragDrop.OpenOrImport.InfoText");

                    TaskButton openTB = new TaskButton(
                        PdnResources.GetImageResource("Icons.MenuFileOpenIcon.png").Reference,
                        PdnResources.GetString("DragDrop.OpenOrImport.OpenButton.ActionText"),
                        PdnResources.GetString("DragDrop.OpenOrImport.OpenButton.ExplanationText"));

                    string importLayersExplanation;
                    if (this.appWorkspace.DocumentWorkspaces.Length == 0)
                    {
                        importLayersExplanation = PdnResources.GetString("DragDrop.OpenOrImport.ImportLayers.ExplanationText.NoImagesYet");
                    }
                    else
                    {
                        importLayersExplanation = PdnResources.GetString("DragDrop.OpenOrImport.ImportLayers.ExplanationText");
                    }

                    TaskButton importLayersTB = new TaskButton(
                        PdnResources.GetImageResource("Icons.MenuLayersImportFromFileIcon.png").Reference,
                        PdnResources.GetString("DragDrop.OpenOrImport.ImportLayers.ActionText"),
                        importLayersExplanation);

                    TaskButton clickedTB = TaskDialog.Show(
                        this,
                        formIcon,
                        title,
                        null,
                        false,
                        infoText,
                        new TaskButton[] { openTB, importLayersTB, TaskButton.Cancel },
                        null,
                        TaskButton.Cancel);

                    if (clickedTB == openTB)
                    {
                        importAsLayers = false;
                    }
                    else if (clickedTB == importLayersTB)
                    {
                        importAsLayers = true;
                    }
                    else
                    {
                        return;
                    }
                }

                if (!importAsLayers)
                {
                    // open files into new tabs
                    this.appWorkspace.OpenFilesInNewWorkspace(files);
                }
                else
                {
                    // no image open? we will have to create one
                    if (this.appWorkspace.ActiveDocumentWorkspace == null)
                    {
                        Size newSize = this.appWorkspace.GetNewDocumentSize();

                        this.appWorkspace.CreateBlankDocumentInNewWorkspace(
                            newSize,
                            Document.DefaultDpuUnit,
                            Document.GetDefaultDpu(Document.DefaultDpuUnit),
                            false);
                    }

                    ImportFromFileAction action = new ImportFromFileAction();
                    HistoryMemento ha = action.ImportMultipleFiles(this.appWorkspace.ActiveDocumentWorkspace, files);

                    if (ha != null)
                    {
                        this.appWorkspace.ActiveDocumentWorkspace.History.PushNewMemento(ha);
                    }
                }
            }

            base.OnDragDrop(drgevent);
        }


        private void DeferredInitialization(object sender, EventArgs e)
        {
            this.deferredInitializationTimer.Enabled = false;
            this.deferredInitializationTimer.Tick -= new EventHandler(DeferredInitialization);
            this.deferredInitializationTimer.Dispose();
            this.deferredInitializationTimer = null;

            // TODO
            this.appWorkspace.ToolBar.MainMenu.PopulateEffects();
        }

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            // F1 is already handled by the Menu->Help menu item. No need to process it twice.
            hevent.Handled = true;

            base.OnHelpRequested(hevent);
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {            
            // Since defaultButton is the AcceptButton, hitting Enter will get 'eaten' by this button
            // So we have to give the Enter key to the Tool
            if (this.appWorkspace.ActiveDocumentWorkspace != null)
            {
                this.appWorkspace.ActiveDocumentWorkspace.Focus();

                if (this.appWorkspace.ActiveDocumentWorkspace.Tool != null)
                {
                    this.appWorkspace.ActiveDocumentWorkspace.Tool.PerformKeyPress(new KeyPressEventArgs('\r'));
                    this.appWorkspace.ActiveDocumentWorkspace.Tool.PerformKeyPress(Keys.Enter);
                }
            }
        }
/*
#if DEBUG
        static MainForm()
        {
            new Thread(FocusPrintThread).Start();
        }

        private static string GetControlName(Control control)
        {
            if (control == null)
            {
                return "null";
            }

            string name = control.Name + "(" + control.GetType().Name + ")";

            if (control.Parent != null)
            {
                name += " <- " + GetControlName(control.Parent);
            }

            return name;
        }

        private static void PrintFocus()
        {
            Control c = Utility.FindFocus();
            Tracing.Ping("Focused: " + GetControlName(c));
        }

        private static void FocusPrintThread()
        {
            Thread.CurrentThread.IsBackground = true;

            while (true)
            {
                try
                {
                    FormCollection forms = Application.OpenForms;
                    Form form;
                    if (forms.Count > 0)
                    {
                        form = forms[0];
                        form.BeginInvoke(new Procedure(PrintFocus));
                    }
                }

                catch
                {
                }

                Thread.Sleep(1000);
            }
        }
#endif*/

    }    }

