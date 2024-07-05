/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */


import java.io.*;


import java.net.*;
/**
 *
 * @author Administrator
 */
class SocketReturn {
        private Socket clientSocket;
        private PrintWriter out;

        public SocketReturn()
        {
            try 
            {
		        clientSocket = new Socket("127.0.0.1",56295);
                
                out = new PrintWriter(clientSocket.getOutputStream(), true);
            }
            catch (UnknownHostException e) {
                System.err.println("Don't know about host: taranis.");
                System.exit(1);
            } catch (IOException e) {
                System.err.println("Couldn't get I/O for "
                                   + "the connection to: taranis.");
                System.exit(1);
            }

        }
        
    	public void SendCommand(String Command) {
            out.println(Command);
	}

    
}