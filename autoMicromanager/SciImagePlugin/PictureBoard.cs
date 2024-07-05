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
    /// <summary>
    /// A simple usercontrol for displaying a coreimage.
    /// </summary>
    [Guid("1514adf6-7cb1-4561-0007-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class PictureBoard : UserControl,IPictureView 
    {
        public PictureBoard()
        {
            InitializeComponent();
        }
        public void UpdatesPaused()
        {

        }
        public void SendImage(CoreImage cImage)
        {
            SendImage(cImage.ImageRGB);
        }
        public void SendImage(CoreImage[] cImages)
        {
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
