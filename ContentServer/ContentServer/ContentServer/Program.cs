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
        ListeningServer transferServer;
        ListeningServer commandServer;

        private ILog log;

        static void Main(string[] args)
        {
            Program program = null;
            try
            {
                program = new Program();
            }
            catch (Exception e)
            {

                Console.WriteLine("Error:" + e.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Escriba Q para terminar.");
            while (!"q".Equals(Console.ReadLine().ToLower())) ;

            if (program != null)
            {
                program.Disconnect();
            }

        }

        public void Disconnect()
        {
            dns.EndConnection();
            //   transferServer.
        }



        public bool running = true;


        public bool DEBUG = bool.Parse(Settings.GetInstance().GetProperty("debug", "false"));


        public Program()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log4net.GlobalContext.Properties["serverName"] = Settings.GetInstance().GetProperty("server.name", "rodrigo-nb");
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            Console.Title = "Servidor De Contenidos: " + Settings.GetInstance().GetProperty("server.name", "rodrigo-nb").ToUpper();
            Console.WriteLine("----- Servidor De Contenidos -----");
            UsersContactsPersistenceHandler.GetInstance().LoadContacts();
            log.Info("contactos cargados");
            Console.WriteLine("contactos cargados");

            Console.WriteLine("[{0}] Starting server...", DateTime.Now);

            string listenAddressStr = Settings.GetInstance().GetProperty("listen.ip", "ANY");

            IPAddress ip = "ANY".Equals(listenAddressStr) ? IPAddress.Any : IPAddress.Parse(listenAddressStr);
            int port = int.Parse(Settings.GetInstance().GetProperty("server.port", "2001"));
            int portTransfers = int.Parse(Settings.GetInstance().GetProperty("server.transfers.port", "20001"));


            transferServer = new ListeningServer("Transfers", ip, portTransfers);
            commandServer = new ListeningServer("Control", ip, port);



            Console.WriteLine("[{0}] Server is running properly!", DateTime.Now);
            log.Info("Server is running properly!");




            if (!DEBUG)
            {
                Console.WriteLine("[{0}] Connecting Dns...", DateTime.Now);
                dns = new DNSConnection();
                try
                {
                    dns.SetupConn();
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    log.ErrorFormat("No se pudo registrar con el DNS, terminando", e);
                    throw new Exception("No se pudo registrar con el DNS, terminando", e);
                }
                Console.WriteLine("[{0}] DNS connection ready...", DateTime.Now);
            }
            else
            {
                Console.WriteLine("[{0}] Modo DEBUG Sin conezion al Dns...", DateTime.Now);
            }


        }

    }



    public class ListeningServer
    {
        string function;
        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TcpListener server;
        int port;
        IPAddress ip;
        bool running = true;
        Thread thread;

        int connectionCounter = 0;
        List<Connection> openConnections = new List<Connection>();


        Connection.ConnectionDroppedDelegate delegado;

        public ListeningServer(string function, IPAddress ip, int port)
        {
            this.function = function;
            this.ip = ip;
            this.port = port;
            delegado = new Connection.ConnectionDroppedDelegate(OneConnectionDroppedEvent);
            (thread = new Thread(new ThreadStart(Listen))).Start();
        }

        public void EndConnection()
        {
            running = false;
            server.Stop(); //deja de aceptar conexiones
            Console.WriteLine("VER COMO HACER EL KILL ALL CONNECTIONS");
        }

        public void OneConnectionDroppedEvent(string idName)
        {
            log.Debug("conexion dropeada: " + idName);
        }

        void Listen()  // Listen to incoming connections.
        {
            log.InfoFormat("Listen");
            server = new TcpListener(ip, port);
            server.Start();

            while (running)
            {
                log.InfoFormat("[Listen:{0}] Waiting for new connection", port);

                TcpClient tcpClient = server.AcceptTcpClient();  // Accept incoming connection.
                log.InfoFormat("nueva conexion de control(:{2}) desde {0}:{1}", ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address, ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Port, port);

                Connection client = new Connection(connectionCounter.ToString(), tcpClient, new ReceiveEventHandler(), delegado);     // Handle in another thread.
                lock (openConnections)
                {
                    openConnections.Add(client);
                    connectionCounter++;

                }

            }
        }
    }

    public class DNSConnection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string DNSServer { get { return Settings.GetInstance().GetProperty("dns.ip", "127.0.0.1"); } }// return "192.168.0.201"; } }
        public int DNSPort { get { return int.Parse(Settings.GetInstance().GetProperty("dns.port", "2000")); } }

        Connection connection;

        public void SetupConn()  // Setup connection and login
        {

            connection = new Connection("DNS", new TcpClient(DNSServer, DNSPort), new ReceiveEventHandler());


            string payload = Settings.GetInstance().GetProperty("server.name", "rodrigo-nb")
                                + ":" + Settings.GetInstance().GetProperty("server.ip", "127.0.0.1")
                                + ":" + Settings.GetInstance().GetProperty("server.port", "2001")
                                + ":" + Settings.GetInstance().GetProperty("server.transfers.port", "20001")
                                + ":" + UsersContactsPersistenceHandler.GetInstance().Count;

            Data data = new Data()
            {
                Command = Command.REQ,
                OpCode = 3,//REQ_SERVER_CONNECT
                Payload = new Payload(payload)
            };

            int cont = 0;
            foreach (var item in data.GetBytes())
            {
                Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(item));
                log.Info("Enviando linea " + cont++ + "   --->" + ConversionUtil.GetString(item));
                connection.WriteToStream(item);
            }

            log.Info("End Register Server");

        }


        public void EndConnection()
        {
            connection.CloseConn();
        }
    }


}
