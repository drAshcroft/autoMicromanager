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
using System.Collections;
using System.Data;
using System.Xml;
using System.Web;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Net;

namespace ImageJPluginFactory
{
	namespace FileMemoryMap
	{
		public class Win32MapApi
		{
		}	
		public class DevCache  
		{
            Socket m_socClient;
            
			private System.Threading.Mutex oMutex = new System.Threading.Mutex(false,"MmfUpdater");
            Socket Listener;
            public void ListenForImageJ()
            {
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                String szIPSelected = "127.0.0.1";
                int alPort = 56295;

                System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(szIPSelected);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, alPort);
                Listener.Bind(remoteEndPoint);
                Listener.Listen(1);
                
               // Listener.Connect(remoteEndPoint);
            }
            public void WaitForImageJ()
            {
                Listener.Accept();
            }
            public void ConnectToImageJ()
            {
                try
                {
                    if (m_socClient==null ||   !m_socClient.Connected)
                    {
                        //create a new client socket ...
                        m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        String szIPSelected = "127.0.0.1";
                        int alPort = 56294;

                        System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(szIPSelected);
                        System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, alPort);
                       
                        m_socClient.Connect(remoteEndPoint);
                    }
                   

                }
                catch (SocketException se)
                {
                    
                    m_socClient.Shutdown(SocketShutdown.Both );
                    throw (new Exception( se.Message.ToString()));
                }
            }
            public void SendCommand(string Command)
            {
                String szData = Command  + "\n";
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(szData);
                m_socClient.Send(byData);

            }

            
            public string GetReturn()
            {
                try
                {
                   
                    byte[] buffer = new byte[1024];
                    int iRx = m_socClient.Receive(buffer);
                    char[] chars = new char[iRx];

                    System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                    int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                    System.String szData = new System.String(chars);
                    return  szData;
                }
                catch (SocketException se)
                {
                    throw new Exception(se.Message);
                }


            }
			public  DevCache(string GenFilename): base()
			{
                ObjectNamesMMF = GenFilename;
			}

            private  string ObjectNamesMMF = "Mem_Map_File";


            private void WriteBytesToMMF(byte[] InArray, string arrayName, int BPP,int NumChannels,int Width,int Height)
            {
                oMutex.WaitOne();
                
               /* byte [] L=System.BitConverter.GetBytes((int)InArray.Length);
                byte[] header=new byte[4+L.Length ];
                header[0]=(byte)BPP;
                header[1]=(byte)NumChannels ;
                header[2] = (byte)Width;
                header[3] = (byte)Height;
                for (int i=0;i<L.Length ;i++) header[4+i]=L[ i ];
                */
                MemoryMappedFile map = new MemoryMappedFile(ObjectNamesMMF + arrayName, MapProtection.PageReadWrite, MapAccess.FileMapAllAccess, InArray.Length  + 100 , ObjectNamesMMF + arrayName);				
                MapViewStream stream = map.MapView(MapAccess.FileMapAllAccess, 0, (int)InArray.Length +  100 , "");
                stream.Position = 0;

                byte[] header = new byte[2];
                header[0] = (byte)BPP;
                header[1] = (byte)NumChannels;
                stream.Write(header, 0, header.Length);

                byte[] L = System.BitConverter.GetBytes((int)Width);
                stream.Write(L, 0, L.Length);

                L = System.BitConverter.GetBytes((int)Height);
                stream.Write(L, 0, L.Length);

                L = System.BitConverter.GetBytes((int)InArray.Length );
                stream.Write(L, 0, L.Length);

                stream.Write(InArray, 0 , (int)InArray.Length);
                stream.Flush();
                stream.Close();
                oMutex.ReleaseMutex();
            }

