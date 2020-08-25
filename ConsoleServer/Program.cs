using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerControl Server = new ServerControl();//初始化Socket
            Server.Start("0.0.0.0",1231,10000);//启动侦听连接,绑定IP，端口，最大连接数
            Console.ReadKey();
        }
    }
}
