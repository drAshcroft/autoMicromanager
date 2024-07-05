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
using System.Runtime.InteropServices;
using CWrapper;

namespace CoreDevices.Devices
{
    /// <summary>
    /// Class to control a filter wheel
    /// </summary>
    [Serializable]
    [Guid("1514adf6-7cb1-0023-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
   public  class FilterWheel : MMDeviceBase,IFilterWheelCom  
    {

        private int nStops=6;      
        private List<string> _Stops=new List<string>();

        /// <summary>
        /// List of all the named filter wheel stops
        /// </summary>
        public List<string> Stops
        {
            get { return _Stops; }
        }

        public override  void StopDevice()
        {

        }
        public void RenameStop(int Index, string NewName)
        {
            _Stops[Index] = NewName;
           
        }
        public void RenameStop(string OldName, string NewName)
        {
            int i = 0;
            foreach (string z in _Stops)
            {
                if (z.ToLower() == OldName.ToLower()) RenameStop(i, NewName);
                i++;

            }
        }
        /// <summary>
        /// Allows you to go to a index stop
        /// </summary>
        /// <param name="StopNumber"></param>
        public void GoToStop(int StopNumber)
        {
            if (StopNumber <_Stops.Count )  SetState(StopNumber  );
        }

        /// <summary>
        /// Allows you to select a named stop
        /// </summary>
        /// <param name="LocationName"></param>
        public void GoToLocation(String LocationName)
        {
            int i=0;
            foreach (string  z in _Stops)
            {
                if (z.ToLower() == LocationName.ToLower()) SetState(i );
                i++;
            }

        }

        /// <summary>
        /// Moves the filterwheel to selected index
        /// </summary>
        /// <param name="State"></param>
        private void SetState(int State)
        {
            SetDeviceProperty("State",State.ToString());

        }
        
        /// <summary>
        /// Returns number of stops built into filterwheel
        /// </summary>
        public int NumStops
        {
            get {return nStops;}
        }

        public int CurrentStop
        {
            get
            {
                int N = 0;
                int.TryParse(GetDevicePropertyValue("State"), out N);
                return N;
            }
            set
            {
                int N = value;
                if (N >= nStops) N = 0;
                if (N < 0) N = nStops - 1;
                SetState(N);
            }
        }
        public void AdvanceToNextStop()
        {
            CurrentStop++;
        }
        public void MoveToPreceedingStop()
        {
            CurrentStop--;
        }

        public FilterWheel(EasyCore ECore,string DeviceLabel,string LibraryName,string DeviceAdapter )
        {
            SetCore(ECore, DeviceLabel, LibraryName, DeviceAdapter);
           
        }
        public FilterWheel(EasyCore ECore, string DeviceLabel)
        {
            DeviceName = DeviceLabel;
            SetCore(ECore);
            
        }
        public override void MakeOffical()
        {
            Ecore.MMFilterWheel = this;
        }
    }

    [Guid("5A88092E-69DF-0023-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual )]
    public interface IFilterWheelCom
    {
        int NumStops { get; }
        List<string> Stops { get; }

        void GoToLocation(string LocationName);
        void GoToStop(int StopNumber);
        
        void RenameStop(int Index, string NewName);
        void RenameStop(string OldName, string NewName);
       


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
