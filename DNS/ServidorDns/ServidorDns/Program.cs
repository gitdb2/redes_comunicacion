using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine();
            Console.WriteLine("Enter para terminar.");
            Console.ReadLine();
        }

        public IPAddress ip = IPAddress.Parse("127.0.0.1");
        public int port = 2000;
        public bool running = true;
        public TcpListener server;

        public Program()
        {
            Console.Title = "Servidor DNS y CHAT";
            Console.WriteLine("----- DNS y CHAT Server -----");
            //LoadUsers();
            Console.WriteLine("[{0}] Starting server...", DateTime.Now);

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
                ClientConnection client = new ClientConnection(tcpClient);     // Handle in another thread.
            }
        }

    }
}
