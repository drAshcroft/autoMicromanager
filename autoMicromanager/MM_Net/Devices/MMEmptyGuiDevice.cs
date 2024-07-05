using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CWrapper;

namespace CoreDevices.Devices
{
    /// <summary>
    /// This class is used for those devices that are only gui.  For example scripts.  This should be updated more to provide nice automatic property handling
    /// </summary>
    [Guid("1514adf6-7cb1-0024-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable]
    public class MMEmptyGuiDevice:MMDeviceBase ,IEmptyGuiDeviceCom 
    {
        private string[] DefaultPropertyNames = new string[] { "None" };
        public MMEmptyGuiDevice(EasyCore ECore,string DeviceLabel,string LibraryName,string DeviceAdapter )
        {
            deviceName = DeviceLabel;
            SetCore(ECore);
            Ecore.RegisterDevice(this);
        }
        public MMEmptyGuiDevice(EasyCore ECore, string DeviceLabel)
        {
            deviceName = DeviceLabel;
            SetCore(ECore);
            ECore.RegisterDevice(this);
        }
        public string[] DevicePropertyList
        {
            get { return DefaultPropertyNames; }
            set { DefaultPropertyNames = value; }
        }
        public override  void MakeOffical()
        {
        }
        public override  void StopDevice()
        {
        }

        /// <summary>
        /// This allows your graphical interface to interact with the rest of the system with the same property list paradim.  You have to 
        /// intercept the calls that would be aimed instead at the micromanager core.
        /// </summary>
        /// <param name="ClearProperties"></param>
        protected override  void InitializePropertyList(bool ClearProperties)
        {
            
            //  sv = core.getLoadedDevices();
            Dictionary<string, PropertyInfo> PIs;
            if (ClearProperties == true)
                PIs = new Dictionary<string, PropertyInfo>();
            else
                PIs = AllProperties;

            foreach (string s in DefaultPropertyNames )
            {
                PIs.Add(s, new PropertyInfo(ECore, DeviceName, s, ""));

            }

            foreach (KeyValuePair<string, PropertyInfo> pi in PIs)
            {
                pi.Value.OnPropValueSet += new OnPropValueSetEvent(pi_OnPropValueSet);

            }
            AllProperties = PIs;
        }

    }

    [Guid("5A88092E-69DF-0024-AD8D-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IEmptyGuiDeviceCom
    {
        

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
