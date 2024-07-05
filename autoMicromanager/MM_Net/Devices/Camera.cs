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
using CWrapper;

namespace CoreDevices.Devices
{
    public delegate void ImageUpdatedEvent(System.Drawing.Image image, bool NewImage);
    public delegate void OnHistogramMadeEvent(object sender, int[] Values, double MaxValue, double MinValue, long MaxPossible, CoreImage CI);

    [Serializable]
    /// <summary>
    /// The camera class is not a usual MMdevice class as it must allow for both real and virtual cameras.  This class cannot be used directly as it is a 
    /// abstract class but should instead use NormalCamera or ScanningConfocal
    /// </summary>
   public abstract class Camera : MMDeviceBase
    {
        
        public event OnHistogramMadeEvent OnHistogram;
       
        protected Thread FocusThread;

        private CoreImage SaveFrameBuffer;
        private double ContrastMin = 0;
        private double ContrastMax = 255;
        
        private Mutex mutImage = new Mutex();

        protected CoreImage LastImageHolder = null;

       /// <summary>
       /// The last image that was aquired with this camera
       /// </summary>
        public CoreImage LastImage
        {
            get
            {
                if (LastImageHolder == null)
                {
                    LastImageHolder = SnapOneFrame(false);
                }
                return LastImageHolder;
            }
        }
        public Camera()
        {
            DeviceName = "Camera";

        }

       /// <summary>
       /// Starts camera from library and adapter
       /// </summary>
       /// <param name="ECore"></param>
       /// <param name="DeviceLabel"></param>
       /// <param name="LibraryName"></param>
       /// <param name="DeviceAdapter"></param>
        public Camera(EasyCore ECore, string DeviceLabel, string LibraryName, string DeviceAdapter)
        {
            SetCore(ECore, DeviceLabel, LibraryName, DeviceAdapter);
        }

        public Camera(EasyCore ECore, string DeviceLabel)
        {
            DeviceName = DeviceLabel;
            SetCore(ECore);
        }

        public virtual void ClearROI()
        {
            core.clearROI();
        }
        public virtual  void SetROI(int X1, int Y1, int Width, int Height)
        {
            core.setROI(X1, Y1, Width, Height);
        }
        public virtual void GetROI(out int X1, out int Y1, out int Width, out int Height)
        {
            core.getROI(out X1,out Y1,out Width,out Height);
        }
        
       /// <summary>
       /// Designates this as the main or official camera of system
       /// </summary>
        public override void MakeOffical()
        {
            try
            {
                core.setCameraDevice(DeviceName);
            }
            catch
            {
                Ecore.LogMessage("Could not make camera offical");
            }
            ECore.MMCamera = this;
        }

        /// <summary>
        /// this is used for setting the contrast on movies and other tiems where the contrast does not change much, but speed is essential.
        /// </summary>
        /// <param name="MinPercent"></param>
        /// <param name="MaxPercent"></param>
        public void SetContrast(double MinPercent, double MaxPercent)
        {
            ContrastMax = MaxPercent;
            ContrastMin = MinPercent;
        }
       
        //todo: these would not work if this is not the offical camera.  need to get these from the property 
        //list
       /// <summary>
       /// Gets exposure of offical camera (will be fixed later to be exposure of this camera)
       /// </summary>
       /// <returns></returns>
        public double GetExposure()
        {
           
            return (core.getExposure());
        }

       /// <summary>
       /// Sets exposure of offical camera
       /// </summary>
       /// <param name="exposure"></param>
        public void SetExposure(double exposure)
        {
            if (DeviceBusy || core.deviceBusy(DeviceName))
            {
                SetDeviceProperty("Exposure", exposure.ToString());
            }
            else
            {
                core.setExposure(exposure);
            }

        }

       /// <summary>
       /// Gives the width of the screen in microns
       /// </summary>
        public virtual double ScreenSize_um
        {
            get
            {
                    double pixSize;
                    try
                    {

                        pixSize = core.getPixelSizeUm();
                        if (pixSize == 0) pixSize = 140f / 512f;
                        pixSize = (double)LastImage.Width * pixSize;
                        if (pixSize == 0) pixSize = 140;
                    }
                    catch { pixSize = 1; }
                    return ( pixSize ); 
            }
        }

       /// <summary>
       /// Does the work of snapping one frame
       /// </summary>
       /// <returns></returns>
        protected abstract CoreImage SnapFrame();


        delegate void PaintUpdate(CoreImage c);

