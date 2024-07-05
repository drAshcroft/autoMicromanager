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

namespace CoreDevices.Channels
{
    public partial class ChannelSetup : Form
    {
        private EasyCore ECore;
        private ChannelState _ChannelState=null;
        private StateCommand[] Commands = new StateCommand[7];

        /// <summary>
        /// Allows the creation and editing of a channel state
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="ChannelState"></param>
        public ChannelSetup(EasyCore eCore,ref ChannelState ChannelState)
        {
            ECore = eCore;
            _ChannelState = ChannelState;
            InitializeComponent();
            channelSetupControl1.SetupControl(eCore, ref ChannelState);
            channelSetupControl1.CloseRequested += new EventHandler(channelSetupControl1_CloseRequested);
        }

        void channelSetupControl1_CloseRequested(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
