using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MMUI_ScriptModules
{
    public class cTextWriter:TextWriter 
    {
        IronPythonScriptModule Console;
        public cTextWriter(IronPythonScriptModule ConsoleParent)
        {
            Console = ConsoleParent;
        }
        public cTextWriter()
        {
            Console = null;
        }
        
        public override void WriteLine(string value)
        {
            if (Console !=null)
                Console.WriteLine(value);
            //base.WriteLine(value);
        }
        public override void Write(string value)
        {
            base.Write(value);
        }
        public override void Write(object value)
        {
            base.Write(value);
        }
        public override void WriteLine(object value)
        {
            base.WriteLine(value);
        }
        public override Encoding Encoding
        {
            get { return Encoding.ASCII ; }
        }
    }
}