            private void WriteBytesToMMF(IntPtr  InArray, int stride,int Size, string  arrayName, int BPP, int NumChannels,int Width,int Height)
            {
                oMutex.WaitOne();

               /* byte[] L = System.BitConverter.GetBytes((int)Size);
                byte[] header = new byte[4 + L.Length];
                header[0] = (byte)BPP;
                header[1] = (byte)NumChannels;
                header[2] = (byte)Width;
                header[3] = (byte)Height;
                for (int i = 0; i < L.Length; i++) header[4 + i] = L[i];
                */
                
                
                MemoryMappedFile map = new MemoryMappedFile(ObjectNamesMMF + arrayName, MapProtection.PageReadWrite, MapAccess.FileMapAllAccess, Size  +100, ObjectNamesMMF + arrayName);
                MapViewStream stream = map.MapView(MapAccess.FileMapAllAccess, 0, (int)Size  +  100, "");
                if (stream == null)
                {
                    map.Close();
                    map.Dispose();
                    map = new MemoryMappedFile(ObjectNamesMMF + arrayName, MapProtection.PageReadWrite, MapAccess.FileMapAllAccess, Size + 100, ObjectNamesMMF + arrayName);
                    stream = map.MapView(MapAccess.FileMapAllAccess, 0, (int)Size + 100, "");

                }
                stream.Position = 0;

                byte[] header = new byte[2];
                header[0] = (byte)BPP;
                header[1] = (byte)NumChannels;
                stream.Write(header, 0, header.Length);

                byte[] L = System.BitConverter.GetBytes((int)Width);
                stream.Write(L, 0, L.Length);

                L = System.BitConverter.GetBytes((int)Height );
                stream.Write(L, 0, L.Length);

                L = System.BitConverter.GetBytes((int)Size);
                stream.Write(L, 0, L.Length);
                /*byte[] b=new byte[Size ];
                for (int i = 0; i < b.Length; i++)
                    b[i] = 2;
                stream.Write(b, 0, (int)Size);*/
                stream.Write(InArray, 0, (int)Size );
                stream.Flush();
                stream.Close();
                oMutex.ReleaseMutex();
            }

			/// <summary>
			/// create a MMF and serialize the object in to.
			/// </summary>
			/// <param name="InObject"></param>
			/// <param name="obectName"></param>
			/// <param name="ObjectSize"></param>
			private void WriteObjectToMMF(object InObject, string objectName,int ObjectSize)
			{
				MemoryStream ms = new MemoryStream ();
				BinaryFormatter bf= new BinaryFormatter();

				bf.Serialize (ms,InObject);
				
				oMutex.WaitOne ();
                MemoryMappedFile map = new MemoryMappedFile(ObjectNamesMMF + objectName, MapProtection.PageReadWrite, MapAccess.FileMapAllAccess, ms.Length, objectName);				
				MapViewStream stream = map.MapView(MapAccess.FileMapAllAccess, 0,(int) ms.Length,"" );
				stream.Write(ms.GetBuffer() , 0,(int)ms.Length);
				stream.Flush();
				stream.Close();	
				oMutex.ReleaseMutex();
			}

            static public void CloseMap(string Filename, string objName)
            {
                   MemoryMappedFile map=new MemoryMappedFile();
                   map.OpenEx(Filename  + objName, MapProtection.PageReadWrite, Filename , MapAccess.FileMapAllAccess);
                   map.Close();
                   map.Dispose();

            }

            public void AddImage(string objName, int BPP, int NumChannels, bool UpdateDomain)
            {
                byte[] buffer = new byte[512 * 512];
                int k=0;
                for (int i=0;i<512;i++)
                    for (int j = 0; j < 512; j++)
                    {
                        buffer [k]=(byte)((i+j)/2);
                        k++;
                    }
                AddImage(objName, buffer, BPP, NumChannels, UpdateDomain,512,512);
            }


            public void AddImage(string objName, System.Drawing.Image nImage, int BPP, int NumChannels, bool UpdateDomain)
            {
                Bitmap Image = (Bitmap)nImage;
                MemoryMappedFile map = new MemoryMappedFile();
                System.IntPtr oAtom = System.IntPtr.Zero;

                
                BitmapData outdata = Image.LockBits(new Rectangle(0, 0,Image.Width, Image.Height), ImageLockMode.ReadWrite, Image.PixelFormat );

                try
                {

                    map.OpenEx(ObjectNamesMMF + objName, MapProtection.PageReadWrite, ObjectNamesMMF, MapAccess.FileMapAllAccess);
                    try
                    {
                        //WriteBytesToMMF(outdata.Scan0 ,objName,
                        WriteBytesToMMF(outdata.Scan0,outdata.Stride,  outdata.Stride*outdata.Height   , "", BPP, NumChannels,nImage.Width,nImage.Height );
                    }
                    catch (Exception e)
                    {
                        map.Close();
                        map.Dispose();
                    }

                }
                catch (Exception e)
                {
                    throw new Exception("Cannot Open File " + objName, e);
                }
                finally
                {
                    Win32MapApis.GlobalDeleteAtom(oAtom);

                }

                Image.UnlockBits(outdata);

            }


