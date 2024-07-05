using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoreDevices.Channels;
using CoreDevices;

namespace CoreDevices.Channels
{
    [Serializable]
    public partial class ChannelSetupControl : UserControl
    {
        private EasyCore ECore;
        private ChannelState _ChannelState=null;
        public event  EventHandler CloseRequested;

        private StateCommand[] Commands = new StateCommand[7];

        public ChannelSetupControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Allows the creation and editing of a channel state
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="ChannelState"></param>
        public void SetupControl(EasyCore eCore,ref ChannelState ChannelState)
        {
            ECore = eCore;
            _ChannelState = ChannelState;
            _ChannelState.ChannelUpdated += new EventHandler(_ChannelState_ChannelUpdated);
            //InitializeComponent();
            _ChannelState_ChannelUpdated(null, EventArgs.Empty);
        }

        void _ChannelState_ChannelUpdated(object sender, EventArgs e)
        {
            stateCommand1.SetCore(ECore);
            stateCommand2.SetCore(ECore);
            stateCommand3.SetCore(ECore);
            stateCommand4.SetCore(ECore);
            stateCommand5.SetCore(ECore);
            stateCommand6.SetCore(ECore);
            stateCommand7.SetCore(ECore);
            Commands[0] = stateCommand1;
            Commands[1] = stateCommand2;
            Commands[2] = stateCommand3;
            Commands[3] = stateCommand4;
            Commands[4] = stateCommand5;
            Commands[5] = stateCommand6;
            Commands[6] = stateCommand7;
            for (int i = 0; i < _ChannelState.NumCommands(); i++)
            {
                string DeviceName = "";
                string MethodName = "";
                string param = "";
                _ChannelState.GetCommand(i, out DeviceName, out MethodName, out param);
                Commands[i].SetSelectedCommand(new string[] { DeviceName, MethodName, param });
            }
            ChannelNameTB.Text = _ChannelState.ChannelName;
            colorPicker1.SelectedColor = _ChannelState.ChannelColor;
            UseFalseColorCB.Checked = _ChannelState.ShowFalseColor; 
        }

        private void CancelB_Click(object sender, EventArgs e)
        {
            _ChannelState = null;
            if (CloseRequested != null) CloseRequested(this, EventArgs.Empty);
        }

        private void SavB_Click(object sender, EventArgs e)
        {
            if (ChannelNameTB.Text == "")
            {
                MessageBox.Show("You must enter an unique channel name to save.");
                return;

            }
            _ChannelState.ChannelName = ChannelNameTB.Text;
            _ChannelState.ShowFalseColor = UseFalseColorCB.Checked;
            if (UseFalseColorCB.Checked != true)
                _ChannelState.ChannelColor = Color.Empty;
            else
                _ChannelState.ChannelColor = colorPicker1.SelectedColor ;
            _ChannelState.ClearCommands();
            for (int i = 0; i < Commands.Length; i++)
            {
                string[] command=  Commands[i].GetSelectedCommand();
                if (command[0].Trim() != "")
                    _ChannelState.AddCommand(command[0], command[1], command[2]);
            }
            if (CloseRequested != null) CloseRequested(this, EventArgs.Empty);
        }
    }
}
