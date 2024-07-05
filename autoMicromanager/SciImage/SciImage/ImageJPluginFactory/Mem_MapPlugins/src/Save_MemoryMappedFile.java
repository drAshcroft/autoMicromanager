/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */


import ij.*;
import ij.io.*;
import ij.plugin.PlugIn;
import ij.process.ImageProcessor;
import java.awt.*;


/**
 *
 * @author Administrator
 */
public class Save_MemoryMappedFile  implements PlugIn {

    private int mapFilePtr=0;

    ImagePlus imp;
    private byte mmfFileHeader [] = new byte [14];
    private int BitsPP=32;
    private int biWidth = 0;
     private int padWidth = 0;
     private int biHeight = 0;
     private int biPlanes = 1;
     
     private int biCompression = 0;
     private int biSizeImage = 0;
     private int biXPelsPerMeter = 0x0;
     private int biYPelsPerMeter = 0x0;
    
     private int biClrImportant = 0;
     //--- Bitmap raw data
     private int intBitmap [];
     private short shortBitmap [];
     private byte byteBitmap [];
    
   public void run(String path) {
           
           IJ.showProgress(0);
           imp = WindowManager.getCurrentImage();
           if (imp==null)
             {IJ.noImage(); return;}
           try {
             writeImage(imp, path);
           } catch (Exception e) {
             String msg = e.getMessage();
             if (msg==null || msg.equals(""))
           msg = ""+e;
             IJ.error("BMP Writer", "An error occured writing the file.\n \n" + msg);
           }
           IJ.showProgress(1);
           IJ.showStatus("");
            
 }
   
   void writeImage(ImagePlus imp, String path) throws Exception {
           

           if (path==null || path.equals("")) {
             String prompt = "Save as " + BitsPP + " bit BMP";
             SaveDialog sd = new SaveDialog(prompt, imp.getTitle(), ".bmp");
             if(sd.getFileName()==null)
           return;
             path = sd.getDirectory()+sd.getFileName();
           }
           imp.startTiming();
           
           saveBitmap (path, imp.getImage(), imp.getWidth(), imp.getHeight() );
 }


 public void CloseFile()
 {

     MemMapFile.closeHandle( mapFilePtr);

 }
 private void saveBitmap (String parFilename, Image parImage, int parWidth, int parHeight) throws Exception {
   
       ImageProcessor ip;
       BitsPP=imp.getBitDepth();
       
       try
       {

       if (BitsPP==32)
       {


       }
       else if(BitsPP == 24 )
       {
          intBitmap = (int[]) imp.getProcessor().getPixels();
          mapFilePtr = MemMapFile.EasyWriteImageInt(parFilename, intBitmap,(int) intBitmap.length ,(int) BitsPP,(int) biPlanes,(int) parWidth,(int) parHeight);
       }
       else if (BitsPP==16)
       {
          shortBitmap = (short[]) imp.getProcessor().getPixels();
          mapFilePtr = MemMapFile.EasyWriteImageShort(parFilename, shortBitmap,(int) shortBitmap.length ,(int) BitsPP,(int) biPlanes,(int) parWidth,(int) parHeight);
       }
       else
       {
          byteBitmap = (byte[]) imp.getProcessor().convertToByte(true).getPixels();
          mapFilePtr = MemMapFile.EasyWriteImageByte(parFilename, byteBitmap,(int) byteBitmap.length ,(int) BitsPP,(int) biPlanes,(int) parWidth,(int) parHeight);
       }




       }
       catch (Throwable e)
       {
           if (mapFilePtr!=0) MemMapFile.closeHandle(mapFilePtr);
            throw new IllegalArgumentException(e.getMessage());
           
       }
       
   
 }


    

}
