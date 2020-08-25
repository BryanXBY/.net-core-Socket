# .net-core-Socket
c# .NET CORE的socket。
注：最新代码请访问https://bryanXBY@dev.azure.com/bryanXBY/Socket/_git/Socket
服务端启动
添加 ServerControl.cs到项目
使用
ServerControl Server = new ServerControl(); //初始化Socket
Server.Start("0.0.0.0",1231,10000);//启动侦听连接,绑定IP，端口，最大连接数

客户端启动
添加ClientControl.cs到项目

使用
ClientControl Client = new ClientControl();//初始化Socket
Client.Connect("127.0.0.1",1231);//连接到服务器，服务器地址string，端口int

发送消息
Client.Send(); //发送string的消息
