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
using System.Xml;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using CWrapper;
namespace CoreDevices
{
    public delegate void ImageProducedEvent(CoreImage cImage);
    public delegate void ClearAllFormsEvent(object sender);
    public delegate CoreImage[] ImageProcessorStep(CoreImage[] cImage);
    
    [ProgId("Micromanager.EasyCore")]
    [Guid("1514adf6-7cb1-0001-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class EasyCore : IEasyCoreCom
    {
        //Reference to the micromanager adapter.
        [NonSerialized]
        CMMCore core;

        public EasyCore()
        {

        }
        public EasyCore(string configFile)
        {
            StartCore(configFile);
        }
        public EasyCore(string configFile, string PluginDirectory)
        {
            StartCore(PluginDirectory, configFile);
        }
        //A set of the most common microscopy devices.  
        [NonSerialized]
        Devices.Camera camera;
        [NonSerialized]
        Devices.XYStage xyStage;
        [NonSerialized]
        Devices.ZStage focusStage;
        [NonSerialized]
        Devices.FilterWheel filterWheel;
        [NonSerialized]
        Devices.FunctionGenerator funcGen;

        //Dictionary<string , UI.GUIDeviceControl> MyDeviceGUIs=new Dictionary<string,CoreDevices.UI.GUIDeviceControl>();

        //A indexed list of all the device GUIs in use.  by device name
        [NonSerialized]
        Dictionary<string, Type > AllPossibleGUIs = new Dictionary<string, Type >();

        //a indexed list of device adapters.  Indexed by device name 
        [NonSerialized]
        Dictionary<string, Devices.MMDeviceBase> MyDevices = new Dictionary<string, Devices.MMDeviceBase>();

        //a general purpose threadpool.  All threads should be added to this pool so the program can shut down correctly.
        [NonSerialized]
        private List<Thread> ThreadPool = new List<Thread>();

        //all channels should be registered here so they can be accessable to whole program.
        [NonSerialized]
        Dictionary<string, ChannelState> AllChannels = new Dictionary<string, ChannelState>();
        //same for channelgroups
        [NonSerialized]
        Dictionary<string, ChannelGroup> AllGroups = new Dictionary<string, ChannelGroup>();

        //Imageprocessors are indexed for ease of use,  they are executed by the order that they have been added.
        [NonSerialized]
        Dictionary<string, ImageProcessorStep> ImageProcessors = new Dictionary<string, ImageProcessorStep>();

        /// <summary>
        /// After the image is aquired, a imageprocessor can handle it before it is displayed or saved.  Imageprocessors are performed in the order that they are added
        /// </summary>
        /// <param name="ProcessorName"></param>
        /// <param name="ProcessorStep"></param>
        public void AddImageProcessor(string ProcessorName, ImageProcessorStep ProcessorStep)
        {
            ImageProcessors.Add(ProcessorName,ProcessorStep);
        }

        /// <summary>
        /// Allows you to change the order of processor steps
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ProcessorName"></param>
        /// <param name="ProcessorStep"></param>
        public void InsertImageProcessor(int index,string ProcessorName, ImageProcessorStep ProcessorStep)
        {
            Dictionary<string, ImageProcessorStep> temp = new Dictionary<string, ImageProcessorStep>();
           
            int i = 0;
            foreach (KeyValuePair<string, ImageProcessorStep> kvp in ImageProcessors)
            {
                temp.Add(kvp.Key, kvp.Value);
                if (i == index)
                    temp.Add(ProcessorName, ProcessorStep);
                i++;
            }
        }

        /// <summary>
        /// Full Image processor list. Manipulation should be done  
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ImageProcessorStep> ImageProcessorsList()
        {
            return ImageProcessors;
        }
        /// <summary>
        /// Gets ordered names of all image processors in list
        /// </summary>
        /// <returns></returns>
        public string[] ImageProcessorNameList()
        {
            string[] Keys = new string[ImageProcessors.Count];
            int i = 0;
            foreach (KeyValuePair<string, ImageProcessorStep> kvp in ImageProcessors)
            {
                Keys[i] = kvp.Key;
                i++;
            }
            return Keys;
        }

        /// <summary>
        /// Removes names processor from list and collapses list
        /// </summary>
        /// <param name="ProcessorName"></param>
        public void RemoveImageProcessor(string ProcessorName)
        {
            ImageProcessors.Remove(ProcessorName);
        }

        
        // This holds the reference to the viewer. 
        [NonSerialized]
        private IPictureView[]  Paint;

        
        public event ImageProducedEvent ImageProduced;
        public event ClearAllFormsEvent RequestAllFormsClose;

        private string _ExperimentFolder;
        private string _PluginFolder;
        /// <summary>
        /// This is the current active save folder
        /// </summary>
        public string ExperimentFolder
        {
            get { return _ExperimentFolder; }
            set { _ExperimentFolder = value; }
        }
        public string PluginFolder
        {
            get { return _PluginFolder; }
            set { _PluginFolder = value; }
        }

        #region ChannelHandling


        public string[] GetAllChannelNames()
        {
            return AllChannels.Keys.ToArray<string>();
        }

        /// <summary>
        /// Adds a channel to the list of possible channels.  Channel should be already initialized and set.
        /// </summary>
        /// <param name="newChannel"></param>
        public void AddChannel(ChannelState newChannel)
        {
            if (AllChannels.ContainsKey(newChannel.ChannelName) == true)
            {
                AllChannels.Remove(newChannel.ChannelName);
                AllChannels.Add(newChannel.ChannelName, newChannel);
            }
            else
                AllChannels.Add(newChannel.ChannelName, newChannel);
        }
        public ChannelState GetChannel(string ChannelName)
        {
            return AllChannels[ChannelName];
        }
        public string[] GetAllGroupNames()
        {
            return AllGroups.Keys.ToArray<string>();
        }

        /// <summary>
        /// This adds a channelgroup to the list of possible channelgroups.  Group should already be initialized
        /// </summary>
        /// <param name="newGroup"></param>
        public void AddGroup(ChannelGroup newGroup)
        {
            if (AllGroups.ContainsKey(newGroup.GroupName) == true)
            {
                AllGroups.Remove(newGroup.GroupName);
                AllGroups.Add(newGroup.GroupName, newGroup);
            }
            else
                AllGroups.Add(newGroup.GroupName, newGroup);
        }
        public ChannelGroup GetChannelGroup(string GroupName)
        {
            return AllGroups[GroupName];
        }
        #endregion

        #region AcquisitionHandling

        [NonSerialized]
            AcquisitionEngine acqEngine =null;

        public AcquisitionEngine CurrentAcquisitionEngine
        {
            get { return acqEngine; }
            set { acqEngine = value; }
        }
            /// <summary>
            /// Primary acquisition method.  This will work for 2D aquisition
            /// </summary>
            /// <param name="ActiveGroup">Channelgroup describing the states of the microscope</param>
            /// <param name="CameraDevice">The camera that is desired for this aquisition</param>
            /// <param name="TimePerFrameMS">The time from the start of one image to start of next image</param>
            /// <param name="Save">Auto save raw image</param>
            /// <param name="path">SAve path (this should be set to the experiment folder)</param>
            /// <param name="filename">Save filename</param>
            /// <param name="NumFrames">Number of images to aquire, -1 means unlimited</param>
            public void RunChannelAcquisition(ChannelGroup ActiveGroup, string CameraDevice, long TimePerFrameMS,bool Save, string path,string filename,int NumFrames)
            {
                acqEngine.RunChannelAcquisition(ActiveGroup, CameraDevice, TimePerFrameMS,Save,path,filename ,NumFrames  );
            }

        /// <summary>
            /// Stack acquisition method. 
        /// </summary>
        /// <param name="ActiveGroup">Image aquisition instructions</param>
        /// <param name="CameraDevice">Active camera</param>
        /// <param name="FocusDevice">desired focus device (stage or piezo or ...)</param>
            /// <param name="TimePerFrameMS">The time from the start of one image to start of next image</param>
        /// <param name="FirstStackThenChannel">Determines order of aquisition.  if true whole stack is performed and then the next channel is performed.  </param>
        /// <param name="DisplaySlices">Report each slice as aquired image</param>
        /// <param name="CurrentSliceIsMiddle">If true, the current position is the middle of the z stack, if false then current slice is bottom of z stack </param>
        /// <param name="NumSlices">Number of slices in z stack</param>
        /// <param name="SliceDistance">Distance between stacks</param>
        /// <param name="Save">autosave stack on aquisition</param>
        /// <param name="path">save folder</param>
        /// <param name="filename">save filename, slice numbers will be appended on filename</param>
            public void RunZStackAcquisition(ChannelGroup ActiveGroup, string CameraDevice, string FocusDevice, long TimePerFrameMS, bool FirstStackThenChannel, bool DisplaySlices, bool CurrentSliceIsMiddle, int NumSlices, double SliceDistance, bool Save, string path, string filename)
            {
                acqEngine.RunZStackAcquisition(ActiveGroup, CameraDevice, FocusDevice, TimePerFrameMS, FirstStackThenChannel, DisplaySlices, CurrentSliceIsMiddle, NumSlices, SliceDistance,Save,path,filename );
            }
            
        /// <summary>
        /// sends a stop signal to all acquisition processes.
        /// </summary>
            public void StopAcquisition()
            {
                try
                {
                    acqEngine.StopAcquisition();
                    foreach (IPictureView ip in Paint)
                        if (ip != null) ip.UpdatesPaused();
                }
                catch { }
            }
        #endregion

        /// <summary>
        /// Allows all threads to be stopped when the program is stopped.
        /// </summary>
        /// <param name="thread"></param>
          
        public void AddThreadToPool(Thread thread)
        {
            ThreadPool.Add(thread);
        }

        #region DevicePluginHandlers
        /// <summary>
        /// This clears the list of all active devices as well as clearing the active devices from micromanager
        /// </summary>
            public void ClearDevices()
            {
                MyDevices.Clear();
                core.unloadAllDevices();
                core.reset();
                camera = null;
                xyStage = null;
                focusStage = null;
                filterWheel = null;
                if (RequestAllFormsClose != null) RequestAllFormsClose(this);
            }
        /// <summary>
        /// Gets a list of all the possible Device names
        /// </summary>
        /// <returns></returns>
            public string[] GetAllLoadedDeviceNames()
            {
                string[] Names = new string[MyDevices.Count];
                int i = 0;
                foreach (KeyValuePair<string, Devices.MMDeviceBase> kvp in MyDevices)
                {
                    Names[i] = kvp.Key;
                    i++;
                }
                return Names;
            }
            public Devices.MMDeviceBase GetDevice(string DeviceName)
            {
                Devices.MMDeviceBase devB=MyDevices[DeviceName.ToLower() ];
                return (devB );
            }
            public List<Devices.MMDeviceBase> GetLoadedDevices()
            {
                List<Devices.MMDeviceBase> Devices = new List<Devices.MMDeviceBase>();
                foreach (KeyValuePair<string, Devices.MMDeviceBase> kvp in MyDevices)
                    Devices.Add(kvp.Value);
                return Devices ;

            }
            public string[] GetLoadedDeviceNames()
            {
                List<string> DeviceNames = new List<String>();
                foreach (KeyValuePair<string, Devices.MMDeviceBase> kvp in MyDevices)
                    DeviceNames.Add (kvp.Key );
                return DeviceNames.ToArray();

            }
            private Dictionary<string, CoreDevices.UI.GUIDeviceControl> ExampleGUITypes = new Dictionary<string, CoreDevices.UI.GUIDeviceControl>();
            public string[] GetPossibleDeviceGUIsFromDeviceType(CWrapper.DeviceType DeviceType)
            {
                string[] PossibleGUIS = GetAllDeviceGUIs();
                List<string> GUIList = new List<string>();

                string TargetDevType = DeviceType.ToString().ToLower();
                foreach (string s in PossibleGUIS)
                {
                    if (ExampleGUITypes.ContainsKey(s) != true)
                    {
                        CoreDevices.UI.GUIDeviceControl gd=GetDeviceGUI(s);
                        ExampleGUITypes.Add(s, gd);
                    }

                    if (ExampleGUITypes[s].DeviceType().ToLower() == TargetDevType)
                        GUIList.Add(s);
                }
                return GUIList.ToArray();

            }
        /// <summary>
        /// Takes the GUI name of a loaded assembly and returns the GUI object
        /// </summary>
        /// <param name="DeviceGUIName"></param>
        /// <returns></returns>
            public CoreDevices.UI.GUIDeviceControl GetDeviceGUI(string DeviceGUIName)
            {
                Type type = AllPossibleGUIs[DeviceGUIName.ToLower()];
                ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                CoreDevices.UI.GUIDeviceControl GUI = (UI.GUIDeviceControl)ci.Invoke(null);

                return (GUI );
            }
            /*
                          private List<UI.GUIDeviceControl> GetDeviceUIAssemblies()
                {

                    List<Assembly> assemblies = new List<Assembly>();

                    // SciImage.Effects.dll
                    //assemblies.Add(Assembly.GetAssembly(typeof(Effect)));

                    // TARGETDIR\Effects\*.dll
                    string homeDir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                    bool dirExists;

                    try
                    {
                        dirExists = Directory.Exists(homeDir);
                    }
                    catch
                    {
                        dirExists = false;
                    }

                    if (dirExists)
                    {
                        string fileSpec = "MMUI*.dll";
                        string[] filePaths = Directory.GetFiles(homeDir, fileSpec);

                        foreach (string filePath in filePaths)
                        {
                            Assembly pluginAssembly = null;

                            try
                            {
                                pluginAssembly = Assembly.LoadFrom(filePath);
                                assemblies.Add(pluginAssembly);
                            }

                            catch (Exception ex)
                            {
                                //Tracing.Ping("Exception while loading " + filePath + ": " + ex.ToString());
                            }
                        }

                    }
                    List<UI.GUIDeviceControl> GUIs = new List<CoreDevices.UI.GUIDeviceControl>();
                    foreach (Assembly assembly in assemblies)
                    {
                        foreach (Type type in assembly.GetTypes())
                        {

                            System.Diagnostics.Debug.Print(type.ToString());
                            try
                            {
                                ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                                CoreDevices.UI.GUIDeviceControlGUI = (UI.GUIDeviceControl)ci.Invoke(null);

                                GUIs.Add(GUI);
                            }

                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    foreach (CoreDevices.UI.GUIDeviceControlgdc in GUIs)
                        try
                        {
                            MyDeviceGUIs.Add(gdc.GetType().ToString().ToLower(), gdc);
                            System.Diagnostics.Debug.Print(gdc.GetType().ToString());
                        }
                        catch
                        { }
                    return GUIs;


                }

             */

          
        /// <summary>
        /// Finds all possible device GUIs and loads descriptions into memory.
        /// </summary>
        /// <param name="PluginPath"></param>
        /// <returns></returns>
            private List<Type> GetDeviceUIAssemblies(string PluginPath)
            {
                //MessageBox.Show(PluginPath);
                List<Assembly> assemblies = new List<Assembly>();

                // SciImage.Effects.dll
                //assemblies.Add(Assembly.GetAssembly(typeof(Effect)));

                // TARGETDIR\Effects\*.dll
                string homeDir = PluginPath;// System.IO.Path.GetDirectoryName(PluginPath);

                
                bool dirExists;
                if (PluginPath ==null || PluginPath.Trim() == "") return null;
                try
                {
                    dirExists = Directory.Exists(homeDir);
                }
                catch
                {
                    dirExists = false;
                }

                if (dirExists)
                {
                    string fileSpec = "MMUI*.dll";
                    string[] filePaths = Directory.GetFiles(homeDir, fileSpec);

                    foreach (string filePath in filePaths)
                    {
                        Assembly pluginAssembly = null;

                        try
                        {
                            pluginAssembly = Assembly.LoadFrom(filePath);
                            assemblies.Add(pluginAssembly);
                        }
                        catch 
                        {
                            //Tracing.Ping("Exception while loading " + filePath + ": " + ex.ToString());
                        }
                    }

                }
                List<Type > GUIs = new List<Type >();
                foreach (Assembly assembly in assemblies)
                {
                    foreach (Type type in assembly.GetTypes())
                    {

                        //System.Diagnostics.Debug.Print(type.ToString());
                        try
                        {
                            if (typeof(UI.GUIDeviceControl).IsAssignableFrom(type))
                                GUIs.Add(type);
                        }
                        catch 
                        {
                        }
                    }
                }
                foreach (Type  gdc in GUIs)
                    try
                    {
                        if ( AllPossibleGUIs.ContainsKey( gdc.ToString().ToLower() ) ==false )
                            AllPossibleGUIs.Add(gdc.ToString().ToLower(), gdc);
                    }
                    catch
                    { }
                return GUIs;


            }
            
            /// <summary>
            /// Returns a list of all the possible device GUI Assemblies
            /// </summary>
            /// <returns></returns>
            public string[] GetAllDeviceGUIs()
            {
                List<string> outStrings = new List<string>();
                foreach (Type t in AllPossibleGUIs.Values )
                {

                    outStrings.Add(t.FullName.ToString());
                }
                return outStrings.ToArray();
            }
        /// <summary>
        /// Returns a list of all the possible Device Adapters from a library on micromanager core
        /// </summary>
        /// <returns></returns>
            public string[] GetAllDeviceAdapters()
            {
                string[] Libs = GetDeviceLibraries;
                List<string> Adapters = new List<string>();
                for (int i = 0; i < Libs.Length; i++)
                {
                    try
                    {
                        Adapters.AddRange(GetDevicesFromLibrary(Libs[i]));
                    }
                    catch { }
                }
                return Adapters.ToArray();
            }
        /// <summary>
        /// Returns a list of all the possible device libraries from Micromanager core
        /// </summary>
            public string[] GetDeviceLibraries
            {
                get
                {
                    StrVector libs = core.getDeviceLibraries(PluginFolder );
                    return (ConvertStrVectortoArray(libs));
                }

            }
        /// <summary>
        /// Returns a list of all the device adapters in a particular library
        /// </summary>
        /// <param name="LibName">Device library, the names can be found from GetDeviceLibraries</param>
        /// <returns></returns>
            public string[] GetDevicesFromLibrary(string LibName)
            {
                StrVector libs = core.getAvailableDevices(LibName);
                return (ConvertStrVectortoArray(libs));
            }
        /// <summary>
        /// A convience method for working with CMMCore or the micromanager adapter
        /// </summary>
        /// <param name="inVector"></param>
        /// <returns></returns>
            public string[] ConvertStrVectortoArray(StrVector inVector)
            {
                string[] temp = new string[inVector.Count];
                inVector.CopyTo(temp);
                return (temp);
            }
            /// <summary>
            /// Starts a device only on the micromanager core.  This does not notify EasyCore of the event.
            /// </summary>
            /// <param name="DeviceName"></param>
            /// <param name="LibName"></param>
            /// <param name="AdapterName"></param>
            public void StartCoreOnlyDevice(string DeviceName, string LibName, string AdapterName)
            {
                core.loadDevice(DeviceName, LibName, AdapterName);
                core.initializeDevice(DeviceName);
            }

        /// <summary>
        /// Registers a device with EasyCore.  This allows the greatest power.
        /// </summary>
        /// <param name="Device"></param>
            public void RegisterDevice(Devices.MMDeviceBase Device)
            {
                try
                {
                    MyDevices.Add(Device.deviceName.ToLower(), Device);
                }
                catch
                {
                    MyDevices.Remove(Device.deviceName.ToLower());
                    MyDevices.Add(Device.deviceName.ToLower(), Device);
                }
            }
           
        #endregion

            #region Viewer
        /// <summary>
        /// A list of the possible viewer plugins
        /// </summary>
        /// <returns></returns>
            public string[] GetViewerNames()
            {
                List<string> outStrings = new List<string>();
                foreach (Type t in ViewerTypes.Values)
                {
                    outStrings.Add(t.FullName.ToString());
                }
                return outStrings.ToArray();
            }
        /// <summary>
        /// Starts a viewer plugin from the type name,  Use GetViewerNames() to get a list of possible plugins
        /// </summary>
        /// <param name="ViewerTypeName"></param>
        /// <returns></returns>
            public IPictureView GetViewerObject(string ViewerTypeName)
            {
                try
                {
                    Type type = ViewerTypes[ViewerTypeName.ToLower()];
                    ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                    IPictureView GUI = (IPictureView)ci.Invoke(null);

                    return (GUI);
                }
                catch
                {
                    return new CoreDevices.NI_Controls.PictureBoard();

                }

            }
            private Dictionary<string, Type> ViewerTypes = new Dictionary<string, Type>();
           /// <summary>
           /// Finds all the viewer plugins
           /// </summary>
           /// <param name="PluginPath"></param>
           /// <returns></returns>
            private List<Type> GetMicroscopyViewerAssemblies(string PluginPath)
            {

                List<Assembly> assemblies = new List<Assembly>();

                // SciImage.Effects.dll
                //assemblies.Add(Assembly.GetAssembly(typeof(Effect)));

                // TARGETDIR\Effects\*.dll
                string homeDir = PluginPath;// System.IO.Path.GetDirectoryName(PluginPath);


                bool dirExists;

                try
                {
                    dirExists = Directory.Exists(homeDir);
                }
                catch
                {
                    dirExists = false;
                }

                if (dirExists)
                {
                    string fileSpec = "*.dll";
                    string[] filePaths = Directory.GetFiles(homeDir, fileSpec);

                    foreach (string filePath in filePaths)
                    {
                        Assembly pluginAssembly = null;

                        try
                        {
                            pluginAssembly = Assembly.LoadFrom(filePath);
                            assemblies.Add(pluginAssembly);
                        }
                        catch
                        {
                            //Tracing.Ping("Exception while loading " + filePath + ": " + ex.ToString());
                        }
                    }

                }
                List<Type> Viewers = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    foreach (Type type in assembly.GetTypes())
                    {

                        //System.Diagnostics.Debug.Print(type.ToString());
                        try
                        {
                            if (typeof(IPictureView).IsAssignableFrom(type))
                                Viewers.Add(type);
                        }
                        catch
                        {
                        }
                    }
                }
                foreach (Type gdc in Viewers)
                    try
                    {
                        ViewerTypes.Add(gdc.ToString().ToLower(), gdc);

                    }
                    catch
                    { }
                return Viewers;


            }
            #endregion

            #region ScreenHandling
        /// <summary>
        /// Sends an image through the imageprocessors and onto the screen
        /// </summary>
        /// <param name="cImage"></param>
            public void UpdatePaintSurface(CoreImage cImage)
            {
                //LogMessage("cImage exists" + (cImage != null).ToString() );
                CoreImage[] ImageStack = new CoreImage[] { cImage };
                //LogMessage("new ImageStackLength " + ImageStack.Length.ToString());
                if (cImage != null)
                {
                    try
                    {
                        foreach (KeyValuePair<string, ImageProcessorStep> kvp in ImageProcessors)
                        {
                            if (kvp.Value != null) ImageStack  = kvp.Value(ImageStack );
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        LogErrorMessage(ex.Message, 3);
                    }
                    //if (Paint != null) Paint.SendImage(cImage._Width, cImage._Height, cImage._Stride
                    //     , cImage._BitsPerPixel, cImage._lSize, cImage.Data, cImage.MaxContrast, cImage.MinContrast);
                    //LogMessage("ImageStackLength " + ImageStack.Length.ToString());
                    if (ImageStack !=null && ImageStack[0] != null)
                    {
                        
                        if (Paint!=null && Paint[0] != null && ImageStack[0]!=null ) Paint[0].SendImage(ImageStack[0]);

                        if (ImageProduced != null) ImageProduced(ImageStack[0]);
                    }
                    //LogMessage("Exit update");
                }
            }
        /// <summary>
        /// sends a stack of images through imageprocessors and onto the screen
        /// </summary>
        /// <param name="cImage"></param>
            public void UpdatePaintSurface( CoreImage[] cImage)
            {
                if (cImage != null)
                {
                    //if (Paint != null) Paint.SendImage(cImage._Width, cImage._Height, cImage._Stride
                    //     , cImage._BitsPerPixel, cImage._lSize, cImage.Data, cImage.MaxContrast, cImage.MinContrast);
                    //for (int i = 0; i < cImage.Length; i++)
                    //{
                        foreach (KeyValuePair<string, ImageProcessorStep> kvp in ImageProcessors)
                        {
                            if (kvp.Value != null) cImage = kvp.Value(cImage);
                        }
                    //}
                    //System.Drawing.Image[] Images = new System.Drawing.Image[cImage.Length];
                    //for (int i = 0; i < cImage.Length; i++)
                     //   Images[i] = cImage[i].ImageARGB;
                        LogMessage(cImage.Length.ToString());
                    if (Paint != null)
                    {
                        for (int i = 0; i < Paint.Length; i++)
                        {
                            if (Paint[i] != null && cImage[i]!=null) Paint[i].SendImage(cImage[i]);
                        }
                    }

                    if (ImageProduced != null) ImageProduced(cImage[0]);
                }
            }
            /// <summary>
            /// forces viewer to save currently displayed image
            /// </summary>
            /// <param name="Filename"></param>
            public void DoForcedSave(string Filename)
            {
                if (Paint != null)
                {
                    for (int i=0;i<Paint.Length ;i++)
                    {
                        if (Paint[i] != null) Paint[i].ForceSave(Path.GetFileNameWithoutExtension( Filename) + i.ToString() + Path.GetExtension(Filename)  );
                    }
                }
            }
        /// <summary>
        /// Default image viewer
        /// </summary>
            public void  PaintSurface( IPictureView[] PaintSurface)
            {
                Paint = PaintSurface;
            }
            public void PaintSurface(IPictureView PaintSurface)
            {
                Paint = new IPictureView[] { PaintSurface };
            }
           

            public double ScreenSize
            {
                get {
                    try
                    {
                        return camera.ScreenSize_um;
                    }
                    catch
                    {
                        return 100d;
                    }
                
                }
                    
            }
        #endregion

        #region DefaultDevices

            public Devices.FunctionGenerator MMFunctionGenerator
            {
                get
                {
                    return funcGen;
                }
                set
                {
                    funcGen = value;
                }
            }
            public Devices.XYStage MMXYStage
            {
                get
                {
                    if (xyStage == null)
                    {
                        string sname = core.getXYStageDevice();
                        if (sname != "")
                        {
                            try
                            {
                                xyStage = (Devices.XYStage)MyDevices[sname]; //new Camera(this, sname);
                            }
                            catch
                            {
                                xyStage = null;//new XYStage(this, sname);
                            }
                        }
                    }
                    return xyStage;
                }
                set
                {
                    xyStage = value;
                }
            }
            public Devices.ZStage MMFocusStage
            {
                get
                {
                    if (focusStage == null)
                    {
                        string sname = core.getFocusDevice();
                        if (sname != "")
                        {
                            try
                            {
                                focusStage = (Devices.ZStage)MyDevices[sname]; //new Camera(this, sname);
                            }
                            catch
                            {
                                focusStage = null;//new ZStage(this, sname);
                            }

                        }
                    }

                    return focusStage;
                }
                set
                {

                    focusStage = value;
                }
            }
            public Devices.Camera MMCamera
            {
                get
                {
                    //MessageBox.Show("GetCamera");
                    if (camera == null)
                    {
                        
                        string sname = core.getCameraDevice();
                       // MessageBox.Show(sname);
                        if (sname != "")
                        {
                            try
                            {
                                camera = (Devices.Camera)MyDevices[sname]; //new Camera(this, sname);
                            }
                            catch
                            {
                                camera = null;// new Camera(this, sname);
                            }
                        }
                    }

                    return camera;
                }
                set
                {
                    
                    camera = value;
                }
            }
            public Devices.FilterWheel MMFilterWheel
            {
                get
                {


                    return filterWheel;
                }
                set
                {
                    filterWheel = value;
                }
            }
       
        #endregion
               
        #region StartandStop

        /// <summary>
        /// Starts core with a specific config file
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
            public string StartCore(string configFile)
            {
                string aPath;
                string aName;

                aName = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
                
                aPath = System.IO.Path.GetDirectoryName(aName);
                
               return  StartCore(aPath , configFile);
            }
        /// <summary>
        /// Start core with default options
        /// </summary>
            public void StartCoreCore()
            {
                string aPath;
                string aName;

                aName = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;

                aPath = System.IO.Path.GetDirectoryName(aName);

                StartCoreCore(aPath);
            }
            public void StartCoreCore(string CoreDirPath)
            {
                if (CoreDirPath == "") CoreDirPath = Path.GetDirectoryName( Application.ExecutablePath);
                string CorelogFile = System.IO.Path.Combine(CoreDirPath , "CoreLog.txt");
                _PluginFolder  = CoreDirPath;
                //MessageBox.Show(_PluginFolder);
                if (File.Exists(CorelogFile) == true)
                {
                    string FileCreationTime = File.GetLastAccessTime(CorelogFile).ToString();
                    string FileCreateFormated = FileCreationTime.Replace("/", "_").Replace(":", "_").Replace(" ", "_");
                    // System.Diagnostics.Debug.Print(Path.GetFileNameWithoutExtension(CorelogFile) + FileCreationTime.ToString() + ".txt");
                    File.Move(CorelogFile, Path.GetFileNameWithoutExtension(CorelogFile) + FileCreateFormated + ".txt");
                    File.Delete(CorelogFile);
                }


                try
                {
                    core = new CMMCore();
                }
                catch (Exception ex)
                {
                    MessageBox.Show (ex.Message);
                    if (ex.InnerException != null)
                        MessageBox.Show(ex.InnerException.ToString());
                    MessageBox.Show(Application.ExecutablePath);
                    MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
                }
                core.SetPluginFolder(_PluginFolder + "\\");
                core.enableDebugLog(true);

                core.enableStderrLog(true);
                try
                {
                   
                        LogMessage("Checking for plugins");
                        //MessageBox.Show("Checking for plugins");
                        if (AllPossibleGUIs == null)
                            AllPossibleGUIs = new Dictionary<string, Type>();
                        AllPossibleGUIs.Clear();

                        List<string> FJunk =new  List<string>();
                        try { FJunk.Add(Path.GetDirectoryName(CoreDirPath)); }catch { }
                        try { FJunk.Add (Path.GetDirectoryName( AppDomain.CurrentDomain.BaseDirectory));}catch { }
                        try { FJunk.Add(Path.GetDirectoryName(this.GetType().Assembly.Location)); }      catch { }
                        try { FJunk.Add( Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace("file:\\", "")); }catch { }
                        try { FJunk.Add( Path.GetDirectoryName(Application.ExecutablePath)); }catch { }
                        try { FJunk.Add( Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)); }catch { }
                        try { FJunk.Add(CoreDirPath);  }catch { }
                   
                    foreach (string s in FJunk)
                    {
                        try
                        {
                            LogMessage("Checking for GUI plugins in indicated directory : " + s );
                            GetDeviceUIAssemblies(s);
                        }
                        catch
                        { }
                    }
                    foreach (string s in FJunk)
                    {
                        try
                        {
                            LogMessage("Searching for viewer assemblies : " + s);
                            GetMicroscopyViewerAssemblies(s);
                        }
                        catch { }
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Easycore " + ex.Message);
                    MessageBox.Show(ex.InnerException.Message );
                }

                try
                {
                    acqEngine = new AcquisitionEngine(this);
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex.Message, 3);
                }
            }
      
        public string StartCore(string CoreDirPath, string configFile)
        {

            if (core == null)
                StartCoreCore(CoreDirPath);

            try
            {
                ChannelState light = new ChannelState(this);
                light.AddCommand("filter", "State", "255");
                light.ChannelName = "light";
                light.ChannelColor = System.Drawing.Color.Empty;
                light.ShowFalseColor = false;
                AllChannels.Add("light", light);
                ChannelState dark = new ChannelState(this);
                dark.AddCommand("filter", "State", "0");
                dark.ChannelName = "dark";
                dark.ChannelColor = System.Drawing.Color.FromArgb(0, 255, 0);
                dark.ShowFalseColor = true;
                AllChannels.Add("dark", dark);
                ChannelGroup test = new ChannelGroup(this);
                test.GroupName = "test";
                test.CombineChannels = true;
                test.Channels.Add(dark);
                test.Channels.Add(light);
                AllGroups.Add("test", test);
            }
            catch { }

            LogMessage(core.getAPIVersionInfo());
           
            //MessageBox.Show("!");
            string ErrorString="";
            if (configFile != "")
            {
                try
                {
                    ErrorString = LoadFullConfigFile(configFile);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.InnerException.ToString());
                    }
                    else
                        MessageBox.Show(ex.Message + "\n");

                    core.unloadAllDevices();
                    ClearDevices();
                    try
                    {
                        //todo: load a very default file here
                    }
                    catch
                    {
                        core.unloadAllDevices();
                        ClearDevices();

                    }

                }
            }
           
            return ErrorString;
        }
        
