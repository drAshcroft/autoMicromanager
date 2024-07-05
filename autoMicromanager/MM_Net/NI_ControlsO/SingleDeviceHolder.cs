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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Micromanager_net.NI_Controls
{
    [Guid("1514adf6-7cb1-4561-0012-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class SingleDeviceHolder : UserControl,ISingleDeviceHolder 
    {
        public SingleDeviceHolder()
        {
            InitializeComponent();
        }
        public void AddControl(UserControl NewControl)
        {
            NewControl.Dock = DockStyle.Fill;
            Controls.Add(NewControl);
            this.Refresh();
        }
    }

    [Guid("5A88092E-69DF-4bb8-0012-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISingleDeviceHolder
    {
        void AddControl(UserControl NewControl);
    }
}
