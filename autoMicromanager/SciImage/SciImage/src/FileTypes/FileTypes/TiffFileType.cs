       /////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.IndirectUI;
using SciImage.PropertySystem;
using SciImage.SystemLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using FreeImageAPI;
using System.Runtime.InteropServices;


namespace SciImage
{

       public sealed class TiffFileType
        : FileType
    {
            public TiffFileType()
            : base("Tiff File",
                   
                       FileTypeFlags.SupportsCustomHeaders |
                       FileTypeFlags.SupportsLoading |
                       FileTypeFlags.SupportsSaving,
                   new string[] { ".Tiff",".tif" })
        {
        }

        protected override Document OnLoad(Stream input)
        {
            FreeImageBitmap fib = new FreeImageBitmap(input);
            Document untitled;
            untitled = Document.FromImage(fib.Width, fib.Height, fib.Pitch, fib.ColorDepth , fib.DataSize, fib.Bits  , -1,-1);
           
            return untitled ;
        }

        private FIBITMAP MakeFIB(Surface surf)
        {
            
            FIBITMAP dib;
            
            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_UINT16,surf.Width,surf.Height, 16, 0, 0, 0);

            for (int y = 0; y < surf.Height; y++)
            {
                System.IntPtr scanline = FreeImage.GetScanLine(dib, y);
                unsafe
                {

                    ushort* bits = (ushort*)(void*)scanline;
                    uint* inData = (uint*)((byte*)surf.Scan0.VoidStar    + (y * surf.Stride  ));
                    for (int x = 0; x < surf. Width; x++)
                    {
                        uint gray = inData[x];
                        bits[x] =(ushort) gray;
                    }

                }

            }
            
           
            return dib;
        }
        /*
                
           string filename = Path.GetTempFileName() + ".tif";
           FIMULTIBITMAP fmb = FreeImage.OpenMultiBitmapEx(filename);

           for (int i = 0; i < input.Layers.Count; i++)
           {
             //  if (((Layer)input.Layers[i]).RawData == null)
               {
                //   FreeImage.AppendPage(fmb, ((Layer)input.Layers[i]).RawData.DIB);
               }
              // else
               {
                   BitmapLayer bl = ((BitmapLayer)input.Layers[i]);
                   Bitmap b = bl.Surface.CreateAliasedBitmap();
                   FreeImageBitmap fib = new FreeImageBitmap(b);
                   FreeImage.AppendPage(fmb, fib.DIB);
               }
           }
           FreeImage.CloseMultiBitmapEx(ref fmb);
            */
        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback)
        {
            //todo: This does not handle multiple layers.  needs to cycle though all the layers
            if (  ((Layer)input.Layers[0]).Surface.DataType =="Intensity" )
            {
                FIBITMAP fib = MakeFIB(  ( (Layer)input.Layers[0]).Surface  );
                
                FreeImage.SaveToStream(fib, output, FREE_IMAGE_FORMAT.FIF_TIFF);
              
            }
            else
            {
                using (Bitmap bitmap = scratchSurface.CreateAliasedBitmap())
                {

                   
                        GdiPlusFileType.LoadProperties(bitmap, input);
                        bitmap.Save(output, ImageFormat.Tiff);

                   
                }
            }
        }

        public override bool IsReflexive(SaveConfigToken token)
        {
            return true;
        }

        private sealed class UpdateProgressTranslator
        {
            private long maxBytes;
            private long totalBytes;
            private ProgressEventHandler callback;

            public void IOEventHandler(object sender, IOEventArgs e)
            {
                double percent;

                lock (this)
                {
                    totalBytes += (long)e.Count;
                    percent = Math.Max(0.0, Math.Min(100.0, ((double)totalBytes * 100.0) / (double)maxBytes));
                }

                callback(sender, new ProgressEventArgs(percent));
            }

            public UpdateProgressTranslator(long maxBytes, ProgressEventHandler callback)
            {
                this.maxBytes = maxBytes;
                this.callback = callback;
                this.totalBytes = 0;
            }
        }
    }
}

     
  