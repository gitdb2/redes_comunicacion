using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ContentServer
{

    /*
        base.shared.dir.path=c:/shared
listen.ip=ANY

server.ip=192.168.0.242
server.port=2001
server.name=rodrigo-nb

dns.ip=127.0.0.1
dns.port=2000
         
        */


    public class Program
    {
        DNSConnection dns;

        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine();
            Console.WriteLine("Enter para terminar.");
            Console.ReadLine();
        }


        /*
           Settings.GetInstance().GetProperty("listen.ip","ANY")
           Settings.GetInstance().GetProperty("server.ip","127.0.0.1")
           Settings.GetInstance().GetProperty("server.port","2001")
           Settings.GetInstance().GetProperty("server.name","rodrigo-nb")
           Settings.GetInstance().GetProperty("dns.ip","127.0.0.1")
           Settings.GetInstance().GetProperty("dns.port","2000")
           */

        public bool running = true;
        public TcpListener server;

        public Program()
        {
            Console.Title = "Servidor De Contenidos";
            Console.WriteLine("----- Servidor De Contenidos -----");
            UsersContactsPersistenceHandler.GetInstance().LoadContacts();
            Console.WriteLine("contactos cargados");

            Console.WriteLine("[{0}] Connecting Dns...", DateTime.Now);
            dns = new DNSConnection();
            dns.SetupConn();
            Console.WriteLine("[{0}] DNS connection ready...", DateTime.Now);

            Console.WriteLine("[{0}] Starting server...", DateTime.Now);

            string listenAddressStr = Settings.GetInstance().GetProperty("listen.ip","ANY");

            IPAddress ip = "ANY".Equals(listenAddressStr) ? IPAddress.Any : IPAddress.Parse(listenAddressStr);//IPAddress.Parse("192.168.0.242");//127.0.0.1");
            int port =  int.Parse(Settings.GetInstance().GetProperty("server.port","2001"));


            server = new TcpListener(ip, port);
            server.Start();
            Console.WriteLine("[{0}] Server is running properly!", DateTime.Now);

            Listen();
        }


        void Listen()  // Listen to incoming connections.
        {
            while (running)
            {
                TcpClient tcpClient = server.AcceptTcpClient();  // Accept incoming connection.
                Connection client = new Connection(tcpClient);     // Handle in another thread.
            }
        }

    }

   

    public class DNSConnection
    {

        public string DNSServer { get { return Settings.GetInstance().GetProperty("dns.ip", "127.0.0.1"); } }// return "192.168.0.201"; } }
        public int DNSPort { get { return int.Parse(Settings.GetInstance().GetProperty("dns.port","2000")); } }
      
     

        public void SetupConn()  // Setup connection and login
        {
            /*
            Settings.GetInstance().GetProperty("listen.ip","ANY")
            Settings.GetInstance().GetProperty("server.ip","127.0.0.1")
            Settings.GetInstance().GetProperty("server.port","2001")
            Settings.GetInstance().GetProperty("server.name","rodrigo-nb")
            Settings.GetInstance().GetProperty("dns.ip","127.0.0.1")
            Settings.GetInstance().GetProperty("dns.port","2000")
            */

            Connection client = new Connection(new TcpClient(DNSServer, DNSPort));
           
            string payload =  Settings.GetInstance().GetProperty("server.ip","127.0.0.1")
                                +":"+ Settings.GetInstance().GetProperty("server.port","2001")
                                +":"+ Settings.GetInstance().GetProperty("server.name","rodrigo-nb")
                                + ":" + UsersContactsPersistenceHandler.GetInstance().Count;
                                
            Data data = new Data()
            {
                Command = Command.REQ,
                OpCode = 3,
                Payload = new Payload(payload)
            };

            int cont = 0;
            foreach (var item in data.GetBytes())
            {
                Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(item));
                client.WriteToStream(item);
            }

            Console.WriteLine("mande");
        
        }
       
   
      
    }
  
    
}
