using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{
    partial class Server
    {
        //下载文件协议
        void _http_cmdres_get(System.Net.HttpListenerContext req)
        {
            byte[] file = new byte[1];
            file[0] = 0;
            _http_response(req, file);
        }
        string _http_cmdres_info(System.Net.HttpListenerContext req)
        {//获取GameList
            MyJson.JsonNode_Object map = new MyJson.JsonNode_Object();


            return map.ToString();
        }
        string _http_cmdres_op(System.Net.HttpListenerContext req)
        {//获取GameList
            MyJson.JsonNode_Object map = new MyJson.JsonNode_Object();


            return map.ToString();
        }
        string _http_cmdver_begin(System.Net.HttpListenerContext req)
        {//获取GameList
            MyJson.JsonNode_Object map = new MyJson.JsonNode_Object();
            return map.ToString();
        }
        string _http_cmdver_end(System.Net.HttpListenerContext req)
        {//获取GameList
            MyJson.JsonNode_Object map = new MyJson.JsonNode_Object();
            return map.ToString();
        }
    }
}
