using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleClient
{
    public class ClientControl
    {
        private Socket ClientSocket;

        public ClientControl()
        {
            //创建套接字，ipv4寻址方式，套接字类型，传输协议
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        public void Connect(string IP, Int32 Port)
        {
            try
            {
                ClientSocket.Connect(IP, Port);//连接到指定服务器
                Console.WriteLine("成功连接到服务器!");//提示信息
                //收到消息是线程会被挂起，创建新线程处理收到的消息
                Thread ThreadReceive = new Thread(Receive);
                ThreadReceive.IsBackground = true;
                ThreadReceive.Start();
            }
            catch(SocketException e)
            {
                Console.WriteLine("无法连接到{0}:{1}{2}",IP,Port,e);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg)
        { 
            ClientSocket.Send(Encoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private void Receive()
        {
            try
            {
                byte[] ByteReceive = new byte[1024];
                int ReceiveLenght = ClientSocket.Receive(ByteReceive);//此处线程会被挂起
                string themsg = Encoding.UTF8.GetString(ByteReceive, 0, ReceiveLenght);
                Console.WriteLine("{0}","收到消息：["+ themsg+"]");
                Receive();
            }
            catch 
            {
                Console.WriteLine("服务器已断开");
            }
        }
    }
}
