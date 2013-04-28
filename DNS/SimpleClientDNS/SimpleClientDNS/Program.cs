using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    class Program
    {
        static void Main(string[] args)
        {
           Program p =  new Program();
           p.connect();
           Console.ReadLine();
        }

        string login = "fffffff";

        Thread tcpThread;      // Receiver


        public string Server { get { return "localhost"; } }  // Address of server. In this case - local IP address.
        public int Port { get { return 2000; } }


        // Start connection thread and login or register.
        void connect()
        {

                tcpThread = new Thread(new ThreadStart(SetupConn));
                tcpThread.Start();
            
        }
       
        TcpClient client;
        NetworkStream netStream;
    
        StreamReader br;
        StreamWriter bw;

        void SetupConn()  // Setup connection and login
        {
            client = new TcpClient(Server, Port);  // Connect to the server.
            netStream = client.GetStream();

            br = new StreamReader(netStream, Encoding.UTF8);
            bw = new StreamWriter(netStream, Encoding.UTF8);

            Data data = new Data() { Command = Command.REQ, OpCode = 1, Payload = new Payload(login) };
            int cont = 0;
            foreach (var item in data.GetBytes())
            {
                Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(item));
                bw.Write(item);
                bw.Flush();
            }

            Console.WriteLine("mande");

            Data data2 = LoadObject(br);

            Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(data2.GetBytes()[0]));
           
            Console.WriteLine("termino");
            
            Console.WriteLine("Pido lista de contactos");

            data = new Data() { Command = Command.REQ, OpCode = 2, Payload = new Payload(login) };
            foreach (var item in data.GetBytes())
            {
                Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(item));
                bw.Write(item);
                bw.Flush();
            }

            Data data3 = LoadObject(br);

            Console.WriteLine("lista de contactos en el cliente " + cont++ + "   --->" + ConversionUtil.GetString(data3.GetBytes()[0]));

            CloseConn();
        }

       

        void CloseConn() // Close connection.
        {
            br.Dispose();
            bw.Dispose();
            netStream.Close();
            client.Close();
        }

        public Data LoadObject(StreamReader br)
        {

            char[] buffer = new char[10];
            int readQty = br.Read(buffer, 0, 10);//REQ99000050101A

            if (readQty < 10) throw new Exception("Errror en trama largo fijo");

            Command type = (Command)Enum.Parse(typeof(Command), ArrayToString(buffer, 0, 3));
            int opCode = int.Parse(ArrayToString(buffer, 3, 2));
            int payloadLength = int.Parse(ArrayToString(buffer, 5, 5));
            //int partsTotal          = int.Parse(ArrayToString(buffer, 10, 2));
            //int partsCurrent        = int.Parse(ArrayToString(buffer, 12, 2));

            Console.WriteLine(type + " " + opCode + " " + payloadLength);//+ " " + partsTotal + " " + partsCurrent);

            buffer = new char[payloadLength];
            readQty = br.Read(buffer, 0, payloadLength);
            if (readQty < payloadLength) throw new Exception("Errror en trama largo fijo leyendo payload");


            Data ret = new Data() { Command = type, OpCode = opCode, Payload = new Payload(ArrayToString(buffer, 0, readQty)) };

            return ret;
        }

        private static string ArrayToString(char[] buffer, int startIndex, int length)
        {
            return new string(buffer).Substring(startIndex, length);
        }
    }
}
