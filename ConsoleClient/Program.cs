using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientControl Client = new ClientControl();//初始化Socket
            Client.Connect("127.0.0.1",1231);//连接到服务器
            Console.WriteLine("输入消息后，按下回车发送消息，输入exit退出！");//提示信息
            string msg=Console.ReadLine();//输入
            while (msg!="exit")//判断是否退出
            {
                Client.Send(msg);//发送消息
                msg = Console.ReadLine();//输入消息

            }
            
        }
    }
}
