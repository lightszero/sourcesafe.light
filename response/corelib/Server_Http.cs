using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{
    partial class Server
    {
        System.Net.HttpListener _http_listener = new System.Net.HttpListener();
        //string _http_basehost = "ss.light";
        #region _http_init
        void _http_init(string url)
        {

            _http_listener.Prefixes.Add(url);
            try
            {
                _http_listener.Start();
                _http_beginRecv();
                logger.Log_Warn("Http on:" + url + config.basehost);
            }
            catch (Exception err)
            {
                logger.Log_Error(err.ToString());
                logger.Log_Error("Http Init fail.");
                return;
            }
            if (System.IO.Directory.Exists("publish") == false)
            {
                System.IO.Directory.CreateDirectory("publish");
            }

        }

        void _http_beginRecv()
        {
            try
            {
                _http_listener.BeginGetContext(_http_onRequest, null);
            }
            catch (Exception err)
            {
                if (exited) return;
                else logger.Log_Error("err");
            }
        }
        void _http_onRequest(IAsyncResult hr)
        {
            _http_beginRecv();
            if (exited) return;
            var reqcontext = _http_listener.EndGetContext(hr);
            if (reqcontext.Request.HttpMethod.ToLower() == "post")
            {
                byte[] buf = new byte[reqcontext.Request.ContentLength64];
                int bufread = 0;

                AsyncCallback onPostReq = null;
                onPostReq = (phr) =>
                     {
                         bufread += reqcontext.Request.InputStream.EndRead(phr);
                         if (bufread == reqcontext.Request.ContentLength64)
                         {
                             _http_OnHttpIn(reqcontext, buf);
                         }
                         else
                         {
                             reqcontext.Request.InputStream.BeginRead(buf, bufread, (int)reqcontext.Request.ContentLength64 - bufread, onPostReq, null);
                         }
                     };
                reqcontext.Request.InputStream.BeginRead(buf, 0, buf.Length, onPostReq, null);
            }
            else
            {
                _http_OnHttpIn(reqcontext, null);
            }
        }
        #endregion
        #region _http_response
        void _http_response(System.Net.HttpListenerContext req, byte[] buf)
        {
            req.Response.ContentEncoding = System.Text.Encoding.UTF8;
            req.Response.ContentLength64 = buf.Length;
            req.Response.ContentType = "application/octet-stream";
            AsyncCallback onResponse = onResponse = (rhr) =>
            {
                req.Response.OutputStream.EndWrite(rhr);
                req.Response.Close();
            };
            req.Response.OutputStream.BeginWrite(buf, 0, buf.Length, onResponse, null);
        }
        void _http_response(System.Net.HttpListenerContext req, string str)
        {
            req.Response.ContentEncoding = System.Text.Encoding.UTF8;
            req.Response.ContentType = "text/plain";
            byte[] bufout = System.Text.Encoding.UTF8.GetBytes(str);
            req.Response.ContentLength64 = bufout.Length;

            AsyncCallback onResponse = onResponse = (rhr) =>
            {
                req.Response.OutputStream.EndWrite(rhr);
                req.Response.Close();
            };
            req.Response.OutputStream.BeginWrite(bufout, 0, bufout.Length, onResponse, null);
        }
        void _http_response_html(System.Net.HttpListenerContext req, string str)
        {
            req.Response.ContentEncoding = System.Text.Encoding.UTF8;
            req.Response.ContentType = "text/html";
            byte[] bufout = System.Text.Encoding.UTF8.GetBytes(str);
            req.Response.ContentLength64 = bufout.Length;

            AsyncCallback onResponse = onResponse = (rhr) =>
            {
                req.Response.OutputStream.EndWrite(rhr);
                req.Response.Close();
            };
            req.Response.OutputStream.BeginWrite(bufout, 0, bufout.Length, onResponse, null);
        }


        #endregion
        #region _http_quit
        void _http_Stop()
        {
            _http_listener.Abort();
        }
        #endregion

        void _http_OnHttpIn(System.Net.HttpListenerContext req, byte[] postdata)
        {
            try
            {
                if (req.Request.Url.AbsolutePath.IndexOf("/publish") == 0)
                {
                    _http_download(req);
                    return;
                }

                if (req.Request.Url.AbsolutePath == "/" + config.basehost)
                {
                    string strreturn = null;//统一json返回串
                    switch (req.Request.QueryString["c"])
                    {
                        case "res_get"://Get返回二进制格式
                            _http_cmdres_get(req);
                            return;

                        case "play":
                            strreturn = _http_cmd_play(req);
                            break;
                        case "ping":
                            strreturn = _http_cmd_ping(req);
                            break;
                        case "login":
                            strreturn = _http_cmd_login(req);
                            break;
                        case "res_info"://信息
                            strreturn = _http_cmdres_info(req);
                            break;
                        case "res_op"://操作
                            strreturn = _http_cmdres_op(req);
                            break;
                        case "ver_begin"://信息
                            strreturn = _http_cmdver_begin(req);
                            break;
                        case "ver_end"://操作
                            strreturn = _http_cmdver_end(req);
                            break;
                        default:
                            _http_cmd_help(req);//Help返回html
                            return;
                    }
                    _http_response(req, strreturn);
                    //command
                    return;
                }
                else
                {
                    _http_response_html(req, "<html>not found page.</html>");

                }

            }
            catch (Exception err)
            {
                _http_response(req, err.ToString());
            }
        }
        void _http_cmd_help(System.Net.HttpListenerContext req)
        {
            string helpstr =
@"<html>
<head> <meta charset=""UTF-8""></head>
<a>FORALL 00 ?c=play try to get the gamelist</a><br/>
<hr/>
<a>FORALL 01 ?c=ping test that if this server is a sourcesafe.light server or not.</a><br/>
<hr/>
<a>Artist 00 ?c=login&g=[gamename]&u=[username]&p=[passwod]</a><br/>
<a>                  login for a game</a>        <br/>            
<hr/> 
Artist 01 res_info 获取资源信息<br/>
?c=res_list&t=token&v=<br/>
list ress in game,v=0 for nearest version,v=other for specfil version<br/>             
<hr/>
Artist 02 res_get 获取资源<br/>
?c=res_list&t=token&v=<br/>
list ress in game,v=0 for nearest version,v=other for specfil version<br/>             
<hr/>
Artist 03 ver_begin 获取并锁定资源<br/>
?c=res_list&t=token&v=<br/>
list ress in game,v=0 for nearest version,v=other for specfil version<br/>             
<hr/>
Artist 04 ver_end 解锁资源<br/>
?c=res_list&t=token&v=<br/>
list ress in game,v=0 for nearest version,v=other for specfil version<br/>             
<hr/>
Artist 05 res_op 资源操作<br/>
?c=res_list&t=token&v=<br/>
list ress in game,v=0 for nearest version,v=other for specfil version<br/>             
<hr/>
</html>";
            _http_response_html(req, helpstr);
        }

        string _http_cmd_play(System.Net.HttpListenerContext req)
        {//获取GameList
            MyJson.JsonNode_Object map = new MyJson.JsonNode_Object();
            map.SetDictValue("status", 0);
            map["list"] = new MyJson.JsonNode_Object();
            var dict = map["list"].asDict();
            foreach (var g in config.games)
            {
                map["list"].SetDictValue("name", g.name);
                map["list"].SetDictValue("desc", g.desc);
                map["list"].SetDictValue("admin", g.admin);
            }

            return map.ToString();
        }
        string _http_cmd_ping(System.Net.HttpListenerContext req)
        {//固定协议
            MyJson.JsonNode_Object map = new MyJson.JsonNode_Object();
            map["status"] = new MyJson.JsonNode_ValueNumber(0);
            map["ver"] = new MyJson.JsonNode_ValueString("V0.01");
            map["sign"] = new MyJson.JsonNode_ValueString("sha1");
            return map.ToString();
        }



        class LoginInfo
        {
            public string username;
            public string token;
            public string game;
        }
        Dictionary<string, LoginInfo> tokens = new Dictionary<string, LoginInfo>();
        string _http_cmd_login(System.Net.HttpListenerContext req)
        {
            MyJson.JsonNode_Object map = new MyJson.JsonNode_Object();
            
            string game = req.Request.QueryString["g"];
            string username = req.Request.QueryString["u"];
            string password = req.Request.QueryString["p"];
            var _user=config.getArtist(username);
            var _game =config.getGame(game);
            if(_user==null)
            {
                map.SetDictValue("status", -1001);
                map.SetDictValue("msg", "user not exist.");
                return map.ToString();
            }
            if(_user.code!=password)
            {
                map.SetDictValue("status", -1002);
                map.SetDictValue("msg", "user password is error.");
                return map.ToString();
            }
            if(_game==null)
            {
                if (_user.level > 10)
                {
                    if (System.IO.Directory.Exists("games/" + game) == false)
                    {
                        System.IO.Directory.CreateDirectory("games/" + game);
                    }
                    if(System.IO.Directory.Exists("games/" + game))
                    {
                        _game = new ServerConfig.Game();
                        _game.name = game;
                        _game.desc = "";
                        _game.admin = username;
                        _game.users.Add(username);
                        config.games.Add(_game);
                        config.Save();
                    }
                }
                if(_game==null)
                {
                    map.SetDictValue("status", -1003);
                    map.SetDictValue("msg", "game not exist.when superadmin Login to a empty game will create one.");
                    return map.ToString();
                }
            }

            if (_game.users.Contains(username) == false)
            {
                map.SetDictValue("status", -1004);
                map.SetDictValue("msg", "you can not edit this game.");
                return map.ToString();
            }

            LoginInfo info = new LoginInfo();
            info.game = game;
            info.username = username;
            info.token = Guid.NewGuid().ToString();
            tokens[info.token] = info;
            map.SetDictValue("status", 0);
            map.SetDictValue("token", info.token);
            return map.ToString();
        }


    }
}
