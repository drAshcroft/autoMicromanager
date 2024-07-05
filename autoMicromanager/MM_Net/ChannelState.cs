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
using System.Drawing;
using System.Runtime.InteropServices;

namespace CoreDevices
{
    [Serializable]
    [Guid("1514adf6-7cb1-0030-7Ebb-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ChannelState:IChannelStateCom 
    {
        private bool _ShowFalseColor = false;
        private Color _ChannelColor=Color.Empty;
        private string _ChannelName="";

        public event EventHandler ChannelUpdated;

        /// <summary>
        /// Determines if itensity of translated into another color
        /// </summary>
        public bool ShowFalseColor
        {
            get { return _ShowFalseColor; }
            set 
            {
                _ShowFalseColor = value;
                if (ChannelUpdated != null) ChannelUpdated(this, EventArgs.Empty);
            
            }
        }
        
        /// <summary>
        /// if ShowFalseColor is true then this is the false color
        /// </summary>
        public Color ChannelColor
        {
            get { return _ChannelColor; }
            set { _ChannelColor = value; if (ChannelUpdated != null) ChannelUpdated(this, EventArgs.Empty); }

        }

        /// <summary>
        /// This is to make it easier for someone who does not have .Net to set the channel color
        /// </summary>
        /// <param name="Red"></param>
        /// <param name="Green"></param>
        /// <param name="Blue"></param>
        public void SetChannelColor(double Red, double Green, double Blue)
        {
            try
            {
                _ChannelColor = Color.FromArgb( (int)(255 * Red),(int)( 255 * Green),(int)( 255 * Blue));
                if (ChannelUpdated != null) ChannelUpdated(this, EventArgs.Empty);
            }
            catch { }

        }

        public string ChannelName
        {
            get { return _ChannelName; }
            set { _ChannelName = value; if (ChannelUpdated != null) ChannelUpdated(this, EventArgs.Empty); }
        }
        private EasyCore ECore = null;
        public ChannelState(EasyCore eCore)
        {
            ECore = eCore;
        }

        private List<MethodCall> ChannelCommands = new List<MethodCall>();
        
        /// <summary>
        /// Class that encapsulates setting a property in CMMCore
        /// </summary>
        private class MethodCall
        {
            public string DeviceName;
            public string MethodName;
            public object[] Parameters;
            public MethodCall(string DeviceName, string MethodName, object[] Parameters)
            {
                this.DeviceName = DeviceName;
                this.MethodName = MethodName;
                this.Parameters = Parameters;
            }
            public MethodCall(string DeviceName, string MethodName, string  Parameters)
            {
                this.DeviceName = DeviceName;
                this.MethodName = MethodName;
                this.Parameters =new object[]{ Parameters};
            }
        }
        
        /// <summary>
        /// Clears the list of commands
        /// </summary>
        public void ClearCommands()
        {
            ChannelCommands.Clear(); if (ChannelUpdated != null) ChannelUpdated(this, EventArgs.Empty);
        }
        public void AddCommand(string DeviceName, string Methodname, object[] Parameters)
        {
            ChannelCommands.Add(new MethodCall(DeviceName, Methodname, Parameters));
            if (ChannelUpdated != null) ChannelUpdated(this, EventArgs.Empty);
        }
        public void AddCommand(string DeviceName, string Methodname, string Parameters)
        {
            ChannelCommands.Add(new MethodCall(DeviceName, Methodname, Parameters ));
            if (ChannelUpdated != null) ChannelUpdated(this, EventArgs.Empty);
        }
        public int NumCommands()
        {
            return ChannelCommands.Count;
        }
       
        /// <summary>
        /// Returns the information needed to send a command
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="DeviceName"></param>
        /// <param name="MethodName"></param>
        /// <param name="Parameters"></param>
        public void GetCommand(int Index, out string DeviceName, out string MethodName, out object[] Parameters)
        {
            DeviceName = ChannelCommands[Index].DeviceName;
            MethodName = ChannelCommands[Index].MethodName;
            Parameters = ChannelCommands[Index].Parameters;
        }
        public void GetCommand(int Index, out string DeviceName, out string MethodName, out string Value)
        {
            DeviceName = ChannelCommands[Index].DeviceName;
            MethodName = ChannelCommands[Index].MethodName;
            Value = (string)ChannelCommands[Index].Parameters[0];
        }
        
        /// <summary>
        /// This will run the command on the core
        /// </summary>
        public void TestRunCommands()
        {
            foreach (MethodCall mc in ChannelCommands)
            {
                try
                {
                    ECore.GetDevice(mc.DeviceName).RunCommand(mc.MethodName, mc.Parameters);
                }
                catch { }

            }
        }
        public void SetCore(EasyCore ECore)
        {
            this.ECore = ECore;
        }
    }

    [Guid("5A88092E-69FF-0030-AD8D-8FA83E550F20")]
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IChannelStateCom
    {
        void SetCore(EasyCore ECore);
        Color ChannelColor { get; set; }
        string ChannelName { get; set; }
        bool ShowFalseColor { get; set; }

        void AddCommand(string DeviceName, string Methodname, object[] Parameters);
        void AddCommand(string DeviceName, string Methodname, string Parameters);
        void ClearCommands();
        void GetCommand(int Index, out string DeviceName, out string MethodName, out object[] Parameters);
        void GetCommand(int Index, out string DeviceName, out string MethodName, out string Value);
        int NumCommands();
        void TestRunCommands();

    }
}
