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
using CWrapper;

namespace CoreDevices
{
    //this allows whatever wants to handle the property change to handle it as an event
    public delegate void OnPropValueSetEvent(object sender,string DeviceName,string PropName,string PropValue);
    //because the camera has a delay before the properties is adjusted, we set up a call back that informs everything else that
    //the value has been updated.
    public delegate void OnPropertyUpdatedEvent(object sender,string PropName,string PropValue);
    
    /// <summary>
    /// Class handles property changes of the devices.  It uses an event model to make all the players (device, adapter, and GUI)
    /// </summary>
    public class PropertyInfo
    {
        //private CMMCore core;
        private Devices.MMDeviceBase MyDevice;
        private string DeviceName;
        public string PropName;
        public PropertyType tType;
        public bool HasLimits;
        public bool HasAllowedValues;
        public string[] AllowedValues;
        public bool ReadOnly;
        private  string _Value;
        public double  MaxValue;
        public double  MinValue;
        public event OnPropValueSetEvent OnPropValueSet;
        public event OnPropertyUpdatedEvent OnPropUpdate;
        
        /// <summary>
        /// Notifies all the other players that the value is updated
        /// </summary>
        public void ValueIsUpdated()
        {
            if (OnPropUpdate!=null) OnPropUpdate(this,PropName,Value);
        }

        /// <summary>
        /// Sets/Gets the value and then updates all the players
        /// </summary>
        public string Value
        {
            get
            {
                    if (MyDevice ==null )
                    {
                        return _Value;
                    }
                    else
                        return (MyDevice.GetDevicePropertyValue(PropName));
            }
            set
            {
                bool ValueChanged = false;
                if (tType == PropertyType.Float || tType == PropertyType.Integer)
                {
                    float f1;
                    try
                    {
                        f1 = float.Parse(value);
                    }
                    catch {
                        f1 = 0;
                        _Value = "0"; }
                    float f2 = float.Parse(_Value);
                    if (Math.Abs(f1 - f2) > .01)
                    {
                        ValueChanged = true;
                    }
                }
                else 
                {
                    if (_Value != value)
                    {
                        ValueChanged = true;
                    }
                }
                _Value = value;
                if (ValueChanged)
                {

                    if (MyDevice != null)
                    {
                        try
                        {
                            ECore.CurrentAcquisitionEngine.PauseThreads = true;
                            MyDevice.SetDeviceProperty(PropName, value);
                            ECore.CurrentAcquisitionEngine.PauseThreads = false;
                        }
                        catch (Exception ex)
                        {
                            ECore.CurrentAcquisitionEngine.PauseThreads = false;
                            ECore.LogErrorMessage("Could not set property " + PropName + " on " + DeviceName + "\n" + ex.Message, 1);
                            if (ex.InnerException !=null)
                                ECore.LogErrorMessage( ex.InnerException.Message  , 1);
                        }
                    }
                    if (OnPropValueSet != null)
                        OnPropValueSet(this, DeviceName, PropName, value);
                }
                ValueIsUpdated();
            }
        }
        /*public PropertyInfo(CMMCore Core, string DeviceName, string PropName)
        {
            this.DeviceName = DeviceName;
            this.core = Core;
            GetInfo (DeviceName,PropName );
        }*/
        private EasyCore ECore=null;
        public PropertyInfo(EasyCore ecore, Devices.MMDeviceBase Device, string DeviceName, string PropName)
        {
            ECore = ecore;
            MyDevice = Device;
            GetInfo (DeviceName,PropName );

        }
        public PropertyInfo(EasyCore ecore, string DeviceName, string PropName, string DefaultValue)
        {
            ECore = ecore;
            this.PropName = PropName;
            _Value = DefaultValue;
            HasAllowedValues = false;
            HasLimits = false;
        }
        public PropertyInfo(EasyCore ecore, string PropName, string _Value, bool HasLimits, double MinValue, double MaxValue,
            PropertyType tType, bool ReadOnly, bool HasAllowedValues, string[] AllowedValues)
        {
            ECore = ecore;
            this.PropName=PropName ;
            this._Value=_Value ;
            this.HasLimits=HasLimits;
            this.MinValue=MinValue;
            this.MaxValue=MaxValue;
            this.tType=tType;
            this.ReadOnly=ReadOnly;
            this.HasAllowedValues=HasAllowedValues;
            this.AllowedValues = AllowedValues;
        }

        /// <summary>
        /// Checks the information directly from the device
        /// </summary>
        /// <param name="DeviceName"></param>
        /// <param name="PropName"></param>
        private void GetInfo(string DeviceName,string PropName)
        {
                this.PropName=PropName ;
                MyDevice.GetDevicePropertyInfoDetails(PropName, out _Value, out HasLimits,
                    out MinValue, out MaxValue, out tType, out ReadOnly
                    , out HasAllowedValues, out AllowedValues);
        }

    }
}
