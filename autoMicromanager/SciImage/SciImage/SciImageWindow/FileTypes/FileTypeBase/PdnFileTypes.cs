/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.Data;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using SciImage.InternalTypes;

namespace SciImage
{
    /// <summary>
    /// This is the default Paint.NET FileTypeFactory. It provides all the built-in FileTypes.
    /// </summary>
    public class PdnFileTypes
        : IFileTypeFactory
    {
        public static readonly FileType Bmp = new BmpFileType();
        public static readonly FileType Jpeg = new JpgFileType();
        public static readonly FileType Gif = new GifFileType();
        public static readonly FileType Tiff = new TiffFileType(); //new GdiPlusFileType("TIFF", ImageFormat.Tiff, false, new string[] { ".tif", ".tiff" });
        public static readonly PngFileType Png = new PngFileType();
        public static readonly FileType Pdn = new PdnFileType();
        public static readonly FileType Tga = new TgaFileType();
        
        private static FileType[] fileTypes = new FileType[] { 
                                                                  Pdn,
                                                                  Bmp,
                                                                  Gif,
                                                                  Jpeg,
                                                                  Png,
                                                                  Tiff,
                                                                  Tga
                                                              };

         public FileTypeCollection GetFileTypeCollection()
         {
             return new FileTypeCollection(fileTypes);
         }
        
         public FileType[] GetFileTypeInstances()
         {
             return  (FileType[])fileTypes.Clone();
         }
         
    }
}
