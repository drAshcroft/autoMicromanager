/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */


import java.awt.image.*;
import java.awt.*;

import ij.ImagePlus;
import ij.process.ByteProcessor;
import ij.process.ImageProcessor;
import ij.process.ShortProcessor;
import ij.process.FloatProcessor;
import ij.process.ColorProcessor;

class LoadMMFImage {

   private ColorModel colorModel;

   public LoadMMFImage()
    {
//      Image i=        ij.IJ.createImage("test","8bit",10,10,1);
      
      colorModel =ColorModel.getRGBdefault();// generateColorModel();
        
    
    }
   static ImagePlus createImagePlus() {
		//ImagePlus imp = WindowManager.getCurrentImage();
		//if (imp!=null)
		//	return imp.createImagePlus();
		//else
		return new ImagePlus();
    }

   private ImagePlus LoadShortImage(String title, short[] pixels, int numChannels, int width,int height, int Depth)
    {
	    ImageProcessor ip = new ShortProcessor(width, height, pixels, null);
     //   	ip.invertLut();
	    ImagePlus imp = createImagePlus();
	    imp.setProcessor(title, ip);
	    imp.getProcessor().setMinAndMax(0, 65535); // default display range
	    return imp;

        
        
    }
   private ImagePlus LoadByteImage(String title, byte[] pixels, int numChannels, int width,int height, int Depth)
    {
        
		ImageProcessor ip = new ByteProcessor(width, height, pixels, null);
		ImagePlus imp = createImagePlus();
		imp.setProcessor(title, ip);
		return imp;

        
    }
   public int ImgWidth=0;
   public int ImgHeight=0;
   public int ImgBPP=0;
   public Image LoadImage(String fileMappingObjName) 
   {
        int mapFilePtr=0;
       try
       {
            mapFilePtr= MemMapFile.openFileMapping(MemMapFile.FILE_MAP_READ, false,
                fileMappingObjName);
            if(mapFilePtr != 0) {
              int viewPtr = MemMapFile.mapViewOfFile(mapFilePtr,
                  MemMapFile.FILE_MAP_READ, 0, 0, 0);
              if(viewPtr != 0) {
                   long[] info=MemMapFile.ReadImageInfo(viewPtr);
                   int Width=(int)info[2];
                   int Height=(int)info[3];
                   int BPP=(int)info[0];
                   ImgWidth=Width;
                   ImgHeight=Height;
                   ImgBPP=BPP;
                   
                   Object img = MemMapFile.ReadImage(viewPtr); 
                   
                   ImagePlus ipp = createImagePlus();
                   ImageProcessor imp;
                   
                   if (BPP == 1)
                      imp = new ByteProcessor((int)Width, (int)Height);
                   else if (BPP==2)
                      imp = new ShortProcessor((int)Width, (int)Height);
                   else if (BPP==4)
                       imp = new ColorProcessor((int)Width, (int)Height);
                      //imp = new FloatProcessor((int)Width,(int)Height);
                   else 
                       imp =null;
                   imp.setPixels(img);
                   if (BPP<4)
                        imp.setMinAndMax(0,255);
                   ipp.setProcessor(fileMappingObjName, imp);
                   MemMapFile.closeHandle(mapFilePtr);
                  // SaveImage("test3.mmf",ipp);
                   
                   return ipp.getImage();
                }
                  
              MemMapFile.closeHandle(mapFilePtr);
       
             
            }
       }
       catch (Throwable e)
       {
           if (mapFilePtr!=0) MemMapFile.closeHandle(mapFilePtr);
       }
       
       return null;
   }
   
