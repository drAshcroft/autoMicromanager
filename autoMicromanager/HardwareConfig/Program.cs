using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HardwareConfig
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //start the easycore because it is just easier to do all the paths this way.
            CoreDevices.NI_Controls.NIEasyCore nEcore = new CoreDevices.NI_Controls.NIEasyCore();
            CoreDevices.EasyCore ecore = nEcore.StartEcore("");
            Micromanager_net.Setup.HardwareSetup2 hs = new Micromanager_net.Setup.HardwareSetup2(ecore, null,true );

            //run the application
            Application.Run( hs  );

            try
            { Environment.Exit(0); }
            catch { }
            try
            {
                Application.Exit();
            }
            catch { }

        }
    }
}
