///////////////////////////////////////////////////////////////////////////////
// 
// PROJECT:       Micro-Manager
// SUBSYSTEM:     Effects
//-----------------------------------------------------------------------------
// DESCRIPTION:   An Experiment to link code to ImageJ using memorymaps
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the BSD license.
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
using System.Reflection;
using System.IO;
using SciImage.Effects;
using System.Windows.Forms;

namespace SciImage.Effects
{
    public class ImageJInterface : SciImage.Effects.SciEffect  
    {
        protected override SciImage.PropertySystem.PropertyCollection OnCreatePropertyCollection()
        {
            throw new NotImplementedException();
        }
        public static Image StaticImage
        {
            get
            {
                Assembly ourAssembly = Assembly.GetCallingAssembly();
                //Stream imageStream = ourAssembly.GetManifestResourceStream("SciImage.Effects.Icons.images.jpeg");
                //Image image = Image.FromStream(imageStream);
                return new Bitmap(10, 10);// image;
            }
        }

        private string ImageJEffect = "";
        public ImageJInterface(string ImageJEffect)
            : base(ImageJEffect , StaticImage,"ImageJ", EffectFlags.SingleThreaded ,false ,true  )
        {
            this.ImageJEffect = ImageJEffect;
        }
        public override  object Clone()
        {
            return new ImageJInterface(ImageJEffect);

        }
        ImageJPluginFactory.FileMemoryMap.DevCache FMM;
        public override string End(string[] AlreadyEndedKeys)
        {
            //bool Found = false;
            
            
            if (!AlreadyEndedKeys.Contains("ImageJ"))
            {
                try
                {
                    FMM = new ImageJPluginFactory.FileMemoryMap.DevCache("Mem_Map_File.mmf");
                    FMM.ConnectToImageJ();
                    FMM.SendCommand("End");
                }
                finally
                {
                    
                }
                return "ImageJ";
            }
            return "";
        }

        

        private void  GetMMFImage(Surface dstSurf)
        {
            ImageJPluginFactory.FileMemoryMap.DevCache gMM = new ImageJPluginFactory.FileMemoryMap.DevCache("memMapFileReturn.mmf");
            byte[] Buff;
            int BPP;
            int Width;
            int Height;
            int lSize;
            int numChannels;
            gMM.GetImage("",out Buff,out BPP, out numChannels,out Width, out Height ,out lSize );
            if (BPP == 24) BPP = 32;
            int bWidth = Width * BPP / 8;

            if (dstSurf.GetType() == typeof(Surface))
            {
                
                if (BPP == 32)
                {
                    unsafe
                    {
                        fixed (byte* p = Buff)
                        {
                            for (int y = 0; y < Height; y++)
                            {
                                byte* src = &p[y * bWidth];
                                byte* dst = (byte*)dstSurf.GetRowAddressUnchecked(y);

                                for (int x = 0; x < bWidth; x++)
                                {
                                    dst[x] = src[x];
                                }
                            }
                        }
                    }
                }
                else if (BPP == 16)
                {
                    MessageBox.Show("Bit Depth changes have not been implemented yet");
                }

            }
            else if (dstSurf.GetType()==typeof(IntensitySurface)) 
            {
                IntensitySurface di = (IntensitySurface)dstSurf;
                unsafe
                {
                    fixed (byte* p = Buff)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            byte* src = &p[y * bWidth];
                            byte* dst = (byte*)di.GetRawRowAddressUnchecked(y);

                            for (int x = 0; x < bWidth; x++)
                            {
                                dst[x] = src[x];
                            }
                        }
                    }
                }
                di.RefreshVisibleImage();
            }
            
        }

        protected override void OnSetRenderInfo(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            if (FMM == null)
            {
                FMM = new ImageJPluginFactory.FileMemoryMap.DevCache("Mem_Map_File.mmf");
            }
            try
            {
                FMM.ConnectToImageJ();
            }
            catch
            {
               // FMM.ListenForImageJ();
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Effects\\ImageJ\\ImageJ.exe");
                string startmacro= System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Effects\\ImageJ\\macros\\StartImageJ.txt");
                p.StartInfo.Arguments = "-batch \"" + startmacro + "\" " ;
                p.StartInfo.UseShellExecute = false;
                p.Start();
                
                p.WaitForInputIdle(10000);
                MessageBox.Show("Please wait for imageJ startup.  This only happens the first time you run an ImageJ filter.");
                System.Threading.Thread.Sleep(10000);
                //MessageBox.Show("Please wait for ImageJ to start and then activate the 'Csharp Control' Plugin.  You only have to do this the first time a ImageJ filter is run.");
                //FMM.WaitForImageJ();
                FMM.ConnectToImageJ();
                 
            }
            base.OnSetRenderInfo(parameters, dstArgs, srcArgs);
        }
        public EffectConfigDialog CreateConfigDialog2()
        {
            if (FMM != null)
            {
                try
                {
                    ImageJPluginFactory.FileMemoryMap.DevCache.CloseMap("Mem_Map_File.mmf", "");
                }
                catch { }
            }
            FMM = new ImageJPluginFactory.FileMemoryMap.DevCache("Mem_Map_File.mmf");
            try 
            {
               // 
            }
            catch 
            {
               /* 
                FMM.ConnectToImageJ();*/
            }
           // throw new NotImplementedException();
            ImageJConfig secd = new ImageJConfig();
            return secd;
        }

        public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            if (FMM == null)
            {
                FMM = new ImageJPluginFactory.FileMemoryMap.DevCache("Mem_Map_File.mmf");
            }
            else
            {
                try
                {
                    ImageJPluginFactory.FileMemoryMap.DevCache.CloseMap("Mem_Map_File.mmf", "");
                }
                catch { }

            }

            if (srcArgs.Surface.GetType() == typeof(IntensitySurface))
            {
                IntensitySurface s = (IntensitySurface)srcArgs.Surface;
                FMM.AddImage("", s.RawDataBlock, s.RStride, s.RlSize, s.RBPP/8, 1, true, s.Width, s.Height);
            }
            else
            {
                unsafe
                {
                    Surface s = srcArgs.Surface;
                    try
                    {
                        FMM.AddImage("",(Image)s.CreateAliasedBitmap(),4,1,true );// s.Scan0.ToByteArray(), (int)32, 1, true, s.Width, s.Height);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Print(ex.Message);
                    }
                }
            }
            FMM.ConnectToImageJ();
            FMM.SendCommand("MMF run " + ImageJEffect);
            string ret = "";
            do
            {
                ret= FMM.GetReturn();
            } while (!ret.Trim().StartsWith("FileSaved"));

            GetMMFImage(dstArgs.Surface );

            FMM.SendCommand("Done");
            ImageJPluginFactory.FileMemoryMap.DevCache.CloseMap("Mem_Map_File.mmf", "");
            //string ret = FMM.GetReturn();

        }
    }
}
