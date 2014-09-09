using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace GitProxyServer
{
    partial class Program
    {
        static string[] _response_callgit(string workingpath, string _params)
        {
            List<string> list = new List<string>();
            var cmdP = new Process();
            cmdP.StartInfo = new ProcessStartInfo("git", _params);
            cmdP.StartInfo.RedirectStandardError = true;
            cmdP.StartInfo.RedirectStandardOutput = true;
            cmdP.StartInfo.WorkingDirectory = workingpath;
            cmdP.StartInfo.RedirectStandardInput = true;
            cmdP.StartInfo.UseShellExecute = false;
            cmdP.StartInfo.CreateNoWindow = false;

            cmdP.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if(e.Data!=null)
                        list.Add(e.Data);
                };
            cmdP.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                if (e.Data != null)
                    list.Add(e.Data);
            };
            cmdP.Start();
            cmdP.BeginOutputReadLine();
            cmdP.BeginErrorReadLine();

            while (cmdP.HasExited == false)
            {
                System.Threading.Thread.Sleep(1);
            }
            System.Threading.Thread.Sleep(10);
            return list.ToArray();
        }
        static bool HaveString(IList<string> list, string txt)
        {
            foreach (var s in list)
            {
                if (s.Contains(txt)) return true;
            }
            return false;
        }
        static bool HaveString(IList<string> list, string txt, string txt2)
        {
            foreach (var s in list)
            {
                if (s == null) continue;
                if (s.Contains(txt) && s.Contains(txt2)) return true;
            }
            return false;
        }
        static List<string> responses = new List<string>();
        static void _response_restore()
        {
            Log("_response_restore");
            if (System.IO.Directory.Exists("data") == false)
            {
                System.IO.Directory.CreateDirectory("data");
            }
            string[] dataDir = System.IO.Directory.GetDirectories("data");
            foreach (var _d in dataDir)
            {

                string dir = System.IO.Path.GetFullPath(_d);
                string d = System.IO.Path.GetFileName(_d);
                var returnlist = _response_callgit(dir, "status");
                if (HaveString(returnlist, "fatal: Not a git repository"))
                {
                    Log("_response_restore:" + d + " 并非仓库");
                    continue;//这个目录并非仓库
                }
                responses.Add(d);
                Log("_response_restore:" + d + " 仓库恢复中");
                if (HaveString(returnlist, "Your branch is up-to-date") && HaveString(returnlist, "nothing to commit, working directory clean"))
                {
                    Log("_response_restore:" + d + " 仓库恢复完成");
                    continue;//干净的仓库
                }
                else if (HaveString(returnlist, "Your branch is up-to-date") && HaveString(returnlist, "Untracked files:"))
                {//有一些文件未提交

                    int b = 0;
                    List<string> fileadd = new List<string>();
                    foreach (var l in returnlist)
                    {
                        if (b == 0)
                        {
                            if (l.Contains("Untracked files:"))
                            {
                                b = 1;
                                continue;
                            }
                        }
                        if (b == 1)
                        {
                            if (l == "")
                            {
                                b = 2;
                                continue;
                            }
                        }
                        if (b == 2)
                        {
                            if (l == "")
                            {
                                break;
                            }
                            else
                            {
                                fileadd.Add(l.Replace("\t", ""));
                            }
                        }

                    }
                    foreach (var f in fileadd)
                    {
                        var r = _response_callgit(dir, "add " + f);
                    }
                    var r1 = _response_callgit(dir, "commit -m \"auto\"");
                    var r2 = _response_callgit(dir, "push");
                    Log("_response_restore:" + d + " 仓库恢复完成");
                    continue;
                }
                else if (HaveString(returnlist, "Your branch is up-to-date") && HaveString(returnlist, "Changes to be committed:"))
                {
                    var r1 = _response_callgit(dir, "commit -m \"auto\"");
                    var r2 = _response_callgit(dir, "push");
                    Log("_response_restore:" + d + " 仓库恢复完成");
                    continue;
                }
                else if (HaveString(returnlist, "Your branch is ahead of", "by 1 commit."))
                {
                    var r2 = _response_callgit(dir, "push");
                    Log("_response_restore:" + d + " 仓库恢复完成");
                    continue;
                }
                else//不能处理的情况
                {
                    string excstr = "发生了无法处理的GIT结果";
                    Log_Error(excstr);

                    foreach (var r in returnlist)
                    {
                        Log(r);
                        excstr += "\n" + r;
                    }
                    excstr += "\n" + "请呼叫管理解决";
                    throw new Exception(excstr);
                }

            }

            //启动仓库工作线程
            Thread t = new Thread(_response_watcher);
            t.IsBackground = true;
            t.Start();
        }
        static void _response_watcher()
        {
            DateTime pushLast = DateTime.Now;

            Dictionary<string, List<string>> unpushfile = new Dictionary<string, List<string>>();

            while (true)
            {
                System.Threading.Thread.Sleep(1);
                if (cmds.Count > 0)
                {
                    for (int i = 0; i < cmds.Count; i++)
                    {
                        SaveCmd cmd;
                        if (cmds.TryDequeue(out cmd))
                        {
                            string path = "data\\" + cmd.response + "\\" + cmd.file;
                            System.IO.File.WriteAllBytes(path, cmd.data);
                            if (unpushfile.ContainsKey(cmd.response) == false)
                            {
                                unpushfile[cmd.response] = new List<string>();
                            }
                            unpushfile[cmd.response].Add(cmd.file);
                        }
                    }

                }
                if (unpushfile.Count > 0 && (DateTime.Now - pushLast).TotalMinutes > 1.0)//等太久，来挑战
                {
                    foreach (var res in unpushfile.Keys)
                    {
                        foreach (var file in unpushfile[res])
                        {
                            _response_callgit("data\\" + res, "add " + file);
                        }
                        _response_callgit("data\\" + res, "commit -m \"auto\"");
                        _response_callgit("data\\" + res, "push");
                    }
                    unpushfile.Clear();

                    pushLast = DateTime.Now;
                }
            }
        }
        class SaveCmd
        {
            public string response;
            public string file;
            public byte[] data;
        }
        static System.Collections.Concurrent.ConcurrentQueue<SaveCmd> cmds = new System.Collections.Concurrent.ConcurrentQueue<SaveCmd>();
        //向仓库保存一个文件
        static bool _response_Save(string response, string file, byte[] data)
        {
            if (responses.Contains(response) == false) return false;

            SaveCmd cmd = new SaveCmd();
            cmd.response = response;
            cmd.file = file;
            cmd.data = data;
            cmds.Enqueue(cmd);
            return true;
        }
    }
}
