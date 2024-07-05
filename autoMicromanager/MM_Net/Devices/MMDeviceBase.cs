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
using FreeImageAPI;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using CWrapper;

namespace CoreDevices.Devices
{
    /// <summary>
    /// The most basic adapter for defining the behavior of a device.  Device can be either in micromanager or independant(pure .NET or some other API).  This allows EasyCore to have a consistant interface to any device.
    /// </summary>
    [Serializable]
    public abstract class MMDeviceBase
    {
        private string[] _SaveableProperties;
       
        /// <summary>
        /// Some device properties are not saved from one session to the next
        /// This gives a list of those properties that the user defined as needing to be saved
        /// </summary>
        public string[] SaveableProperties
        {
            get { return _SaveableProperties; }
            set { _SaveableProperties = value; }
        }

        /// <summary>
        /// Determines if a device property has been marked as needing to be saved
        /// </summary>
        /// <param name="PropName"></param>
        /// <returns></returns>
        public bool PropertyIsSaveable(string PropName)
        {
            string lPropName=PropName.ToLower();
            for (int i = 0; i < _SaveableProperties.Length; i++)
            {
                if (_SaveableProperties[i].ToLower() == lPropName)
                    return true;

            }
            return false;
        }
        private string _GuiPersistenceProperties = "";

        /// <summary>
        /// Notifications to the GUI on how it should behave.  This is very important for those devices that are virtual, (especially scripts.)
        /// </summary>
        public string GuiPersistenceProperties
        {
            get { return _GuiPersistenceProperties; }
            set { _GuiPersistenceProperties = value; }
        }

        /// <summary>
        /// This runs a CommandState command. This is used to change a property value, or can be a custom command for this device 
        /// </summary>
        /// <param name="MethodName"></param>
        /// <param name="Pars"></param>
        public void RunCommand(string MethodName, object[] Pars)
        {
            SetDeviceProperty(MethodName, (string)Pars[0]);
        }

        /// <summary>
        /// This should be set when the device should not be interupped 
        /// such as the stage being in motion, or an expose has started
        /// </summary>
        protected bool DeviceBusy = false;

        // a list of the properties that are waiting to be changed
        protected List<PropertyHolder> PropBuffer = new List<PropertyHolder>();
       
        protected EasyCore ECore = null;
        protected CMMCore core = null;
        //The basic information for finding the device.  If it is a Micromanager Device
        protected string DeviceName = "";
        public string LibraryName = "";
        public string LibDeviceName = "";

        public EasyCore Ecore
        {
            get { return ECore; }
        }

        /// <summary>
        /// All devices should be given a name, but it does not have to be unique
        /// </summary>
        public string deviceName
        {
            get { return DeviceName; }
            set { DeviceName = value; }
        }

        /// <summary>
        /// If this is a common device, then this will register the device with micromanager and easycore that it is the go-to device
        /// </summary>
        public abstract void MakeOffical();

        /// <summary>
        /// This will start the device, register it with EasyCore and register it with Micromanager
        /// </summary>
        /// <param name="ECore"></param>
        /// <param name="DeviceName"></param>
        /// <param name="LibraryName"></param>
        /// <param name="DeviceAdapter"></param>
        public void SetCore(EasyCore ECore, string DeviceName, string LibraryName, string DeviceAdapter)
        {
            this.DeviceName = DeviceName;// core.getCameraDevice();
            this.LibraryName = LibraryName;
            this.LibDeviceName = DeviceAdapter;

            SetCore(ECore);
            List<String> devs = new List<string>();
            devs.AddRange(Ecore.ConvertStrVectortoArray(core.getLoadedDevices()));
            if (!devs.Contains(DeviceName))
            {
                core.loadDevice(DeviceName, LibraryName, DeviceAdapter);
                core.initializeDevice(DeviceName);
            }

        }

        /// <summary>
        /// Registers the device with Easycore only
        /// </summary>
        /// <param name="ECore"></param>
        public void SetCore(EasyCore ECore)
        {
            this.ECore = ECore;
            core = ECore.Core;
            Ecore.RegisterDevice(this);
        }

        #region Properties

        //a list of the possible properties for the device
        protected Dictionary<string, PropertyInfo> AllProperties = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// An internal class that allows the properties to go into classes.  Should be updated to something more useful
        /// </summary>
        protected class PropertyHolder
        {
            public string PropName;
            public string PropValue;
            public PropertyHolder(string PropName, string PropValue)
            {
                this.PropName = PropName;
                this.PropValue = PropValue;
            }
        }

