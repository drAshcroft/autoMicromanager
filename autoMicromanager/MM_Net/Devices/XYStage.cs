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
using System.Runtime.InteropServices;
using CWrapper;

namespace CoreDevices.Devices
{
    [Guid("1514adf6-7cb1-0021-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class XYStage:MMDeviceBase,IXYStageCom  
    {
        private double StageX;
        private double StageY;
        private bool UpdateLocation=false ;

        private Thread StageMovementThread;

        public override  void StopDevice()
        {
            StopStageThread(); 
        }

        public event OnStopsChanged StopsChanged;
        #region LocationList

        /// <summary>
        /// A way to save and name a number of locations for the stage to travel
        /// </summary>
        [Guid("1514adf6-7cb1-0022-7Ebb-b75c1467149b")]
        [ComVisible(true)]
        [ClassInterface(ClassInterfaceType.None)]
        public class XYZlocation:IXYZlocationCom 
        {
            private string _LocationName = "";
            private double _x = 0;
            private double _y=0;
            private double _z=0;
            public string LocationName { get { return _LocationName; } set { _LocationName  = value; } }
            public double x { get { return _x; } set { _x = value; } }
            public double y { get { return _y; } set { _y = value; } }
            public double z { get { return _z; } set { _z = value; } }
            public XYZlocation()
            { }
            public XYZlocation(string lName, double posX,double posY,double posZ)
            {
                _LocationName = lName;
                _x = posX;
                _y = posY;
                _z = posZ;
            }
        }
        private List<XYZlocation> _Stops = new List<XYZlocation>();
       
        /// <summary>
        /// Returns a stop or location where the stage remembers
        /// </summary>
        public List<XYZlocation> Stops
        {
            get { return _Stops; }
        }

        /// <summary>
        /// Allows the renaming of a stop
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="NewName"></param>
        public void RenameStop(int Index, string NewName)
        {
            _Stops[Index].LocationName = NewName;
            if (StopsChanged != null) StopsChanged(this);
        }
        /// <summary>
        /// Allows the renaming of a stop
        /// </summary>
        /// <param name="OldName"></param>
        /// <param name="NewName"></param>
        public void RenameStop(string OldName, string NewName)
        {
            int i = 0;
            foreach (XYZlocation z in _Stops)
            {
                if (z.LocationName.ToLower() == OldName.ToLower()) RenameStop(i, NewName);
                i++;
            }
        }
        /// <summary>
        /// Travels to a remembered location or stop.  
        /// </summary>
        /// <param name="LocationNumber"></param>
        public void GoToLocation(int LocationNumber)
        {
            if (LocationNumber < _Stops.Count)
            {
                MoveStageAbsolute(_Stops[LocationNumber].x, _Stops[LocationNumber].y);
                if (Ecore.MMFocusStage != null) Ecore.MMFocusStage.SetPositionAbsolute(_Stops[LocationNumber].z);
            }
        }
        /// <summary>
        /// Travels to a named stop
        /// </summary>
        /// <param name="LocationName"></param>
        public void GoToLocation(String LocationName)
        {
            int i = 0;
            foreach (XYZlocation z in _Stops)
            {
                if (z.LocationName.ToLower() == LocationName.ToLower()) GoToLocation(i);
                i++;
            }
        }

        #endregion

        ~XYStage()
        {
            StopStageThread();
        }

        public void Dispose()
        {
            
            //Finalize();
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Usually busy when the stage is in motion
        /// </summary>
        public bool Busy
        {
            get { return core.deviceBusy(DeviceName); }
        }

        public XYStage(EasyCore ECore,string DeviceLabel,string LibraryName,string DeviceAdapter )
        {
            SetCore(ECore, DeviceLabel, LibraryName, DeviceAdapter);
            GetStagePosition(out StageX,out StageY);
            StartStageThread();
        }
        public XYStage(EasyCore Ecore, string DeviceLabel)
        {
            DeviceName = DeviceLabel;
            SetCore(Ecore);
            GetStagePosition(out StageX, out StageY);
            StartStageThread();
        }
        public override void MakeOffical()
        {
            core.setXYStageDevice(deviceName);
            ECore.MMXYStage = this;
        }

        #region MoveStage

        /// <summary>
        /// Requests that the sage freezes
        /// </summary>
        public void StopStage()
        {
            core.stop(DeviceName);
            GetStagePosition(out StageX, out StageY);
            UpdateLocation = true;
        }

        /// <summary>
        /// Moves the stage relative to its current location
        /// </summary>
        /// <param name="X_um"></param>
        /// <param name="Y_um"></param>
        public void MoveStageRelative(double X_um, double Y_um)
        {
            double X = 0;
            double Y = 0;
            core.getXYPosition(DeviceName,out X,out Y);
            core.setXYPosition(DeviceName,X+ X_um,Y+ Y_um);

            StageX =X+ X_um;
            StageY =Y+ Y_um;
            UpdateLocation = true;
           
        }
        /// <summary>
        /// Moves stage to an absolute position.  What that posiition is depends on the stage driver and so forth.
        /// </summary>
        /// <param name="X_um"></param>
        /// <param name="Y_um"></param>
        public void MoveStageAbsolute(double X_um, double Y_um)
        {
            StageX = X_um;
            StageY = Y_um;
            UpdateLocation = true;
            core.setXYPosition(DeviceName, X_um, Y_um);
        }

        /// <summary>
        /// This is a blocking move.  Moves stage until it reaches its position and then stops
        /// </summary>
        /// <param name="X_um"></param>
        /// <param name="Y_um"></param>
        public void MoveStageGuarantee(double X_um, double Y_um)
        {
            StageMovementThread.Suspend();
            core.setXYPosition(DeviceName,X_um , Y_um );
            do
            {
                Thread.Sleep(100);
            }while (core.deviceBusy(DeviceName ));
            StageMovementThread.Resume();
        }

        /// <summary>
        /// Gets Current stage position
        /// </summary>
        /// <param name="X_um"></param>
        /// <param name="Y_um"></param>
        public void GetStagePosition(out double X_um, out double Y_um)
        {
            try
            {
                core.getXYPosition(DeviceName, out X_um, out Y_um);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("-70006") != true)
                    Ecore.LogErrorMessage(ex.Message, 0);
                X_um = 0;
                Y_um = 0;
            }
        }
        #endregion 

        private bool bStopTread = false; 

        /// <summary>
        /// In order to enable asyncronous moves, the stage control is done on its own thread.  All controls are sent to the stage thread, that does the work
        /// </summary>
        private void StartStageThread()
        {
            if (StageMovementThread == null) // if currently there is no capture thread running
            {

                if (Ecore == null)
                {
                    throw (new Exception("You must initialize the stage before you start this procedure"));

                }

                StageMovementThread = new Thread(
                    delegate()
                    {
                        
                        while (!bStopTread )
                        {
                            if (UpdateLocation)
                            {
                                if (!core.deviceBusy(DeviceName))
                                {
                                    UpdateLocation = false;
                                    core.setXYPosition(DeviceName, StageX, StageY);
                                }
                            }
                            if (!bStopTread ) Thread.Sleep(100);
                        }
                        bStopTread = false;
                    });
                StageMovementThread.Priority = ThreadPriority.Lowest;
                StageMovementThread.Start();
                Ecore.AddThreadToPool(StageMovementThread);

            }
            else // if currently capturing
            {
                StopStageThread();
                //StartFocusMovie();

            }

        }
        
        /// <summary>
        /// Kills the stage thread.  Should be done before the control is stopped.
        /// </summary>
        public void StopStageThread()
        {
            try
            {
                bStopTread = true;
                do
                {
                  
                    StageMovementThread.Abort();
                    //StageMovementThread.Abort(100);
                    if (StageMovementThread.ThreadState==ThreadState.Aborted) bStopTread=false ;
                    if (StageMovementThread.ThreadState == ThreadState.Stopped) bStopTread = false; 
                } while (bStopTread ==true  )  ;

                StageMovementThread = null;
            }
            catch { }
        }

    }


    [Guid("5A88092E-69DF-0021-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IXYStageCom
    {

        bool Busy { get; }
        List<XYStage.XYZlocation> Stops { get; }

        event OnStopsChanged StopsChanged;

        void Dispose();
        void GetStagePosition(out double X_um, out double Y_um);
        void GoToLocation(int LocationNumber);
        void GoToLocation(string LocationName);
        
        void MoveStageAbsolute(double X_um, double Y_um);
        void MoveStageGuarantee(double X_um, double Y_um);
        void MoveStageRelative(double X_um, double Y_um);
        void RenameStop(int Index, string NewName);
        void RenameStop(string OldName, string NewName);
      
        void StopStage();
        void StopStageThread();

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

    [Guid("5A88092E-69DF-0022-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IXYZlocationCom
    {
        string LocationName {get;set;}
        double x { get; set; }
        double y { get; set; }
        double z { get; set; }


    }
}
