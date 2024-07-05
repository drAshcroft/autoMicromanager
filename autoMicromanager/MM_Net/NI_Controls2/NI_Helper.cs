using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CWrapper;

namespace CoreDevices.NI_Controls
{
    [Guid("1474adf6-7cb1-5461-0006-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None )]
    [Serializable]
    /// <summary>
    /// This class is used to make a number of operations easier when the program is automated by labview
    /// </summary>
    public class NiHelpers:INIHelpers 
    {
        private EasyCore Ecore;

        private List<UserControl> StartedGUIs = new List<UserControl>();

        public NiHelpers(EasyCore ECore)
        {
            this.Ecore = ECore;
        }

        public string[] AvailableGUIS()
        {
            return Ecore.GetAllDeviceGUIs();
        }
        public UserControl StartGui(string GUITypeName)
        {
            UserControl c=(UserControl)Ecore.GetDeviceGUI(GUITypeName);
            StartedGUIs.Add (c);
            return c;

        }
        internal List<UserControl> AllStartedGUIs()
        {
            return StartedGUIs;
        }

        #region CreateDevices
        public Devices.Camera CreateCameraDevice(string DeviceName, string LibraryName, string DeviceAdapter)
        {
            try
            {
                return new Devices.NormalCamera(Ecore, DeviceName, LibraryName, DeviceAdapter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public Devices.Camera  CreateScanningConfocalDevice(string DeviceName, string LibraryName, string DeviceAdapter)
        {
            return new Devices.ScanningConfocalCamera(Ecore, DeviceName, LibraryName, DeviceAdapter);
        }
        public Devices.FilterWheel CreateFilterWheelDevice(string DeviceName, string LibraryName, string DeviceAdapter)
        {
            return new Devices.FilterWheel(Ecore, DeviceName, LibraryName, DeviceAdapter);
        }
        public Devices.FunctionGenerator CreateFunctionGeneratorDevice(string DeviceName, string LibraryName, string DeviceAdapter)
        {
            return new Devices.FunctionGenerator(Ecore, DeviceName, LibraryName, DeviceAdapter);
        }
        public Devices.StateDevice CreateStateDevice(string DeviceName, string LibraryName, string DeviceAdapter)
        {
            return new Devices.StateDevice(Ecore, DeviceName, LibraryName, DeviceAdapter);
        }
        public Devices.XYStage CreateXYStageDevice(string DeviceName, string LibraryName, string DeviceAdapter)
        {
            return new Devices.XYStage(Ecore, DeviceName, LibraryName, DeviceAdapter);
        }
        public Devices.ZStage CreateZStageDevice(string DeviceName, string LibraryName, string DeviceAdapter)
        {
            return new Devices.ZStage(Ecore, DeviceName, LibraryName, DeviceAdapter);
        }
        #endregion
        #region ConvertDevices
        public Devices.Camera CastToCamera(Devices.MMDeviceBase Device)
        {
            return (Devices.Camera)Device;
        }
        public Devices.FilterWheel CastToFilterWheel(Devices.MMDeviceBase Device)
        {
            return (Devices.FilterWheel)Device;
        }
        public Devices.FunctionGenerator CastToFunctionGenerator(Devices.MMDeviceBase Device)
        {
            return (Devices.FunctionGenerator)Device;
        }
        public Devices.NormalCamera CastToNormalCamera(Devices.MMDeviceBase Device)
        {
            return (Devices.NormalCamera)Device;
        }
        public Devices.ScanningConfocalCamera CastToScanningConfocalCamera(Devices.MMDeviceBase Device)
        {
            return (Devices.ScanningConfocalCamera)Device;
        }
        public Devices.StateDevice CastToStateDevice(Devices.MMDeviceBase Device)
        {
            return (Devices.StateDevice)Device;
        }
        public Devices.XYStage CastToXYStage(Devices.MMDeviceBase Device)
        {
            return (Devices.XYStage)Device;
        }
        public Devices.ZStage CastToZStage(Devices.MMDeviceBase Device)
        {
            return (Devices.ZStage)Device;
        }
        #endregion

        #region OtherConverters
        public string[] ConvertStrVectortoArray(StrVector inVector)
        {
            string[] temp = new string[inVector.Count];
            inVector.CopyTo(temp);
            return (temp);
        }

        #endregion

        #region Channels
        public ChannelGroup CreateChannelGroup(string[] ChannelNames, Channels.ChannelSetupControl[] ChannelSetupControls)
        {

            ChannelGroup CG = new ChannelGroup(Ecore);
            int i = 0;
            foreach (string s in ChannelNames)
            {
                ChannelState CS=new ChannelState(Ecore);
                CS.ChannelName = s;
                CG.AddChannel(CS );
                try
                {
                    ChannelSetupControls[i].SetupControl(Ecore, ref CS);
                }
                catch { }
                i++;
            }
            return CG;
        }
        #endregion 

        public void DoNetEvents()
        {
            Application.DoEvents();
        }
    }

    [Guid("1474adf6-7cb1-5461-0006-b75c14671497")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual )]
    public interface INIHelpers
    {
       
        Devices.Camera CreateCameraDevice(string DeviceName, string LibraryName, string DeviceAdapter);
        Devices.Camera CreateScanningConfocalDevice(string DeviceName, string LibraryName, string DeviceAdapter);
        Devices.FilterWheel CreateFilterWheelDevice(string DeviceName, string LibraryName, string DeviceAdapter);
        Devices.FunctionGenerator CreateFunctionGeneratorDevice(string DeviceName, string LibraryName, string DeviceAdapter);
        Devices.StateDevice CreateStateDevice(string DeviceName, string LibraryName, string DeviceAdapter);
        Devices.XYStage CreateXYStageDevice(string DeviceName, string LibraryName, string DeviceAdapter);
        Devices.ZStage CreateZStageDevice(string DeviceName, string LibraryName, string DeviceAdapter);
        Devices.Camera CastToCamera(Devices.MMDeviceBase Device);
        Devices.FilterWheel CastToFilterWheel(Devices.MMDeviceBase Device);
        Devices.FunctionGenerator CastToFunctionGenerator(Devices.MMDeviceBase Device);
        Devices.NormalCamera CastToNormalCamera(Devices.MMDeviceBase Device);
        Devices.ScanningConfocalCamera CastToScanningConfocalCamera(Devices.MMDeviceBase Device);
        Devices.StateDevice CastToStateDevice(Devices.MMDeviceBase Device);
        Devices.XYStage CastToXYStage(Devices.MMDeviceBase Device);
        Devices.ZStage CastToZStage(Devices.MMDeviceBase Device);
        string[] ConvertStrVectortoArray(StrVector inVector);
        ChannelGroup CreateChannelGroup(string[] ChannelNames, Channels.ChannelSetupControl[] ChannelSetupControls);
        string[] AvailableGUIS();
        UserControl StartGui(string GUITypeName);
    }
}