        /// <summary>
        /// Returns active property list for this device.  Values autoupdate and produce an event to ensure all users have the same value
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, PropertyInfo> GetAllDeviceProperties()
        {
            if (AllProperties ==null || AllProperties.Count==0) InitializePropertyList (true );
            return (AllProperties );
        }

        /// <summary>
        /// Receives notification that the value has been updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="DeviceName"></param>
        /// <param name="PropName"></param>
        /// <param name="PropValue"></param>
        protected void pi_OnPropValueSet(object sender, string DeviceName, string PropName, string PropValue)
        {
            SetDeviceProperty(PropName, PropValue);
        }

        /// <summary>
        /// Gets active information from each single property value for this device.  Values autoupdate and produce an event to ensure all users have the same value
        /// </summary>
        /// <param name="PropName"></param>
        /// <returns></returns>
        public PropertyInfo GetDevicePropertyInfo(string PropName)
        {
            string lPropname = PropName.ToLower();
            if (AllProperties == null || AllProperties.Count == 0)
            {
                GetAllDeviceProperties();
            }
            try { return AllProperties[PropName]; }
            catch { }
            foreach (KeyValuePair<string, PropertyInfo> pi in AllProperties)
            {
                if (pi.Value.PropName.ToLower() == lPropname) return (pi.Value);
            }

            try
            {
                PropertyInfo pi2 = new PropertyInfo(ECore, this, DeviceName, PropName);
                AllProperties.Add(PropName, pi2);
                return (pi2);
            }
            catch { }
            return null;

        }

        /// <summary>
        /// Gets a list of all the property names possible for this device.
        /// </summary>
        /// <returns></returns>
        public string[] GetDevicePropertyNames()
        {
            if (AllProperties == null || AllProperties.Count == 0)
            {
                GetAllDeviceProperties();
            }
            string[] Names = new string[AllProperties.Count];
            int i=0;
            foreach (KeyValuePair<string, PropertyInfo> kp in AllProperties)
            {
                Names[i] = kp.Key;
                i++;
            }
            return Names;
        }

        /// <summary>
        /// Gets a snapshot of all the device properties and their current values
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDevicePropertyValues()
        {
            if (AllProperties == null || AllProperties.Count == 0)
            {
                GetAllDeviceProperties();
            }
            Dictionary<string, string> props = new Dictionary<string, string>();
            foreach (KeyValuePair<string, PropertyInfo> kp in AllProperties)
            {
                props.Add(kp.Key, kp.Value.Value);
            }
            return props;
        }

        /// <summary>
        /// Gets the value of the device property directly from Micromanager or whatever is the source
        /// </summary>
        /// <param name="PropName"></param>
        /// <returns></returns>
        public virtual  string GetDevicePropertyValue(string PropName)
        {
            return (core.getProperty(DeviceName, PropName));
        }

        /// <summary>
        /// Sets the value of the device property.  All values must be strings as this is the protocol set by micromanager.  If the device is busy, then this will wait until the device is ready to accept commands
        /// </summary>
        /// <param name="PropName"></param>
        /// <param name="PropValue"></param>
        public virtual void SetDeviceProperty(string PropName, string PropValue)
        {
            //Put the incoming property update information into a buffer (i.e if the camera is in the middle of taking a frame).  Otherwise
            //just change the property
            if (DeviceBusy)
            {
                PropBuffer.Add(new PropertyHolder(PropName, PropValue));
            }
            else
            {
                core.setProperty(DeviceName, PropName, PropValue);
                if (AllProperties.Count == 0)
                    InitializePropertyList(false);

                AllProperties[PropName].Value =  core.getProperty(deviceName, PropName);
                AllProperties[PropName].ValueIsUpdated();
            }
        }
        
