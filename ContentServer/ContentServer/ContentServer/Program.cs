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




    public class Program
    {
        DNSConnection dns;

        static void Main(string[] args)
        {
            Program p = new Program();

            //DNSConnection p = new DNSConnection();
            //p.connect();

            Console.WriteLine();
            Console.WriteLine("Enter para terminar.");
            Console.ReadLine();
        }

        public IPAddress ip = IPAddress.Any;//IPAddress.Parse("192.168.0.242");//127.0.0.1");
        public int port = 2001;
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




            server = new TcpListener(ip, port);
            server.Start();
            Console.WriteLine("[{0}] Server is running properly???", DateTime.Now);

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
    

        //Thread tcpThread;      // Receiver



        public string Server { get { return "192.168.0.201"; } }// return "192.168.0.201"; } }
        public int Port { get { return 2000; } }
      
        //void connect()
        //{

        //    tcpThread = new Thread(new ThreadStart(SetupConn));
        //    tcpThread.Start();

        //}


        //TcpClient client;
        //NetworkStream netStream;

        //StreamReader br;
        //StreamWriter bw;

        public void SetupConn()  // Setup connection and login
        {
          //  client = new TcpClient(Server, Port);  // Connect to the server.


            Connection client = new Connection(new TcpClient(Server, Port));
           

            Data data = new Data()
            {
                Command = Command.REQ,
                OpCode = 3,
                Payload = new Payload("192.168.0.242:2001")
            };

            

            int cont = 0;
            foreach (var item in data.GetBytes())
            {
                Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(item));
                client.WriteToStream(item);
            }

            Console.WriteLine("mande");

            //Data data2 = DataProccessor.GetInstance().LoadObject(br);

            //Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(data2.GetBytes()[0]));

            //Console.WriteLine("termino");


         //   CloseConn();
        }
       
   
      
    }
  
    
}
