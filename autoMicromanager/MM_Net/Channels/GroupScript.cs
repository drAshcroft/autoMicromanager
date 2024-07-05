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
    /// <summary>
    /// Not yet implemented.  This will allow for editing of channel scripts and test runs
    /// </summary>
    public partial class GroupScript : Form
    {
        public GroupScript(EasyCore eCore)
        {
            InitializeComponent();
           
        }
        public void AddObject(string ObjectName, object Object)
        {
           
        }
        public void OpenScript(string Filename)
        {
            
        }
        public void LoadScript(string Script)
        {
            throw new Exception("This is not implemented yet");
        }
        void scriptControl1_Execute(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch
            {

            }
        }

        private void scriptControl1_Load(object sender, EventArgs e)
        {

        }

       

    }
}
