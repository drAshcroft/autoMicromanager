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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using JoystickInterface;

namespace Micromanager_net
{
    public partial class StageJoystick : DockContent
    {
        
        CoreDevices.EasyCore ECore;
        CoreDevices.XYStage xyStage;
        public StageJoystick()
        {
            InitializeComponent();
        }

        public void SetCore(CoreDevices.EasyCore  Ecore)
        {
            xyStage =Ecore.MMXYStage ;
            this.ECore = Ecore;

            try
            {
                string[] Joys = joystick1.AvailableJoysticks();
                if (Joys[0] != null && Joys[0]!="")
                    joystick1.StartJoystickDirectPlay(Joys[0]);

            }
            catch { }
        }

        private void joystick1_OnJoystickMoved_1(object sender, double x, double y, double Mag)
        {
            try
            {
                xyStage.MoveStageRelative(-1*x * ECore.ScreenSize, -1*y * ECore.ScreenSize);
            }
            catch { }
        }

        private void joystick1_OnJoyStickReleased(object sender)
        {
            xyStage.StopStage();
        }

               

        private void joystick1_OnJoyStickButtonPushed(object sender, bool[] ButtonStates)
        {
            if (ButtonStates[0] == true)
            {
                double step = ECore.MMFocusStage.StepSize * 4;
                if (ButtonStates[4]) step = step * 10;
                ECore.MMFocusStage.SetPositionRelative(step);
            }
            if (ButtonStates[1] == true)
            {
                double step = ECore.MMFocusStage.StepSize * -4;
                if (ButtonStates[4]) step = step * 10;
                ECore.MMFocusStage.SetPositionRelative(step - ECore.MMFocusStage.StepSize);

            }
        }

    }
}