   static public ImagePlus SaveImage(String fileMappingObjName, ImagePlus ImageOut) 
   {
       int mapFilePtr=0;
       try
       {
            DataBuffer dbuf= ImageOut.getBufferedImage().getData().getDataBuffer();
            int bTypeSize= ImageOut.getBytesPerPixel();
            int viewPtr=0;       
            
            int dwMemFileSize = dbuf.getSize()+100;
            mapFilePtr = MemMapFile.createFileMapping(MemMapFile.PAGE_READWRITE,
                        0, dwMemFileSize, fileMappingObjName);

            if(mapFilePtr != 0) 
              {
                viewPtr = MemMapFile.mapViewOfFile(mapFilePtr,
                                         MemMapFile.FILE_MAP_READ |
                                         MemMapFile.FILE_MAP_WRITE,
                                         0, 0, 0);
               }
             if(viewPtr != 0) {
             if (bTypeSize==1)
              {
                //byte[] Buffer=new byte[dbuf.getSize()];
                //for (int i=0;i<dbuf.getSize();i++)
                //{
                //   Buffer[i]=(byte) dbuf.getElem(i);
                //}
                byte[] t = ((DataBufferByte)(ImageOut.getBufferedImage()).getRaster().getDataBuffer()).getData(); 
                //ImageOut.getBufferedImage().getRaster().getDataBuffer().
                
                MemMapFile.WriteImage(viewPtr, t, bTypeSize , 1, ImageOut.getWidth(), ImageOut.getHeight());
              }
                  
                   MemMapFile.closeHandle(mapFilePtr);
              } 
           }
       
       catch (Throwable e)
       {
           if (mapFilePtr!=0) MemMapFile.closeHandle(mapFilePtr);
       }
       
       return null;
   }
     
   static public ImagePlus LoadImage(String fileMappingObjName, int numChannels, int Width,int Height, int BPP, int Depth)
   {
            int mapFilePtr = MemMapFile.openFileMapping(MemMapFile.FILE_MAP_READ, false,
                fileMappingObjName);
            if(mapFilePtr != 0) {
              int viewPtr = MemMapFile.mapViewOfFile(mapFilePtr,
                  MemMapFile.FILE_MAP_READ, 0, 0, 0);
              if(viewPtr != 0) {
                   Object img = MemMapFile.ReadImage(viewPtr);  
                   ImagePlus ipp = createImagePlus();
                   ImageProcessor imp;
                   if (BPP == 1)
                      imp = new ByteProcessor((int)Width, (int)Height);
                   else
                      imp = new ShortProcessor((int)Width, (int)Height);
                   imp.setPixels(img);
                   ipp.setProcessor(fileMappingObjName, imp);
                   MemMapFile.closeHandle(mapFilePtr);
                   return ipp;
                }
              
              MemMapFile.closeHandle(mapFilePtr);
       
       
            }
            return null;
   }
 
   public ImagePlus LoadImage2(String fileMappingObjName, String type, int numChannels, int Width,int Height, int BPP, int Depth)
   {
       if (BPP==1)
       {
            byte[] ImageData=null;   
            int mapFilePtr = MemMapFile.openFileMapping(MemMapFile.FILE_MAP_READ, false,
                fileMappingObjName);
            if(mapFilePtr != 0) {
              int viewPtr = MemMapFile.mapViewOfFile(mapFilePtr,
                  MemMapFile.FILE_MAP_READ, 0, 0, 0);
              if(viewPtr != 0) {
                //ImageData= MemMapFile.ReadImage(viewPtr);  
                return (LoadByteImage(fileMappingObjName, ImageData,  numChannels, Width,Height, Depth));
            }
            MemMapFile.closeHandle(mapFilePtr);
            
      
            }
            return null;
       
       }
       else if (BPP==2)
       {
            short[] ImageData=null;   
            int mapFilePtr = MemMapFile.openFileMapping(MemMapFile.FILE_MAP_READ, false,
                fileMappingObjName);
            if(mapFilePtr != 0) {
              int viewPtr = MemMapFile.mapViewOfFile(mapFilePtr,
                  MemMapFile.FILE_MAP_READ, 0, 0, 0);
              if(viewPtr != 0) {
               // ImageData= MemMapFile.ReadImage(viewPtr);  
                return (LoadShortImage(fileMappingObjName, ImageData,  numChannels, Width,Height, Depth));
            }
            MemMapFile.closeHandle(mapFilePtr);
            
      
            }
            return null;
       }
       return null;
       
   }
   private byte[] onDataReady(String fileMappingObjName) {
       
    byte[] ImageData=null;   
    int mapFilePtr = MemMapFile.openFileMapping(MemMapFile.FILE_MAP_READ, false,
        fileMappingObjName);
    if(mapFilePtr != 0) {
      int viewPtr = MemMapFile.mapViewOfFile(mapFilePtr,
	  MemMapFile.FILE_MAP_READ, 0, 0, 0);
      if(viewPtr != 0) {
//        ImageData= MemMapFile.ReadImage(viewPtr);  
      }
      MemMapFile.closeHandle(mapFilePtr);
      
    }
    return (ImageData);
  }
}