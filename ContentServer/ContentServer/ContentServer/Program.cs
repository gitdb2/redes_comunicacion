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
        ListeningServer controlServer;

        private ILog log;

        static void Main(string[] args)
        {
            Program program = null;
            //try
            //{
            program = new Program();
            //}
            //catch (Exception e)
            //{

            //Console.WriteLine("Error:" + e.Message);
            //}

            Console.WriteLine();
            Console.WriteLine("Escriba Q para terminar.");



            //if (program != null)
            //{

            if (Program.dnsError)
            {
                program.Disconnect();
            }
            else
            {
                Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
                {
                    e.Cancel = true;
                    program.Disconnect();
                };
                while (Program.running)
                {

                 //   Console.WriteLine("duermo");
                    Thread.Sleep(5000);
                }
            }

            Console.WriteLine("exited gracefully, INTRO para terminar");
            Console.ReadLine();
            //}

        }

        public void Disconnect()
        {
           
            log.InfoFormat("PROGRAM.disconect");
            dns.EndConnection();
            transferServer.EndConnection();
            controlServer.EndConnection();
            Program.running = false;
        }

        public void DisconnectServers(string nada){
           
            log.InfoFormat("delegado DNS cayo, PROGRAM.DisconnectServers");
            transferServer.EndConnection();
            controlServer.EndConnection();
            Program.running = false;
         }


        public static bool running = true;

        public static bool dnsError = false;

        public bool DEBUG = bool.Parse(Settings.GetInstance().GetProperty("debug", "false"));

        Connection.ConnectionDroppedDelegate dnsCaeDelegado;//delegado = new Connection.ConnectionDroppedDelegate(OneConnectionDroppedEvent);

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


            transferServer = new ListeningServer(true, "Transfers", ip, portTransfers);
            controlServer = new ListeningServer(false, "Control", ip, port);


            Console.WriteLine("[{0}] Server is running properly!", DateTime.Now);
            log.Info("Server is running properly!");




            if (!DEBUG)
            {

                dnsCaeDelegado = new Connection.ConnectionDroppedDelegate(DisconnectServers);
                Console.WriteLine("[{0}] Connecting Dns...", DateTime.Now);
                dns = new DNSConnection(dnsCaeDelegado);
                try
                {
                    dns.SetupConn();
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    log.ErrorFormat("No se pudo registrar con el DNS, terminando", e);
                    running = false;
                    dnsError = true;
                    //  throw new Exception("No se pudo registrar con el DNS, terminando", e);
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

        bool isTransfer;
        
        int connectionCounter = 0;
        Dictionary<string, Connection> openConnections = new Dictionary<string, Connection>();


        Connection.ConnectionDroppedDelegate delegado;


     //   private IReceiveEvent responseHandler;
       
        public ListeningServer(bool tranasfer, string function, IPAddress ip, int port)
        {
            isTransfer = tranasfer;
            //this.responseHandler = responseHandler;
            this.function = function;
            this.ip = ip;
            this.port = port;
            delegado = new Connection.ConnectionDroppedDelegate(OneConnectionDroppedEvent);
            (thread = new Thread(new ThreadStart(Listen))).Start();
        }

        public void EndConnection()
        {

            log.InfoFormat("---->LISTENING SERVER: {0}:{1} ---->EndConnection", function, port);
            running = false;
            server.Stop(); //deja de aceptar conexiones
        ///    Console.WriteLine("VER COMO HACER EL KILL ALL CONNECTIONS");
            CloseAllConnectionsToThisServer();
        }

        private void CloseAllConnectionsToThisServer()
        {
            lock (openConnections)
            {
                List <Connection> connections = new List<Connection>(openConnections.Values);

                foreach (var conn in connections)
                {
                    bool removed = openConnections.Remove(conn.Name);
                    log.DebugFormat("--->CloseAllConnectionsToThisServer: {0} eliminada del diccionario? {1}", conn.Name, removed);

                    //if (conn != null)
                    //{
                        // anulo El delegado ya que no quiero que lo ejecute cuando se dispare cuando invoca closeConn
                        conn.OnConnectionDropDelegate = null;
                        conn.CloseConn();
                    //}
                }
            }
        }


        /// <summary>
        /// operacion llamada por una Connection que se cerro sesde el otro lado
        /// </summary>
        /// <param name="idName"></param>
        public void OneConnectionDroppedEvent(string idName)
        {
            log.DebugFormat("---> DELEGADO conexion dropeada: {0}", idName);

            lock (openConnections)
            {
                if (openConnections.ContainsKey(idName))
                {
                    Connection conn = openConnections[idName];
                    openConnections.Remove(idName);
                    log.DebugFormat("--->DELEGADO conexion dropeada: {0} eliminada del diccionario", idName);

                    if (conn != null)
                    {
                       // El delegado ya fue eliminado para que no se dispare cuando invoca closeConn
                        conn.CloseConn();
                    }
                }
                else
                {
                    log.DebugFormat("---> DELEGADO conexion dropeada: {0} NO se encuentra en la lista de conexiones abiertas, no se hace nada", idName);
                }
            }
        }

        void Listen()  // Listen to incoming connections.
        {
            log.InfoFormat("Listen");
            server = new TcpListener(ip, port);
            server.Start();

            while (running)
            {

                log.InfoFormat("[Listen:{0}] Waiting for new connection", port);
                TcpClient tcpClient = null;

                try
                {
                    tcpClient = server.AcceptTcpClient();  // Accept incoming connection.
                }
                catch (SocketException e)// detecto que se efectio un listener.stop
                {

                    log.ErrorFormat("Excepcion [Listen:{0}], se detiene la escucha", port, e);
                    running = false;
                }

                if (running)
                {
                    log.InfoFormat("nueva conexion de control(:{2}) desde {0}:{1}", ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address, ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Port, port);
                    IReceiveEvent responseHandler = null;
                    if(isTransfer){
                        responseHandler = new ReceiveTransfersEventHandler();
                    }else{
                        responseHandler = new ReceiveEventHandler();
                    }
                    Connection conn = new Connection(connectionCounter.ToString(), tcpClient, responseHandler, delegado);     // Handle in another thread.
                    lock (openConnections)
                    {
                        openConnections.Add(conn.Name, conn);
                        connectionCounter++;
                    }
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
        Connection.ConnectionDroppedDelegate delegado; 
        public DNSConnection(Connection.ConnectionDroppedDelegate delegado){
            this.delegado = delegado;
        }

        public void SetupConn()  // Setup connection and login
        {

            connection = new Connection("DNS", new TcpClient(DNSServer, DNSPort), new ReceiveEventHandler(), delegado);


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
            log.InfoFormat("---->DNS---->EndConnection connection === null?{0}", connection == null);
            if (connection != null)
            {
               
                connection.CloseConn();
            }
        }
    }


}
