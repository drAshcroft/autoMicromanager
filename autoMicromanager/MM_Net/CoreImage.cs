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
using System.Linq;
using System.Text;
using System.Drawing ;
using System.Drawing.Imaging;
using FreeImageAPI;
using System.Runtime.InteropServices;
using System.IO;
using CWrapper;

namespace CoreDevices
{
    public enum AllowedPixelFormats   {Format32BppARGB,  Format32BppRGB, Format16BppGrayscale}


    [Guid("1514adf6-7cb1-0002-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
   public  class CoreImage:ICoreImageCom 
    {
        //private FIBITMAP dib;

       cImage cimage=null ;
       public  IntPtr  Data;// = fib.Bits;
       private int _BitsPerPixel;// = fib.ColorDepth;
       private int _Height;// = fib._Height;
       private int _lSize;// = fib.DataSize;
       private int _Stride;// = fib.Pitch;
       private int _Width;// = fib._Width;
       private Color _FalseColor=Color.Empty ;
       private FreeImageBitmap  InternalFreeImage;
       public int BitsPerPixel
       {
           get { return _BitsPerPixel; }
       }
       public int Height
       {
           get { return _Height; }
       }
       public int Width
       {
           get { return _Width; }
       }
       /// <summary>
       /// Size in bytes of image
       /// </summary>
       public int lSize
       {
           get { return _lSize; }
       }
      /// <summary>
      /// Length of each line in bytes
      /// </summary>
       public int Stride
       {
           get { return _Stride; }
       }
       
        /// <summary>
        /// Desired false color for max intensity
        /// </summary>
        public Color FalseColor
       {
           get { return _FalseColor; }
           set { _FalseColor = value; }
       }

        /// <summary>
        /// Constructor for Matlab and COM.  It is not recommended that you use this 
        /// </summary>
        public CoreImage()
        {
        }
        public CoreImage(cImage cimage)
        {
            this.cimage = cimage;
            Data = cimage.Data;
            _BitsPerPixel = cimage.BitsPerPixel;
            _Height = cimage.Height;
            _lSize = cimage.lSize;
            _Stride = cimage.Stride;
            _Width = cimage.Width;
        }
        
        /// <summary>
        /// Open an image from file and set up core image
        /// </summary>
        /// <param name="FileName"></param>
        public CoreImage(string FileName)
        {
            Open(FileName);

        }
        public void Dispose()
        {
            Data = IntPtr.Zero;
            if (cimage != null)
                cimage.Dispose();
            
        }

        /// <summary>
        /// Returns a freeimage bitmap for manipulation (Mostly for internal use)
        /// </summary>
        /// <returns></returns>
        public FIBITMAP MakeFIBITMAP()
        {
            FIBITMAP dib;
            
            if (_BitsPerPixel == 16)
            {
                dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, _Width, _Height, 16, 0, 0, 0);

                for (int y = 0; y < _Height; y++)
                {
                    System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                    unsafe
                    {

                        ushort* bits = (ushort*)(void*)scanline;
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = inData[x];
                            bits[x] = gray;
                           // if (gray > MaxValue) MaxValue = gray;
                           // if (gray < MinValue) MinValue = gray;
                        }

                    }

                }
            }
            else
            {

                dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, _Width, _Height, 16, 0, 0, 0);
                
                for (int y = 0; y < _Height; y++)
                {
                    System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                    unsafe
                    {

                        ushort* bits = (ushort*)scanline;
                        byte* inData = (byte *)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)(256*inData[x]);
                            bits[x] = gray;
                           // if (gray > MaxValue) MaxValue = gray;
                           // if (gray < MinValue) MinValue = gray;
                        }

                    }

                }

            }
           
            InternalFreeImage = new FreeImageBitmap(dib, "");
            return dib;

        }
        /// <summary>
        /// Returns a freeimage bitmap for manipulation (Mostly for internal use)
        /// </summary>
        /// <returns></returns>
        public FIBITMAP  MakeRawFIBITMAP()
        {
            FIBITMAP dib;
            if (_BitsPerPixel == 16)
            {
                dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, _Width, _Height, 16, 0, 0, 0);

                for (int y = 0; y < _Height; y++)
                {
                    System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                    unsafe
                    {

                        ushort* bits = (ushort*)(void*)scanline;
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = inData[x];
                            bits[x] = gray;
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                        }

                    }

                }
            }
            else
            {

                dib = FreeImage.ConvertFromRawBits(Data, (int)_Width, (int)_Height, (int)_Stride, (uint)_BitsPerPixel, 0, 0, 0, false);
                if (_BitsPerPixel == 8)
                {
                    Palette p = FreeImage.GetPaletteEx(dib);
                    for (int i = 0; i < 256; i++)
                    {
                        p[i] = new RGBQUAD(Color.FromArgb(i, i, i));
                    }
                }
            }
            InternalFreeImage = new FreeImageBitmap(dib, "");
            return dib;

        }
        
        /// <summary>
        /// Returns FreeImage bitmap for use
        /// </summary>
        /// <returns></returns>
        public FreeImageBitmap MakeFreeImageBitmap()
        {
            FIBITMAP  fib= MakeRawFIBITMAP();
            return (new FreeImageBitmap(fib, ""));
        }

        private long  MaxValue=-1;
        private long  MinValue=-1;


        /*public long ConvertContrastToPixelValue(double Contrast)
        {
            long Value;
            if (_BitsPerPixel == 8)
                Value = (int)(256.0d * Contrast );
            else
                Value = (int)(65536.0d * Contrast );
            return Value;
        }*/

        public long MaxContrast
        {
            get { return MaxValue; }
            set { 
                MaxValue = value;
            }
        }
        public long MinContrast
        {
            get { return MinValue; }
            set { 
                MinValue = value;
                
            }
        }
        /// <summary>
        /// Gets value of the pixel at center of image
        /// </summary>
        /// <returns></returns>
        public long GetCenterPoint()
        {
            long Sum = 0;
            if ((_BitsPerPixel == 16))
            {

                unsafe
                {
                    int nHeight = (int)(_Height);
                    int nWidth = (int)(_Width);
                    int y=(int)(nHeight/2);
                    //for (int y = hStart; y < nHeight; y++)
                    {
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        // System.Diagnostics.Debug.Print(y.ToString());
                       // for (int x = wStart; x < nWidth; x++)
                        int x = (int)(nWidth / 2);
                        {

                            ushort gray = (ushort)inData[x];
                            Sum += gray;
                        }
                    }
                }

            }
            else
            {

                int pitch = 1;

                unsafe
                {

                   // for (int y = 0; y < _Height; y++)
                    int y = (int)(_Height / 2);
                    {
                        byte* inData = (byte*)Data + (y * pitch);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        //for (int x = 0; x < _Width; x++)
                        int x = (int)(_Width / 2);
                        {
                            byte gray = inData[x];
                            Sum += gray;

                        }

                    }
                }
            }

            return Sum;
        }
        
        /// <summary>
        /// Gets sum all all pixels in image
        /// </summary>
        /// <returns></returns>
        public long SumImage()
        {
            
            long Sum = 0;
            if ((_BitsPerPixel == 16))
            {

                unsafe
                {
                    int wStart = (int)(0);
                    int hStart = (int)(0);
                    int nHeight = (int)(_Height);
                    int nWidth = (int)(_Width);
                    for (int y = hStart; y < nHeight; y++)
                    {
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = wStart; x < nWidth; x++)
                        {

                            ushort gray = (ushort)inData[x];
                            Sum += gray;
                        }
                    }
                }

            }
            else
            {

                int pitch = 1;

                unsafe
                {

                    for (int y = 0; y < _Height; y++)
                    {
                        byte* inData = (byte*)Data + (y * pitch);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = inData[x];
                            Sum += gray;

                        }

                    }
                }
            }

            return Sum;
        }
        
        /// <summary>
        /// Gets the average intensity of whole image
        /// </summary>
        /// <returns></returns>
        public double AverageIntensity()
        {

            long Sum = SumImage();
            return (double)Sum/((double)(_Width*_Height));

        }
        
        /// <summary>
        /// Subtracts off a background image
        /// </summary>
        /// <param name="BackgroundImage"></param>
        public void SubtractBackground(CoreImage BackgroundImage)
        {
            MaxValue = 0;
            MinValue = 100000;

            if ((this._BitsPerPixel == 16))
            {
                Int16[,] MyArray = GetArrayInt16();
                Int16[,] OutArray = BackgroundImage.GetArrayInt16();
                for (int i=0;i<_Width ;i++)
                    for (int j = 0; j < _Height; j++)
                    {
                        MyArray[i,j] -= OutArray[i, j];
                        if (MyArray[i, j] < MinValue) MinValue = MyArray[i, j];
                        
                    }
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            long gray = (long)(MyArray[x,y] -MinValue );
                            if (gray < 0) gray = 0;
                            inData[x] = (ushort)gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {
                    byte[,] OutArray = BackgroundImage.GetArrayByte8();
                    byte[,] MyArray = GetArrayByte8();
                    for (int i = 0; i < _Width; i++)
                        for (int j = 0; j < _Height; j++)
                        {
                            MyArray[i, j] -= OutArray[i, j];
                            if (MyArray[i, j] < MinValue) MinValue = MyArray[i, j];

                        }
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            long gray = (long)(MyArray[x, y] - MinValue);
                            if (gray < 0) gray = 0;
                            inData[x] = (byte)gray;

                        }

                    }
                }
            }

        }
        /// <summary>
        /// Subtracts another image from this one
        /// </summary>
        /// <param name="SecondImage"></param>
        public void SubtractImage(CoreImage SecondImage)
        {
           
            MaxValue = 0;
            MinValue = 100000;
            
            if ((this._BitsPerPixel == 16))
            {
                Int16[,] OutArray = SecondImage.GetArrayInt16();
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            long  gray = (long )(inData[x]-OutArray [x,y]);
                            if (gray < 0) gray = 0;
                            inData[x] = (ushort)gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {
                    byte [,] OutArray = SecondImage.GetArrayByte8();
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            long gray =(long)( inData[x]-OutArray[x,y]);
                            if (gray < 0) gray = 0;
                            inData[x]=(byte) gray;

                        }

                    }
                }
            }
            
        }
        
        /// <summary>
        /// Divides whole image by given number
        /// </summary>
        /// <param name="Denominator"></param>
        public void DivideByScalar(double Denominator)
        {
            MaxValue = 0;
            MinValue = 100000;

            if ((this._BitsPerPixel == 16))
            {
                
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)((double)inData[x] /Denominator );
                            if (gray < 0) gray = 0;
                            inData[x] = gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {
                   
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = (byte)((double)inData[x] /Denominator );
                            if (gray < 0) gray = 0;
                            inData[x] = gray;

                        }

                    }
                }
            }


        }
        
        /// <summary>
        /// Adds one image to another overfloes are corrected to white
        /// </summary>
        /// <param name="SecondImage"></param>
        public void AddImage(CoreImage SecondImage)
        {

            MaxValue = 0;
            MinValue = 100000;

            if ((this._BitsPerPixel == 16))
            {
                Int16[,] OutArray = SecondImage.GetArrayInt16();
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)(inData[x] + OutArray[x, y]);
                            if (gray < 0) gray = 0;
                            inData[x] = gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {
                    byte[,] OutArray = SecondImage.GetArrayByte8();
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = (byte)(inData[x] + OutArray[x, y]);
                            if (gray < 0) gray = 0;
                            inData[x] = gray;

                        }

                    }
                }
            }

        }
        /// <summary>
        /// Converts image into a regular array
        /// </summary>
        /// <returns></returns>
        public double [,] GetArrayDouble()
        {
            MaxValue = 0;
            MinValue = 100000;
            double[,] OutArray = new double [_Width, _Height];
            if ((this._BitsPerPixel == 16))
            {
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (double )gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {

                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (double )gray;

                        }

                    }
                }
            }

            return OutArray;
        }
        /// <summary>
        /// Converts image into a regular linear array
        /// </summary>
        /// <returns></returns>
        public double[] GetLinearArrayDouble()
        {
            MaxValue = 0;
            MinValue = 100000;
            double[] OutArray = new double[_Width* _Height];
            if ((this._BitsPerPixel == 16))
            {
                long cc = 0;
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[cc] = (double)gray;
                            cc++;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {
                    long cc = 0;
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[cc] = (double)gray;
                            cc++;
                        }

                    }
                }
            }

            return OutArray;
        }
        /// <summary>
        /// Converts image into a regular array
        /// </summary>
        /// <returns></returns>
        public byte [,] GetArrayByte8()
        {
            MaxValue = 0;
            MinValue = 100000;
            byte [,] OutArray = new byte[_Width, _Height];
            if ((this._BitsPerPixel == 16))
            {
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                       // IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP);
                        ushort* inData =(ushort*)( (byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (byte)(gray/256);
                        }
                    }
                }
            }
            else
            {
                unsafe
                {

                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte* inData = (byte*)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride );

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (byte)gray;

                        }

                    }
                }
            }
            return OutArray;
        }
        /// <summary>
        /// Converts image into a regular array
        /// </summary>
        /// <returns></returns>
        public int[,] GetArrayInt32()
        {
            MaxValue = 0;
            MinValue = 100000;
            int [,] OutArray = new int[_Width, _Height];
            if ((this._BitsPerPixel == 16))
            {
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP);
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (int)gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {

                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte* inData = (byte*)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride );

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (int)gray;

                        }

                    }
                }
            }
            return OutArray;  
        }
        /// <summary>
        /// Converts image into a regular array
        /// </summary>
        /// <returns></returns>
        public Int16[,] GetArrayInt16( )
        {
                MaxValue = 0;
                MinValue = 100000;
                Int16 [,] OutArray = new Int16[_Width, _Height];
                if ((this._BitsPerPixel  == 16))
                {
                    unsafe
                    {
                        for (int y = 0; y < _Height; y++)
                        {
                            //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                            //ushort* inData = (ushort*)((byte*)lineP );
                            ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                            for (int x = 0; x < _Width; x++)
                            {
                                ushort gray = (ushort)inData[x];
                                if (gray > MaxValue) MaxValue = gray;
                                if (gray < MinValue) MinValue = gray;
                                OutArray [x, y] = (Int16)gray;
                            }
                        }
                    }
                }
                else
                {
                    unsafe
                    {

                        for (int y = 0; y < _Height; y++)
                        {
                            //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                            //byte * inData = (byte *)((byte*)lineP);
                            byte* inData = (byte*)Data + (y * _Stride );

                            // System.Diagnostics.Debug.Print(y.ToString());
                            for (int x = 0; x < _Width; x++)
                            {
                                byte gray = inData[x];
                                if (gray > MaxValue) MaxValue = gray;
                                if (gray < MinValue) MinValue = gray;
                                OutArray [x, y] = (Int16)gray;

                            }

                        }
                    }
                }
                return OutArray;
            
        }
        /// <summary>
        /// Converts image into a regular array
        /// </summary>
        /// <returns></returns>
        public int[,] GetArrayInt()
        {
            MaxValue = 0;
            MinValue = 100000;
            int[,] OutArray = new int[_Width, _Height];
            if ((this._BitsPerPixel == 16))
            {
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (Int16)gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {

                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (Int16)gray;

                        }

                    }
                }
            }
            return OutArray;

        }
        /// <summary>
        /// Converts image into a regular array
        /// </summary>
        /// <returns></returns>
        public long[,] GetArrayLong64()
        {
            MaxValue = 0;
            MinValue = 100000;
            long[,] OutArray = new long[_Width, _Height];
            if ((this._BitsPerPixel == 16))
            {
                unsafe
                {
                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP= FreeImage.GetScanLine(dib, y);
                        //ushort* inData = (ushort*)((byte*)lineP );
                        ushort* inData = (ushort*)((byte*)Data + (y * _Stride));
                        for (int x = 0; x < _Width; x++)
                        {
                            ushort gray = (ushort)inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (long)gray;
                        }
                    }
                }
            }
            else
            {
                unsafe
                {

                    for (int y = 0; y < _Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte * inData = (byte *)((byte*)lineP);
                        byte* inData = (byte*)Data + (y * _Stride);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < _Width; x++)
                        {
                            byte gray = inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            OutArray[x, y] = (long)gray;

                        }

                    }
                }
            }
            return OutArray;
        }
        /// <summary>
        /// Converts image into a bytearray
        /// </summary>
        /// <returns></returns>
        public byte[] GetRawDataArray()
        {
            MaxValue = 0;
            MinValue = 100000;
            byte[] OutArray = new byte[_lSize ];
            Marshal.Copy(Data, OutArray, 0, _lSize);
            return OutArray;
        }
        
        /// <summary>
        /// Some image outputs need a color LUT table
        /// </summary>
        /// <returns></returns>
        public Int32[] GetImageTable()
        {
            Int32[] OutputArray;
            if (_BitsPerPixel ==16)
                OutputArray=new Int32[2^16];
            else 
                OutputArray=new Int32[2^8];

            
            if ((MaxValue == -1 && MinValue == -1) || (MaxValue == 0 && MinValue == 0))
            {
                int[] hist;
                long MaxPossible = 255;
                GetHistogram(_BitsPerPixel, _Stride, _Width, _Height, Data, out hist, out  MaxValue, out  MinValue, out  MaxPossible);
            }
            
            double ValueLength = (MaxValue - MinValue);
            double cRed = 1;
            double cBlue = 1;
            double cGreen = 1;
            if (_FalseColor != Color.Empty)
            {
                cBlue = (double)_FalseColor.B / 255;
                cRed = (double)_FalseColor.R / 255;
                cGreen = (double)_FalseColor.G / 255;
            }


            
            unsafe
            {
                for (int y = 0; y < OutputArray.Length ; y++)
                {
                    long gray = (long)(y);
                    gray = (long)((gray - MinValue) / ValueLength * 256);

                    if (gray < 0) gray = 0;
                    else if (gray > 255) gray = 255;
                    byte v = (byte)gray;

                    OutputArray [y]=(Int32)( gray*cBlue  + 255*gray*cGreen  + 255*255*gray*cRed );
                }
            }
            
           
            return OutputArray ;
        }
        
        /// <summary>
        /// Returns the histogram of intensities for this image
        /// </summary>
        /// <param name="histogram"></param>
        /// <param name="MaxValue"></param>
        /// <param name="MinValue"></param>
        /// <param name="MaxPossible"></param>
        /// <returns></returns>
        public bool GetHistogram(out int[] histogram, out long MaxValue, out long MinValue, out long MaxPossible)
        {
          
           return GetHistogram(_BitsPerPixel, _Stride, _Width, _Height, Data,out histogram,out MaxValue,out MinValue,out MaxPossible);
        }
        public static bool GetHistogram(int BitsPerPixel, int Stride,int Width, int Height,IntPtr Data, out int[] histogram, out long MaxValue, out long MinValue, out long MaxPossible)
        {
            long Bins;
            if ((BitsPerPixel  == 16))
            {
                Bins = 1000;
                MaxPossible = (long)Math.Pow(2, 16) + 1;
                double BinSize = (double)(MaxPossible) / (double)Bins;
                histogram = new int[Bins];
                MaxValue = 0;
                MinValue = 100000;
                
                unsafe
                {
                    int wStart = (int)(Width * 0.2);
                    int hStart=(int)( Height*0.2);
                    int nHeight = (int)(.8 * Height);
                    int nWidth = (int)(.8 * Width);
                    for (int y =hStart ; y < nHeight ; y++)
                    {
                        ushort* inData = (ushort*)((byte*)Data  + (y * Stride  ));
                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = wStart ; x < nWidth; x++)
                        {

                            ushort gray = (ushort)inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            gray = (ushort)(gray / BinSize);
                            histogram[gray]++;
                        }
                    }
                }

            }
            else
            {
                Bins = 257;
                int pitch = 1;
                histogram = new int[Bins - 1];
                MaxValue = 0;
                MinValue = 100000;
                MaxPossible = 257;
                unsafe
                {

                    for (int y = 0; y < Height; y++)
                    {
                        byte* inData = (byte*)Data  + (y * pitch);

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < Width; x++)
                        {
                            byte gray = inData[x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            histogram[gray]++;

                        }

                    }
                }
            }



            //this.MaxValue = MaxValue;
            //this.MinValue = MinValue;

            return true;
        }

        public static bool GetHistogram(int BitsPerPixel, int Stride, int Width, int Height, Byte[] Data, out int[] histogram, out long MaxValue, out long MinValue, out long MaxPossible)
        {
            
            long Bins;
            if ((BitsPerPixel == 16))
            {
                Bins = 1000;
                MaxPossible = (long)Math.Pow(2, 16) + 1;
                double BinSize = (double)(MaxPossible) / (double)Bins;
                histogram = new int[Bins];
                MaxValue = 0;
                MinValue = 100000;

                unsafe
                {
                    int wStart = (int)(Width * 0.2);
                    int hStart = (int)(Height * 0.2);
                    int nHeight = (int)(.8 * Height);
                    int nWidth = (int)(.8 * Width);
                    
                    for (int y = hStart; y < nHeight; y++)
                    {
                        long ByteY = y*Stride ;
                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = wStart; x < nWidth; x++)
                        {

                            ushort gray = (ushort)((ushort)Data[ByteY + x * 2] * 256 + (ushort)Data[ByteY + x * 2 + 1]);
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            gray = (ushort)(gray / BinSize);
                            histogram[gray]++;
                        }
                    }
                }

            }
            else
            {
                Bins = 257;
                int pitch = 1;
                histogram = new int[Bins - 1];
                MaxValue = 0;
                MinValue = 100000;
                MaxPossible = 257;
                unsafe
                {

                    for (int y = 0; y < Height; y++)
                    {
                        long ByteY = y * Stride;

                        // System.Diagnostics.Debug.Print(y.ToString());
                        for (int x = 0; x < Width; x++)
                        {
                            byte gray = Data[ByteY + x];
                            if (gray > MaxValue) MaxValue = gray;
                            if (gray < MinValue) MinValue = gray;
                            histogram[gray]++;

                        }

                    }
                }
            }



            //this.MaxValue = MaxValue;
            //this.MinValue = MinValue;

            return true;
        }

        /// <summary>
        /// Converts Data into a nice usable bitmap
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="NumBytes"></param>
        /// <param name="BitsPerPixel"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="ContrastMinValue"></param>
        /// <param name="ContrastMaxValue"></param>
        /// <param name="OutPutFormat"></param>
        /// <param name="FalseColor"></param>
        /// <returns></returns>
        public static Bitmap CreateGrayScaleBitmap(IntPtr  Data, long NumBytes, int BitsPerPixel, int Width, int Height, long ContrastMinValue, long ContrastMaxValue, AllowedPixelFormats OutPutFormat, Color FalseColor )
        {
                int Stride = (int)NumBytes  /Height ;
                Bitmap result;
                if (OutPutFormat==AllowedPixelFormats.Format32BppARGB )
                    result = new Bitmap(Width, Height, PixelFormat.Format32bppArgb  );
                else
                    result = new Bitmap(Width, Height, PixelFormat.Format32bppRgb );

                BitmapData outdata = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, result.PixelFormat );
                long MaxValue;
                long MinValue;
                if ((ContrastMaxValue == -1 && ContrastMinValue == -1) || (ContrastMaxValue == 0 && ContrastMinValue == 0))
                {
                    int[] hist;
                    long MaxPossible = 255;
                    GetHistogram(BitsPerPixel,Stride,Width,Height,Data, out hist, out  MaxValue, out  MinValue, out  MaxPossible);
                }
                else 
                {
                    MaxValue=ContrastMaxValue ;
                    MinValue =ContrastMinValue;
                }
                double  ValueLength = ( MaxValue-MinValue);
                int PixelSize = 4;
                double cRed = 1;
                double cBlue = 1;
                double cGreen = 1;
                if (FalseColor != Color.Empty)
                {
                    cBlue = (double)FalseColor.B / 255;
                    cRed = (double)FalseColor.R / 255;
                    cGreen = (double)FalseColor.G / 255;
                }
                

                if (BitsPerPixel == 16)
                {
                    unsafe
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            ushort* inData = (ushort*)((byte*)Data + (y * Stride ));

                            //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                            //ushort* inData = (ushort*)(lineP);
                            byte* row = (byte*)outdata.Scan0  + (y * outdata.Stride );

                            // System.Diagnostics.Debug.Print(y.ToString());
                            if (OutPutFormat == AllowedPixelFormats.Format32BppARGB)
                            {
                                for (int x = 0; x < Width; x++)
                                {
                                    long gray = (long)(inData[x]);

                                    gray = (long)((gray - MinValue) / ValueLength * 256);

                                    if (gray < 0) gray = 0;
                                    else if (gray > 255) gray = 255;
                                    byte v = (byte)gray;
                                    
                                    row[x * PixelSize] =(byte)( v*cBlue) ;
                                    row[x * PixelSize + 1] =(byte)( v*cGreen) ;
                                    row[x * PixelSize + 2] =(byte)( v*cRed) ;
                                    row[x * PixelSize + 3] = 255;
                                }
                            }
                            else
                            {
                                for (int x = 0; x < Width; x++)
                                {
                                    long gray = (long)(inData[x]);

                                    gray = (long)((gray - MinValue) / ValueLength * 256);

                                    if (gray < 0) gray = 0;
                                    else if (gray > 255) gray = 255;
                                    byte v = (byte)gray;
                                    row[x * PixelSize] = (byte)(cRed* v);
                                    row[x * PixelSize + 1] =(byte)(cGreen* v);
                                    row[x * PixelSize + 2] =(byte)(cBlue* v);

                                }
                            }
                        }
                    }
            }
            else 
            {
                unsafe
                {
                    
                    for (int y = 0; y < Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte* inData = (byte*)(lineP);
                        byte* inData = (byte*)Data + (y * Stride );
                        byte* row = (byte*)outdata.Scan0 + (y * outdata.Stride);
                       // System.Diagnostics.Debug.Print(y.ToString());
                        if (OutPutFormat == AllowedPixelFormats.Format32BppARGB)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int gray = (int)(inData[x]);

                                gray = (int)((gray - MinValue) / ValueLength * 255);
                                if (gray < 0)
                                    gray = 0;
                                if (gray > 250)
                                    gray = 250;

                                row[x * PixelSize] = (byte)255;
                                row[x * PixelSize + 1] = (byte)gray;
                                row[x * PixelSize + 2] = (byte)gray;
                                row[x * PixelSize + 3] = (byte)gray;
                            }
                        }
                        else
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int gray = (int)(inData[x]);

                                gray = (int)((gray - MinValue) / ValueLength * 255);
                                if (gray < 0)
                                    gray = 0;
                                if (gray > 250)
                                    gray = 250;

                                
                                row[x * PixelSize ] = (byte)gray;
                                row[x * PixelSize + 1] = (byte)gray;
                                row[x * PixelSize + 2] = (byte)gray;
                            }
                        }
                    }
                }
            }
                // Unlock the bitmap
                result.UnlockBits(outdata);
                return result;
       }

        public static Bitmap CreateGrayScaleBitmap(byte[] Data, long NumBytes, int BitsPerPixel, int Width, int Height, long ContrastMinValue, long ContrastMaxValue, AllowedPixelFormats OutPutFormat)
        {
                int Stride = (int)NumBytes  /Height ;
                Bitmap result;
                if (OutPutFormat == AllowedPixelFormats.Format32BppARGB)
                    result = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                else
                    result = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);

                BitmapData outdata = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, result.PixelFormat);
                long MaxValue;
                long MinValue;
                if ((ContrastMaxValue == -1 && ContrastMinValue == -1) || (ContrastMaxValue == 0 && ContrastMinValue == 0))
                {
                    int[] hist;
                    long MaxPossible = 255;
                    GetHistogram(BitsPerPixel,Stride,Width,Height,Data, out hist, out  MaxValue, out  MinValue, out  MaxPossible);
                }
                else 
                {
                    MaxValue = ContrastMaxValue;
                    MinValue = ContrastMinValue;
                }
                double  ValueLength = ( MaxValue-MinValue);
                int PixelSize = 4;
                if (BitsPerPixel == 16)
                {
                    unsafe
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            //ushort* inData = (ushort*)((byte*)Data + (y * _Stride ));
                            long ByteY = y * Stride;
                            //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                            //ushort* inData = (ushort*)(lineP);
                            byte* row = (byte*)outdata.Scan0  + (y * outdata.Stride );

                            if (OutPutFormat == AllowedPixelFormats.Format32BppARGB)
                            {
                                for (int x = 0; x < Width; x++)
                                {
                                    long gray = (long)((long)Data[ByteY + x * 2] * 256 + (long)Data[ByteY + x * 2 + 1]);
                                    gray = (long)((gray - MinValue) / ValueLength * 256);

                                    if (gray < 0) gray = 0;
                                    else if (gray > 256) gray = 256;


                                    row[x * PixelSize] = 255;
                                    row[x * PixelSize + 1] = (byte)(gray);
                                    row[x * PixelSize + 2] = (byte)(gray);
                                    row[x * PixelSize + 3] = (byte)(gray);

                                }
                            }
                            else if (OutPutFormat == AllowedPixelFormats.Format32BppRGB)
                            {
                                for (int x = 0; x < Width; x++)
                                {
                                    long gray = (long)((long)Data[ByteY + x * 2] * 256 + (long)Data[ByteY + x * 2 + 1]);
                                    gray = (long)((gray - MinValue) / ValueLength * 256);

                                    if (gray < 0) gray = 0;
                                    else if (gray > 256) gray = 256;

                                    row[x * PixelSize ] = (byte)(gray);
                                    row[x * PixelSize + 1] = (byte)(gray);
                                    row[x * PixelSize + 2] = (byte)(gray);

                                }
                            }
                        }
                    }
            }
            else 
            {
                unsafe
                {
                    
                    for (int y = 0; y < Height; y++)
                    {
                        long ByteY = Stride * y;
                        byte* row = (byte*)outdata.Scan0 + (y * outdata.Stride);

                        if (OutPutFormat == AllowedPixelFormats.Format32BppARGB)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int gray = (int)(Data[ByteY + x]);

                                gray = (int)((gray - MinValue) / ValueLength * 255);
                                if (gray < 0)
                                    gray = 0;
                                if (gray > 255)
                                    gray = 255;

                                row[x * PixelSize] = (byte)255;
                                row[x * PixelSize + 1] = (byte)gray;
                                row[x * PixelSize + 2] = (byte)gray;
                                row[x * PixelSize + 3] = (byte)gray;
                            }
                        }
                        else if (OutPutFormat ==AllowedPixelFormats.Format32BppRGB )
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int gray = (int)(Data[ByteY + x]);

                                gray = (int)((gray - MinValue) / ValueLength * 255);
                                if (gray < 0)
                                    gray = 0;
                                if (gray > 255)
                                    gray = 255;

                                
                                row[x * PixelSize ] = (byte)gray;
                                row[x * PixelSize + 1] = (byte)gray;
                                row[x * PixelSize + 2] = (byte)gray;
                            }
                        }
                    }
                }
            }
                // Unlock the bitmap
                result.UnlockBits(outdata);
                return result;
            }


        /// <summary>
        /// Converts Data into a nice usable bitmap
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="NumBytes"></param>
        /// <param name="BitsPerPixel"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="ContrastMinValue"></param>
        /// <param name="ContrastMaxValue"></param>
        /// <param name="OutPutFormat"></param>
        /// <param name="FalseColor"></param>
        /// <returns></returns>
        public static Bitmap CreateRawBitmap(IntPtr Data, long NumBytes, int BitsPerPixel, int Width, int Height)
        {
            bool GrayScale = true;
            int Stride = (int)NumBytes / Height;
            Bitmap result=null ;
            if (BitsPerPixel == 16 || BitsPerPixel == 8)
                result = new Bitmap(Width, Height, PixelFormat.Format16bppGrayScale );
            else if (BitsPerPixel == 32 || BitsPerPixel == 24 )
                result = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);

            if (BitsPerPixel == 32)
                GrayScale = false;

            BitmapData outdata = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, result.PixelFormat);

            int PixelSize = 4;
            if (BitsPerPixel == 16)
            {
                unsafe
                {
                    for (int y = 0; y < Height; y++)
                    {
                        ushort * inData = (ushort*)((byte*)Data + (y * Stride));
                        ushort * row    = (ushort*)((byte*)outdata.Scan0 + (y * outdata.Stride));

                            for (int x = 0; x < Width; x++)
                            {
                                ushort  gray = (ushort )(inData[x]);
                                row[x ] = gray ;
                            }
                    }
                }
            }
            else if (BitsPerPixel == 8)
            {
                unsafe
                {
                    for (int y = 0; y < Height; y++)
                    {
                        byte* inData = ((byte*)Data + (y * Stride));
                       
                        UInt16   *row =(UInt16*) ((byte*)outdata.Scan0 + (y * outdata.Stride));

                        for (int x = 0; x < Width; x++)
                        {
                            byte  gray = (inData[x]);
                            row[x] = gray;
                        }
                    }
                }
            }
            else 
            {
                unsafe
                {

                    for (int y = 0; y < Height; y++)
                    {
                        //IntPtr lineP = FreeImage.GetScanLine(dib, y);
                        //byte* inData = (byte*)(lineP);
                        byte* inData = (byte*)Data + (y * Stride);
                        byte* row = (byte*)outdata.Scan0 + (y * outdata.Stride);
                        // System.Diagnostics.Debug.Print(y.ToString());
                            for (int x = 0; x < Width; x++)
                            {
                                row[x * PixelSize] = inData[x * PixelSize];
                                row[x * PixelSize + 1] = inData[x * PixelSize+1];
                                row[x * PixelSize + 2] = inData[x * PixelSize+2];
                            }
                    }
                }
            }
            // Unlock the bitmap
            result.UnlockBits(outdata);
            return result;
        }

        public Bitmap  ImageARGB
        {
            get
            {
                return CreateGrayScaleBitmap(Data, _lSize, _BitsPerPixel, _Width, _Height,MinValue , MaxValue ,AllowedPixelFormats.Format32BppARGB,_FalseColor  );
              
            }

        }

        public Bitmap ImageRaw
        {
            get
            {
                return CreateRawBitmap(Data, _lSize, _BitsPerPixel, _Width, _Height);
            }
        }
        /// <summary>
        /// COM Compadible image
        /// </summary>
        public object ImageIPicture
        {
            get
            {
                Image i = CreateGrayScaleBitmap(Data, _lSize, _BitsPerPixel, _Width, _Height, MinValue, MaxValue, AllowedPixelFormats.Format32BppRGB, _FalseColor);
                return Microsoft.VisualBasic.Compatibility.VB6.Support.ImageToIPicture(i);
            }
        }

        /// <summary>
        /// GDI compatible image
        /// </summary>
        public object ImageIPictureDisp
        {
            get
            {
                Image i = CreateGrayScaleBitmap(Data, _lSize, _BitsPerPixel, _Width, _Height, MinValue, MaxValue, AllowedPixelFormats.Format32BppRGB, _FalseColor);
                return Microsoft.VisualBasic.Compatibility.VB6.Support.ImageToIPictureDisp (i);
            }
        }
       
   

        public Bitmap ImageRGB
        {
            get
            {

                return CreateGrayScaleBitmap(Data, _lSize, _BitsPerPixel, _Width, _Height, MinValue , MaxValue , AllowedPixelFormats.Format32BppRGB,_FalseColor  );
            }

        }

        /// <summary>
        /// Returns a nice pixel from an intensity.  No contrasting.
        /// </summary>
        /// <param name="pixelValue"></param>
        /// <returns></returns>
        public uint ConvertPixelToColorBgra(long pixelValue)
        {
            
            long gray = pixelValue ;

            uint j = 0;
            unsafe
            {
                uint* jp = &j;
                jp[0] = 255;
                jp[1] = (byte)(gray / 257);
                jp[2] = (byte)(gray / 257);
                jp[3] = (byte)(gray / 257);
            }
            return j;
        }

        /// <summary>
        /// Opens a nice file and formats it for coreimage
        /// </summary>
        /// <param name="Filename"></param>
        public void Open(string Filename)
        {
            InternalFreeImage =new FreeImageBitmap(Filename);


            this.Data = InternalFreeImage.Bits;
            this._BitsPerPixel = InternalFreeImage.ColorDepth;
            this._Height = InternalFreeImage.Height;
            this._lSize = InternalFreeImage.DataSize;
            this._Stride = InternalFreeImage.Pitch;
            this._Width = InternalFreeImage.Width;

        }

        /// <summary>
        /// This takes an array and returns an image.  All values in the array should be positive
        /// </summary>
        /// <param name="ImageArray"></param>
        /// <returns></returns>
        public static  CoreImage  CreateImageFromArray(long[,] ImageArray)
        {

            FIBITMAP dib;
            
            int iWidth = ImageArray.GetLength(0);
            int iHeight = ImageArray.GetLength(1);
            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, iWidth, iHeight, 16, 0, 0, 0);

            for (int y = 0; y < iHeight ; y++)
              {
                    System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                    unsafe
                    {

                        ushort* bits = (ushort*)(void*)scanline;
                        
                        for (int x = 0; x < iWidth ; x++)
                        {
                            ushort gray =(ushort) ImageArray[x,y];
                            bits[x] = gray;
                           
                        }

                    }

                }

            
            return new CoreImage(new FreeImageBitmap(dib,""));
        }
        /// <summary>
        /// This takes an array and returns an image.  All values in the array should be positive
        /// </summary>
        /// <param name="ImageArray"></param>
        /// <returns></returns>
        public static CoreImage CreateImageFromArray(int[,] ImageArray)
        {

            FIBITMAP dib;

            int iWidth = ImageArray.GetLength(0);
            int iHeight = ImageArray.GetLength(1);
            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, iWidth, iHeight, 16, 0, 0, 0);

            for (int y = 0; y < iHeight; y++)
            {
                System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                unsafe
                {

                    ushort* bits = (ushort*)(void*)scanline;

                    for (int x = 0; x < iWidth; x++)
                    {
                        ushort gray = (ushort)ImageArray[x, y];
                        bits[x] = gray;

                    }

                }

            }


            return new CoreImage(new FreeImageBitmap(dib, ""));
        }
        /// <summary>
        /// This takes an array and returns an image.  All values in the array should be positive
        /// </summary>
        /// <param name="ImageArray"></param>
        /// <returns></returns>
        public static CoreImage CreateImageFromArray(double[,] ImageArray)
        {

            FIBITMAP dib;

            int iWidth = ImageArray.GetLength(0);
            int iHeight = ImageArray.GetLength(1);
            double iMax = -10000;
            double iMin = 10000;
            
            for (int i=0; i<iWidth ;i++)
                for (int j = 0; j < iHeight; j++)
                {
                    if (iMax < ImageArray[i, j]) iMax = ImageArray[i, j];
                    if (iMin > ImageArray[i, j]) iMin = ImageArray[i, j];
                }
            double iLength = iMax-iMin;
            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, iWidth, iHeight, 16, 0, 0, 0);

            for (int y = 0; y < iHeight; y++)
            {
                long maxPoss=Int16.MaxValue ;
                System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                unsafe
                {

                    ushort* bits = (ushort*)(void*)scanline;

                    for (int x = 0; x < iWidth; x++)
                    {
                        double g = (ImageArray[x, y] - iMin) / iLength;
                        ushort gray = (ushort)(g*maxPoss) ;
                        bits[x] = gray;

                    }

                }

            }


            return new CoreImage(new FreeImageBitmap(dib, ""));
        }

        /// <summary>
        /// This takes a linear array and returns an image.  All values in the array should be positive
        /// </summary>
        /// <param name="ImageArray"></param>
        /// <returns></returns>
        public static CoreImage CreateImageFromArray(double[] ImageArray,int Width,int Height)
        {

            FIBITMAP dib;

            int iWidth = Width;// ImageArray.GetLength(0);
            int iHeight = Height;// ImageArray.GetLength(1);
            double iMax = -10000;
            double iMin = 10000;

            for (int i = 0; i < ImageArray.Length ; i++)
            {
                    if (iMax < ImageArray[i]) iMax = ImageArray[i];
                    if (iMin > ImageArray[i]) iMin = ImageArray[i];
            }
            double iLength = iMax - iMin;
            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, iWidth, iHeight, 16, 0, 0, 0);

            for (int y = 0; y < iHeight; y++)
            {
                long maxPoss = Int16.MaxValue;
                System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                unsafe
                {

                    ushort* bits = (ushort*)(void*)scanline;

                    for (int x = 0; x < iWidth; x++)
                    {
                        double g = (ImageArray[x+ y*iWidth  ] - iMin) / iLength;
                        ushort gray = (ushort)(g * maxPoss);
                        bits[x] = gray;

                    }

                }

            }


            return new CoreImage(new FreeImageBitmap(dib, ""));
        }
        /// <summary>
        /// This takes an array and returns an image.  All values in the array should be positive
        /// </summary>
        /// <param name="ImageArray"></param>
        /// <returns></returns>
        public static CoreImage CreateImageFromArray( Int16[,] ImageArray)
        {

            FIBITMAP dib;

            int iWidth = ImageArray.GetLength(0);
            int iHeight = ImageArray.GetLength(1);
            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16, iWidth, iHeight, 16, 0, 0, 0);

            for (int y = 0; y < iHeight; y++)
            {
                System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                unsafe
                {

                    ushort* bits = (ushort*)(void*)scanline;

                    for (int x = 0; x < iWidth; x++)
                    {
                        ushort gray = (ushort)ImageArray[x, y];
                        bits[x] = gray;

                    }
                    
                }

            }


            return new CoreImage(new FreeImageBitmap(dib, ""));
        }
     
        //this is for the python code, It cannot handle the 2-D arrays, so 
        //we need to read them in 1 line at a time.  painful, but required
        private static int[,] BuildCoreImage;
        
        /// <summary>
        /// this is for the python code, It cannot handle the 2-D arrays, so 
        /// we need to read them in 1 line at a time.  painful, but required
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public static void CreateBuildUpImagePy(int Width, int Height)
        {
            BuildCoreImage = new int[Width,Height ];
        }
        /// <summary>
        /// this is for the python code, It cannot handle the 2-D arrays, so 
        /// we need to read them in 1 line at a time.  painful, but required
        /// </summary>
        /// <param name="YIndex"></param>
        /// <param name="ImageArray"></param>
        public static void BuildUpArrayLine(int YIndex,int[] ImageArray)
        {
            for (int i = 0; i < BuildCoreImage.GetLength(1); i++)
            {
                BuildCoreImage[i, YIndex] = ImageArray[i];
            }
        }
        /// <summary>
        /// this is for the python code, It cannot handle the 2-D arrays, so 
        /// we need to read them in 1 line at a time.  painful, but required
        /// </summary>
        /// <returns></returns>
        public static CoreImage GetBuildUpImage()
        {
            return CreateImageFromArray(BuildCoreImage);
        }
        private CoreImage(FreeImageBitmap fib)
        {
            InternalFreeImage = fib;
            this.Data = InternalFreeImage.Bits;
            this._BitsPerPixel = InternalFreeImage.ColorDepth;
            this._Height = InternalFreeImage.Height;
            this._lSize = InternalFreeImage.DataSize;
            this._Stride = InternalFreeImage.Pitch;
            this._Width = InternalFreeImage.Width;
        }

        public void Save(string Filename,bool Multipage)
        {
            if (InternalFreeImage != null)
            {
                
                try
                {
                    if (Multipage == false)
                        InternalFreeImage.Save(Filename);
                    else
                        InternalFreeImage.SaveAdd(Filename);
                }
                catch
                {
                    this.ImageRGB.Save(Filename);
                }
            }
            else
            {
                FIBITMAP dib = MakeFIBITMAP();

                string extenstion = System.IO.Path.GetExtension(Filename);

                if (Multipage)
                {
                    FIMULTIBITMAP fmb = FreeImage.OpenMultiBitmapEx(Filename);
                    FreeImage.AppendPage(fmb, dib);
                    FreeImage.CloseMultiBitmapEx(ref fmb);
                }
                else
                {
                    // if (extenstion == "tiff" || extenstion == "tif")
                    //     FreeImage.Save(FREE_IMAGE_FORMAT.FIF_TIFF, dib, Filename, FREE_IMAGE_SAVE_FLAGS.TIFF_NONE);
                    // else
                    if (extenstion == "")
                        Filename += ".tif";

                    if (FreeImage.SaveEx(dib, Filename) == false)
                    {
                        this.ImageRGB.Save( Path.GetDirectoryName ( Filename ) + "\\" 
                            + Path.GetFileNameWithoutExtension(Filename ) + ".bmp");
                    }
                }
                // FreeImage.Save(FREE_IMAGE_FORMAT.FIF_BMP, dib, Filename, FREE_IMAGE_SAVE_FLAGS.BMP_SAVE_RLE);
                //FreeImage.Unload(dib);
            }

        }
        public string FileFormats()
        {
            string FilterS = "Tagged Image File Format (*.TIFF)|*.tif|";
            FilterS += "Bitmap  (*.BMP)|*.bmp|";
            FilterS += "Independent JPEG Group (*.JPG)|*.jpg|";
            FilterS += "Graphics Interchange Format (*.GIF)|*.gif|";
            FilterS += "JPEG Network Graphics (*.JNG)|*.jng|";
            FilterS += "Photoshop (*.PSD)|*.psd|";
            FilterS += "Targa files (*.TGA)|*.TGA|";
            FilterS += "Portable Network Graphics (*.PNG)}*.pgn|";

            FilterS += "Dr. Halo (*.CUT)|*.CUT|";
            FilterS += "DirectDraw Surface (*.DDS)|*.dds|";
            FilterS += "Windows Icon (*.ICO)|*.ico|";
            FilterS += "Amiga IFF (*.IFF, *.LBM)|*.iff;*.lbm|";
            FilterS += "Commodore 64 Koala format (*.KOA)|*.koa|";
            FilterS += "Multiple Network Graphics (*.MNG)|*.mng|";
            FilterS += "Portable Bitmap (ASCII) (*.PBM)|*.pbm|";
            FilterS += "Portable Bitmap (BINARY) (*.PBM)|*.pbm|";
            FilterS += "Kodak PhotoCD (*.PCD)|*.pcd|";
            FilterS += "PCX bitmap format (*.PCX)|*.pcx|";
            FilterS += "Portable Graymap (*.PGM)|*.pgm|";
            FilterS += "Portable Pixelmap (*.PPM)|*.ppm|";
            FilterS += "Sun Rasterfile (*.RAS)|*.ras|";
            FilterS += "Wireless Bitmap (*.WBMP)|*.wbmp|";
            FilterS += "X11 Bitmap Format (*.XBM)}*.xbm|";
            FilterS += "X11 Pixmap Format (*.XPM)|*.xpm|";
            return FilterS;
        }
        
    }


    [Guid("5A88092E-69FF-0002-AD8D-8FA83E550F20")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ICoreImageCom
    {

        int BitsPerPixel
        {
            get;
        }
        int Height
        {
            get;
        }
        int Width
        {
            get;
        }
        int lSize
        {
            get;
        }
        int Stride
        {
            get;
        }
        Color FalseColor
        {
            get;
            set;
        }
        
       
         void Dispose();
         
         
         //FIBITMAP MakeFIBITMAP();
         //FIBITMAP  MakeRawFIBITMAP();
         //FreeImageBitmap MakeFreeImageBitmap();
         //long ConvertContrastToPixelValue(double Contrast);
         long MaxContrast
        { get;set;}
         long MinContrast
        {get;set;}
         long GetCenterPoint();
         long SumImage();
         double AverageIntensity();
         double [,] GetArrayDouble();
         byte [,] GetArrayByte8();
         int[,] GetArrayInt32();
         Int16[,] GetArrayInt16( );
         long[,] GetArrayLong64();
         byte[] GetRawDataArray();
         Int32[] GetImageTable();
         bool GetHistogram(out int[] histogram, out long MaxValue, out long MinValue, out long MaxPossible);
         Bitmap  ImageARGB
        {
            get;
        }
         Bitmap ImageRGB
        {
            get;
        }
        object ImageIPicture
        {
            get;
        }

        object ImageIPictureDisp
        {
            get;
        }
         uint ConvertPixelToColorBgra(long pixelValue);
         void Open(string Filename);
        
         void Save(string Filename,bool Multipage);
         string FileFormats();
    }

    [Serializable]
    public class ImageContainer
    {
        private CoreDevices.CoreImage[] _InImage;
        private CoreDevices.CoreImage[] _OutImage;
        public CoreDevices.CoreImage[] InImage
        {
            get {return _InImage; }
            set { _InImage = value; }

        }
        public CoreDevices.CoreImage[] OutImage
        {
            get { return _OutImage; }
            set { _OutImage = value; }
        }
        public void SetImage(CoreDevices.CoreImage[] OutImage)
        {
            _OutImage = OutImage;
        }
    }
}
