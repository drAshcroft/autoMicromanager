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
    public partial class ChannelGroupDefinition : Form
    {
        private EasyCore ECore;
        private ChannelGroup _ChannelGroup;

        /// <summary>
        /// Provides a way to show and edit an existing channel group
        /// </summary>
        /// <param name="eCore"></param>
        /// <param name="ChannelGroup"></param>
        public ChannelGroupDefinition(EasyCore eCore,ref ChannelGroup ChannelGroup)
        {
            
            InitializeComponent();

            ChannelBoxes[0] = ChannelCB1;
            ChannelBoxes[1] = ChannelCB2;
            ChannelBoxes[2] = ChannelCB3;
            ChannelBoxes[3] = ChannelCB4;
            ChannelBoxes[4] = ChannelCB5;
            ChannelBoxes[5] = ChannelCB6;
            ChannelBoxes[6] = ChannelCB7;


            _ChannelGroup = ChannelGroup;
            ECore = eCore;
            
            RelistAllChannels();
            ShowGroup();
        }

         
        private void EditScriptButton_Click(object sender, EventArgs e)
        {
            GroupScript gs = new GroupScript(ECore);
            gs.ShowDialog();
        }

        ChannelState StartUpChannel=null;

        private void button1_Click(object sender, EventArgs e)
        {
            ChannelState newChannel = new ChannelState(ECore);
            ChannelSetup cs = new ChannelSetup(ECore, ref newChannel);
            cs.ShowDialog();
            if (newChannel != null)
            {
                StartupCommandCB.Text = newChannel.ChannelName;
                StartUpChannel = newChannel;
            }
        }
        //CoreDevices.ChannelState[] Channels = new Micromanager_net.CoreDevices.ChannelState[8];
        ComboBox[] ChannelBoxes = new ComboBox[7];
        #region Edit_Channels

        private void ShowGroup()
        {
            if ( _ChannelGroup.GroupName!=null) GroupNameTB.Text = _ChannelGroup.GroupName;
            if (_ChannelGroup.StartUpChannel!=null) StartupCommandCB.Text = _ChannelGroup.StartUpChannel.ChannelName;
            runScriptRTB.Text = _ChannelGroup.RunScript;
            CombineImagesRB.Checked = _ChannelGroup.CombineChannels;
            int nChannels=  _ChannelGroup.Channels.Count;
            for (int i = 0; i < nChannels; i++)
            {
                if (_ChannelGroup.Channels[i]!=null) ChannelBoxes[i] .Text = _ChannelGroup.Channels[i].ChannelName;
            }
            
        }

        private void RelistAllChannels()
        {
            ListChannels(StartupCommandCB);
            for (int i = 0; i < ChannelBoxes.Length; i++)
            {
                ListChannels(ChannelBoxes[i]);
            }
        }
        private void ListChannels(ComboBox cBox)
        {
            string[] Channelnames = ECore.GetAllChannelNames();
            cBox.Items.Clear();
            foreach (string s in Channelnames)
            {
                if (s != null)
                    cBox.Items.Add(s);
            }

        }
        private void SaveChannel(int index, ComboBox cBox)
        {
            ChannelState newChannel;
            try
            {
                newChannel = ECore.GetChannel(cBox.Text ) ;
            }
            catch
            {
                newChannel = new ChannelState(ECore);
            }
            ChannelSetup cs = new ChannelSetup(ECore, ref newChannel);
            cs.ShowDialog();

            if (newChannel != null)
            {
                ECore.AddChannel(newChannel);
                cBox.Text = newChannel.ChannelName;
                RelistAllChannels();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveChannel(0, ChannelCB1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveChannel(1, ChannelCB2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveChannel(2, ChannelCB3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveChannel(3, ChannelCB4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveChannel(4, ChannelCB5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveChannel(5, ChannelCB6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveChannel(6, ChannelCB7);
        }
        #endregion

        private void CancelB_Click(object sender, EventArgs e)
        {
            _ChannelGroup = null;
            this.Close();
        }

        private void SaveB_Click(object sender, EventArgs e)
        {
            if (GroupNameTB.Text == "")
            {
                MessageBox.Show("You must enter an unique group name to save the group.");
                return;
            }
            _ChannelGroup.GroupName = GroupNameTB.Text;
            _ChannelGroup.CombineChannels = CombineImagesRB.Checked;
            _ChannelGroup.Channels.Clear();
            for (int i = 0;i< ChannelBoxes.Length; i++)
            {
                ChannelState cs;
                try
                {
                    cs = ECore.GetChannel(ChannelBoxes[i] .Text);
                }
                catch
                {
                    cs = null;
                }
                if (cs != null && cs.ChannelName != "")
                    _ChannelGroup.Channels.Add(cs);
            }
            _ChannelGroup.StartUpChannel = StartUpChannel;
            _ChannelGroup.RunScript = runScriptRTB.Text;
            ECore.AddGroup(_ChannelGroup);
            this.Close();
        }


    }
}