       /// <summary>
       /// A convience method that can get an image from SnapFrame, adjust the contrast and then display image
       /// </summary>
       /// <param name="DisplayImage"></param>
       /// <returns></returns>
        public CoreImage SnapOneFrame(bool DisplayImage)
        {
            
            if (FocusThread != null)
                if (FocusThread.IsAlive)
                {
                    FocusThread.Suspend();
                    PausedFocus = true;
                }

            
            CoreImage b = SnapFrame();
            if (b != null)
            {
                if (DisplayImage == true)
                {
                   // Thread ShowThread = new Thread(delegate()
                     //   {
                            if (OnHistogram != null)
                            {

                                int[] hist = new int[255];
                                long MaxValue = 255;
                                long MinValue = 0;
                                long MaxPossible = 255;
                                b.GetHistogram(out hist, out MaxValue, out MinValue, out MaxPossible);
                                OnHistogram(this, hist, (double)MaxValue / (double)MaxPossible, (double)MinValue / (double)MaxPossible, MaxPossible, b);

                            }

                            lock (b)
                            {
                                (new PaintUpdate(Ecore.UpdatePaintSurface)).BeginInvoke(b,null,null);
                                //Ecore.UpdatePaintSurface(b);

                            }
                       // });
                    //ShowThread.Start();
                }
                SaveFrameBuffer = b;


                if (PausedFocus)
                {
                    FocusThread.Resume();
                    PausedFocus = false;
                }
                LastImageHolder = SaveFrameBuffer;
                
                return SaveFrameBuffer;
            }
            else
            {
                
                return null;
            }
        }

