using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using CoreDevices;
using System.IO;

namespace MMUI_ScriptModules
{
    internal  class EvaluatePython
    {
        cTextWriter ctw = new cTextWriter();
        public EvaluatePython()
        {
            engine = Python.CreateEngine();
            scope = engine.CreateScope();
            //
           // Console.SetOut(ctw);
           // log_out.Close(); 
        }
        private ScriptEngine engine;
        private ScriptScope scope;

        public void SetOutputPath()
        {
            try
            {
                engine.Runtime.IO.SetOutput(new IPEStreamWrapper(IPEStreamWrapper.IPEngineResponse), engine.Runtime.IO.InputEncoding);
                Console.SetOut(ctw);
            }
            catch { }
        }

        public string evaluate(string code,IronPythonScriptModule Host, EasyCore ECore)
        {

            CoreDevices.NI_Controls.NIEasyCore ni = new CoreDevices.NI_Controls.NIEasyCore();
            ni.Ecore = ECore;
            scope.SetVariable("ECore", ECore);
            scope.SetVariable("Console", Host);
            scope.SetVariable("ScriptPanel", Host.CodePanel);
            scope.SetVariable("NIEasyCore", ni);
            try
            {
                ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
                source.Execute(scope);
                
            }
            catch (Exception ex)
            {
                return "Error executing code: " + ex.ToString();
            }

            //if (!scope.ContainsVariable("x"))
            {
                //  return "x was deleted";
            }
            //string result = scope.GetVariable<object>("x").ToString();
            return "Code Executed";
        }
    }
}
