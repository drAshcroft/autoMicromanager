using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SciImage;

namespace FileTypes
{
    // We need this to register our DdsFileType object
    public class AllAddedFileTypes : IFileTypeFactory
    {
        public static readonly FileType Bmp = new BmpFileType();
        public static readonly FileType Jpeg = new JpegFileType();
        public static readonly FileType Tiff = new TiffFileType(); //new GdiPlusFileType("TIFF", ImageFormat.Tiff, false, new string[] { ".tif", ".tiff" });
        

        private static FileType[] fileTypes = new FileType[] { 
                                                                  Bmp,
                                                                  Jpeg,
                                                                  Tiff
                                                              };

        public FileTypeCollection GetFileTypeCollection()
        {
            return new FileTypeCollection(fileTypes);
        }

        public FileType[] GetFileTypeInstances()
        {
            return (FileType[])fileTypes.Clone();
        }
    }
}