        /// <summary>
        /// Used for shut down.  Will close all the devices and the core
        /// </summary>
        public void StopCore()
        {
            foreach (KeyValuePair<string, Devices.MMDeviceBase> kp in MyDevices)
            {
                try
                {
                    kp.Value.StopDevice();
                }
                catch { }
            }

            foreach (Thread t in ThreadPool)
            {
                try
                {
                    t.Abort();
                    t.Suspend();
                }
                catch { }
            }

        }
        /// <summary>
        /// Loads a device configuration file
        /// </summary>
        /// <param name="ConfigFileName"></param>
        /// <returns></returns>
        public string  LoadFullConfigFile(string ConfigFileName)
        {
            string ErrorString = "";
            try
            {
               // core.loadSystemConfiguration(ConfigFileName);
               // core.initializeAllDevices();
                
            }
            catch (Exception ex)
            {
                core.unloadAllDevices();
                LogErrorMessage(ex.Message,1);
            }
            string fn = System.IO.Path.GetFileName(ConfigFileName);
            string pth = System.IO.Path.GetFullPath(ConfigFileName);
            string exten = System.IO.Path.GetExtension(fn);
            pth = pth.Replace(exten, "");
            XmlDocument  reader = new XmlDocument();
            //MessageBox.Show(pth +"_full.xml");
            try
            {
                reader.Load(pth + "_full.xml");
            }
            catch
            {
                reader.Load(ConfigFileName);
            }
            XmlNode oNode = reader.DocumentElement;
            XmlNodeList oNodeList = oNode.SelectNodes("/Root/Devices")[0].ChildNodes ;
            
            Dictionary<string, Dictionary<string, string>> AllProps = new Dictionary<string, Dictionary<string, string>>();
            for(int x = 0; x < oNodeList.Count; x++)
            {   
                //System.Diagnostics.Debug.Print("<br>NodeList Item#" + x + " " + oNodeList.Item(x).InnerText + "<br>");

                XmlNode device = oNodeList.Item(x);
                
                Dictionary<string, string> Proplist = new Dictionary<string, string>();
                XmlAttributeCollection attributes = device.Attributes;
                for (int i=0;i<attributes.Count ;i++)
                {
                    Proplist.Add(attributes[i].Name.Replace("_"," ") ,attributes[i].Value );
                }

                AllProps.Add(oNodeList.Item(x).Name.Replace("___", " "), Proplist);
            }
            //first load all the devices into the micromanager library
            foreach (KeyValuePair<string, Dictionary<string, string>> KVP in AllProps)
            {
                string DevName="";
                try
                {
                    Dictionary<string, string> Proplist = KVP.Value;
                    DevName  = KVP.Key;
                    string libName = Proplist["LibraryName"];
                    string libDev = Proplist["LibDeviceName"];
                    if (libName.Trim () !="")
                        core.loadDevice(DevName, libName, libDev);
                }
                catch (Exception ex)
                {
                    LogErrorMessage("Unable to load " + DevName + " Error: " + ex.Message,0);
                }
            }
            try
            {
                //now get them initialized
                core.initializeAllDevices();
            }
            catch ( Exception ex)
            {
                LogErrorMessage(ex.Message, 1);
            }
            //now set up all the device interfaces for c#
            foreach (KeyValuePair<string, Dictionary<string, string>> KVP in AllProps)
            {
                Dictionary<string, string> Proplist = KVP.Value;
                string DevName = KVP.Key;
                string libName = Proplist["LibraryName"];
                string libDev = Proplist["LibDeviceName"];
                switch (Proplist["DeviceType"])
                {
                    case "CoreDevices.Devices.MMEmptyGuiDevice":
                        try
                        {

                            Devices.MMEmptyGuiDevice egd = new Devices.MMEmptyGuiDevice(this, DevName);

                        }
                        catch
                        {
                        }
                        break;
                    case "CameraDevice":
                        try
                        {
                            
                            //MessageBox.Show("Camera Found");
                            camera = new Devices.NormalCamera(this, DevName, libName, libDev);
                           // camera = new ScanningConfocalCamera(this, DevName, libName, libDev);
                        }
                        catch (Exception ex)
                        {
                           /* try
                            {
                                MessageBox.Show(ex.Message + "\n" + ex.InnerException.ToString(), "Error");
                            }
                            catch
                            {
                                MessageBox.Show(ex.Message);
                                MessageBox.Show(Application.ExecutablePath);
                            }*/
                            ErrorString += "Camera did not start " + ex.Message + "\n";
                            LogErrorMessage(ErrorString, 3);
                        }
                        try
                        {
                            if (Proplist["Official"].Trim().ToLower() == "true") camera.MakeOffical();
                        }
                        catch { }
                        break;
                    case "SignalIODevice":
                        try 
                        {
                            funcGen = new Devices.FunctionGenerator(this, DevName, libName, libDev);
                        }
                        catch (Exception ex)
                        { ErrorString += "Signal IO did not start " + ex.Message + "\n"; }
                        try
                        {
                            if (Proplist["Official"].Trim().ToLower() == "true") funcGen.MakeOffical();
                        }
                        catch { }
                        break;
                    case "StageDevice":
                        try 
                        {
                            focusStage = new Devices.ZStage(this, DevName, libName, libDev);
                        }
                        catch (Exception ex)
                        { ErrorString += "Stage Device did not start " + ex.Message + "\n"; }
                        try
                        {
                            if (Proplist["Official"].Trim().ToLower() == "true") focusStage.MakeOffical();
                        }
                        catch { }
                        break;
                    case "XYStageDevice":
                        try
                        {
                            xyStage = new Devices.XYStage(this, DevName, libName, libDev);
                        }
                        catch (Exception ex)
                        { ErrorString += "XYStage did not start " + ex.Message + "\n"; }
                        try
                        {
                            if (Proplist["Official"].Trim().ToLower() == "true") xyStage.MakeOffical();
                        }
                        catch { }
                        break;
                    case "StateDevice":
                        Devices.StateDevice sd = null;
                        try
                        {
                            sd = new Devices.StateDevice(this, DevName, libName, libDev);
                        }
                        catch (Exception ex)
                        { ErrorString += "StateDevice (" + DevName + ") did not start " + ex.Message + "\n"; }
                        try
                        {
                            if (Proplist["Official"].Trim().ToLower() == "true") sd.MakeOffical();
                        }
                        catch { }
                        break;
                    case "FilterWheel":
                         
                        try
                        {
                            filterWheel = new Devices.FilterWheel(this, DevName, libName, libDev);
                        }
                        catch (Exception ex)
                        { ErrorString += "FilterWheel (" + DevName + ") did not start " + ex.Message + "\n"; }
                        try
                        {
                            if (Proplist["Official"].Trim().ToLower() == "true") filterWheel.MakeOffical();
                        }
                        catch { }
                        break;
                    default:
                        CoreDevices.Devices.GenericDevice gd;
                        try
                        {
                            gd = new Devices.GenericDevice(this, DevName, libName, libDev);
                        }
                        catch (Exception ex)
                        { ErrorString += "Generic Device (" + DevName + ") did not start " + ex.Message + "\n"; }
                       
                        break;
                }
               

            }
            //now load the property lists into the adapters
            foreach (KeyValuePair<string, Dictionary<string, string>> KVP in AllProps)
            {
                Dictionary<string, string> Proplist = KVP.Value;
                string DevName = KVP.Key;
                string libName = Proplist["LibraryName"];
                string libDev = Proplist["LibDeviceName"];

                Devices.MMDeviceBase dev = GetDevice(DevName);
                List<string> SaveProps = new List<string>();
                List<string> DevPropNames = new List<string>(dev.GetDevicePropertyNames());
                foreach (KeyValuePair<string, string> pPair in Proplist)
                {
                    try
                    {
                        string propname = pPair.Key;
                        string propvalue = pPair.Value;
                        SaveProps.Add(pPair.Key);
                        if (DevPropNames.Contains(propname) == true)
                        {
                            if (propname != "Position")
                            {
                                LogMessage("Setting " + propname + " To " + propvalue + " on " + DevName);
                                dev.SetDeviceProperty(propname, propvalue);
                            }
                            if (propname == "Exposure" && DevName == camera.deviceName)
                            {
                                double Exposure;
                                double.TryParse(propvalue, out Exposure);
                                core.setExposure(Exposure);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage(ex.Message);
                        //ErrorString += "Property Initialization threw error " + ex.Message + "\n";
                    }

                }
                dev.SaveableProperties = SaveProps.ToArray();

            }
            reader=null;
            return ErrorString;
        }

        /// <summary>
        /// A convience method to put a message in the whole log
        /// </summary>
        /// <param name="Message"></param>
        public void LogMessage(string Message)
        {
            core.logMessage(Message);
        }
        /// <summary>
        /// A Convience method to log error messages
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Urgency">0 - Warning, 1 - Error, 2 - Fatal Error</param>
        public void LogErrorMessage(string Message,int Urgency)
        {
            core.logMessage(Message + " Urgency " + Urgency.ToString() );
        }
      
        /// <summary>
        /// Saves the current configuration of the system to the defined file.
        /// </summary>
        /// <param name="ConfigFileName"></param>
        public void SaveConfigFile(string ConfigFileName)
        {
            try
            {
                if (System.IO.Path.GetExtension(ConfigFileName).Trim() == "")
                    core.saveSystemConfiguration(ConfigFileName + ".xml");
                else 
                    core.saveSystemConfiguration(ConfigFileName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }

            string fn = System.IO.Path.GetFileName(ConfigFileName);
            string pth = System.IO.Path.GetFullPath(ConfigFileName);
            string exten = System.IO.Path.GetExtension(fn);
            if (exten == "") exten = ".xml";
            pth = pth.Replace(exten, "");
           
            try
            {
                //pick whatever filename with .xml extension
                string filename = pth + "_full.xml";
                if (  File.Exists(filename))
                    File.Delete(filename);

                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(filename);
                }
                catch (System.IO.FileNotFoundException)
                {
                    //if file is not found, create a new xml file
                    XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                    xmlWriter.WriteStartElement("Root");
                    //If WriteProcessingInstruction is used as above,
                    //Do not use WriteEndElement() here
                    //xmlWriter.WriteEndElement();
                    //it will cause the &ltRoot></Root> to be &ltRoot />
                    xmlWriter.Close();
                    xmlDoc.Load(filename);
                }
                XmlNode root = xmlDoc.DocumentElement;
                XmlElement DeviceNode = xmlDoc.CreateElement("Devices");
                

                root.AppendChild(DeviceNode);
               
                

                string s;
                foreach (KeyValuePair<string, Devices.MMDeviceBase> kp in MyDevices)
                {


                    Devices.MMDeviceBase mmD = kp.Value;
                    s = mmD.deviceName.Replace(" ","___");
                    XmlElement childNode2 = xmlDoc.CreateElement(s);
                    DeviceNode.AppendChild(childNode2);
                    try
                    {
                        childNode2.SetAttribute("LibraryName", mmD.LibraryName);
                        childNode2.SetAttribute("LibDeviceName", mmD.LibDeviceName);

                        if (mmD.GetType() == typeof(Devices.MMEmptyGuiDevice))
                        {
                            childNode2.SetAttribute("DeviceType", mmD.GetType().ToString());
                        }
                        else
                        {
                            DeviceType deviceType = core.getDeviceType(s);
                            childNode2.SetAttribute("DeviceType", deviceType.ToString());
                        }
                        Type deviceType2 = mmD.GetType();
                        if (typeof(Devices.Camera )==deviceType2 )
                        {
                                if (camera ==mmD )
                                {
                                    childNode2.SetAttribute("Official", "true");
                                }
                        }
                        else if (xyStage ==mmD  )
                        {
                           
                                if (core.getXYStageDevice() == s)
                                {
                                    childNode2.SetAttribute("Official", "true");
                                }
                        }
                        else if (focusStage ==mmD  )
                        {
                                if (core.getFocusDevice() == s)
                                {
                                    childNode2.SetAttribute("Official", "true");
                                }
                        }

                        Dictionary<string,PropertyInfo > props= mmD.GetAllDeviceProperties();
                        foreach (KeyValuePair<string, PropertyInfo> kvp in props)
                        {
                            if (kvp.Value.ReadOnly != true && mmD.PropertyIsSaveable( kvp.Key)==true  )
                            {

                                string pValue = kvp.Value.Value;
                                string fixedName = kvp.Key.Replace(" ", "_");
                                childNode2.SetAttribute(fixedName, pValue);
                            }

                        }
                        childNode2.SetAttribute("GuiInfo", mmD.GuiPersistenceProperties );
                      

                    }
                    catch { }
                    
                    

                }

                xmlDoc.Save(filename);
            }
            catch (Exception ex)
            {
                
            }

        }

        internal List<string[]> LoadGUIDescriptionFromConfigFile(string ConfigFileName)
        {
            string ErrorString = "";
           
            string fn = System.IO.Path.GetFileName(ConfigFileName);
            string pth = System.IO.Path.GetFullPath(ConfigFileName);
            string exten = System.IO.Path.GetExtension(fn);
            pth = pth.Replace(exten, "");

            XmlDocument reader = new XmlDocument();
            try
            {
                reader.Load(pth + "_Desktop.config");
            }
            catch
            {
                reader.Load(ConfigFileName);
            }
            XmlNode oNode = reader.DocumentElement;
            XmlNodeList oNodeList = oNode.SelectNodes("/DockPanel/Contents")[0].ChildNodes;

            List<string[]> AllGUIs = new List<string[]>();
            for (int x = 0; x < oNodeList.Count; x++)
            {
                XmlNode device = oNodeList.Item(x);


                string s1 = device.Attributes["PersistString"].Value;
                string s2 = device.Attributes["ExtraInformation"].Value;
                string[] sss= s2.Split(new char[] { '|' });
                if (sss.Length ==2)
                    AllGUIs.Add(new string[] { s1, sss[0], sss[1] });
                else
                    AllGUIs.Add(new string[] { s1, sss[0], "" });
            }
           
            reader = null;
            return AllGUIs ;

        }
        /// <summary>
        /// Saves the current Desktop GUI configuration (This is mostly for labview.  Form2 has 
        /// methods to save the desktop correctly,  Never use this
        /// </summary>
        /// <param name="ConfigFileName"></param>
        public  void SaveFakeGUIFile(string ConfigFileName,List<string[]> GUIDescriptions)
        {
           

            string fn = System.IO.Path.GetFileName(ConfigFileName);
            string pth = System.IO.Path.GetFullPath(ConfigFileName);
            string exten = System.IO.Path.GetExtension(fn);
            if (exten == "") exten = ".xml";
            pth = pth.Replace(exten, "");

            try
            {
                //pick whatever filename with .xml extension
                
                string filename = pth + "_Desktop.config";
                //if (File.Exists(filename))
                //    File.Delete(filename);
                try
                {
                    File.Delete(filename);
                }
                catch { }
                XmlDocument xmlDoc = new XmlDocument();

               
                    //if file is not found, create a new xml file
                    XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                    xmlWriter.WriteStartElement("DockPanel");
                    //If WriteProcessingInstruction is used as above,
                    //Do not use WriteEndElement() here
                    //xmlWriter.WriteEndElement();
                    //it will cause the &ltRoot></Root> to be &ltRoot />
                    xmlWriter.Close();
                    xmlDoc.Load(filename);
                

                XmlNode root = xmlDoc.DocumentElement;
                XmlElement DeviceNode = xmlDoc.CreateElement("Contents");


                root.AppendChild(DeviceNode);



                string s;
                foreach (string[] s3 in GUIDescriptions )
                {
                    XmlElement childNode2 = xmlDoc.CreateElement("Content");
                    DeviceNode.AppendChild(childNode2);
                    try
                    {
                        childNode2.SetAttribute("PersistString", s3[0]);
                        childNode2.SetAttribute("ExtraInformation", s3[1] + "|" + s3[2]);
                    }
                    catch { }
                }
                xmlDoc.Save(filename);
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
        
        
        public CMMCore Core
        {
            get { return (core); }
        }

       /* /// <summary>
        /// Convience method for COM and labview to create a camera
        /// </summary>
        /// <param name="DeviceLabel"></param>
        /// <param name="LibraryName"></param>
        /// <param name="DeviceAdapter"></param>
        /// <returns></returns>
        public Devices.Camera CreateCamera(string DeviceLabel, string LibraryName, string DeviceAdapter)
        {
            return new Devices.NormalCamera(this, DeviceLabel, LibraryName, DeviceAdapter);
        }
        /// <summary>
        /// Convience method for COM and labview
        /// </summary>
        /// <param name="DeviceLabel"></param>
        /// <returns></returns>
        public Devices.Camera CreateCamera(string DeviceLabel)
        {
            return new Devices.NormalCamera(this, DeviceLabel);
        }*/
    }

    /// <summary>
    /// Interface to allow EasyCore to be visible to COM components
    /// </summary>
    [Guid("5A88092E-69DF-0001-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IEasyCoreCom
    {
        void AddImageProcessor(string ProcessorName, ImageProcessorStep ProcessorStep);
        void InsertImageProcessor(int index, string ProcessorName, ImageProcessorStep ProcessorStep);
        Dictionary<string, ImageProcessorStep> ImageProcessorsList();
        string[] ImageProcessorNameList();
        void RemoveImageProcessor(string ProcessorName);
       

       // Devices.Camera CreateCamera(string DeviceLabel, string LibraryName, string DeviceAdapter);
       // Devices.Camera CreateCamera(string DeviceLabel);
         string[] GetAllChannelNames();
         void AddChannel(ChannelState newChannel);
         ChannelState GetChannel(string ChannelName);
         string[] GetAllGroupNames();
         void AddGroup(ChannelGroup newGroup);
         ChannelGroup GetChannelGroup(string GroupName);
         void RunChannelAcquisition(ChannelGroup ActiveGroup, string CameraDevice, long TimePerFrameMS,bool Save, string path,string filename,int NumFrames);
         void RunZStackAcquisition(ChannelGroup ActiveGroup, string CameraDevice, string FocusDevice, long TimePerFrameMS, bool FirstStackThenChannel, bool DisplaySlices, bool CurrentSliceIsMiddle, int NumSlices, double SliceDistance, bool Save, string path, string filename);
         void StopAcquisition();
         void AddThreadToPool(Thread thread);
         void ClearDevices();
         string[] GetAllLoadedDeviceNames();
         Devices.MMDeviceBase GetDevice(string DeviceName);
         List<Devices.MMDeviceBase> GetLoadedDevices();
         CoreDevices.UI.GUIDeviceControl GetDeviceGUI(string DeviceGUIName);
         string[] GetAllDeviceGUIs();
         string[] GetAllDeviceAdapters();
         string[] GetDeviceLibraries
        {
            get;
        }
         string[] GetDevicesFromLibrary(string LibName);
         string[] ConvertStrVectortoArray(StrVector inVector);
         void StartCoreOnlyDevice(string DeviceName, string LibName, string AdapterName);
         void RegisterDevice(Devices.MMDeviceBase Device);
         void UpdatePaintSurface(CoreImage cImage);
         void UpdatePaintSurface( CoreImage[] cImage);
         void DoForcedSave(string Filename);
         void PaintSurface(IPictureView PaintSurface);
         void PaintSurface(IPictureView[] PaintSurface);
         double ScreenSize
        {
            get ;
        }
         Devices.FunctionGenerator MMFunctionGenerator
        {
            get;
            set;
        }
         Devices.XYStage MMXYStage
        {
            get;
            set;
        }
         Devices.ZStage MMFocusStage
        {
            get;
            set;
        }
         Devices.Camera MMCamera
        {
            get;
            set;
        }
         Devices.FilterWheel MMFilterWheel
        {
            get;
            set;
        }
         
         string StartCore(string CoreDirPath, string configFile);
         void StopCore();
         string  LoadFullConfigFile(string ConfigFileName);
         void LogMessage(string Message);
         void LogErrorMessage(string Message,int Urgency);
         void SaveConfigFile(string ConfigFileName);
         CMMCore Core
        {
            get;
        }
  }
    


    

}