            public void AddImage(string objName, byte[] ImageInfo,int BPP, int NumChannels, bool UpdateDomain,int Width,int Height)
            {
                MemoryMappedFile map = new MemoryMappedFile();
                System.IntPtr oAtom = System.IntPtr.Zero;
                

                try
                {

                    map.OpenEx(ObjectNamesMMF + objName, MapProtection.PageReadWrite, ObjectNamesMMF, MapAccess.FileMapAllAccess);
                    try
                    {
                        WriteBytesToMMF(ImageInfo, "", BPP, NumChannels,Width,Height );
                    }
                    catch (Exception e)
                    {
                        map.Close();
                        map.Dispose();
                        MemoryMappedFile oMMf = new MemoryMappedFile();
                        if (oMMf.Open(MapAccess.FileMapAllAccess, ObjectNamesMMF + objName))
                        {
                            oMMf.Close();
                            oMMf.Dispose();
                        }
                        
                    }

                }
                catch (Exception e)
                {
                    throw new Exception("Cannot Open File " + objName, e);
                }
                finally
                {
                    Win32MapApis.GlobalDeleteAtom(oAtom);
                   
                }
            }

            public void AddImage(string objName, IntPtr  ImageInfo,int Stride, int lSize, int BPP, int NumChannels, bool UpdateDomain, int Width, int Height)
            {
                MemoryMappedFile map = new MemoryMappedFile();
                System.IntPtr oAtom = System.IntPtr.Zero;


                try
                {

                    map.OpenEx(ObjectNamesMMF + objName, MapProtection.PageReadWrite, ObjectNamesMMF, MapAccess.FileMapAllAccess);
                    try
                    {
                        WriteBytesToMMF(ImageInfo,Stride,lSize, "", BPP, NumChannels, Width, Height);
                    }
                    catch (Exception e)
                    {
                        map.Close();
                        map.Dispose();
                        MemoryMappedFile oMMf = new MemoryMappedFile();
                        if (oMMf.Open(MapAccess.FileMapAllAccess, ObjectNamesMMF + objName))
                        {
                            oMMf.Close();
                            oMMf.Dispose();
                        }

                    }

                }
                catch (Exception e)
                {
                    throw new Exception("Cannot Open File " + objName, e);
                }
                finally
                {
                    Win32MapApis.GlobalDeleteAtom(oAtom);

                }
            }
			/// <summary>
			/// this function will 
			/// 1) open exsisting (if not create) MMF that hold all the MMf names for  each object
			/// 2) look if aname allready exist
			/// 3) if exist 
			///			-Delete the MMF
			///			-create new MMF
			///			-Enter the onject into
			///		if not
			///			-create new MMF
			///			-Enter the onject into
			///			-enter the new name and MMF name into MMF of object and MMF name
			/// </summary>
			/// <param name="objName"></param>
			/// <param name="inObject"></param>
			public void AddObject(string objName, object inObject, bool UpdateDomain)
			{
				MemoryMappedFile map = new MemoryMappedFile();
				System.Collections.Hashtable oFilesMap;
				System.IntPtr oAtom = System.IntPtr.Zero;
				string strIps ="";
				
				try
				{
					
					if (! map.OpenEx(ObjectNamesMMF + objName ,MapProtection.PageReadWrite ,ObjectNamesMMF,MapAccess.FileMapAllAccess))
					{
						//Create MMF for the object and serialize it
						WriteObjectToMMF(inObject,objName,0);
						//create hashtable
						oFilesMap = new System.Collections.Hashtable ();					
						//add object name and mmf name to hash
						oFilesMap.Add (objName,objName);
						//create main MMF
						WriteObjectToMMF(oFilesMap,ObjectNamesMMF,0);
					}
					else
					{
						BinaryFormatter bf = new BinaryFormatter();
						Stream mmfStream = map.MapView(MapAccess.FileMapRead, 0, 0,"");
						mmfStream.Position = 0;
						oFilesMap = bf.Deserialize(mmfStream) as Hashtable; 
						long StartPosition = mmfStream.Position;

						if (oFilesMap.ContainsKey(objName))
						{
							//name exist so we need to 
							//	open the MMF of the existing and update it
							MemoryMappedFile MemberMap = new MemoryMappedFile();
							oMutex.WaitOne ();
							MemberMap.OpenEx(objName + objName ,MapProtection.PageReadWrite,objName,MapAccess.FileMapAllAccess);   //(MapAccess.FileMapAllAccess ,objName);						
							MapViewStream stream = MemberMap.MapView(MapAccess.FileMapAllAccess, 0,(int) 0,"" );
							bf = new BinaryFormatter();
							MemoryStream ms = new MemoryStream ();
							bf.Serialize (ms,inObject);
							stream.Position = 0;
							stream.Write(ms.GetBuffer() , 0,(int)ms.Length);
							stream.Flush();
							stream.Close();	
							oMutex.ReleaseMutex ();
						}
						else
						{
							//name not apear so we nedd to
							//	craete new MMF file and serialize
							WriteObjectToMMF(inObject,objName,0);							
							oMutex.WaitOne ();
							MapViewStream stream = map.MapView(MapAccess.FileMapAllAccess, 0,(int) 0,"" );
							// update the main HashTable
							oFilesMap.Add (objName,objName);
							// serialize new Hash
							bf = new BinaryFormatter();
							MemoryStream ms = new MemoryStream ();
							bf.Serialize (ms,oFilesMap);
							stream.Position = 0;
							stream.Write(ms.GetBuffer() , 0,(int)ms.Length);
							stream.Flush();
							stream.Close();	
							oMutex.ReleaseMutex ();
						}
					}
					
				}
				
				catch (Exception e)
				{					
					throw new Exception("Cannot Open File "+objName,e);
				}
				finally
				{
					Win32MapApis.GlobalDeleteAtom (oAtom );
				}
			}

