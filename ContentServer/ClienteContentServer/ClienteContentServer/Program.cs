using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using Comunicacion;
using uy.edu.ort.obligatorio.Commons;


namespace ClienteContentServer
{
   
        class Program
        {
            static void Main(string[] args)
            {
                Console.Title = "Cliente d servidor de contenidos";
                Program p = new Program();
                p.connect();
                Console.ReadLine();
            }


            Thread tcpThread;      // Receiver


        //    public string Server { get { return "localhost"; } }  // Address of server. In this case - local IP address.
            public string Server { get { return "localhost";} }// return "192.168.0.201"; } }
            public int Port { get { return 2001; } }


            // Start connection thread and login or register.
            void connect()
            {

                tcpThread = new Thread(new ThreadStart(SetupConn));
                tcpThread.Start();

            }






            // Events



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
                //--------------------------------------------------------------



                string login = "rodrigo";
                string pattern = "*";
                string timestamp = ""+DateTime.Now;

                String hashQuery = StringUtils.CalculateMD5Hash(String.Format("{0}|{1}|{2}", login, pattern, timestamp));

                string dataToSend = login + "|" + hashQuery + "|"+pattern;

                Data data = new Data() { Command = Command.REQ,
                                         OpCode = OpCodeConstants.REQ_SEARCH_FILES,
                                         Payload = new Payload(dataToSend)
                };
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

                Console.ReadLine();

                CloseConn();
            }
            void CloseConn() // Close connection.
            {

                br.Dispose();
                bw.Dispose();
                netStream.Close();
                client.Close();


                //br.Close();
                //bw.Close();
                //netStream.Close();
                //client.Close();

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