       /// <summary>
       /// Begins a independant thread that continously aquires images from the camera and displays them in the viewer.
       /// </summary>
        public void StartFocusMovie()
        {

            if (FocusThread == null) // if currently there is no capture thread running
            {

                if (Ecore == null)
                {
                    throw (new Exception("You must initialize the camera before you start this procedure"));

                }

                CoreImage c1 = SnapFrame();
                Ecore.UpdatePaintSurface(c1);

                FocusThread = new Thread(
                    delegate()
                    {
                       // MessageBox.Show("Starting Focus Movie");
                        StopWatch frameMonitor = new StopWatch();
                        int j = 0;
                        while (true)
                        {
                             try
                            {
                                if (!core.deviceBusy(DeviceName))
                                {
                                    long LastTime = frameMonitor.GetStopWatchInterval();
                                    frameMonitor.StartStopWatch();
                                    CoreImage b = null;
                                    b=SnapFrame();
                                    if (b == null) MessageBox.Show("FM Null Image");
                                    if (b == LastImageHolder) MessageBox.Show("FM last image");
                                    Ecore.LogMessage("FM ImageTry");
                                    long Frametime = frameMonitor.GetStopWatchInterval() - LastTime;
                                   
                                    frameMonitor.StopStopWatch();
                                    if (b != null)
                                    {

                                        long MaxPossible = 255;
                                        if (OnHistogram != null)
                                        {
                                            j++;
                                            if ((j > 0))
                                            {
                                                int[] hist = new int[255];

                                                long MinValue = 0;
                                                long MaxValue = 255;
                                                b.GetHistogram(out hist, out MaxValue, out MinValue, out MaxPossible);
                                                OnHistogram(this, hist, (double)MaxValue / (double)MaxPossible, (double)MinValue / (double)MaxPossible, MaxPossible, b);
                                                j = 0;
                                            }
                                        }

                                        b.MaxContrast = (long)(ContrastMax * MaxPossible);
                                        b.MinContrast = (long)(ContrastMin * MaxPossible);
                                       // MessageBox.Show("FM Sending Image");
                                        if (Ecore == null) MessageBox.Show("No Ecore");
                                        lock (b)
                                        {
                                            Ecore.UpdatePaintSurface(b);
                                         //   MessageBox.Show("FM Out");
                                        }
                                        LastImageHolder = b;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                               // MessageBox.Show("FM" + ex.Message);
                                Ecore.LogMessage("Startfocusmovie: error--" + ex.Message);
                                try { if (ex.InnerException != null) Ecore.LogMessage(ex.InnerException.Message); }  catch { }
                                
                            }
                            
                        }
                        //MessageBox.Show("Ending Focus Movie");
                    }
                   );

                FocusThread.Start();
                Ecore.AddThreadToPool(FocusThread);
            }
            else // if currently capturing
            {
                StopFocusMovie();
                Ecore.StopAcquisition();

            }

        }
       
       /// <summary>
       /// Stops the focus movie, if it has been started
       /// </summary>
       public void StopFocusMovie()
        {

            //StopCamera = true;
            if (FocusThread != null)
            {
                try
                {
                    FocusThread.Abort();
                    FocusThread = null;
                }
                catch { }
            }
            Ecore.StopAcquisition();
        }

        //public CoreImage VideoFrame = null;
        protected  bool PausedFocus = false;
        protected  bool DisplaySequenceFrames = false;
        
       /// <summary>
       /// Starts the camera in streaming mode
       /// </summary>
       /// <param name="NumFrames">Number of desired frames</param>
       /// <param name="Interval">Exposure and rest time</param>
       /// <param name="DisplayImage">Show image when it is aquired</param>
        public abstract void StartSequence(int NumFrames, double Interval, bool DisplayImage);
       
       /// <summary>
       /// Get a frame if there is one, but do not wait for frame
       /// </summary>
       /// <returns></returns>
        public abstract CoreImage GetSequenceFrame();

       /// <summary>
       /// Wait until a frame apears from the camera to get image
       /// </summary>
       /// <returns></returns>
        public abstract CoreImage GetBlockingSequenceFrame(double MaxWaitTime);

       /// <summary>
       /// Forcefully stop sequence
       /// </summary>
        public abstract  void EndSequence();
       
       /// <summary>
       /// Close this device for quiting.  Will try to stop a focus movie and sequence
       /// </summary>
        public override void StopDevice()
        {
            try { StopFocusMovie(); }
            catch { }
            try { EndSequence(); }
            catch { }
        }

       /// <summary>
       /// Send histogram out to whatever LUT monitors are attached
       /// </summary>
       /// <param name="image"></param>
        public void DoHistogram(CoreImage image)
        {

            long MaxPossible = 255;
            if (OnHistogram != null)
            {
               
                    int[] hist = new int[255];

                    long MinValue = 0;
                    long MaxValue = 255;
                    image.GetHistogram(out hist, out MaxValue, out MinValue, out MaxPossible);
                    OnHistogram(this, hist, (double)MaxValue / (double)MaxPossible, (double)MinValue / (double)MaxPossible, MaxPossible, image);
                   
            }

            image.MaxContrast = (long)(ContrastMax * MaxPossible);
            image.MinContrast = (long)(ContrastMin * MaxPossible);


        }
    }

   [Guid("5A88092E-69DF-0003-AD8D-8FA83E550F20")]
   [InterfaceType(ComInterfaceType.InterfaceIsDual)]
   public interface ICameraCom
   {
           
        CoreImage LastImage
        {  get;}

        
        void SetContrast(double MinPercent, double MaxPercent);
        double GetExposure();
        void SetExposure(double exposure);
        double ScreenSize_um
        {
            get;
        }
        CoreImage SnapOneFrame(bool DisplayImage);
        void StartFocusMovie();
        void StopFocusMovie();
        void StartSequence(int NumFrames, double Interval, bool DisplayImage);
        CoreImage GetSequenceFrame();
        CoreImage GetBlockingSequenceFrame(double MaxWaitTime);

        void EndSequence();
        
        void DoHistogram(CoreImage image);

        //MMDeviceBase Stuff
        string[] SaveableProperties
        {
            get;
            set;
        }
        bool PropertyIsSaveable(string PropName);
        string GuiPersistenceProperties
        {
            get;
            set;
        }

        void RunCommand(string MethodName, object[] Pars);


        EasyCore Ecore
        {
            get;
        }
        string deviceName
        {
            get;
            set;
        }
        void MakeOffical();
        void SetCore(EasyCore ECore, string DeviceName, string LibraryName, string DeviceAdapter);
        void SetCore(EasyCore ECore);
        [ComVisible(false)]
        Dictionary<string, PropertyInfo> GetAllDeviceProperties();
        PropertyInfo GetDevicePropertyInfo(string PropName);
        string[] GetDevicePropertyNames();
        [ComVisible(false)]
        Dictionary<string, string> GetDevicePropertyValues();
        string GetDevicePropertyValue(string PropName);
        void SetDeviceProperty(string PropName, string PropValue);
        void GetDevicePropertyInfoDetails
            (string PropName, out string _Value, out bool HasLimits, out double MinValue
            , out double MaxValue, out PropertyType tType, out bool ReadOnly
            , out bool HasAllowedValues, out string[] AllowedValues);
        void BuildPropertyList();
        void SetPropUI(IPropertyList TargetPropertyList);
        void StopDevice();

        
    
   }
}

