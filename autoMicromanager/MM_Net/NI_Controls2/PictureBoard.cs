using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CoreDevices.NI_Controls
{
    [Guid("1514adf6-7cb1-4561-0007-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public partial class PictureBoard : UserControl,IPictureView 
    {
        
        public PictureBoard()
        {
            InitializeComponent();
        }
        private CoreDevices.DeviceControls.LUTGraph lutGraph = null;
        public void UpdatesPaused()
        {

        }
        private void LutIt(ref CoreImage CI)
        {
            if (lutGraph != null)
            {
                lutGraph.ProcessImage(CI);
            }
        }
        private void LutIt(ref CoreImage[] CI)
        {
            if (lutGraph != null)
            {
                foreach (CoreImage ci in CI)
                    lutGraph.ProcessImage(ci);
            }
        }
        public void AttachLUTGraph(DeviceControls.LUTGraph LutGraph)
        {
            lutGraph = LutGraph;
        }
        public void SendImage(CoreImage cImage)
        {
            LutIt(ref cImage);
            SendImage(cImage.ImageRGB);
        }
        public void SendImage(CoreImage[] cImages)
        {
            LutIt(ref cImages);
            Bitmap[] Images = new Bitmap[cImages.Length];
            for (int i = 0; i < cImages.Length; i++)
                Images[i] = cImages[i].ImageRGB;
            SendImage(Images);
        }
        public void SendImage(int Width, int Height, int Stride, int BPP, int ByteSize, IntPtr Data, long MaxPixelValue, long MinPixelValue)
        {
            throw new Exception("Not yet Implemented");
        }
        public void SendImage(Image newImage)
        {
            pictureBox1.Image = newImage;
        }
        public void SendImage(Image[] NewImages)
        {
            throw new Exception("Not yet implemented");
        }
        public void ForceSave(string Filename)
        {
            FreeImageAPI.FreeImageBitmap image = new FreeImageAPI.FreeImageBitmap(pictureBox1.Image);
            image.Save(Filename);
        }
        public void SetCore(EasyCore Ecore)
        {

        }
    }
}
