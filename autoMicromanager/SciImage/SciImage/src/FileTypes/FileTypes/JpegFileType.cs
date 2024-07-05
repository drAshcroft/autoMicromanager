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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using SciImage.Core.ColorsAndPixelOps;

namespace SciImage
{
       public sealed class JpegFileType
        : FileType
    {
            public JpegFileType()
            : base("JPEG File",
                   
                       FileTypeFlags.SupportsCustomHeaders |
                       FileTypeFlags.SupportsLoading |
                       FileTypeFlags.SupportsSaving,
                   new string[] { ".jpg", ".jpeg", ".jpe", ".jfif" })
        {
        }

        protected override Document OnLoad(Stream input)
        {
            return GdiPlusFileType.OnLoadImage(input);
            
        }

        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback)
        {

            int quality = 100;

            ImageCodecInfo icf = GdiPlusFileType.GetImageCodecInfo(ImageFormat.Jpeg);
            EncoderParameters parms = new EncoderParameters(1);
            EncoderParameter parm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            parms.Param[0] = parm;

            scratchSurface.Clear(ColorPixelBase.White);

            using (RenderArgs ra = new RenderArgs(scratchSurface))
            {
                input.Render(ra, false);
            }

            using (Bitmap bitmap = scratchSurface.CreateAliasedBitmap())
            {
                GdiPlusFileType.LoadProperties(bitmap, input);
                bitmap.Save(output, icf, parms);
            }
           
        }

        public override bool IsReflexive(SaveConfigToken token)
        {
            return false ;
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
/*

    public sealed class JpegFileType
        : PropertyBasedFileType
    {
        public JpegFileType()
            : base("JPEG", FileTypeFlags.SupportsLoading | FileTypeFlags.SupportsSaving, new string[] { ".jpg", ".jpeg", ".jpe", ".jfif" })
        {
        }

        public enum PropertyNames
        {
            Quality = 0
        }

        public override PropertyCollection OnCreateSavePropertyCollection()
        {
            pdnList<Property> props = new pdnList<Property>();

            props.Add(new Int32Property(PropertyNames.Quality, 95, 0, 100));

            return new PropertyCollection(props);
        }

        public override ControlInfo OnCreateSaveConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultSaveConfigUI(props);

            configUI.SetPropertyControlValue(
                PropertyNames.Quality,
                ControlInfoPropertyNames.DisplayName,
                PdnResources.GetString("JpegFileType.ConfigUI.Quality.DisplayName") ?? "??");

            return configUI;
        }
       
        protected override void OnSaveT(Document input, Stream output, PropertyBasedSaveConfigToken token, Surface scratchSurface, ProgressEventHandler progressCallback)
        {
            int quality = token.GetProperty<Int32Property>(PropertyNames.Quality).Value;

            ImageCodecInfo icf = GdiPlusFileType.GetImageCodecInfo(ImageFormat.Jpeg);
            EncoderParameters parms = new EncoderParameters(1);
            EncoderParameter parm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            parms.Param[0] = parm;

            scratchSurface.Clear(ColorPixelBase.White);

            using (RenderArgs ra = new RenderArgs(scratchSurface))
            {
                input.Render(ra, false);
            }

            using (Bitmap bitmap = scratchSurface.CreateAliasedBitmap())
            {
                GdiPlusFileType.LoadProperties(bitmap, input);
                bitmap.Save(output, icf, parms);
            }
        }

        protected override Document OnLoad(Stream input)
        {
            using (Image image = PdnResources.LoadImage(input))
            {
                Document document = Document.FromImage(image);
                return document;
            }
        }
     
    }
}*/