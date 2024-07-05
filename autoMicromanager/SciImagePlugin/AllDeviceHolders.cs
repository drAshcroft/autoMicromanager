﻿// DESCRIPTION:   
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

using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace Micromanager_net
{
    /// <summary>
    /// A COM helper for holding the SciImage plugin in a COM enabled application
    /// </summary>
    [Guid("1514adf6-7cb1-4561-0006-b75c1467149b")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class AllDeviceHolders : UserControl, IAllDeviceHolders 
    {
        public AllDeviceHolders()
        {
            InitializeComponent();
        }
        public void AddGUI(UserControl tabControl)
        {
            
           
                AddTabPage(tabControl );

           
        }

        private void AddTabPage(UserControl NewControl)
        {

            TabPage NewPage = new TabPage();
            
            this.tabControl1.Controls.Add(NewPage );

           
            NewPage.Controls.Add(NewControl);
            NewPage.Location = new System.Drawing.Point(4, 22);
            NewPage.Name = "tabPage1";
            NewPage.Padding = new System.Windows.Forms.Padding(3);
            NewPage.Size = new System.Drawing.Size(142, 124);
            NewPage.TabIndex = 0;
            NewPage.Text = "tabPage1";
            NewPage.UseVisualStyleBackColor = true;


            NewControl.Dock = DockStyle.Fill;

            try
            {
                string Caption = ((CoreDevices.UI.GUIDeviceControl)NewControl).Caption();
                NewPage.Text = Caption;
                NewPage.Name = Caption;
            }
            catch { }
            Refresh();
        }
    }

    [Guid("5A88092E-69DF-4bb8-0006-8FA83E550F20")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAllDeviceHolders
    {
        void AddGUI(UserControl tabControl);
    }
}
