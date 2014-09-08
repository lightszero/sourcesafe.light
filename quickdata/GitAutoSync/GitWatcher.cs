using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GitAutoSync
{
    public interface IConsole
    {
        void Show(string line);
        void ShowErr(string line);
    }
    public class GitWatcher
    {
        Process cmdP = new Process();
        IConsole console;
        //System.Threading.Thread tread;
        public void Start(IConsole console,string firstcmd,string _params,string workingpath)
        {
            this.console = console;
            cmdP.StartInfo = new ProcessStartInfo(firstcmd, _params);
            cmdP.StartInfo.RedirectStandardError=true;
            cmdP.StartInfo.RedirectStandardOutput=true;
            cmdP.StartInfo.WorkingDirectory = workingpath;
            cmdP.StartInfo.RedirectStandardInput=true;
            cmdP.StartInfo.UseShellExecute = false;
            cmdP.StartInfo.CreateNoWindow = true;

            //cmdP.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            //    {
            //        this.console.Show(e.Data+"\n");
            //    };
            //cmdP.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            //{
            //    this.console.ShowErr(e.Data+"\n");
            //};
            cmdP.Start();
            //cmdP.BeginOutputReadLine();
            //cmdP.BeginErrorReadLine();
            var treadMsg = new System.Threading.Thread(threadWatchMsgOutPut);
            treadMsg.IsBackground = true;//后台线程才能自动退掉
            treadMsg.Start();
            var treadErr = new System.Threading.Thread(threadWatchErrOutPut);
            treadErr.IsBackground = true;//后台线程才能自动退掉
            treadErr.Start();
        }
        //bool bStop = false;
        //public void Stop()
        //{
            
        //    //tread.Abort();
        //    //tread.Interrupt();
        //    //cmdP.Close();
        //    bStop = true;
        //}
        public void WriteLine(string sline)
        {
            cmdP.StandardInput.WriteLine(sline);
            //Lines.Enqueue(sline);
        }
        System.Collections.Concurrent.ConcurrentQueue<string> Lines =new System.Collections.Concurrent.ConcurrentQueue<string>();
        void threadWatchMsgOutPut()
        {
          
            char[] buf =new char[256];
            while(true)
            {
                int len = cmdP.StandardOutput.Read(buf, 0, 255);
                //int len = cmdP.StandardOutput.Read(buf, 0, 2);
                string text = "";
                for (int i = 0; i < len; i++)
                {
                    text += buf[i];
                    if (buf[i] == '\n')
                    {
                        console.Show(text);
                        text = "";
                    }
                }
                if (text.Length > 0)
                {
                    console.Show(text);
                }
              
                System.Threading.Thread.Sleep(10);
            }
        }
        void threadWatchErrOutPut()
        {

            char[] buf = new char[256];
            while (true)
            {
                int len = cmdP.StandardError.Read(buf, 0, 255);
                string text = "";
                for (int i = 0; i < len; i++)
                {
                    text += buf[i];
                    if (buf[i] == '\n')
                    {
                        console.ShowErr(text);
                        text = "";
                    }
                }
                if (text.Length > 0)
                {
                    console.ShowErr(text);
                }

                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
