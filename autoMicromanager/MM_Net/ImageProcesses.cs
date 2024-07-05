using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using System.Drawing;
using System.IO;
using System.Collections;

using FreeImageAPI;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Imaging;

namespace Micromanager_net
{
    class ImageProcesses
    {

        static public FreeImageBitmap   GetFreeImage(CMMCore core, cImage cI)
        {

            IntPtr p = cI.data;

            int length = cI.lSize;// core.getImageBufferSize();
            FreeImageBitmap b = null;
            int BPP = cI.bitsperpixel * 8;
            
            if (BPP == 1)
            {
                //b = (new FreeImageBitmap((int)core.getImageHeight  (), (int)core.getImageWidth(), 1, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, p));
                b = (new FreeImageBitmap(cI.width, cI.height, cI.width, PixelFormat.Format1bppIndexed, p));
            }
            else if (BPP == 8)
            {
                //b = (new FreeImageBitmap((int)core.getImageWidth(), (int)core.getImageHeight(),  (int)core.getImageWidth(), System.Drawing.Imaging.PixelFormat.Format8bppIndexed, p));
                b = (new FreeImageBitmap(cI.width, cI.height, cI.width, PixelFormat.Format8bppIndexed, p));
                int[] hist;
                b.GetHistogram(FREE_IMAGE_COLOR_CHANNEL.FICC_RGB, out hist);
                // Convert the bitmap to 8bpp and greyscale
               // b= b.GetColorConvertedInstance( FREE_IMAGE_COLOR_DEPTH.FICD_08_BPP | FREE_IMAGE_COLOR_DEPTH.FICD_FORCE_GREYSCALE);
            }
            else if (BPP == 16)
            {
                //b = (new FreeImageBitmap((int)core.getImageWidth(), (int)core.getImageHeight(), 2*(int)core.getImageWidth(), System.Drawing.Imaging.PixelFormat.Format16bppGrayScale, p));
                b = (new FreeImageBitmap(cI.width, cI.height, 2 * cI.width, PixelFormat.Format16bppGrayScale, p));
            }
            else if (BPP == 24)
            {
                //b = (new FreeImageBitmap((int)core.getImageWidth(), (int)core.getImageHeight(), 3 * (int)core.getImageWidth(), System.Drawing.Imaging.PixelFormat.Format24bppRgb, p));
                b = (new FreeImageBitmap(cI.width, cI.height,3* cI.width, PixelFormat.Format24bppRgb, p));
            }
            else if (BPP ==32)
            {
                b=(new FreeImageBitmap(cI.width, cI.height,4* cI.width, PixelFormat.Format32bppRgb , p));
            }
            else
            {

                MessageBox.Show("This program does not support your pixel format.  Please add more formats", "", MessageBoxButtons.OK);
            }
            //FreeImageBitmapData bmd = bb.LockBits(new Rectangle(0, 0, bb.Width, bb.Height), ImageLockMode.WriteOnly, bb.PixelFormat);

            Bitmap bb = new Bitmap(25, 25, PixelFormat.Format16bppGrayScale);
            //IntPtr ppp= FreeImageHelper.FreeImage_AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, 25, 25, 16, 0, 0, 0);
            b = new FreeImageBitmap(25, 25, PixelFormat.Format16bppGrayScale);
            b.SetPixel(10, 10, Color.White);
            b.Save("C:\\Users\\shawntel\\Desktop\\Brian\\micro-manager-1.1.47\\csharp_core\\bin\\Debug\\test.tif", FREE_IMAGE_FORMAT.FIF_TIFF);
            Bitmap b2 = new FreeImageBitmap(bb);
            // new FreeImageBitmap(25,25 );
            //  return (bb);
          
            return (b);//GetBitmapFromPtr(core, p));
        }


        static public Bitmap  GetImage(CMMCore core, cImage cI )
        {
           
            IntPtr p = cI.data;
            
            int length = cI.lSize;// core.getImageBufferSize();
            Bitmap b=null;
            int BPP = cI.bitsperpixel * 8;
            
            if (BPP   == 1)
            {
                //b = (new Bitmap((int)core.getImageHeight  (), (int)core.getImageWidth(), 1, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, p));
                b = (new Bitmap(cI.width  ,cI.height  , cI.width , System.Drawing.Imaging.PixelFormat.Format1bppIndexed, p));
            }
            else if (BPP ==8)
            {
                //b = (new Bitmap((int)core.getImageWidth(), (int)core.getImageHeight(),  (int)core.getImageWidth(), System.Drawing.Imaging.PixelFormat.Format8bppIndexed, p));
                b= (new Bitmap (cI.width,cI.height ,cI.width,System.Drawing.Imaging.PixelFormat.Format8bppIndexed,p));
            }
            else if (BPP ==16)
            {
                //b = (new Bitmap((int)core.getImageWidth(), (int)core.getImageHeight(), 2*(int)core.getImageWidth(), System.Drawing.Imaging.PixelFormat.Format16bppGrayScale, p));
                b= (new Bitmap (cI.width,cI.height ,2*cI.width,System.Drawing.Imaging.PixelFormat.Format16bppGrayScale ,p));
            }
            else if (BPP   == 24)
            {
                //b = (new Bitmap((int)core.getImageWidth(), (int)core.getImageHeight(), 3 * (int)core.getImageWidth(), System.Drawing.Imaging.PixelFormat.Format24bppRgb, p));
                b= (new Bitmap (cI.width,cI.height ,3*cI.width,System.Drawing.Imaging.PixelFormat.Format24bppRgb ,p));
            }
            else if (BPP == 32)
            {
                //b = (new Bitmap((int)core.getImageWidth(), (int)core.getImageHeight(), 3 * (int)core.getImageWidth(), System.Drawing.Imaging.PixelFormat.Format24bppRgb, p));
                b = (new Bitmap(cI.width, cI.height,4* cI.width, System.Drawing.Imaging.PixelFormat.Format32bppRgb , p));
            }
            else
            {

                MessageBox.Show("This program does not support your pixel format.  Please add more formats", "", MessageBoxButtons.OK);
            }
            //BitmapData bmd = bb.LockBits(new Rectangle(0, 0, bb.Width, bb.Height), ImageLockMode.WriteOnly, bb.PixelFormat);
           
            
            // new Bitmap(25,25 );
          //  return (bb);
            return (ConvertToGray( b));//GetBitmapFromPtr(core, p));
        }

       

