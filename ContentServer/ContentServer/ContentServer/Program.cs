using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Comunicacion;
using uy.edu.ort.obligatorio.Commons;
using log4net;
using System.Reflection;

namespace uy.edu.ort.obligatorio.ContentServer
{


    public class Program
    {
        DNSConnection dns;
        private ILog log;

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
        public bool DEBUG = bool.Parse(Settings.GetInstance().GetProperty("debug", "false"));
        public Program()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log4net.GlobalContext.Properties["serverName"] = Settings.GetInstance().GetProperty("server.name", "rodrigo-nb");
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            Console.Title = "Servidor De Contenidos";
            Console.WriteLine("----- Servidor De Contenidos -----");
            UsersContactsPersistenceHandler.GetInstance().LoadContacts();
            log.Info("contactos cargados");
            Console.WriteLine("contactos cargados");
            if (!DEBUG)
            {
                Console.WriteLine("[{0}] Connecting Dns...", DateTime.Now);
                dns = new DNSConnection();
                dns.SetupConn();
                Console.WriteLine("[{0}] DNS connection ready...", DateTime.Now);
            }
            else
            {
                Console.WriteLine("[{0}] Modo DEBUG Sin conezion al Dns...", DateTime.Now);
            }
            Console.WriteLine("[{0}] Starting server...", DateTime.Now);

            string listenAddressStr = Settings.GetInstance().GetProperty("listen.ip","ANY");

            IPAddress ip = "ANY".Equals(listenAddressStr) ? IPAddress.Any : IPAddress.Parse(listenAddressStr);
            int port =  int.Parse(Settings.GetInstance().GetProperty("server.port","2001"));


            server = new TcpListener(ip, port);
            server.Start();
            Console.WriteLine("[{0}] Server is running properly!", DateTime.Now);
            log.Info("Server is running properly!");
            Listen();
        }


        void Listen()  // Listen to incoming connections.
        {
            while (running)
            {
                TcpClient tcpClient = server.AcceptTcpClient();  // Accept incoming connection.
                Connection client = new Connection(tcpClient, new ReceiveEventHandler());     // Handle in another thread.
                
            }
        }

    }

   

    public class DNSConnection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            Connection client = new Connection("DNS", new TcpClient(DNSServer, DNSPort), new ReceiveEventHandler());
           

            string payload =            Settings.GetInstance().GetProperty("server.name","rodrigo-nb")
                                + ":" + Settings.GetInstance().GetProperty("server.ip","127.0.0.1")
                                + ":" + Settings.GetInstance().GetProperty("server.port", "2001")
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
                log.Info("Enviando linea " + cont++ + "   --->" + ConversionUtil.GetString(item));
                client.WriteToStream(item);
            }

            log.Info("End Register Server");
        
        }
       
   
      
    }
  
    
}
