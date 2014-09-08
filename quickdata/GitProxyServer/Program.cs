using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitProxyServer
{
    partial class Program
    {
        //不需要RPC
        //可以作为内网安全服务器

        //文件位置分为 ，0缓存，1服务器存储、2GIT备份
        //1.Post File 接口,异步，Post完成仅代表文件提交至缓存，何时备份不知道
        //2.Get FileStatus //查询文件状态，0 1 2 三种
        //3.Get 直接从这里下载文件

        //4.增加仓库

        //post都在缓存中，有一个线程一直从缓存往服务器存储写
        //定时，往GIT备份

        static bool bRun = true;
        static void Main(string[] args)
        {
            int callcount = 0;
            Init();

            while (bRun)
            {
                callcount++;
                if (callcount > 10) callcount = 0;

                System.Threading.Thread.Sleep(100);
                int t = Console.CursorTop;
                int l = Console.CursorLeft;
                string strdyn = "running";
                for (int i = 0; i < callcount; i++)
                    strdyn += '.';
                WriteString(strdyn);
                Console.SetCursorPosition(l, t);

            }
            Console.ReadLine();
        }
        static void Init()
        {
            _response_restore();//检查并回复目录

            string url = "http://*:25080/";

            _http_listener.Prefixes.Add(url);
            try
            {
                _http_listener.Start();
                _http_beginRecv();
                Console.WriteLine("Http on:" + url);
            }
            catch (Exception err)
            {
                Log_Error(err.ToString());
                Log_Error("Http Init fail.");
                bRun = false;
                return;
            }

            
        }

        static void WriteString(string str)
        {
            string sss = str;
            for (int i = sss.Length; i < 40; i++)
                sss += ' ';
            Console.WriteLine(sss);

        }

        static void Log(string str)
        {
            Console.WriteLine(str);
        }
        static void Log_Warn(string str)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        static void Log_Error(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ResetColor();
        }
    }
}
