

/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */


import ij.io.*;
import ij.plugin.*;
import java.awt.*;

import ij.ImagePlus;

/**
 *
 * @author Administrator
 */
public class Load_MemoryMappedFile  extends ImagePlus implements PlugIn {

     public static Image UpdateImage(String arg)
     {
                if (arg=="")
                {
                    OpenDialog od = new OpenDialog("Open MMF...", arg);
                    String directory = od.getDirectory();
                    String name = od.getFileName();

                    if (name==null || name=="null" || name == "NULL" || name == "Null")
                    {
                        //this.close();
                        return null;
                    }
                    String path = directory + name;
                    arg=name;
                }
                LoadMMFImage LMI=new LoadMMFImage();
                Image img=LMI.LoadImage(arg);
                return img;
     }
     public void run(String arg)
     {
                img = UpdateImage(arg);

                FileInfo fi = new FileInfo();
                /*if ( LMI.ImgBPP==1)
                    fi.fileFormat = FileInfo.GRAY8;
                else 
                    fi.fileFormat = FileInfo.GRAY16_UNSIGNED;*/

                fi.fileName = arg;
                fi.directory = "c:/";
                setImage(img);
                setTitle(arg);
                setFileInfo(fi);
                //arg="";
                //if (arg.equals(""))
                   show();
                
        }
    

}






/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: Copyright (c) 2002</p>
 * <p>Company: </p>
 * @author Stanley  Wang
 * @version 1.0


class MemMapProxy {
  static {
    System.load("C:\\Documents and Settings\\Administrator\\Desktop\\ImageJ\\ImageJ\\dist\\MemMapProxyLib.dll");
  }
  private MemMapFileObserver observer;
  public MemMapProxy(MemMapFileObserver observer) {
    this.observer = observer;
    
    Init();
  }

  public void fireDataReadyEvent() {
    observer.onDataReady();
  }

  private native boolean Init();
  public native void destroy();
}
 *  */
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: Copyright (c) 2002</p>
 * <p>Company: </p>
 * @author Stanley  Wang
 * @version 1.0
 

class MemMapJavaClient extends Frame implements MemMapFileObserver{
  
  protected MemMapProxy proxy;
  public static final String fileMappingObjName = "Mem_Map_File-{70122C30-8765-4f98-9D21-36885C8A8121}";

  //Component initialization
  private void init() throws Exception  {
    proxy = new MemMapProxy(this);
  }

  private void destroy() {
    proxy.destroy();
  }

  public void onDataReady() {
    int mapFilePtr = MemMapFile.openFileMapping(MemMapFile.FILE_MAP_READ, false,
        fileMappingObjName);
    if(mapFilePtr != 0) {
      int viewPtr = MemMapFile.mapViewOfFile(mapFilePtr,
	  MemMapFile.FILE_MAP_READ, 0, 0, 0);
      if(viewPtr != 0) {
	String content = MemMapFile.readFromMem(viewPtr);
	MemMapFile.unmapViewOfFile(viewPtr);
      }
      MemMapFile.closeHandle(mapFilePtr);
    }
  }
}



/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: Copyright (c) 2002</p>
 * <p>Company: </p>
 * @author Stanley  Wang
 * @version 1.0
 */
/*
interface MemMapFileObserver {
  public void onDataReady();
}
/**
 * <p>Title: </p>
 * <p>Description: </p>
 * <p>Copyright: Copyright (c) 2002</p>
 * <p>Company: </p>
 * @author Stanley Wang
 * @version 1.0
 */




