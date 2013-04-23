using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

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


        Thread tcpThread;      // Receiver


        public string Server { get { return "localhost"; } }  // Address of server. In this case - local IP address.
        public int Port { get { return 2000; } }


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


            bw.Write("REQ");
            bw.Write("02");
            bw.Write("00005");
            bw.Write("HOLA!");
            bw.Flush();
            Console.WriteLine("mande");

            char[] buffer = new char[30];
            int cantLecturas = br.Read(buffer, 0, 3);

            string tmp1 = new string(buffer);
            tmp1 = tmp1.Substring(0, 3);

            string tmp2 = "";
            string tmp3 = "";
            string tmp4 = "";
            //string tmp1 = br.ReadString();//RES
            //string tmp2 = br.ReadString();//03
            //string tmp3 = br.ReadString();//00004
            //string tmp4 = br.ReadString();//CHAU
            Console.WriteLine(tmp1 + " " + tmp2 + " " + tmp3 + " " + tmp4);
            Console.WriteLine("termino");

            // Receive "hello"
            //int hello = br.ReadInt32();
            //if (hello == IM_Hello)
            //{
            //    // Hello OK, so answer.
            //    bw.Write(IM_Hello);

            //    bw.Write(reg ? IM_Register : IM_Login);  // Login or register
            //    bw.Write(UserName);
            //    bw.Write(Password);
            //    bw.Flush();

            //    byte ans = br.ReadByte();  // Read answer.
            //    if (ans == IM_OK)  // Login/register OK
            //    {
            //        if (reg)
            //            OnRegisterOK();  // Register is OK.
            //        OnLoginOK();  // Login is OK (when registered, automatically logged in)
            //        Receiver(); // Time for listening for incoming messages.
            //    }
            //    else
            //    {
            //        IMErrorEventArgs err = new IMErrorEventArgs((IMError)ans);
            //        if (reg)
            //            OnRegisterFailed(err);
            //        else
            //            OnLoginFailed(err);
            //    }
            //}
            //if (_conn)
                CloseConn();
        }
        void CloseConn() // Close connection.
        {
            br.Close();
            bw.Close();
   
            netStream.Close();
            client.Close();
          
        }
        //void Receiver()  // Receive all incoming packets.
        //{
        //    _logged = true;

        //    try
        //    {
        //        while (client.Connected)  // While we are connected.
        //        {
        //            byte type = br.ReadByte();  // Get incoming packet type.

        //            if (type == IM_IsAvailable)
        //            {
        //                string user = br.ReadString();
        //                bool isAvail = br.ReadBoolean();
        //                OnUserAvail(new IMAvailEventArgs(user, isAvail));
        //            }
        //            else if (type == IM_Received)
        //            {
        //                string from = br.ReadString();
        //                string msg = br.ReadString();
        //                OnMessageReceived(new IMReceivedEventArgs(from, msg));
        //            }
        //        }
        //    }
        //    catch (IOException) { }

        //    _logged = false;
        //}

        //// Packet types
        //public const int IM_Hello = 2012;      // Hello
        //public const byte IM_OK = 0;           // OK
        //public const byte IM_Login = 1;        // Login
        //public const byte IM_Register = 2;     // Register
        //public const byte IM_TooUsername = 3;  // Too long username
        //public const byte IM_TooPassword = 4;  // Too long password
        //public const byte IM_Exists = 5;       // Already exists
        //public const byte IM_NoExists = 6;     // Doesn't exist
        //public const byte IM_WrongPass = 7;    // Wrong password
        //public const byte IM_IsAvailable = 8;  // Is user available?
        //public const byte IM_Send = 9;         // Send message
        //public const byte IM_Received = 10;    // Message received
        
      
    
    }
}