            public string GetImage(string objName, out byte[] ImageBuf, out int BPP, out int  numChannels,out int Width, out int Height, out int bSize)
            {

                MemoryMappedFile map = new MemoryMappedFile();
                MemoryMappedFile mapOfName = new MemoryMappedFile();
                try
                {
                    oMutex.WaitOne();
                    if (!map.OpenEx(ObjectNamesMMF + objName, MapProtection.PageReadOnly, ObjectNamesMMF, MapAccess.FileMapRead))
                        throw new Exception("No Desc FileFound");
                    BinaryFormatter bf = new BinaryFormatter();
                    Stream mmfStream = map.MapView(MapAccess.FileMapRead, 0, 0, "");
                    mmfStream.Position = 0;

                    byte[] buffer = new byte[20];
                    mmfStream.Read(buffer, 0, (int)20);
                    BPP=  System.BitConverter.ToInt32(buffer,0);
                    numChannels = System.BitConverter.ToInt32(buffer, 4);
                    Width = System.BitConverter.ToInt32(buffer, 8);
                    Height = System.BitConverter.ToInt32(buffer, 12);
                    bSize = System.BitConverter.ToInt32(buffer, 16);

                    
                    ImageBuf = new byte[bSize];
                    mmfStream.Read(ImageBuf, 0, bSize);

                    /*string ss = "";
                    for (int i = 0; i < mmfStream.Length; i++)
                    {
                        ss += (char)buffer[i];
                    }*/

                    mmfStream.Close();
                    mmfStream = null;

                    oMutex.ReleaseMutex();
                    return "";
                }
                catch
                {
                    ImageBuf = null;
                    BPP = 0;
                    Width = 0;
                    Height = 0;
                    numChannels = 0;
                    bSize = 0;

                    oMutex.ReleaseMutex();
                    return "";
                }
            }
            public string GetString(string objName)
            {

				MemoryMappedFile map = new MemoryMappedFile();
				MemoryMappedFile mapOfName = new MemoryMappedFile();
                try
                {
                    oMutex.WaitOne();
                    if (!map.OpenEx(ObjectNamesMMF + objName, MapProtection.PageReadOnly, ObjectNamesMMF, MapAccess.FileMapRead))
                        throw new Exception("No Desc FileFound");
                    BinaryFormatter bf = new BinaryFormatter();
                    Stream mmfStream = map.MapView(MapAccess.FileMapRead, 0, 0, "");
                    mmfStream.Position = 0;
                  
                    byte[] buffer = new byte[mmfStream.Length ];
                    mmfStream.Read(buffer, 0,(int) mmfStream.Length );
                    string ss = "";
                    for (int i = 0; i < mmfStream.Length ; i++)
                    {
                        ss += (char)buffer[i];
                    }

                    mmfStream.Close();
                    mmfStream = null;

                    oMutex.ReleaseMutex();
                    return ss;
                }
                catch 
                {
                    oMutex.ReleaseMutex();
                    return "";
                }
            }
			
