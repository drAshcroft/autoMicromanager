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
using System.Text;
using System.Threading;
using System.Windows.Forms ;
using System.Runtime.InteropServices;

namespace CoreDevices.Devices
{
    /// <summary>
    /// This class is used for cameras that take an image the return the image.  This is a normal camera
    /// </summary>
    [ProgId("Micromanager_net.NormalCamera")]
    [Guid("1514adf6-7cb1-0003-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class NormalCamera : Camera, ICameraCom
    {

        
        public NormalCamera():base()
        {
           
        }

        public NormalCamera(EasyCore ECore, string DeviceLabel, string LibraryName, string DeviceAdapter)
            :base(ECore,DeviceLabel,LibraryName,DeviceAdapter )
        {
        }
        public NormalCamera(EasyCore ECore, string DeviceLabel):base(ECore,DeviceLabel )
        {
            
        }
     
        /// <summary>
        /// Just pulls the image from micromanager
        /// </summary>
        /// <returns></returns>
        protected override  CoreImage SnapFrame()
        {

            try
            {
                // Clear out all the property change requests before taking the image
                
                ClearPropBuffer();
                
                DeviceBusy = true;
                core.snapImage();
               
                // Seems to reduce the number of crashes to make a little pause here
                Thread.Sleep((int)5);
               
                CoreImage b = core.getImage();
                DeviceBusy = false;
                return (b);
            }
            catch (Exception ex )
            {
                Ecore.LogErrorMessage("Camera is malfunctioning \n" + ex.Message,1);
                return (null);
            }
        }
      
        /// <summary>
        /// Runs the camera in streaming mode
        /// </summary>
        /// <param name="NumFrames"></param>
        /// <param name="Interval"></param>
        /// <param name="DisplayImage"></param>
        public override void StartSequence(int NumFrames, double Interval, bool DisplayImage)
        {
            DisplaySequenceFrames = DisplayImage;
            if (FocusThread != null)
                if (FocusThread.IsAlive)
                {
                    FocusThread.Suspend();
                    PausedFocus = true;
                }
            const int memoryFootprintMB = 100;
            core.setCircularBufferMemoryFootprint(memoryFootprintMB);
            string camera = core.getCameraDevice();
            //core.setExposure(200.0);
            double exposure = core.getExposure();
            core.startSequenceAcquisition(NumFrames, Interval,true  );
        }

        /// <summary>
        /// Waits for the camera to pull a new frame
        /// </summary>
        /// <returns></returns>
        public override CoreImage GetBlockingSequenceFrame(double MaxWaitTime)
        {
            CoreImage c=null;
            StopWatch sw = new StopWatch();
            sw.StartStopWatch();
            while (c == null && sw.GetStopWatchValue() <MaxWaitTime )
            {
                try
                {
                    c = core.popNextImage();
                }
                catch { }
            }
            sw.StopStopWatch();
            if (c != null)
            {
                LastImageHolder = c;
                if (DisplaySequenceFrames)
                    Ecore.UpdatePaintSurface(c);
            }
            return c;// getLastImage();
        }

        /// <summary>
        /// Returns a frame if the camera has produces one, otherwise returns null
        /// </summary>
        /// <returns></returns>
        public override CoreImage GetSequenceFrame()
        {
           
            CoreImage c = core.getLastImage();
            if (c != null)
            {
                LastImageHolder = c;
                if (DisplaySequenceFrames)
                    Ecore.UpdatePaintSurface(c);
            }
            return c;// getLastImage();
        }

        /// <summary>
        /// Forces the camera out of streaming mode
        /// </summary>
        public override void EndSequence()
        {
            try
            {
                Ecore.Core.stopSequenceAcquisition();
            }
            catch
            { }
                if (PausedFocus)
                {
                    FocusThread.Resume();
                    PausedFocus = false;
                }
        }

      
    }


}