        private static Bitmap  CreateBitmapFromBytes(byte[] pixelValues, int width, int height)
        {
            //Create an image that will hold the image data
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format16bppRgb555     );
            //PixelFormat.Format16bppGrayScale 
            //Get a reference to the images pixel data
            Rectangle dimension = new Rectangle(0, 0, pic.Width, pic.Height);
            BitmapData picData = pic.LockBits(dimension, ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr pixelStartAddress = picData.Scan0;

            //Copy the pixel data into the bitmap structure
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, pixelStartAddress, pixelValues.Length);

            pic.UnlockBits(picData);
            //pic.Save(@"c:\mypic1.bmp", ImageFormat.Bmp);
            return (( pic));
        }

        static public Bitmap ConvertToGray(Bitmap img)
        {


            ColorMatrix cm1 = new ColorMatrix(new float[][]{   new float[]{0.5f,0.5f,0.5f,0,0},
                                  new float[]{0.5f,0.5f,0.5f,0,0},
                                  new float[]{0.5f,0.5f,0.5f,0,0},
                                  new float[]{0,0,0,1,0,0},
                                  new float[]{0,0,0,0,1,0},
                                  new float[]{0,0,0,0,0,1}});
            //Gilles Khouzams colour corrected grayscale shear
            ColorMatrix cm2 = new ColorMatrix(new float[][]
                                  {   new float[]{0.3f,0.3f,0.3f,0,0},
                                      new float[]{0.59f,0.59f,0.59f,0,0},
                                      new float[]{0.11f,0.11f,0.11f,0,0},
                                      new float[]{0,0,0,1,0,0},
                                      new float[]{0,0,0,0,1,0},
                                      new float[]{0,0,0,0,0,1}});
            ColorMatrix invert = new ColorMatrix(new float[][] {
                      new float[] {-1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                      new float[] { 0.0f,-1.0f, 0.0f, 0.0f, 0.0f },
                      new float[] { 0.0f, 0.0f,-1.0f, 0.0f, 0.0f },
                      new float[] { 0.0f, 0.0f, 0.0f, 1.0f, 0.0f },
                      new float[] { 1.0f, 1.0f, 1.0f, 0.0f, 1.0f }
               });
            

            ColorMatrix cm = MultiplyColorMatrix(cm2,invert);
            


            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            Bitmap b = new Bitmap(img.Width,img.Height,PixelFormat.Format16bppRgb555 );
            Graphics g=Graphics.FromImage(b);
          
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            return (b);
        }

        static public ColorMatrix MultiplyColorMatrix(ColorMatrix cm1, ColorMatrix cm2)
        {

            		
		     ColorMatrix  loCombinedMatrix=new ColorMatrix() ;
            

            for (int  lnCounter=0; lnCounter <5;lnCounter++)
            {
                float lnMatrixVal1= cm1[0,lnCounter] ;
                float lnMatrixVal2= cm1[1,lnCounter] ;
                float lnMatrixVal3= cm1[2,lnCounter] ;
                float lnMatrixVal4= cm1[3,lnCounter] ;
                float lnMatrixVal5= cm1[4,lnCounter] ;
                for (int j = 0; j < 5; j++)
                {
                    loCombinedMatrix[j, lnCounter] =
                      cm2[j, 0] * lnMatrixVal1 +
                      cm2[j, 1] * lnMatrixVal2 +
                      cm2[j, 2] * lnMatrixVal3 +
                      cm2[j, 3] * lnMatrixVal4 +
                      cm2[j, 4] * lnMatrixVal5;
                }
			}

            return (loCombinedMatrix);
		}
		

        static public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        static public Image byteArrayToImage(byte[] myByteArray)
        {
            Image newImage;

            MemoryStream ms = new MemoryStream(myByteArray, 0, myByteArray.Length);
                ms.Write(myByteArray, 0, myByteArray.Length);

                newImage = Image.FromStream(ms, true);

                // work with image here. 

                // You'll need to keep the MemoryStream open for 
                // as long as you want to work with your new image. 

           
            return (newImage);

        }
    }
}
