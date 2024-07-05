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


namespace CoreDevices
{
    [Serializable]
    [Guid("1514adf6-7cb1-0029-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ChannelGroup:IChannelGroupCom 
    {
        List<ChannelState> _Channels = new List<ChannelState>();
        ChannelState _StartUpChannel;
        string _GroupName;
        bool _CombineChannels = false;
        string _RunScript = "";
        EasyCore ECore = null;

        public void AddChannel(ChannelState[] NewChannelStates)
        {
            _Channels.AddRange(NewChannelStates);
        }
        public void AddChannel(ChannelState NewChannelState)
        {
            _Channels.Add(NewChannelState);
        }
        public bool CombineChannels
        {
            get { return _CombineChannels; }
            set { _CombineChannels = value; }
        }

        public ChannelGroup(EasyCore eCore)
        {
            ECore = eCore;
        }

        /// <summary>
        /// A group will only show up for the whole application and saving if it us registered with the core
        /// </summary>
        public void RegisterGroupWithCore()
        {
            for (int i = 0; i < _Channels.Count; i++)
            {
                ChannelState cs=_Channels[i];
                if (ECore.Core.isGroupDefined(cs.ChannelName) == true)
                    ECore.Core.deleteConfigGroup(cs.ChannelName);
                ECore.Core.defineConfigGroup( cs.ChannelName );
                for (int j=0;j<cs.NumCommands() ;j++)
                {
                    string Devicename="";
                    string PropName="";
                    string PropValue="";
                    cs.GetCommand(j,out Devicename,out PropName,out PropValue );
                    ECore.Core.defineConfig(cs.ChannelName, cs.ChannelName + j.ToString(), Devicename, PropName, PropValue);
                }
            }
        }
        /// <summary>
        /// The startup channel is run before the whole aquisition.  It is defined seperate from all the other channels
        /// </summary>
        public void RunStartupChannel()
        {
            if (_StartUpChannel !=null)
                RunChannelEngine( _StartUpChannel) ;
        }
        /// <summary>
        /// This executes each of the channels.
        /// </summary>
        /// <param name="cs"></param>
        private void RunChannelEngine(ChannelState cs)
        {
            if (cs != null)
            {
                if (cs.NumCommands() > 0)
                {
                    for (int j = 0; j < cs.NumCommands(); j++)
                    {
                        cs.TestRunCommands();
                        //ECore.Core.setConfig(cs.ChannelName, cs.ChannelName + j.ToString());
                    }

                    //ECore.Core.waitForConfig(_GroupName, cs.ChannelName + (cs.NumCommands() - 1).ToString());
                }
            }
        }
        private int NextChannelToRun = 0;
        /// <summary>
        /// Used To run one indexed Channel
        /// </summary>
        /// <param name="ChannelNumber">Desired Channel</param>
        public void RunChannel(int ChannelNumber)
        {

            RunChannelEngine(_Channels[ChannelNumber]);
            NextChannelToRun =ChannelNumber+1;
            if (NextChannelToRun >=_Channels.Count ) NextChannelToRun =0;
        }
        /// <summary>
        /// Keeps a rotating index and keeps moving through all the channels
        /// </summary>
        public void RunNextChannel()
        {
            RunChannelEngine(_Channels[NextChannelToRun]);
            NextChannelToRun += 1;
            if (NextChannelToRun >= _Channels.Count) NextChannelToRun = 0;
        }

        /// <summary>
        /// The group can also a script before the channels run.  
        /// </summary>
        public string RunScript
        {
            get { return _RunScript; }
            set { _RunScript = value; }

        }


        public int NumChannels()
        {
            return _Channels.Count;
        }

        public ChannelState GetChannel(int ChannelNumber)
        {
            return _Channels[ChannelNumber];
        }

        /// <summary>
        /// A list of all the channels in the channelgroup
        /// </summary>
        public List<ChannelState> Channels
        {
            get { return _Channels; }

        }
        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }

        }
        public ChannelState StartUpChannel
        {
            get { return _StartUpChannel; }
            set { _StartUpChannel = value; }
        }

        /// <summary>
        /// A convience method for labview and COM
        /// </summary>
        /// <param name="eCore"></param>
        public void SetCore(EasyCore eCore)
        {
            ECore = eCore;
        }
       
    }

    [Guid("5A88092E-69FF-0029-AD8D-8FA83E550F20")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IChannelGroupCom
    {
        void SetCore(EasyCore eCore);
        List<ChannelState> Channels { get; }
        bool CombineChannels { get; set; }
        string GroupName { get; set; }
        string RunScript { get; set; }
        ChannelState StartUpChannel { get; set; }

        void RegisterGroupWithCore();
        void RunChannel(int ChannelNumber);
        void RunNextChannel();
        void RunStartupChannel();
    }
}
