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
    /// This class holds the SciImage viewer in a way that enables the labview application to hold and view the control.  It follows the interfaces needed for
    /// easycore to interact with it
    /// </summary>
    [Guid("1514adf6-7cb1-4561-0009-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class NiViewer :UserControl  ,IPictureView,ISubMDI  // SciImage.FloatingToolForm   
    {
      //  SciImage.NiToolBarHolder LayerForm = new SciImage.NiToolBarHolder();
      //  SciImage.NiToolBarHolder ColorForm = new SciImage.NiToolBarHolder();
      //  SciImage.NiToolBarHolder HistoryForm = new SciImage.NiToolBarHolder();
      //  SciImage.NiToolBarHolder ToolForm = new SciImage.NiToolBarHolder();

        Form ShamLayerForm = new Form();
        Form ShamColorForm = new Form();
        Form ShamHistoryForm = new Form();
        Form ShamToolForm = new Form();

        public void ShowDocking(DockPanel dp)
        {
          
        }
        public void CloseAndStop()
        {
            PS.Dispose();
            PS = null;
        }
        EasyCore eCore;
        private SciImage.AppWorkspace PS;
        public event EventHandler<NewDockableFormEvents> OnNewDockableForm;
        public event EventHandler<NewFormEvents> OnNewForm;
        public DockContent GetForm(string FormPersistenceString, string ExtraInformation)
        {

            if (FormPersistenceString.Contains("LayerForm"))
            {
                LayerFormControl lc;

                PS.InitializeLayerForm(out lc);
                layerFormControl = lc;
                return null;
            }
            if (FormPersistenceString.Contains("HistoryForm"))
            {
                HistoryFormControl hc;

                PS.InitializeHistoryForm(out hc);
                historyFormControl = hc;
                return null;
            }
            if (FormPersistenceString.Contains("ColorsForm"))
            {
                ColorPickers.ColorsFormControl cc;

                PS.InitializeColorsForm(out cc);
                colorsFormControl = cc;
                return null;
            }
            if (FormPersistenceString.Contains("ToolsForm"))
            {
                ToolsFormControl tc;
                PS.InitializeToolForm(out tc);
                toolsFormControl = tc;
                return null;
            }
            return null;
        }

        LayerFormControl layerFormControl;
        HistoryFormControl historyFormControl;
        ColorPickers.ColorsFormControl colorsFormControl;
        ToolsFormControl toolsFormControl;

        public UserControl LayerToolBar
        {
            get { return layerFormControl ; }
        }

        public UserControl HistoryToolBar
        {
            get { return historyFormControl ; }
        }
        public UserControl ColorsToolBar
        {
            get { return colorsFormControl ; }
        }
        public UserControl ToolsBar
        {
            get {
                try
                { return toolsFormControl; }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.InnerException);
                    return null;
                }
            }
        }

        public void CreateBasicSetup()
        {

            this.PS = new SciImage.AppWorkspace();
            PS.MakeNewForm += new MakeNewMDISubFormEvent(PS_MakeNewForm);
            
            // 
            // PS
            // 
            this.PS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PS.Location = new System.Drawing.Point(0, 0);
            this.PS.Name = "PS";
            this.PS.RulersEnabled = false;
            this.PS.Size = new System.Drawing.Size(1046, 651);
            this.PS.TabIndex = 1;
            //this.PS.Units = SciImage.MeasurementUnit.Pixel;

            // 
            // View
            // 
          
            this.Controls.Add(this.PS);
           
            



            LayerFormControl lc;
            HistoryFormControl hc;
            ColorPickers.ColorsFormControl cc;
            ToolsFormControl tc;

            PS.InitializeLayerForm(out lc);
            PS.InitializeHistoryForm(out  hc);
            PS.InitializeColorsForm(out cc);
            PS.InitializeToolForm(out tc);

            historyFormControl = hc;
            layerFormControl = lc;
            colorsFormControl = cc;
            toolsFormControl = tc;

            if (OnNewDockableForm != null)
            {
               /* OnNewDockableForm(this, new NewDockableFormEvents(LayerForm));
                OnNewDockableForm(this, new NewDockableFormEvents(HistoryForm));
                OnNewDockableForm(this, new NewDockableFormEvents(ColorForm));
                OnNewDockableForm(this, new NewDockableFormEvents(ToolForm));*/
            }
            if (OnNewForm != null)
            {
              
            }
        }

        public SciImage.AppWorkspace AppWorkSpace()
        {
            return PS;
        }
        public void ForceSave(string filename)
        {
            PS.AutoSave(filename);
        }
        public void ForceRefresh()
        {
            PS.ActiveDocumentWorkspace.Invalidate();
            PS.ActiveDocumentWorkspace.Refresh();
        }
        public void ShowToolsbars()
        {
            PS.ShowToolsbars();
           // PS.GetToolForm();
        }
        public NiViewer()
        {
            InitializeComponent();
            
        }

        void PS_MakeNewForm(object sender, Form NewForm)
        {
            if (OnNewForm != null)
            {
                OnNewForm(this, new NewFormEvents(NewForm));
            }
            else
                NewForm.Show();
        }
        public void SetCore(EasyCore Ecore)
        {
            eCore = Ecore;
        }

        private delegate void SendImageRawEvent(int Width, int Height, int Stride, int BPP, int ByteSize, IntPtr Data, long MaxPixelValue, long MinPixelValue);
        private delegate void SendImageBitmapEvent(Image NewImage);
        private delegate void SendImageLayerAndBitmap(Image[] NewImages);
        private delegate void SendImageBitmapContrastEvent(SciImage.ImageWithContrast NewImage);
        private delegate void SendImageLayerAndBitmapContrast(SciImage.ImageWithContrast[] NewImages);
        public void SendImage(CoreImage cImage)
        {
            SciImage.ImageWithContrast iwc = new ImageWithContrast(cImage.ImageRaw, cImage.MaxContrast, cImage.MinContrast);
            SendImage( iwc  );
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

        public void SendImage( Image[] newImages)
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
                PS.SetLayerWithImage( newImages);
            }

        }
        delegate void UpdatesPausedEvent();
        public  void UpdatesPaused()
        {
            if (PS.InvokeRequired)
            {
                
                try
                {
                    PS.Invoke(new UpdatesPausedEvent (UpdatesPaused ));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                PS.UpdatesPaused() ;
            }
        }
        public void SendImage(Image newImage)
        {
            if (PS.InvokeRequired)
            {
                object[] pars = {newImage  };
                try
                {
                    PS.Invoke(new SendImageBitmapEvent (SendImage ), pars);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                PS.SetLayerWithImage(0,newImage );
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
                    PS.Invoke(new SendImageRawEvent( SendImage), pars);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }

            }
            else
            {
                Bitmap b;
                if (BPP==16 || BPP==8)
                    b= CoreImage.CreateGrayScaleBitmap((IntPtr) Data,(long)ByteSize,(int)BPP,(int) Width,(int)Height,(long)MinPixelValue,(long)MaxPixelValue,AllowedPixelFormats.Format16BppGrayscale , Color.White );
                else
                    b = CoreImage.CreateGrayScaleBitmap((IntPtr)Data, (long)ByteSize, (int)BPP, (int)Width, (int)Height, (long)MinPixelValue, (long)MaxPixelValue, AllowedPixelFormats.Format32BppARGB , Color.White);
                PS.SetLayerWithImage(0,b );
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
        }

        private void View_Load(object sender, EventArgs e)
        {

            PS.PauseMovieUpdates += new SciImage.PauseMovieUpdatesEvent(PS_PauseMovieUpdates);
            PS.MoveUp += new SciImage.MoveUpEvent(PS_MoveUp);
            PS.MoveDown += new SciImage.MoveDownEvent(PS_MoveDown);
            PS.MoveRight += new SciImage.MoveRightEvent(PS_MoveRight);
            PS.MoveLeft += new SciImage.MoveLeftEvent(PS_MoveLeft);
            PS.Left = 0;
        }
        private void View_Resize(object sender, EventArgs e)
        {
            this.Width = PS.Width +  10;
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
            eCore.MMXYStage.MoveStageRelative(-1*eCore.ScreenSize *.75, 0);
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
    }
}

