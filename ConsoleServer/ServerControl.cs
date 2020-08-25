using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ConsoleServer
{
    public class ServerControl
    {
        public Socket ServerSocket;
        public List<Socket> ClientList=null;//客户端集合

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServerControl()
        {
            //创建套接字，ipv4寻址方式，套接字类型，传输协议
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientList=new List<Socket>();//实例化客户端接入集合介入
        }

        public void Start(string IpAddress,int IpProt,int Maxnum)//IP地址，端口，最大连接数
        {

            ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(IpAddress), IpProt));//绑定IP和端口
            ServerSocket.Listen(Maxnum);
            Console.WriteLine("服务器成功监听端口：" + IpProt.ToString()) ;

            //当有客户接入时当前线程会被挂起，每次新接入客户端创建独立线处理
            Thread ThreadAccept = new Thread(Accept);
            ThreadAccept.IsBackground = true;//设置为后台线程
            ThreadAccept.Start();//启动线程

        }

        private void Accept()
        { 
            Socket Client = ServerSocket.Accept();//客户端接入当前线程挂起
            IPEndPoint point=Client.RemoteEndPoint as IPEndPoint;//将远端节点转为ip和端口
            Console.WriteLine("IP {0}:{1} 连接到服务器！",point.Address,point.Port);//显示接入信息
            ClientList.Add(Client);//将此连接添加到客户集合

            //接收消息只处理一次，线程被挂起，创建新线程处理其新消息
            Thread ThreadReceive = new Thread(Receive);
            ThreadReceive.IsBackground = true;//设置为后台线程
            ThreadReceive.Start(Client);//启动线程
            Accept();//回掉处理新接入客户端
        }

        /// <summary>
        /// 处理收到的消息
        /// </summary>
        /// <param name="obj"></param>
        private void Receive(object obj)
        {
            Socket Client = obj as Socket;
            IPEndPoint point = Client.RemoteEndPoint as IPEndPoint;//将远端节点转为ip和端口
            try
            {
                byte[] ByteReceive = new byte[1024];//消息数组
                int ReceiveLenght = Client.Receive(ByteReceive);//只处理一次消息，此处会被挂起
                string tomsg = "hi";//回复给客户端单独的消息
                string msg = Encoding.UTF8.GetString(ByteReceive, 0, ReceiveLenght);
                Console.WriteLine(point.Address + "[" + point.Port + "]:" + msg);//打印收到的消息
               
                //Broadcast(Client, msg);//广播消息(除开消息源外都会受到此消息)
                Client.Send(Encoding.UTF8.GetBytes(msg));//消息源回复
                //Client.Send(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));//回发当前时间给消息源
                Receive(Client);//回调处理新消息
            }
            catch
            {
                ClientList.Remove(Client);//当客户端断开，从客户端集合移除该客户端
                Console.WriteLine("{0}[{1}]:断开连接", point.Address, point);//打印提示信息
            }

        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="cli"></param>
        /// <param name="msg"></param>
        public void Broadcast(Socket cli,string msg)
        {
            foreach (var  item in ClientList )//遍历所有客户端
            {
                if (item != cli)
                    item.Send(Encoding.UTF8.GetBytes(msg));//给除数据源以外客户端发送广播消息
            }
        }
    }
}
