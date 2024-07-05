using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoreDevices;

namespace Micromanager_net.UI
{
    public partial class StageJoystick : UserControl, CoreDevices.UI.GUIDeviceControl
    {
        EasyCore ECore;
        CoreDevices.Devices.XYStage xyStage;
        public StageJoystick()
        {
            InitializeComponent();
        }

        public string DeviceType()
        {
            return "GUIControl";
        }
        public string Caption()
        {
            return "Joystick";
        }
        public Control GetControl()
        {
            return this;
        }
        private string extraInformation="";
        public string ExtraInformation 
        {
            get { return extraInformation; }
            set { extraInformation = value; }
        }
        private CoreDevices.Devices.MMEmptyGuiDevice MyGuiDev;
        public void SetCore(EasyCore Ecore, string DeviceName)
        {
            if (DeviceName == "")
                DeviceName = "StageJoystick";
            try
            {
                MyGuiDev = (CoreDevices.Devices.MMEmptyGuiDevice)Ecore.GetDevice(DeviceName);
            }
            catch
            {
                MyGuiDev = new CoreDevices.Devices.MMEmptyGuiDevice(Ecore, DeviceName);
               
            }
            xyStage = Ecore.MMXYStage;
            this.ECore = Ecore;

            //Checks if a directx joystick is available. if one is not then uses the mouse for input
            try
            {
                string[] Joys = joystick1.AvailableJoysticks();
                if (Joys[0] != null && Joys[0] != "")
                    joystick1.StartJoystickDirectPlay(Joys[0]);

            }
            catch { }
        }
        /// <summary>
        /// The joystick primarily works with the stage right now.  Its only target is the offical stage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Mag"></param>
        private void joystick1_OnJoystickMoved_1(object sender, double x, double y, double Mag)
        {
            try
            {
                xyStage.MoveStageRelative(-1 * x * ECore.ScreenSize, -1 * y * ECore.ScreenSize);
            }
            catch { }
        }

        /// <summary>
        /// Freeze motion
        /// </summary>
        /// <param name="sender"></param>
        private void joystick1_OnJoyStickReleased(object sender)
        {
            if (xyStage != null)
                xyStage.StopStage();
           
        }


        /// <summary>
        /// Allows buttons to be pushed on joystick that slow down or speed up the stage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ButtonStates"></param>
        private void joystick1_OnJoyStickButtonPushed(object sender, bool[] ButtonStates)
        {
            //todo:Need to make the buttons user mappable to allow all sorts of functions the be performed by the joystick
            if (ButtonStates[0] == true)
            {
                double step = ECore.MMFocusStage.StepSize_um * 4;
                if (ButtonStates[4]) step = step * 10;
                ECore.MMFocusStage.SetPositionRelative(step);
            }
            if (ButtonStates[1] == true)
            {
                double step = ECore.MMFocusStage.StepSize_um * -4;
                if (ButtonStates[4]) step = step * 10;
                ECore.MMFocusStage.SetPositionRelative(step - ECore.MMFocusStage.StepSize_um);

            }
        }
    }
}
