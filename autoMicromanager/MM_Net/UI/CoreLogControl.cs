using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Micromanager_net.UI
{
    public partial class CoreLogControl : UserControl
    {
        public CoreLogControl()
        {
            InitializeComponent();
            try
            {
                string CorelogFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Corelog.txt");
                CoreLogRTB.LoadFile(CorelogFile);
            }
            catch { }
        }
        private void refreshB_Click(object sender, EventArgs e)
        {

            string CorelogFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Corelog");
            File.Copy(CorelogFile + ".txt", CorelogFile + "1.txt", true);
            TextReader tr = new StreamReader(CorelogFile + "1.txt");
            //FileStream  tr = new FileStream(CorelogFile , FileMode.Open);

            string CoreText = tr.ReadToEnd();
            CoreLogRTB.Text = CoreText;
            //CoreLogRTB.LoadFile(CorelogFile + "1.txt");
        }
        public void AddErrorString(string ErrorString)
        {
            coreErrors.Text += ErrorString = "\n";
        }// DESCRIPTION:   
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

    }
}
