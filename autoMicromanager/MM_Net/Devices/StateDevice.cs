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
using System.Runtime.InteropServices;
using CWrapper;

namespace CoreDevices.Devices
{
    [Guid("1514adf6-7cb1-0026-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class StateDevice : MMDeviceBase,IStateDeviceCom 
    {
        private int nStates=0;

        /// <summary>
        /// Sets the state of the device
        /// </summary>
        public string State
        {
            set 
            {    
                SetDeviceProperty("State", value );
            }
            get 
            {
                return GetDevicePropertyValue("State");
            }
        }
        
        /// <summary>
        /// Shows the number of possible states for the device
        /// </summary>
        public int NumStates
        {
            get { return nStates; }
        }

       
      
        public StateDevice(EasyCore ECore, string DeviceLabel, string LibraryName, string DeviceAdapter)
        {
            SetCore(ECore, DeviceLabel, LibraryName, DeviceAdapter);

        }
        public StateDevice(EasyCore ECore, string DeviceLabel)
        {
            DeviceName = DeviceLabel;
            SetCore(ECore);

        }
        private void InitializeDevice()
        {
           nStates=  core.getNumberOfStates(deviceName);

        }
        public override void MakeOffical()
        {
            
        }
        public override void StopDevice()
        {
           
        }
    }

    [Guid("5A88092E-69DF-0026-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IStateDeviceCom
    {
        int NumStates { get; }
        string State { get; set; }

        

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