			/// <summary>
			/// GetObject return the object from MMF 
			/// </summary>
			/// <param name="objName"></param>
			/// <returns></returns>
			public object GetObject(string objName)
			{
				MemoryMappedFile map = new MemoryMappedFile();
				MemoryMappedFile mapOfName = new MemoryMappedFile();
				System.Collections.Hashtable oFilesMap;
				try
				{				
					oMutex.WaitOne ();
                    if (!map.OpenEx(ObjectNamesMMF + objName , MapProtection.PageReadOnly, ObjectNamesMMF, MapAccess.FileMapRead))
						throw new Exception ("No Desc FileFound");
					BinaryFormatter bf = new BinaryFormatter();
					Stream mmfStream = map.MapView(MapAccess.FileMapRead, 0, 0,"");
					mmfStream.Position = 0;
					//mmfStream.Close ();
					oFilesMap = bf.Deserialize(mmfStream) as Hashtable; 
					long StartPosition = mmfStream.Position;
					if (!oFilesMap.ContainsKey (objName))
						throw new Exception ("No Name Found");
					if(! mapOfName.OpenEx (oFilesMap[objName].ToString()+".NAT",MapProtection.PageReadOnly ,oFilesMap[objName].ToString(),MapAccess.FileMapRead ))
						throw new Exception ("No Name File Found");
					mmfStream.Close(); 
					mmfStream = null;
					

					mmfStream = mapOfName.MapView(MapAccess.FileMapRead, 0, 0,oFilesMap[objName].ToString()+".NAT");
					mmfStream.Position = 0;
				
					//mmfStream.SetLength (80000);
					object oRV = bf.Deserialize(mmfStream) as object; 
					mmfStream.Close ();
					return oRV; 
				}
				catch(Exception e)
				{
					return null;
					//throw new Exception("Cannot create File "+ objName,e);
				}
				finally
				{
					oMutex.ReleaseMutex ();
				}
			}
			public void RemoveObject(string ObjName)
			{
				//get the main mmf table and remove the key
				MemoryMappedFile map = new MemoryMappedFile(); 
				if ( map.OpenEx(ObjectNamesMMF + ObjName ,MapProtection.PageReadWrite ,ObjectNamesMMF,MapAccess.FileMapAllAccess))
				{					
					BinaryFormatter bf = new BinaryFormatter();
					MapViewStream mmfStream = map.MapView(MapAccess.FileMapRead, 0, 0,"");
					mmfStream.Position = 0;
					Hashtable oFilesMap = bf.Deserialize(mmfStream) as Hashtable;
					oFilesMap.Remove(ObjName);										
					mmfStream.Close(); 
					//update the main file
					bf = new BinaryFormatter();
					MemoryStream ms = new MemoryStream ();
					bf.Serialize (ms,oFilesMap);
					MapViewStream stream = map.MapView(MapAccess.FileMapAllAccess, 0,(int) 0 ,"" );															
					stream.Position = 0;
					stream.Write(ms.GetBuffer() , 0,(int)ms.Length);
					stream.Flush();
					stream.Close();	
					//delete the map of the object
					MemoryMappedFile oMMf = new MemoryMappedFile ();
					if( oMMf.Open(MapAccess.FileMapAllAccess,ObjName))
					{
						oMMf.Close();
						oMMf.Dispose();
					}
					if (System.IO.File.Exists(map.GetMMFDir() + ObjName + ".nat"))
						System.IO.File.Delete(map.GetMMFDir() + ObjName + ".nat");
				}
				

			}
		}
	}
}