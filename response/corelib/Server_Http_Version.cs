using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{

    partial class Server
    {
        Dictionary<string, VersionGroup> versions = new Dictionary<string, VersionGroup>();
        //下载文件协议
        void _http_cmd_get(System.Net.HttpListenerContext req)
        {
            byte[] file = new byte[1];
            file[0] = 0;
            _http_response(req, file);
        }
        string _http_cmd_rpc(System.Net.HttpListenerContext req, byte[] post)
        {

            string qu = req.Request.Url.Query.Substring(1);
            string[] sp = qu.Split(new char[] { '?', '&', '=' });

            var code = "";
            string usertoken = null;
            for (int i = 0; i < sp.Length / 2; i++)
            {
                if (sp[i * 2] == "s")
                {
                    code = Uri.UnescapeDataString(sp[i * 2 + 1]);
                    //break;
                }
                if (sp[i * 2] == "t")
                {
                    usertoken = sp[i * 2 + 1];
                }

            }
            string s = code;
            try
            {
                var tt = envRPC.ParserToken(s);
                var ee = envRPC.Expr_CompilerToken(tt, true);
                var content = envRPC.CreateContent();
                content.DefineAndSet("token", typeof(string), usertoken);
                content.DefineAndSet("post", typeof(byte[]), post);
                var value = envRPC.Expr_Execute(ee, content);

                MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
                obj.SetDictValue("status", 0);
                if (value == null)
                {

                    obj.SetDictValue("rt", "null");
                    obj.SetDictValue("rv", "null");
                }
                else
                {
                    if (value.type != null)
                        obj.SetDictValue("rt", value.type.ToString());
                    else
                        obj.SetDictValue("rt", "null");
                    if (value.value != null)
                        if (value.value is MyJson.IJsonNode)
                            obj.SetDictValue("rv", value.value as MyJson.IJsonNode);
                        else
                            obj.SetDictValue("rv", value.value.ToString());
                    else
                        obj.SetDictValue("rv", "null");
                }
                return obj.ToString(); ;
            }
            catch (Exception err)
            {
                MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
                obj.SetDictValue("status", -1010);
                obj.SetDictValue("msg", err.ToString());
                return obj.ToString(); ;
            }
        }
        CSLE.CLS_Environment envRPC = null;
        void _init_ScriptRPC()
        {
            envRPC = new CSLE.CLS_Environment(new ScriptLogger(logger));
            envRPC.RegType(new CSLE.RegHelper_Type(typeof(byte[]), "byte[]"));

            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPC(_rpc_help), "help"));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPCJ<int>(_rpc_ver_info), "ver_info"));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPCJ<int, byte[]>(_rpc_ver_begin), "ver_begin"));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPCJ<int>(_rpc_ver_finish), "ver_finish"));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPCJ<int>(_rpc_ver_cancel), "ver_finish"));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPCJ<int, string, byte[]>(_rpc_ver_addfile), "ver_addfile"));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPCJ<int, string, byte[]>(_rpc_ver_updatefile), "ver_updatefile"));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPCJ<int, string>(_rpc_ver_deletefile), "ver_deletefile"));


            this.logger.Log_Warn("Init script engine(for RPC):C#Light" + envScript.version);

        }
        delegate string RPC(string token);
        delegate string RPC<T>(string token, T _param);
        delegate string RPC<T1, T2>(string token, T1 _p1, T2 _p2);
        delegate string RPC<T1, T2, T3>(string token, T1 _p1, T2 _p2, T3 _p3);
        delegate string RPC<T1, T2, T3, T4>(string token, T1 _p1, T2 _p2, T3 _p3, T4 _p4);
        delegate MyJson.IJsonNode RPCJ(string token);
        delegate MyJson.IJsonNode RPCJ<T>(string token, T _param);
        delegate MyJson.IJsonNode RPCJ<T1, T2>(string token, T1 _p1, T2 _p2);
        delegate MyJson.IJsonNode RPCJ<T1, T2, T3>(string token, T1 _p1, T2 _p2, T3 _p3);
        delegate MyJson.IJsonNode RPCJ<T1, T2, T3, T4>(string token, T1 _p1, T2 _p2, T3 _p3, T4 _p4);
        string CheckToken(string token, out string user)
        {


            if (tokens.ContainsKey(token) == false)
            {
                throw new Exception("token not exist.");
            }
            string game = tokens[token].game;
            user = tokens[token].username;
            if (config.getGame(game).users.Contains(tokens[token].username) == false)
            {
                throw new Exception("not right for game.");

            }
            return game;

        }
        //返回something
        string _rpc_help(string token)
        {
            string user;
            CheckToken(token, out user);


            return "help.";
        }
        //查询版本信息
        MyJson.IJsonNode _rpc_ver_info(string token, int ver)
        {
            string user;
            string game = CheckToken(token, out user);
            var vgroup = versions[game];
            if (ver == 0)
            {
                ver = vgroup.nowver;
            }
            MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
            obj.SetDictValue("nowver", vgroup.nowver);
            obj.SetDictValue("reqver", ver);
            Version v = vgroup.getVersion(ver);
            if (v != null)
            {
                obj.SetDictValue("regverfilenum", v.reslist.Count);
            }
            return obj;
        }
        class Cmd
        {

        }
        class AtomOP
        {
            public AtomOP(int ver, byte[] sign)//用签名验证 Cmd是否匹配
            {
                this.ver = ver;
                this.sign = sign;
            }
            public int ver
            {
                get;
                private set;
            }
            byte[] sign;
            List<Cmd> cmds = new List<Cmd>();
            public void AddCmd(Cmd cmd)
            {

            }
            public bool Run(string path,MyJson.JsonNode_Array info)
            {
                return false;
            }
        }
        Dictionary<string, AtomOP> commits = new Dictionary<string, AtomOP>();
        MyJson.IJsonNode _rpc_ver_begin(string token, int ver, byte[] post)
        {
            string user;
            string game = CheckToken(token, out user);
            var vgroup = versions[game];
            MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
            if (ver != vgroup.nowver + 1)
            {
                obj.SetDictValue("status", -2001);
                obj.SetDictValue("msg", "version error. got verinfo");
                return obj;
            }
            if (commits.ContainsKey(user))
            {
                obj.SetDictValue("status", -2003);
                obj.SetDictValue("msg", "have a commit.end it.");
                obj.SetDictValue("commitver", commits[user].ver);
                return obj;
            }
            commits[user] = new AtomOP(ver, post);
            obj.SetDictValue("status", 0);
            obj.SetDictValue("targetver", ver);
            return obj;
        }
        MyJson.IJsonNode _rpc_ver_finish(string token, int ver)
        {
            string user;
            string game = CheckToken(token, out user);
            var vgroup = versions[game];
            MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
            if (commits.ContainsKey(user) == false)
            {
                obj.SetDictValue("status", -2004);
                obj.SetDictValue("msg", "not found commit.");
                return obj;
            }
            var commit = commits[user];
            MyJson.JsonNode_Array array = new MyJson.JsonNode_Array();
            bool b = commit.Run(vgroup.path, array);
            obj.SetDictValue("info", array);
            if (!b)
            {
                obj.SetDictValue("status", -2005);
                obj.SetDictValue("msg", "commit fall.");

            }
            else
            {
                obj.SetDictValue("status", 0);
            }
            return null;
        }
        MyJson.IJsonNode _rpc_ver_cancel(string token, int ver)
        {
            string user;
            string game = CheckToken(token, out user);
            var vgroup = versions[game];
            MyJson.JsonNode_Object obj = new MyJson.JsonNode_Object();
            if (commits.ContainsKey(user))
            {
                commits.Remove(user);
                obj.SetDictValue("status", 0);
            }
            else
            {
                obj.SetDictValue("status", -2006);
                obj.SetDictValue("msg", "no need to cancel any.");
            }
            return null;
        }
        MyJson.IJsonNode _rpc_ver_addfile(string token, int ver, string filename, byte[] post)
        {
            return null;
        }
        MyJson.IJsonNode _rpc_ver_updatefile(string token, int ver, string filename, byte[] post)
        {
            return null;
        }
        MyJson.IJsonNode _rpc_ver_deletefile(string token, int ver, string filename)
        {
            return null;
        }

    }
}
