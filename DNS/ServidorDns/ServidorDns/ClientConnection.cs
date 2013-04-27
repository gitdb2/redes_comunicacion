using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Comunicacion;
using uy.edu.ort.obligatorio.LibOperations.intefaces;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class ClientConnection : IConnection
    {

        public TcpClient client;
        public NetworkStream netStream;
        public StreamReader br;
        public StreamWriter bw;
      //  UserInfo userInfo;

        public ClientConnection(TcpClient c)
        {
          
            client = c;

            // Handle client in another thread.
            (new Thread(new ThreadStart(SetupConn))).Start();
        }

        public void WriteToStream(char[] data)
        {
            lock (this)
            {
                bw.Write(data);
                bw.Flush();
            }
        }

       


        void SetupConn()  // Setup connection and login or register.
        {
            try
            {
                Console.WriteLine("[{0}] New connection!", DateTime.Now);
                netStream = client.GetStream();
                br = new StreamReader(netStream, Encoding.UTF8);
                bw = new StreamWriter(netStream, Encoding.UTF8);

       
                ReceiveData();

            }
            finally { CloseConn(); }
        }
        bool notEnd = true;

        private void ReceiveData()
        {

            while (notEnd)
            {
                try
                {
                    Data dato = DataProccessor.GetInstance().LoadObject(br);
                    CommandHandler.GetInstance().Handle(this, dato);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Message);
                    notEnd = false;
                    CloseConn();
                }
            }
         


            Console.WriteLine("termino");
        }

        public void CloseConn() // Close connection.
        {
            try
            {
             //   userInfo.LoggedIn = false;
                br.Close();
                bw.Close();
                netStream.Close();
                client.Close();
                Console.WriteLine("[{0}] End of connection!", DateTime.Now);
            }
            catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
      
    
    }

}
