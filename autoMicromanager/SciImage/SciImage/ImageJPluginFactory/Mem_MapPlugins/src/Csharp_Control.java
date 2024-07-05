/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */



import ij.*;
import ij.ImagePlus;
import ij.io.*;
import ij.plugin.PlugIn;
import java.awt.Image;
import java.io.*;
import java.lang.String;
import java.net.*;
/**
 *
 * @author Administrator
 */
public class Csharp_Control implements PlugIn{

       ServerSocket serverSocket = null;
       Socket clientSocket;
       BufferedReader is=null;
       PrintWriter out;
       ImagePlus imgP=null;
       InputStreamReader isr=null;
	public Csharp_Control() {

	}

    public void run(String arg) {
		try {
            try
            {
            SocketReturn sr=new SocketReturn();
            sr.SendCommand("Started");
            }
            catch(Exception e)
            {}
			serverSocket = new ServerSocket(56294);
            boolean StayInLoop=true;
			while (StayInLoop) {
				clientSocket = serverSocket.accept();
                boolean isMMF=false;
				try {
                    if (IJ.debugMode) IJ.log("SocketServer: waiting on port "+ImageJ.getPort());

                    InputStream ins= clientSocket.getInputStream();

                    isr = new InputStreamReader(ins);

                    is = new BufferedReader(isr);
					String cmd = is.readLine();
                    if (cmd.startsWith("End"))
                    {
                     StayInLoop=false;   
                    }
                    if (cmd.startsWith("MMF"))
                    {
                        if (imgP==null)
                        {
                            Load_MemoryMappedFile lmm=new Load_MemoryMappedFile();
                            lmm.run("Mem_Map_File.mmf");
                            imgP=lmm;
                        }
                        else
                        {
                            //ImagePlus imgP= ij.IJ.getImage();
                            Image img=Load_MemoryMappedFile.UpdateImage("Mem_Map_File.mmf");
                            imgP.setImage(img);
                        }
                        
                        cmd=cmd.substring(4);
                        isMMF=true ;
                    }
                    else
                    {
                        isMMF=false;
                    }
                    
					if (IJ. debugMode) IJ.log("SocketServer: command: \""+ cmd+"\"");
					if (cmd.startsWith("open "))
						(new Opener()).openAndAddToRecent(cmd.substring(5));
					else if (cmd.startsWith("macro ")) {
						String name = cmd.substring(6);
						String name2 = name;
						arg = null;
						if (name2.endsWith(")")) {
							int index = name2.lastIndexOf("(");
							if (index>0) {
								name = name2.substring(0, index);
								arg = name2.substring(index+1, name2.length()-1);
							}
						}
						IJ.runMacroFile(name, arg);
					} else if (cmd.startsWith("run "))
						IJ.run(cmd.substring(4));
					else if (cmd.startsWith("eval ")) {
						String rtn = IJ.runMacro(cmd.substring(5));
						if (rtn!=null)
							System.out.print(rtn);
					} else if (cmd.startsWith("user.dir "))
						OpenDialog.setDefaultDirectory(cmd.substring(9));
				} catch (Throwable e) {}

                Save_MemoryMappedFile smm=null;
                if (isMMF==true)
                {
                  smm=new Save_MemoryMappedFile() ;
                  smm.run("memMapFileReturn.mmf");
                  out = new PrintWriter(clientSocket.getOutputStream(), true);
                  out.println("FileSaved");
                  String cmdAck="";
                  do
                  {
                    cmdAck= is.readLine();
                  } while (!cmdAck.trim().startsWith("Done"));

                  if (smm!=null)
                    smm.CloseFile();
                }
                
                
				clientSocket.close();
				if (IJ. debugMode) IJ.log("SocketServer: connection closed");
			}
 		} catch (IOException e) {}
	}



}