        /// <summary>
        /// Returns all the gory details about the property given in propname
        /// </summary>
        /// <param name="PropName"></param>
        /// <param name="_Value"></param>
        /// <param name="HasLimits"></param>
        /// <param name="MinValue"></param>
        /// <param name="MaxValue"></param>
        /// <param name="tType"></param>
        /// <param name="ReadOnly"></param>
        /// <param name="HasAllowedValues"></param>
        /// <param name="AllowedValues"></param>
        public virtual void GetDevicePropertyInfoDetails
            (string PropName, out string  _Value, out bool HasLimits, out double MinValue
            ,out double MaxValue, out PropertyType  tType, out bool ReadOnly
            ,out bool HasAllowedValues, out string[] AllowedValues)
        {
                _Value  = core.getProperty(DeviceName  , PropName );
                HasLimits = core.hasPropertyLimits(DeviceName , PropName );
                if (HasLimits)
                {
                    MinValue = core.getPropertyLowerLimit(DeviceName, PropName);
                    MaxValue = core.getPropertyUpperLimit(DeviceName, PropName);
                }
                else
                {
                    MinValue = 0;
                    MaxValue = 0;
                }
                tType  = core.getPropertyType(DeviceName, PropName );
                ReadOnly = core.isPropertyReadOnly(DeviceName, PropName);
                
                //core.isPropertyPreInit 
                //core.hasProperty 
                StrVector sv;
                try
                {
                    sv = core.getAllowedPropertyValues(DeviceName, PropName );
                    if (sv.Count > 0)
                    {
                        AllowedValues = new string[sv.Count];
                        sv.CopyTo(AllowedValues);
                        HasAllowedValues = true;
                    }
                    else
                    {
                        HasAllowedValues = false;
                        AllowedValues = null;
                    }
                }
                catch
                { 
                    HasAllowedValues = false;
                    AllowedValues = null;
                }
        }

        
        /// <summary>
        /// This is used from the startup scripts to build the propertylist without using the
        /// default propertylist
        /// </summary>
        public void BuildPropertyList()
        {
            InitializePropertyList(true);
        }

        /// <summary>
        /// This builds the device property list.  It must be called before the device does anything.  Setting clear properties to false allows the 
        /// device adapter or user to build the property list in several steps.  This is useful for devices that have both real micromanager properties as well as 
        /// virtual properties such as the function generators
        /// </summary>
        /// <param name="ClearProperties"></param>
        protected virtual  void InitializePropertyList(bool ClearProperties)
        {
             StrVector sv = core.getDevicePropertyNames(DeviceName);
             //  sv = core.getLoadedDevices();
             Dictionary<string, PropertyInfo> PIs;
             if (ClearProperties == true)
                 PIs = new Dictionary<string, PropertyInfo>();
             else
                 PIs = AllProperties;
             foreach (string s in sv)
             {
                 PIs.Add(s, new PropertyInfo(ECore, this, DeviceName, s));

             }
             
            foreach (KeyValuePair<string, PropertyInfo> pi in PIs)
            {
                pi.Value.OnPropValueSet += new OnPropValueSetEvent(pi_OnPropValueSet);

            }
            AllProperties = PIs;
        }
        
        /// <summary>
        /// Runs all device property change requests that have been made since the device was declared busy.
        /// </summary>
        protected virtual void ClearPropBuffer()
        {
            if (PropBuffer.Count > 0)
            {
                foreach (PropertyHolder ph in PropBuffer)
                {
                    if (ph.PropName == "Exposure" && DeviceName ==core.getCameraDevice())
                    {
                        
                        core.setExposure(double.Parse(ph.PropValue));
                    }
                    else
                    {
                        SetDeviceProperty(ph.PropName, ph.PropValue);
                    }
                    try
                    {
                        AllProperties[ph.PropName].ValueIsUpdated();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Print(ex.Message);
                    }
                }

            }
            PropBuffer.Clear();
        }

        #endregion

        /// <summary>
        /// Connects the default property listing GUI to this device. The properties are automatically loaded into the prop list.
        /// </summary>
        /// <param name="TargetPropertyList"></param>
        public void SetPropUI(IPropertyList TargetPropertyList)
        {
            InitializePropertyList(true );
            //AllProperties = ECore.GetDevicePropertyList(DeviceName);
            TargetPropertyList.SetCore(AllProperties);
        }

        /// <summary>
        /// Usually called on shut down to get the device to quit
        /// </summary>
        public abstract void StopDevice();

        
    }




   /* public interface IMMDeviceBase
    {
        
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
        Dictionary<string, PropertyInfo> GetAllDeviceProperties();
        PropertyInfo GetDevicePropertyInfo(string PropName);
        string[] GetDevicePropertyNames();
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


    }*/




}

