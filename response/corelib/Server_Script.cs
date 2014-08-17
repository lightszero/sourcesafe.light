using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{
    partial class Server
    {
        void _init_Script()
        {
            envScript = new CSLE.CLS_Environment(new ScriptLogger(logger));
            envScript.RegFunction(new CSLE.RegHelper_Function(new Action(_script_help), "help"));
            envScript.RegFunction(new CSLE.RegHelper_Function(new Action(_script_exit), "exit"));
            envScript.RegFunction(new CSLE.RegHelper_Function(new Action(_script_clear), "clear"));
            this.logger.Log_Warn("Init Script enging:C#Light" + envScript.version);
        }
        CSLE.CLS_Environment envScript = null;

        void _script_exit()
        {
            this.logger.Log_Warn("Server close.");
            this.exited = true;
        }

        void _script_help()
        {
            this.logger.Log("==================================");
            this.logger.Log("type help() got this.");
            this.logger.Log("type exit() quit the server.");

        }
        void _script_clear()
        {
            this.logger.Clear();
        }
    }
}
