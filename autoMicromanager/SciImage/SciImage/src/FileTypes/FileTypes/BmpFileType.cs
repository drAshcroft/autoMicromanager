/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using SciImage.PropertySystem;
using SciImage.IndirectUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SciImage
{
    public sealed class BmpFileType
        : FileType
    {
            public BmpFileType()
            : base("Bitmap File",
                   
                       FileTypeFlags.SupportsCustomHeaders |
                       FileTypeFlags.SupportsLoading |
                       FileTypeFlags.SupportsSaving,
                   new string[] { ".bmp" })
        {
        }

        protected override Document OnLoad(Stream input)
        {
            return GdiPlusFileType.OnLoadImage(input);
        }

        protected override void OnSave(Document input, Stream output, SaveConfigToken token, Surface scratchSurface, ProgressEventHandler callback)
        {
            
                ImageCodecInfo icf = GdiPlusFileType.GetImageCodecInfo(ImageFormat.Bmp);
                EncoderParameters parms = new EncoderParameters(1);
                EncoderParameter parm = new EncoderParameter(Encoder.ColorDepth, 24);
                parms.Param[0] = parm;

                using (Bitmap bitmap = CreateAliased24BppBitmap(scratchSurface))
                {
                    GdiPlusFileType.LoadProperties(bitmap, input);
                    bitmap.Save(output, icf, parms);
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

       

/*


        public BmpFileType()
            : base("BMP", FileTypeFlags.SupportsLoading | FileTypeFlags.SupportsSaving, new string[] { ".bmp" })
        {
        }

        public enum PropertyNames
        {
            BitDepth = 0,
            DitherLevel = 1
        }

        public enum BmpBitDepthUIChoices
        {
            AutoDetect = 0,
            Bpp24 = 1,
            Bpp8 = 2
        }
        
        public override PropertyCollection OnCreateSavePropertyCollection()
        {
            pdnList<Property> props = new pdnList<Property>();

            props.Add(StaticListChoiceProperty.CreateForEnum<BmpBitDepthUIChoices>(PropertyNames.BitDepth, BmpBitDepthUIChoices.AutoDetect, false));
            props.Add(new Int32Property(PropertyNames.DitherLevel, 7, 0, 8));

            pdnList<PropertyCollectionRule> rules = new pdnList<PropertyCollectionRule>();

            rules.Add(new ReadOnlyBoundToValueRule<object, StaticListChoiceProperty>(PropertyNames.DitherLevel, PropertyNames.BitDepth, BmpBitDepthUIChoices.Bpp8, true));

            PropertyCollection pc = new PropertyCollection(props, rules);

            return pc;
        }

        public override ControlInfo OnCreateSaveConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultSaveConfigUI(props);

            configUI.SetPropertyControlValue(
                PropertyNames.BitDepth,
                ControlInfoPropertyNames.DisplayName,
                PdnResources.GetString("BmpFileType.ConfigUI.BitDepth.DisplayName"));

            PropertyControlInfo bitDepthPCI = configUI.FindControlForPropertyName(PropertyNames.BitDepth);
            bitDepthPCI.SetValueDisplayName(BmpBitDepthUIChoices.AutoDetect, PdnResources.GetString("BmpFileType.ConfigUI.BitDepth.AutoDetect.DisplayName"));
            bitDepthPCI.SetValueDisplayName(BmpBitDepthUIChoices.Bpp24, PdnResources.GetString("BmpFileType.ConfigUI.BitDepth.Bpp24.DisplayName"));
            bitDepthPCI.SetValueDisplayName(BmpBitDepthUIChoices.Bpp8, PdnResources.GetString("BmpFileType.ConfigUI.BitDepth.Bpp8.DisplayName"));

            configUI.SetPropertyControlType(PropertyNames.BitDepth, PropertyControlType.RadioButton);

            configUI.SetPropertyControlValue(
                PropertyNames.DitherLevel,
                ControlInfoPropertyNames.DisplayName,
                PdnResources.GetString("BmpFileType.ConfigUI.DitherLevel.DisplayName"));

            return configUI;
        }

        protected override Document OnLoad(Stream input)
        {
            // This allows us to open images that were created in Explorer using New -> Bitmap Image
            // which actually just creates a 0-byte file
            if (input.Length == 0)
            {
                Document newDoc = new Document(800, 600);

                Layer blayer = Layer.CreateBackgroundLayer(newDoc.Width, newDoc.Height);

                newDoc.Layers.Add(blayer);
                return newDoc;
            }
            else
            {
                using (Image image = PdnResources.LoadImage(input))
                {
                    Document document = Document.FromImage(image);
                    return document;
                }
            }
        }

        public override int GetDitherLevelFromToken(PropertyBasedSaveConfigToken token)
        {
            int ditherLevel = token.GetProperty<Int32Property>(PropertyNames.DitherLevel).Value;
            return ditherLevel;
        }

        public override int GetThresholdFromToken(PropertyBasedSaveConfigToken token)
        {
            return 0;
        }

        public override Set<SavableBitDepths> CreateAllowedBitDepthListFromToken(PropertyBasedSaveConfigToken token)
        {
            BmpBitDepthUIChoices bitDepth = (BmpBitDepthUIChoices)token.GetProperty<StaticListChoiceProperty>(PropertyNames.BitDepth).Value;

            Set<SavableBitDepths> bitDepths = new Set<SavableBitDepths>();

            switch (bitDepth)
            {
                case BmpBitDepthUIChoices.AutoDetect:
                    bitDepths.Add(SavableBitDepths.Rgb24);
                    bitDepths.Add(SavableBitDepths.Rgb8);
                    break;

                case BmpBitDepthUIChoices.Bpp24:
                    bitDepths.Add(SavableBitDepths.Rgb24);
                    break;

                case BmpBitDepthUIChoices.Bpp8:
                    bitDepths.Add(SavableBitDepths.Rgb8);
                    break;

                default:
                    throw new InvalidEnumArgumentException("bitDepth", (int)bitDepth, typeof(BmpBitDepthUIChoices));
            }

            return bitDepths;
        }

        public override void FinalSave(
            Document input, 
            Stream output, 
            Surface scratchSurface, 
            int ditherLevel, 
            SavableBitDepths bitDepth,
            PropertyBasedSaveConfigToken token,
            ProgressEventHandler progressCallback)
        {
            // finally, do the save.
            if (bitDepth == SavableBitDepths.Rgb24)
            {
                // In order to save memory, we 'squish' the 32-bit bitmap down to 24-bit in-place
                // instead of allocating a new bitmap and copying it over.
                SquishSurfaceTo24Bpp(scratchSurface);

                ImageCodecInfo icf = GdiPlusFileType.GetImageCodecInfo(ImageFormat.Bmp);
                EncoderParameters parms = new EncoderParameters(1);
                EncoderParameter parm = new EncoderParameter(Encoder.ColorDepth, 24);
                parms.Param[0] = parm;

                using (Bitmap bitmap = CreateAliased24BppBitmap(scratchSurface))
                {
                    GdiPlusFileType.LoadProperties(bitmap, input);
                    bitmap.Save(output, icf, parms);
                }
            }
            else if (bitDepth == SavableBitDepths.Rgb8)
            {
                using (Bitmap quantized = Quantize(scratchSurface, ditherLevel, 256, false, progressCallback))
                {
                    ImageCodecInfo icf = GdiPlusFileType.GetImageCodecInfo(ImageFormat.Bmp);
                    EncoderParameters parms = new EncoderParameters(1);
                    EncoderParameter parm = new EncoderParameter(Encoder.ColorDepth, 8);
                    parms.Param[0] = parm;

                    GdiPlusFileType.LoadProperties(quantized, input);
                    quantized.Save(output, icf, parms);
                }
            }
            else
            {
                throw new InvalidEnumArgumentException("bitDepth", (int)bitDepth, typeof(SavableBitDepths));
            }
        }
 */
  