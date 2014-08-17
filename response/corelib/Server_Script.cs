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
            envScript.RegFunction(new CSLE.RegHelper_Function(new Action<string, string, int>(_script_artist_new), "artist_new"));
            envScript.RegFunction(new CSLE.RegHelper_Function(new Action<string, string, int>(_script_artist_reset), "artist_reset"));
            envScript.RegFunction(new CSLE.RegHelper_Function(new Action<string>(_script_admin_set), "admin_set"));
            envScript.RegFunction(new CSLE.RegHelper_Function(new Action<string>(_script_host_reset), "host_reset"));
            this.logger.Log_Warn("Init script engine:C#Light" + envScript.version);
        }
        CSLE.CLS_Environment envScript = null;

        void _script_exit()
        {
            _http_Stop();
            this.logger.Log_Warn("Server close.");
            this.exited = true;
        }

        void _script_help()
        {
            this.logger.Log_Warn("SourceSafe.light Server");
            this.logger.Log_Warn("ver=" + corelib.Server.ver);
            this.logger.Log("==================================");
            this.logger.Log("type help() got this.");
            this.logger.Log("type exit() quit the server.");
            this.logger.Log("type clear() clear the log onscreen.");
            this.logger.Log(@"type artist_new(""name"",""code"",level) create artist,level 1~9");
            this.logger.Log(@"type artist_reset(""name"",""code"",level) reset artist");
            this.logger.Log("type admin_set(name) set a artist as superadmin.");
            this.logger.Log("type host_reset(name) default host is http://xxx/ss.light, can set 'ss.light' to other.");
        }
        void _script_artist_new(string name, string code, int level)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code) || level < 1 || level > 9)
            {
                this.logger.Log_Error("user and code can not be null.level must be 1~9");
                return;
            }
            if (config.getArtist(name) == null)
            {
                ServerConfig.Artist a = new ServerConfig.Artist();
                a.name = name;
                //a.code = code;
                byte[] sb = System.Text.Encoding.UTF8.GetBytes(code);
                byte[] wt = sha1.ComputeHash(sb);
                a.code = Convert.ToBase64String(sb, 0, sb.Length);
                a.level = level;
                config.users.Add(a);
                config.Save();
                this.logger.Log("user added.");
            }
            else
            {
                this.logger.Log_Error("user has created,use artist_reset().");
            }
        }
        void _script_artist_reset(string name, string code, int level)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code) || level < 1 || level > 9)
            {
                this.logger.Log_Error("user and code can not be null.level must be 1~9");
                return;
            }
            if (config.getArtist(name) == null)
            {
                this.logger.Log_Error("user not exist,use artist_new().");


            }
            else
            {
                ServerConfig.Artist a = config.getArtist(name);
                //a.name = name;
                //a.code = code;
                byte[] sb = System.Text.Encoding.UTF8.GetBytes(code);
                byte[] wt = sha1.ComputeHash(sb);
                a.code = Convert.ToBase64String(sb, 0, sb.Length);
                a.level = level;
                //config.users.Add(a);
                config.Save();
                this.logger.Log("user reseted.");
            }
        }
        void _script_admin_set(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                this.logger.Log_Error("user can not be null.");
                return;
            }
            if (config.getArtist(name) == null)
            {
                this.logger.Log_Error("user not exist,use artist_new().");


            }
            else
            {
                ServerConfig.Artist a = config.getArtist(name);
                a.level = 99;
                config.superadmin = a.name;
                config.Save();
                this.logger.Log("user " + name + " became superadmin");
            }
        }
        void _script_clear()
        {

            this.logger.Clear();
        }

        void _script_host_reset(string name)
        {
            if (config.basehost == name)
            {
                this.logger.Log_Error("you input a same name,no need to reset.");
                return;
            }
            if (name.Contains(' ') || name.Contains('/') || name.Contains('\\'))
            {
                this.logger.Log_Error("name can't use.");
                return;
            }
            config.basehost = name;
            config.Save();
            foreach (var h in _http_listener.Prefixes)
            {
                this.logger.Log("Nowhost is changeto:" + h + name);
            }
        }
    }
}
