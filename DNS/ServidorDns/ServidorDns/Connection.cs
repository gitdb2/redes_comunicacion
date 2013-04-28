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
    public class Connection : IConnection
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        public int Port { get; set; }

        public Connection(TcpClient c)
        {
            tcpClient = c;
            (new Thread(new ThreadStart(SetupConn))).Start();
        }

        public void WriteToStream(char[] data)
        {
            lock (this)
            {
                streamWriter.Write(data);
                streamWriter.Flush();
            }
        }

        void SetupConn()  // Setup connection and login or register.
        {
            try
            {
                Console.WriteLine("[{0}] New connection!", DateTime.Now);
                networkStream = tcpClient.GetStream();
                streamReader = new StreamReader(networkStream, Encoding.UTF8);
                streamWriter = new StreamWriter(networkStream, Encoding.UTF8);
                ReceiveData();
            }
            finally { 
                CloseConn(); 
            }
        }

        bool notEnd = true;

        private void ReceiveData()
        {
            while (notEnd)
            {
                try
                {
                    Data dato = DataProccessor.GetInstance().LoadObject(streamReader);
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
                streamReader.Close();
                streamWriter.Close();
                networkStream.Close();
                tcpClient.Close();
                Console.WriteLine("[{0}] End of connection!", DateTime.Now);
            }
            catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
    
    }

}
