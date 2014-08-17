using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {


            corelib.Server server = new corelib.Server(new logger());
            server.Start();
            while (true)
            {
                Console.Write("->");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line) == false)
                {
                    server.Call(line);
                    if(server.exited)
                    {
                        break;
                    }
                }
            }
        }
        public class logger : corelib.ILogger
        {

            public void Log(string str)
            {
                Console.WriteLine(str);
            }


            public void Log_Warn(string str)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(str);
                Console.ResetColor();
            }

            public void Log_Error(string str)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(str);
                Console.ResetColor();
            }

            public void Clear()
            {
                Console.Clear();
            }
        }
    }
}
