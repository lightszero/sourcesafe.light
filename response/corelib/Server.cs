using System;
using System.Collections.Generic;
using System.Text;

namespace corelib
{
    public partial class Server
    {
        ServerConfig config = null;
        public static string ver
        {
            get
            {
                return "0.01Alpha";
            }
        }
        public bool exited
        {
            get;
            private set;
        }
        public ILogger logger
        {
            get;
            private set;
        }
        public Server(ILogger logger)
        {
            this.logger = logger;
        }
        System.Security.Cryptography.SHA1 sha1= new System.Security.Cryptography.SHA1Managed();
        public void Start(string url = "http://*:25080/")
        {
            config = ServerConfig.Load();
            config.Save();
            this.logger.Log_Warn("SourceSafe.light Server");
            this.logger.Log_Warn("ver=" + corelib.Server.ver);
            if(string.IsNullOrWhiteSpace( config.superadmin))
            {
                this.logger.Log_Error("superadmin has not found.read help and create one.");
            }

            _http_init(url);
            _init_Script();
            this.logger.Log("type help() for more .");
        }
        public void Call(string cmd)
        {
            try
            {
                var t = envScript.ParserToken(cmd);
                var e = envScript.Expr_CompilerToken(t, true);
                envScript.Expr_Execute(e);
            }
            catch (Exception err)
            {
                logger.Log_Error("Sever error:" + err.ToString());
            }
        }

    }

    public interface ILogger
    {
        void Log(string str);
        void Log_Error(string str);
        void Log_Warn(string str);
        void Clear();
    }
    class ScriptLogger : CSLE.ICLS_Logger
    {
        ILogger logger;
        public ScriptLogger(ILogger logger)
        {
            this.logger = logger;
        }
        public void Log(string str)
        {
            logger.Log(str);
        }

        public void Log_Error(string str)
        {
            logger.Log_Error(str);
        }

        public void Log_Warn(string str)
        {
            logger.Log_Warn(str);
        }
    }

}
