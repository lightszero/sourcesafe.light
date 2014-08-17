using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{
    public enum ResoureType
    {
        SingleFile,             //单个文件，只知道相同与不同
        MultilineText,          //按行处理的文本文件
        JsonText,               //按Json树处理的文本文件
        GridData8,              //按网格分块处理的图片数据
        GridData32,
    };
    partial class Server
    {
        public class VersionGroup//管理版本的类
        {
            private VersionGroup()
            {

            }


            public static VersionGroup Load(string name, string path)
            {
                System.Xml.Serialization.XmlSerializer xmls =new System.Xml.Serialization.XmlSerializer(typeof(VersionGroup));
                 VersionGroup group=null;
                try
                    
                {
                  
                using(var s=System.IO.File.OpenRead(path+"/vergroup.xml"))
                {
                    group = xmls.Deserialize(s) as VersionGroup;
                }
                }
                catch
                {

                }

                if (group == null)
                {
                    group = new VersionGroup();
                    group.path = path;
                    group.Save();
                }
                group.name = name;
                group.path = path;
                return group;
            }
            public void Save()
            {
                System.Xml.Serialization.XmlSerializer xmls = new System.Xml.Serialization.XmlSerializer(typeof(VersionGroup));
                using (var s = System.IO.File.Create(path + "/vergroup.xml"))
                {
                    xmls.Serialize(s, this);
                }
            }
            string path;
            public string name;

            public int nowver;

            public class Resource//每个资源
            {
                public string name; //名称
                public ResoureType type;   //类型
                public bool patch;  //是补丁还是存档
                public int basever;//基础版本，补丁就是针对此版本的补丁，存档就是当前版本
            }
            public class Version//每个版本
            {
                public int id;//版本ID
                public List<Resource> reslist = new List<Resource>();
                //public
            }
            public List<Version> versions = new List<Version>();
        }
        Dictionary<string, VersionGroup> versions = new Dictionary<string, VersionGroup>();
        //下载文件协议
        void _http_cmd_get(System.Net.HttpListenerContext req)
        {
            byte[] file = new byte[1];
            file[0] = 0;
            _http_response(req, file);
        }
        string _http_cmd_rpc(System.Net.HttpListenerContext req)
        {
            return "";
        }
        CSLE.CLS_Environment envRPC = null;
        void _init_ScriptRPC()
        {
            envRPC = new CSLE.CLS_Environment(new ScriptLogger(logger));
            envRPC.RegFunction(new CSLE.RegHelper_Function(new RPC(_rpc_help), "help"));

            this.logger.Log_Warn("Init script engine(for RPC):C#Light" + envScript.version);

        }
        delegate string RPC(string token);
        delegate string RPC<T>(string token,T _param);
        delegate string RPC<T1, T2>(string token, T1 _p1, T2 _p2);
        delegate string RPC<T1, T2,T3>(string token, T1 _p1, T2 _p2,T3 _p3);
        delegate string RPC<T1, T2,T3,T4>(string token, T1 _p1, T2 _p2,T3 _p3,T4 _p4);
        string _rpc_help(string token)
        {
            return "help.";
        }
    
    }
}
