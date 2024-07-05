// DESCRIPTION:   
//                 
//                
// AUTHOR:        Brian Ashcroft, Ashcroft@physics.leidenuniv.nl, 05/27/2009
//
// COPYRIGHT:     Brian Ashcroft
// LICENSE:       This file is distributed under the  MIT license.
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
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using CWrapper;


namespace CoreDevices
{
    delegate void FileCreatedEvent(string Filename);
    [Guid("1514adf6-7cb1-0028-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class AcquisitionEngine:IAquisitionEngineCom  
    {
        private event FileCreatedEvent  onFileCreated;
        private EasyCore ECore=null;
        private Devices.Camera  AcqCamera =null;
        private ChannelGroup ActiveGroup = null;
        private StopWatch ActiveStopWatch=null ;
        private bool Playing = false;
        public AcquisitionEngine(EasyCore eCore)
        {
            ECore = eCore;
        }
        /// <summary>
        /// Method for COM and other systems that do not support constructor arguments
        /// </summary>
        /// <param name="eCore"></param>
        public void SetCore(EasyCore eCore)
        {
            ECore = eCore;
        }

        private bool _PauseThreads = false;
        public bool PauseThreads
        {
            set { _PauseThreads = value; }
            get { return _PauseThreads; }
        }
        
        private Thread FocusThread = null;
        private int NumFrames = -1;
        /// <summary>
        /// This will acquire a series of images using a channelgroup to determine the acquisition
        /// </summary>
        /// <param name="ActiveGroup">Aquisition instructions</param>
        /// <param name="CameraDevice">Desired camera</param>
        /// <param name="TimePerFrameMS">Time from start of image to start of next image</param>
        /// <param name="Save">optional to save raw image immediately instead of through viewer</param>
        /// <param name="SavePath">Save Folder</param>
        /// <param name="Savefilename">Filename</param>
        /// <param name="NumFrames">Number of images to aquire</param>
        public void RunChannelAcquisition(ChannelGroup ActiveGroup, string CameraDevice, long TimePerFrameMS, bool Save,string SavePath,string Savefilename,int NumFrames)
        {
            this.NumFrames = NumFrames;
            this.ActiveGroup = ActiveGroup;
            AcqCamera = (Devices.Camera)ECore.GetDevice(CameraDevice);
            Configuration StartingState = null;
            if (ActiveGroup != null)
            {
                StartingState = ECore.Core.getSystemState();
                ActiveGroup.RegisterGroupWithCore();
                ActiveGroup.RunStartupChannel();
            }
            long QuietExposureTimeMS = (long)Math.Round(AcqCamera.GetExposure() + 2);
            
            if (FocusThread != null) // if currently there is no capture thread running
            {
                Playing = false;
                Thread.Sleep(400);
                FocusThread.Abort();
                FocusThread=null;

            }
            Playing = true;
            if (AcqCamera  == null)
            {
                throw (new Exception("You must initialize the camera before you start this procedure"));

            }

         
            FocusThread = new Thread(  delegate()
                {
                    CoreImage[] Images=null;
                    if (ActiveGroup != null )
                        Images=new CoreImage[ActiveGroup.Channels.Count ];
                    int FileNumber=0;
                    //StopWatch sq = new StopWatch();
                   // sq.StartStopWatch();
                    while (Playing && (NumFrames==-1 || NumFrames >0     ) )
                    {
                        // try
                        {
                            //System.Diagnostics.Debug.Print(NumFrames.ToString());
                            while (_PauseThreads != false)
                            { }
                            if (ActiveGroup != null && ActiveGroup.CombineChannels == true)
                            {
                                if (!ECore.Core.deviceBusy(AcqCamera.deviceName))
                                {
                                   // MessageBox.Show(ActiveGroup.Channels.Count.ToString());
                                    for (int i=0;i<ActiveGroup.Channels.Count;i++)
                                    {
                                        ActiveGroup.RunChannel(i);
                                        Images[i] = AcqCamera.SnapOneFrame(false);
                                        //if (Images[i] == null) MessageBox.Show(i.ToString());
                                    }
                                    for (int i = 0; i < ActiveGroup.Channels.Count; i++)
                                    {
                                        if (Images[i] != null)
                                        {
                                            if (ActiveGroup.Channels[i].ShowFalseColor==true  )
                                                Images[i].FalseColor = ActiveGroup.Channels[i].ChannelColor;
                                            //AcqCamera.DoHistogram(Images[i]);

                                           

                                        }
                                    }
                                   // long LastTime = sq.GetStopWatchInterval();
                                    ECore.UpdatePaintSurface(Images);
                                   // System.Diagnostics.Debug.Print((sq.GetStopWatchInterval() - LastTime).ToString());
                                    if (Save )
                                    {
                                        FileNumber++;
                                        for (int i = 0; i < ActiveGroup.Channels.Count; i++)
                                        {
                                            if (Images[i] != null)
                                            {
                                                
                                                Images[i].Save(SavePath + "\\" + Savefilename + "_Channel_" + i.ToString() +  "_" + FileNumber.ToString() + ".tif",false );
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                
                                if (!ECore.Core.deviceBusy(AcqCamera.deviceName))
                                {

                                    CoreImage b = AcqCamera.SnapOneFrame(false);

                                    if (ActiveGroup != null)
                                    {
                                        ActiveGroup.RunNextChannel();
                                    }

                                    if (b != null)
                                    {

                                        AcqCamera.DoHistogram(b);

                                        lock (b)
                                        {
                                            ECore.UpdatePaintSurface(b);

                                        }
                                        if (Save)
                                        {
                                            FileNumber++;
                                           
                                            if (b != null)
                                            {

                                                b.Save(SavePath + "\\" + Path.GetFileNameWithoutExtension(  Savefilename) + "_" + FileNumber.ToString() + ".tif", false);
                                            }
                                           
                                        }

                                    }
                                }
                            }
                        }
                        // catch (Exception ex)
                        {
                            //     Ecore.LogMessage("Startfocusmovie: error--" + ex.Message);
                        }
                        if (NumFrames != -1)
                            --NumFrames;
                        Thread.Sleep((int)TimePerFrameMS );
                    }
                    ECore.LogMessage("Finished Channel Acquisition");
                } );

            FocusThread.Start();
            ECore.AddThreadToPool(FocusThread);
        }

        /// <summary>
        /// This will acquire a stack using a channelgroup to determine the acquisition
        /// </summary>
        /// <param name="ActiveGroup">Aquisition instructions</param>
        /// <param name="CameraDevice">Desired Camera</param>
        /// <param name="FocusDevice">Stage or piezo</param>
        /// <param name="TimePerFrameMS"> Time from start of image to start of next image</param>
        /// <param name="FirstStackThenChannel">Determines if a stack is performed and then a channel.  Or the channelgroup and then the next slice</param>
        /// <param name="DisplaySlices">Report each slice to viewer</param>
        /// <param name="CurrentSliceIsMiddle">If true then currentposition is middle Z of stack.  If false current position is bottom of z stack</param>
        /// <param name="NumSlices"></param>
        /// <param name="SliceDistance">Distance between slices in microns</param>
        /// <param name="Save">optional to save raw image immediately instead of through viewer</param>
        /// <param name="SavePath">Save Folder</param>
        /// <param name="Savefilename">Filename</param>
        public void RunZStackAcquisition(ChannelGroup ActiveGroup, string CameraDevice, string FocusDevice, long TimePerFrameMS, bool FirstStackThenChannel, bool DisplaySlices, bool CurrentSliceIsMiddle, int NumSlices, double SliceDistance, bool Save, string SavePath, string Savefilename)
        {
           
            double Z0 = 0;
            this.ActiveGroup = ActiveGroup;
            AcqCamera = (Devices.Camera)ECore.GetDevice(CameraDevice);
            Configuration StartingState = null;
            if (ActiveGroup != null)
            {
                StartingState = ECore.Core.getSystemState();
                ActiveGroup.RegisterGroupWithCore();
                ActiveGroup.RunStartupChannel();
            }

            Devices.ZStage zStage = (Devices.ZStage)ECore.GetDevice(FocusDevice);

            ECore.MMCamera.StopFocusMovie();
            if (zStage != null) Z0 = zStage.CurrentPosition();
            long NumFrames =NumSlices ;

            double  StartSlice;
            if (CurrentSliceIsMiddle ==true )
                StartSlice =Z0-NumSlices *SliceDistance /2;
            else 
                StartSlice =Z0;

            if (FocusThread != null) // if currently there is no capture thread running
            {
                Playing = false;
                Thread.Sleep(400);
                FocusThread.Abort();
                FocusThread = null;

            }
            Playing = true;
            FocusThread = new Thread(
                  delegate()
                  {
                      int FileNumber = 0;
                      int NumRuns = 1;
                      while (Playing && NumRuns>0 )
                      {
                          while (_PauseThreads != false)
                          { }
                          NumRuns--;
                          CoreImage[] Images = new CoreImage[NumSlices];
                          for (int z = 0; z < NumSlices; z++)
                          {
                              CoreImage b = AcqCamera.SnapOneFrame(false);

                              if (ActiveGroup != null && FirstStackThenChannel == false)
                              {
                                  //todo:this does not help.  Should run through all the channels before going to the next one
                                  ActiveGroup.RunNextChannel();
                              }
                              if (zStage != null) zStage.SetPositionAbsolute(StartSlice + z * SliceDistance);

                              if (b != null && DisplaySlices == true)
                              {

                                  AcqCamera.DoHistogram(b);

                                  lock (b)
                                  {
                                      ECore.UpdatePaintSurface(b);

                                  }

                              }
                              Images[z] = b;

                          }
                          if (Save)
                          {
                              FileNumber++;
                              for (int i = 0; i < NumSlices ; i++)
                              {
                                  if (Images[i] != null)
                                  {

                                      Images[i].Save(SavePath + "\\" + Savefilename + "_Stack_" + FileNumber.ToString() + "_Slice_" + i.ToString() + ".tif", false);
                                  }
                              }
                          }
                          if (ActiveGroup != null && FirstStackThenChannel == true )
                          {
                              ActiveGroup.RunNextChannel();
                          }

                      }
                      if (zStage != null) zStage.SetPositionAbsolute(Z0);

                  });

            FocusThread.Start();
            ECore.AddThreadToPool(FocusThread);
                
            
        }
        
        /// <summary>
        /// Stops all active aquisition processes
        /// </summary>
        public void StopAcquisition()
        {
            Playing = false;
            Thread.Sleep(400);
            if (ActiveStopWatch !=null)
                ActiveStopWatch.StopStopWatch();
            if (FocusThread != null)
            {
                FocusThread.Abort();
                FocusThread = null;
            }
            
        }
        private long LastTime=0;

        /// <summary>
        /// This will acquire images using the internal system timer to take the images.  Not appropriate for fast images
        /// </summary>
        /// <param name="ActiveGroup"></param>
        public void RunChannelAcquisitionTimered(ChannelGroup ActiveGroup, string CameraDevice, long TimePerFrameMS)
        {
            this.ActiveGroup = ActiveGroup;
            AcqCamera = (Devices.Camera)ECore.GetDevice(CameraDevice);
            Configuration StartingState = null;
            if (ActiveGroup != null)
            {
                StartingState = ECore.Core.getSystemState();
                ActiveGroup.RegisterGroupWithCore();
                ActiveGroup.RunStartupChannel();
            }
            Playing = true;
            ActiveStopWatch = new StopWatch();
            //sw.StartTimeTicks(FrameRateMS,AcqCamera.
            ActiveStopWatch.StopWatchTick += new StopWatchTickEvent(sw_StopWatchTick);
            long QuietExposureTimeMS = (long)Math.Round(AcqCamera.GetExposure() + 2);
            ActiveStopWatch.StartTimeTicks(ECore, TimePerFrameMS, QuietExposureTimeMS);
        }
     
        void sw_StopWatchTick(object sender, long millisec, int TickIndex)
        {
            //Captures a image snap , runs the next channel on the internal system timer.
            System.Diagnostics.Debug.Print((millisec - LastTime).ToString());
            LastTime = millisec;
            while (_PauseThreads != false)
            { }
            if (TickIndex == 0)
            {
                
                
               AcqCamera.SnapOneFrame(true);
            }
            else
            {
                if (ActiveGroup != null)
                {
                    ActiveGroup.RunNextChannel();
                }
            }
            System.Windows.Forms.Application.DoEvents();
        }

    }

    [Guid("5A88092E-69FF-0028-AD8D-8FA83E550F20")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAquisitionEngineCom
    {
        void SetCore(EasyCore eCore);
        void RunChannelAcquisition(ChannelGroup ActiveGroup, string CameraDevice, long TimePerFrameMS, bool Save, string SavePath, string Savefilename, int NumFrames);
        void RunChannelAcquisitionTimered(ChannelGroup ActiveGroup, string CameraDevice, long TimePerFrameMS);
        void RunZStackAcquisition(ChannelGroup ActiveGroup, string CameraDevice, string FocusDevice, long TimePerFrameMS, bool FirstStackThenChannel, bool DisplaySlices, bool CurrentSliceIsMiddle, int NumSlices, double SliceDistance, bool Save, string SavePath, string Savefilename);
        void StopAcquisition();
    }
}
