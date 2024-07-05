// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
//                License text is included with the source distribution.
//
//                This file is distributed in the hope that it will be useful,
//                but WITHOUT ANY WARRANTY; without even the implied warranty
//                of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//
//                IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//                CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//                INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
//using VS2005Extender;
using System.Runtime.InteropServices;
using CoreDevices;
namespace SciImage
{
    /// <summary>
    /// The whole MDI child form for showing images in SciImage
    /// </summary>
    [Guid("1514adf6-7cb1-4561-0008-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class View : DockContent, IPictureView, ISubMDI  // SciImage.FloatingToolForm   
    {
        //The standard toolbars of SciImage
        SciImage.DockContentForm LayerForm = new SciImage.DockContentForm();
        SciImage.DockContentForm ColorForm = new SciImage.DockContentForm();
        SciImage.DockContentForm HistoryForm = new SciImage.DockContentForm();
        SciImage.DockingToolsBar ToolForm = new SciImage.DockingToolsBar();

        EasyCore eCore;

        public event EventHandler<NewDockableFormEvents> OnNewDockableForm;
        public event EventHandler<NewFormEvents> OnNewForm;

        /// <summary>
        /// Allows the floating toolbars to get the minor toolbars from sciimage and restore the original setup
        /// </summary>
        /// <param name="FormPersistenceString"></param>
        /// <param name="ExtraInformation"></param>
        /// <returns></returns>
        public DockContent GetForm(string FormPersistenceString, string ExtraInformation)
        {

            if (FormPersistenceString.Contains("LayerForm"))
            {
                LayerFormControl lc;

                PS.InitializeLayerForm(out lc);
                LayerForm.ToolsControl = lc;
                return LayerForm ;
            }
            if (FormPersistenceString.Contains("HistoryForm"))
            {
                HistoryFormControl hc;
                
                PS.InitializeHistoryForm(out hc);
                HistoryForm.ToolsControl = hc;
                return HistoryForm ;
            }
            if (FormPersistenceString.Contains("ColorsForm"))
            {
                ColorPickers.ColorsFormControl cc;

                PS.InitializeColorsForm(out cc);
                ColorForm.ToolsControl = cc;
                return ColorForm ;
            }
            if (FormPersistenceString.Contains("ToolsForm"))
            {
                ToolsFormControl tc;
                PS.InitializeToolForm(out tc);
                ToolForm.ToolsControl = tc;
                return ToolForm ;
            }
            return null;
        }
        
        /// <summary>
        /// Creates a default layout of all the minor forms of sciimage
        /// </summary>
        public void CreateBasicSetup()
        {
            LayerFormControl lc;
            HistoryFormControl hc;
            ColorPickers.ColorsFormControl cc;
            ToolsFormControl tc;

            PS.InitializeLayerForm(out lc);
            PS.InitializeHistoryForm(out  hc);
            PS.InitializeColorsForm(out cc);
            PS.InitializeToolForm(out tc);

            HistoryForm.ToolsControl = hc;
            LayerForm.ToolsControl = lc;
            ColorForm.ToolsControl = cc;
            ToolForm.ToolsControl = tc;

            if (OnNewDockableForm != null)
            {
                OnNewDockableForm(this, new NewDockableFormEvents( LayerForm ));
                OnNewDockableForm(this, new NewDockableFormEvents( HistoryForm ));
                OnNewDockableForm(this, new NewDockableFormEvents( ColorForm ));
                OnNewDockableForm(this, new NewDockableFormEvents( ToolForm ));
            }
            if (OnNewForm != null)
            {
                //OnNewForm(this, new Micromanager_net.NewFormEvents(new ToolsFormX()));

            }
            //lc.Show(PS.dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);


        }

        public SciImage.AppWorkspace AppWorkSpace()
        {
            return PS;
        }
        public void ForceSave(string filename)
        {
            PS.AutoSave(filename);
        }
        public void ShowToolsbars()
        {
            PS.ShowToolsbars();
            // PS.GetToolForm();
        }
        public View()
        {
            InitializeComponent();
            PS.MakeFormFromUsercontrol += new MakeFormFromUserControl(PS_MakeFormFromUsercontrol);
        }

        Form PS_MakeFormFromUsercontrol(UserControl Control)
        {
           /* if (typeof(SciImage.ToolsFormControl).IsAssignableFrom(Control.GetType()) == true)
            {
                Size s = SizeFromClientSize(new Size(Control.Width, Control.Height));
                DockPanel.DefaultFloatWindowSize = s;
               
                return new SciImage.DockingToolsBar((SciImage.ToolsFormControl)Control);
            }
            else
            {*/
                Size s = SizeFromClientSize(new Size(Control.Width, Control.Height));
                DockPanel.DefaultFloatWindowSize = s;
                return new SciImage.DockContentForm(Control);
            //}
            
        }

        void PS_MakeNewForm(object sender, Form NewForm)
        {
            if (OnNewForm != null)
            {
                OnNewForm(this, new NewFormEvents(NewForm));

            }
        }
        public void SetCore(EasyCore Ecore)
        {

            eCore = Ecore;

            // eCore.ImageProduced += new Micromanager_net.CoreDevices.ImageProducedEvent(eCore_ImageProduced);
            // UpdateImage +=new UpdateImageEvent(eCore_ImageProduced);
        }

        private delegate void SendImageRawEvent(int Width, int Height, int Stride, int BPP, int ByteSize, IntPtr Data, long MaxPixelValue, long MinPixelValue);
        private delegate void SendImageBitmapEvent(Image NewImage);
        private delegate void SendImageLayerAndBitmap(Image[] NewImages);
        private delegate void SendImageBitmapContrastEvent(SciImage.ImageWithContrast NewImage);
        private delegate void SendImageLayerAndBitmapContrast(SciImage.ImageWithContrast[] NewImages);
        public void SendImage(CoreImage cImage)
        {
            SciImage.ImageWithContrast iwc = new ImageWithContrast(cImage.ImageRaw, cImage.MaxContrast, cImage.MinContrast);
            SendImage(iwc);
        }
        public void SendImage(CoreImage[] cImages)
        {
            Bitmap[] Images = new Bitmap[cImages.Length];
            for (int i = 0; i < cImages.Length; i++)
                Images[i] = cImages[i].ImageRaw;
            SendImage(Images);
        }
        private void SendImage(SciImage.ImageWithContrast[] newImages)
        {
            if (PS.InvokeRequired)
            {
                object[] pars = { newImages };
                try
                {
                    PS.Invoke(new SendImageLayerAndBitmapContrast(SendImage), pars);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                PS.SetLayerWithImage(newImages);
            }

        }

        public void SendImage(Image[] newImages)
        {
            if (PS.InvokeRequired)
            {
                object[] pars = { newImages };
                try
                {
                    PS.Invoke(new SendImageLayerAndBitmap(SendImage), pars);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                PS.SetLayerWithImage(newImages);
            }

        }
        delegate void UpdatesPausedEvent();
        public void UpdatesPaused()
        {
            if (PS.InvokeRequired)
            {

                try
                {
                    PS.Invoke(new UpdatesPausedEvent(UpdatesPaused));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                PS.UpdatesPaused();
            }
        }
        public void SendImage(Image newImage)
        {
            if (PS.InvokeRequired)
            {
                object[] pars = { newImage };
                try
                {
                    PS.Invoke(new SendImageBitmapEvent(SendImage), pars);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                PS.SetLayerWithImage(0, newImage);
            }

        }

        private void SendImage(SciImage.ImageWithContrast newImage)
        {
            if (PS.InvokeRequired)
            {
                object[] pars = { newImage };
                try
                {
                    PS.Invoke(new SendImageBitmapContrastEvent(SendImage), pars);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                PS.SetLayerWithImage(0, newImage);
            }

        }
        public void SendImage(int Width, int Height, int Stride, int BPP, int ByteSize, IntPtr Data, long MaxPixelValue, long MinPixelValue)
        {
            if (PS.InvokeRequired)
            {
                object[] pars = { Width, Height, Stride, BPP, ByteSize, Data, MaxPixelValue, MinPixelValue };
                try
                {
                    PS.Invoke(new SendImageRawEvent(SendImage), pars);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                Bitmap b;
                if (BPP == 16 || BPP == 8)
                    b = CoreImage.CreateGrayScaleBitmap((IntPtr)Data, (long)ByteSize, (int)BPP, (int)Width, (int)Height, (long)MinPixelValue, (long)MaxPixelValue, AllowedPixelFormats.Format16BppGrayscale, Color.White);
                else
                    b = CoreImage.CreateGrayScaleBitmap((IntPtr)Data, (long)ByteSize, (int)BPP, (int)Width, (int)Height, (long)MinPixelValue, (long)MaxPixelValue, AllowedPixelFormats.Format32BppARGB, Color.White);
                PS.SetLayerWithImage(0, b);
            }
        }
      


        private void View_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                eCore.MMCamera.StopFocusMovie();
            }
            catch
            { }
            //PS.OnClosed(new EventArgs()); 
        }

        private void View_Load(object sender, EventArgs e)
        {

            PS.PauseMovieUpdates += new SciImage.PauseMovieUpdatesEvent(PS_PauseMovieUpdates);
            PS.MoveUp += new SciImage.MoveUpEvent(PS_MoveUp);
            PS.MoveDown += new SciImage.MoveDownEvent(PS_MoveDown);
            PS.MoveRight += new SciImage.MoveRightEvent(PS_MoveRight);
            PS.MoveLeft += new SciImage.MoveLeftEvent(PS_MoveLeft);
            PS.Left = 0;//toolsFormControl1._Width;

            this.DockPanel.Height = PS.Height;
            this.DockPanel.Size = new Size(PS.Width, PS.Height);
            //this.DockPanel.DockLeftPortion =.01;
            //this.DockPanel.DockRightPortion = PS._Width;
            //this.Bounds = new Rectangle(this.Left, this.Top, PS._Width, PS._Height);
            //this._Width = PS._Width + historyFormControl1._Width + 10;
            //this._Height = PS._Height + 10;
            // PS.GetToolForm();
        }
        private void View_Resize(object sender, EventArgs e)
        {
            this.Width = PS.Width + 10;
            this.Height = PS.Height + 10;
        }

        void PS_PauseMovieUpdates(object sender)
        {
            try
            {
                eCore.MMCamera.StopFocusMovie();
            }
            catch { }
        }

        void PS_MoveLeft(object sender)
        {
            eCore.MMXYStage.MoveStageRelative(-1 * eCore.ScreenSize * .75, 0);
        }

        void PS_MoveRight(object sender)
        {
            eCore.MMXYStage.MoveStageRelative(eCore.ScreenSize * .75, 0);
        }

        void PS_MoveDown(object sender)
        {
            eCore.MMXYStage.MoveStageRelative(0, -1 * eCore.ScreenSize * .75);
        }

        void PS_MoveUp(object sender)
        {
            eCore.MMXYStage.MoveStageRelative(0, eCore.ScreenSize * .75);
        }



        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            PS.Dispose();
            //PS.OnClosing(new  CancelEventArgs());  
        }

        private void View_ResizeEnd(object sender, EventArgs e)
        {
            PS.Width = this.Width;
            PS.Height = this.Height;
        }

        

        private void View_Resize_1(object sender, EventArgs e)
        {
            PS.Width = this.Width;
            PS.Height = this.Height;
        }

        private void View_SizeChanged(object sender, EventArgs e)
        {
            PS.Width = this.Width;
            PS.Height = this.Height;

        }
    }
}
