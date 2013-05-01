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

        public string Server { get { return "localhost"; } }
        public int Port { get { return 2001; } }


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

        TcpClient client2;
        NetworkStream netStream2;
        StreamReader br2;
        StreamWriter bw2;

        void SetupConn()  // Setup connection and login
        {
            #region PUERTO_2001
            if(true)          {
                    client = new TcpClient(Server, Port);  // Connect to the server.
                    netStream = client.GetStream();

                    br = new StreamReader(netStream, Encoding.UTF8);
                    bw = new StreamWriter(netStream, Encoding.UTF8);
                    //--------------------------------------------------------------

                    string login = "mauricio";
                    string pattern = "vs.iso";
                    string timestamp = "" + DateTime.Now;

                    String hashQuery = StringUtils.CalculateMD5Hash(String.Format("{0}|{1}|{2}", login, pattern, timestamp));

                    

                    string dataToSend = login + "|" + hashQuery + "|" + pattern;

                    Data data = new Data()
                    {
                        Command = Command.REQ,
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

                    //png
                    //"7aecaa5f325a4fa393f586f638197ac3"

                    //git.exe
                    //"eefc05a7ff11a84d350d561a63014a47"

                    //iso
                    //"e1e8c17baf81af6722feb8987269f22e"

                    Console.WriteLine("termino");

                    CloseConn();
                }
             
            
             
            #endregion

            //--------------------------------------------------------------

            #region puerto 20001
             if (false)
             {
                client2 = new TcpClient("localhost", 20001);  // Connect to the server.
                netStream2 = client2.GetStream();

                br2 = new StreamReader(netStream2, Encoding.UTF8);
                bw2 = new StreamWriter(netStream2, Encoding.UTF8);
                //--------------------------------------------------------------
                //archivo2.txt@e8bad6bd67b24cc8c3e930b69c3064bf@27@rodrigo

                // login + "|" + owner + "|"+hashfile;


                // log4net.dll@179e7321f8bacc32b2bbac8cf02613ca@288768@rodrigo
                //bajar.jpg@df9ccfdeab5070150211ac8fc3b9fe33@9270@rodrigo
                string dataToSend = "rodrigo" + "|" + "rodrigo" + "|" + "df9ccfdeab5070150211ac8fc3b9fe33";

                Data data = new Data()
                {
                    Command = Command.REQ,
                    OpCode = OpCodeConstants.REQ_DOWNLOAD_FILE,
                    Payload = new Payload(dataToSend)
                };
                int cont = 0;
                foreach (var item in data.GetBytes())
                {
                    Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(item));
                    bw2.Write(item);
                    bw2.Flush();
                }

                Console.WriteLine("mande");

                Data data2 = LoadObject(br2);

                Console.WriteLine("line " + cont++ + "   --->" + ConversionUtil.GetString(data2.GetBytes()[0]));
                string PIPE_SEPARATOR = "|";

                string[] payload = data2.Payload.Message.Split(new string[] { PIPE_SEPARATOR }, StringSplitOptions.None);
                string login = payload[0];
                string owner = payload[1];
                string hashfile = payload[2];
                string filename = payload[3];
                long size = long.Parse(payload[4]);

                const int BUFF_SIZE = 1024;
                byte[] buffer = new byte[BUFF_SIZE];

                bool done = false;
                long bytescount = 0;

                Random random = new Random();
                int randomNumber = random.Next();
                string path = @"c:\downloads\" + randomNumber + "_" + filename;
                BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create));

                try
                {
                    while (!done)
                    {
                        int countRead = netStream2.Read(buffer, 0, BUFF_SIZE);
                        bytescount += countRead;

                        if (countRead > 0)
                        {
                            if (countRead < BUFF_SIZE)
                            {
                                done = true;
                            }
                            writer.Write(buffer, 0, countRead);
                        }
                        else
                        {//no leyo nada de la entrada (cantidad de bytes justa, en la siguiente lectura)
                            done = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                writer.Close();

                Console.WriteLine("--------> size= " + size);

                Console.WriteLine("--------> bytescount= " + bytescount);

                CloseConn2();
            }
            #endregion
            Console.ReadLine();
        }

        void CloseConn() // Close connection.
        {
            br.Dispose();
            bw.Dispose();
            netStream.Close();
            client.Close();
        }

        void CloseConn2() // Close connection.
        {
            br2.Dispose();
            bw2.Dispose();
            netStream2.Close();
            client2.Close();
        }

        public Data LoadObject(StreamReader br)
        {
            char[] buffer = new char[10];
            int readQty = br.Read(buffer, 0, 10);//REQ99000050101A

            if (readQty < 10) throw new Exception("Errror en trama largo fijo");

            Command type = (Command)Enum.Parse(typeof(Command), ArrayToString(buffer, 0, 3));
            int opCode = int.Parse(ArrayToString(buffer, 3, 2));
            int payloadLength = int.Parse(ArrayToString(buffer, 5, 5));

            Console.WriteLine(type + " " + opCode + " " + payloadLength);

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
