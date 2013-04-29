using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using uy.edu.ort.obligatorio.Commons;
using Comunicacion;

namespace ClientImplementation
{
    public class ClientHandler
    {

        private const int puertoDNS = 2000;
        private const string ipDNS = "localhost";
        private Connection connection;
        public TcpClient TcpClient { get; set; }

        private static ClientHandler instance = new ClientHandler();

        private ClientHandler() 
        {
            TcpClient = new TcpClient(ipDNS, puertoDNS);
            connection = new Connection(TcpClient, new ReceiveEventHandler());
        }

        public static ClientHandler GetInstance()
        {
            return instance;
        }

        public void Connect(string login)
        {
            connection.Name = login;
           
        }

        public void LoginClient(string login)
        {
            Data data = new Data() { Command = Command.REQ, OpCode = 1, Payload = new Payload(login) };
            foreach (var item in data.GetBytes())
            {
                connection.WriteToStream(item);
            }
        }

        public event EventHandler LoginOK;

        public virtual void OnLoginOK()
        {
            if (LoginOK != null)
                LoginOK(this, EventArgs.Empty);
        }

        public event ChatErrorEventHandler LoginFailed;
        
        public delegate void ChatErrorEventHandler(object sender, LoginErrorEventArgs e);
        
        public virtual void OnLoginFailed(LoginErrorEventArgs e)
        {
            if (LoginFailed != null)
                LoginFailed(this, e);
        }

        public void CloseConnection()
        {
            this.connection.CloseConn();
        }
    }
}
