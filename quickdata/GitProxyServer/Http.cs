using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitProxyServer
{
    partial class Program
    {
        static System.Net.HttpListener _http_listener = new System.Net.HttpListener();
        static void _http_beginRecv()
        {
            try
            {
                _http_listener.BeginGetContext(_http_onRequest, null);
            }
            catch (Exception err)
            {
                if (!bRun) return;
                else Log_Error("err");
            }
        }
        static void _http_onRequest(IAsyncResult hr)
        {
            _http_beginRecv();
            if (!bRun) return;
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
        static System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1Managed();
        static void _http_OnHttpIn(System.Net.HttpListenerContext req, byte[] postdata)
        {
            try
            {
                if (req.Request.Url.AbsolutePath.IndexOf("/data") == 0)
                {
                    _http_download(req);
                    return;
                }
                else if (req.Request.Url.AbsolutePath == "/crossdomain.xml")
                {
                    string xmlr = "<?xml version=\"1.0\"?><cross-domain-policy><allow-access-from domain=\"*\" /></cross-domain-policy>";
                    _http_response(req, xmlr);

                }
                else if (req.Request.Url.AbsolutePath == "/filepost")
                {
                    string strreturn = null;//统一json返回串
                    //1.Post File 接口,异步，Post完成仅代表文件提交至缓存，何时备份不知道
                    //2.Get FileStatus //查询文件状态，0 1 2 三种
                    string resp = req.Request.QueryString["d"];
                    string file = req.Request.QueryString["f"];
                    string hash = req.Request.QueryString["h"];
                    string len = req.Request.QueryString["l"];
                    byte[] data = postdata;
                    if (int.Parse(len) != data.Length)
                    {
                        strreturn = "{\"status\"=-1,\"msg\"=\"len 与数据不匹配\"";
                        _http_response(req, strreturn);
                        return;
                    }
                    string b64 = Convert.ToBase64String(sha1.ComputeHash(data));
                    if (b64 != hash)
                    {
                        strreturn = "{\"status\"=-1,\"msg\"=\"hash 与数据不匹配\"";
                        _http_response(req, strreturn);
                        return;
                    }
                    bool b = _response_Save(resp, file, data);
                    if (b)
                    {
                        strreturn = "{\"status\"=0,\"msg\"=\"保存成功\"";
                        _http_response(req, strreturn);
                        return;
                    }
                    else
                    {
                        strreturn = "{\"status\"=-2,\"msg\"=\"其他原因保存失败\"";
                        _http_response(req, strreturn);
                        return;
                    }
                    _http_response(req, strreturn);
                    //command
                    return;
                }
                //else if (req.Request.Url.AbsolutePath == "/filestatus")
                //{

                //}
                //else if (req.Request.Url.AbsolutePath == "/responselist")
                //{

                //}
                //else if (req.Request.Url.AbsolutePath == "/responsereg")
                //{

                //}
                else
                {
                    _http_response_html(req, @"
<html>not found page.<hr/>
<a>/filepost?d=database&f=filename&h=sha1code&l=filelength <POST:filedata> for send file</a><hr/>
<a>/filestatus?d=database&f=filename</a><hr/>
<a>/responselist</a><hr/>
<a>/responsereg?d=database&git=gitaddresswithcode</a><hr/>
</html>");

                }

            }
            catch (Exception err)
            {
                _http_response(req, err.ToString());
            }
        }
        static void _http_response(System.Net.HttpListenerContext req, byte[] buf)
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
        static void _http_response(System.Net.HttpListenerContext req, string str)
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
        static void _http_response_html(System.Net.HttpListenerContext req, string str)
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

        static void _http_download(System.Net.HttpListenerContext req)
        {
            string path = req.Request.Url.AbsolutePath.Substring(1);
            string[] subpath = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //string path2 = subpath[1];

            if (System.IO.File.Exists(path) == false)
            {
                req.Response.StatusCode = 404;
                req.Response.Close();
            }
            else
            {

                byte[] filebuf = System.IO.File.ReadAllBytes(path);
                string range = req.Request.Headers.Get("range");
                int from = 0;
                int len = filebuf.Length;
                if (string.IsNullOrWhiteSpace(range) == false)
                {
                    string[] ss = range.Split(new char[] { '-', '=' });
                    from = int.Parse(ss[1]);
                    len = filebuf.Length - from;
                    if (string.IsNullOrWhiteSpace(ss[2]) == false)
                    {
                        int end = int.Parse(ss[2]);
                        len = end - from;
                    }

                }
                req.Response.AddHeader("Content-Range", "bytes " + from + "-");
                req.Response.ContentEncoding = System.Text.Encoding.UTF8;
                req.Response.ContentLength64 = filebuf.Length;
                req.Response.ContentType = "application/octet-stream";
                AsyncCallback onResponse = onResponse = (rhr) =>
                {
                    try
                    {
                        req.Response.OutputStream.EndWrite(rhr);
                        req.Response.Close();
                    }
                    catch
                    {
                        try
                        {
                            req.Response.Close();
                        }
                        catch
                        {

                        }
                    }
                };
                req.Response.OutputStream.BeginWrite(filebuf, from, len, onResponse, null);
            }
        }
    }
}
