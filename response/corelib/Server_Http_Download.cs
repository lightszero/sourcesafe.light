using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corelib
{
    partial class Server
    {
        class PathCache
        {

            public PathCache(string path)
            {
                
            }
            Dictionary<string, byte[]> caches = new Dictionary<string, byte[]>();
            public void Lock()
            {
                lock(lockobj)
                {
                    if (locked == 0)
                        locked = 1;
               }
            }
            public void UnLock()
            {
                lock (lockobj)
                {
                    if (locked == 1)
                        locked = 0;
                }
            }
            class LockObj
            {

            }
            LockObj lockobj = new LockObj();
            public int locked
            {
                get;
                private set;
            }
            public byte[] GetBytes(string filepath)
            {

                lock (lockobj)
                {
                    if (locked == 1)
                        return null;
                    if (caches.ContainsKey(filepath))
                    {
                        return caches[filepath];
                    }

                    byte[] filebuf = System.IO.File.ReadAllBytes(filepath);
                    caches[filepath] = filebuf;
                    return filebuf;
                }
            }
        }
        //文件下载缓存机制
        Dictionary<string, PathCache> _http_downcache = new Dictionary<string, PathCache>();
        void _http_download(System.Net.HttpListenerContext req)
        {
            string path = req.Request.Url.AbsolutePath.Substring(1);
            string[] subpath = path.Split(new char[]{'/'},StringSplitOptions.RemoveEmptyEntries);
            if(subpath.Length==1)
            {//return the list;
                string downstr = "<html>";
                foreach(var p in _http_downcache)
                {
                    downstr += p.Key + ",locked=" + p.Value.locked +"<br/>";
                }
                downstr += "</html>";
                _http_response_html(req, downstr);
                return;
            }
            string path2 = subpath[1];

            if (System.IO.File.Exists(path) == false)
            {
                req.Response.StatusCode = 404;
                req.Response.Close();
            }
            else
            {
                if(_http_downcache.ContainsKey(path2)==false)
                {
                    _http_downcache[path2] = new PathCache("/publish/"+path2);
                }
                byte[] filebuf = _http_downcache[path2].GetBytes(path);
                //byte[] filebuf = System.IO.File.ReadAllBytes(path);
                string range = req.Request.Headers.Get("range");
                int from = 0;
                int len = filebuf.Length;
                if(string.IsNullOrWhiteSpace(range)==false)
                {
                    string[] ss = range.Split(new char[] { '-', '=' });
                    from = int.Parse(ss[1]);
                    len = filebuf.Length - from;
                    if(string.IsNullOrWhiteSpace(ss[2])==false)
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
                        }catch
                        {

                        }
                    }
                };
                req.Response.OutputStream.BeginWrite(filebuf, from, len, onResponse, null);
            }
        }



    }
}
