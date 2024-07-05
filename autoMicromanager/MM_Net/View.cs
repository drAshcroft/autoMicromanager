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

namespace Micromanager_net
{

    public partial class View : DockContent,IPictureView // PaintDotNet.FloatingToolForm   
    {
        CoreDevices.EasyCore eCore;

        public PaintDotNet.AppWorkspace AppWorkSpace()
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
        }
        public View()
        {
            InitializeComponent();
        }
        public void SetCore(CoreDevices.EasyCore  Ecore)
        {
            
            eCore = Ecore;
            eCore.ImageProduced += new Micromanager_net.CoreDevices.ImageProducedEvent(eCore_ImageProduced);
            UpdateImage +=new UpdateImageEvent(eCore_ImageProduced);
        }
        private event UpdateImageEvent UpdateImage;
        delegate void UpdateImageEvent(CoreDevices.CoreImage  cImage);
        void eCore_ImageProduced(Micromanager_net.CoreDevices.CoreImage cImage)
        {
            if (PS.InvokeRequired)
            {
                object[] pars = { cImage };
                //try
                {
                    PS.Invoke(UpdateImage , pars);
                }
                //catch { }

            }
            else
            {
                PS.SetLayerWithImage(0, cImage);
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

            PS.PauseMovieUpdates += new PaintDotNet.PauseMovieUpdatesEvent(PS_PauseMovieUpdates);
            PS.MoveUp += new PaintDotNet.MoveUpEvent(PS_MoveUp);
            PS.MoveDown += new PaintDotNet.MoveDownEvent(PS_MoveDown);
            PS.MoveRight += new PaintDotNet.MoveRightEvent(PS_MoveRight);
            PS.MoveLeft += new PaintDotNet.MoveLeftEvent(PS_MoveLeft);
            PS.Left = 0;//toolsFormControl1.Width;
           
            this.DockPanel.Height = PS.Height;
            this.DockPanel.Size = new Size(PS.Width, PS.Height);
            //this.DockPanel.DockLeftPortion =.01;
            //this.DockPanel.DockRightPortion = PS.Width;
            //this.Bounds = new Rectangle(this.Left, this.Top, PS.Width, PS.Height);
            //this.Width = PS.Width + historyFormControl1.Width + 10;
            //this.Height = PS.Height + 10;

        }
        private void View_Resize(object sender, EventArgs e)
        {
            this.Width = PS.Width +  10;
            this.Height = PS.Height + 10;
        }

        void PS_PauseMovieUpdates(object sender)
        {
            eCore.MMCamera.StopFocusMovie();
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
