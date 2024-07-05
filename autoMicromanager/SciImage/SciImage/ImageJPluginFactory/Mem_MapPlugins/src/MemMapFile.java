
import ij.Prefs;
import java.io.File;

/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */


class MemMapFile {
  public static final int PAGE_READONLY = 0x02;
  public static final int PAGE_READWRITE = 0x04;
  public static final int PAGE_WRITECOPY = 0x08;

  public static final int FILE_MAP_COPY = 0x0001;
  public static final int FILE_MAP_WRITE = 0x0002;
  public static final int FILE_MAP_READ = 0x0004;
  public static final int File_Map_AllAccess  = 0x001f;
  
  static {
        String homeDir = Prefs.getHomeDir();
		String pluginsPath="";
		if (homeDir.endsWith("plugins"))
			pluginsPath = homeDir+Prefs.separator;
		else {
			String property = System.getProperty("plugins.dir");
			if (property!=null && (property.endsWith("/")||property.endsWith("\\")))
				property = property.substring(0, property.length()-1);
			String pluginsDir = property;
			if (pluginsDir==null)
				pluginsDir = homeDir;
			else if (pluginsDir.equals("user.home")) {
				pluginsDir = System.getProperty("user.home");
				if (!(new File(pluginsDir+Prefs.separator+"plugins")).isDirectory())
					pluginsDir = pluginsDir + Prefs.separator + "ImageJ";
				property = null;

			}
			pluginsPath = pluginsDir+Prefs.separator+"plugins"+Prefs.separator;
			if (property!=null&&!(new File(pluginsPath)).isDirectory())
				pluginsPath = pluginsDir + Prefs.separator;

		}

    System.load(pluginsPath + "MemMapLib.dll");
  }

  private MemMapFile() {
  }

  public static native int createFileMapping(int lProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, String name);
  public static native int openFileMapping(int dwDesiredAccess, boolean bInheritHandle, String name);
  public static native int mapViewOfFile(int hFileMappingObj, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, int dwNumberOfBytesToMap);
  public static native boolean unmapViewOfFile(int lpBaseAddress);
  public static native void writeToMem(int lpBaseAddress, String content);
  public static native String readFromMem(int lpBaseAddress);

  public static native long[] ReadImageInfo(int lpBaseAddress);
  public static native Object ReadImage(int lpBaseAddress);
  public static native void  WriteImage(int lpBaseAddress, byte[] Image, int BPP , int numChannels, int Width, int Height);
  public static native boolean closeHandle(int hObject);
  public static native int EasyWriteImageByte(String name, byte[] Image, int lSize, int BPP , int numChannels, int Width, int Height);
  public static native int EasyWriteImageShort(String name,short[] Image, int lSize, int BPP , int numChannels, int Width, int Height);
  public static native int EasyWriteImageInt(String name, int[] Image, int lSize, int BPP , int numChannels, int Width, int Height);
  public static native void broadcast();
}


