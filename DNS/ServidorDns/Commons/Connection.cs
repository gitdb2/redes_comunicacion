using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Comunicacion;

namespace uy.edu.ort.obligatorio.Commons
{
    public class Connection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpClient tcpClient;
        private NetworkStream networkStream;
        
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }
       
        public string Name { get; set; }

        public string Ip { get; set; }
        public int Port { get; set; }
        public int UserCount { get; set; }

        public IReceiveEvent EventHandler { get; set; }

        public Connection(TcpClient c, IReceiveEvent ire):this("Unknown", c, ire)
        {           
        }

        public Connection(string name, TcpClient c, IReceiveEvent ire)
        {
            Name = name;
            tcpClient = c;
            EventHandler = ire;
            (new Thread(new ThreadStart(SetupConn))).Start();
        }

        public void WriteToStream(char[] data)
        {
            lock (this)
            {
                StreamWriter.Write(data);
                StreamWriter.Flush();
            }
        }

        void SetupConn()
        {
            try
            {
                Console.WriteLine("[{0}] New connection!", DateTime.Now);
                log.Info("Setup  New connection!");

                networkStream = tcpClient.GetStream();
                StreamReader = new StreamReader(networkStream, Encoding.UTF8);
                StreamWriter = new StreamWriter(networkStream, Encoding.UTF8);
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
                    notEnd = EventHandler.OnReceiveData(this);
                }
                catch (Exception e)
                {
                    notEnd = EventHandler.OnFatalError(this);

                    Console.WriteLine("Se rompio la conexion de: " + this.Name);
                    log.Error("Error mientras se cerraba la conexion", e);
                 //   Console.WriteLine(e.StackTrace);
                    Console.WriteLine("Error: "+e.Message);
                    notEnd = false;
                    CloseConn();
                }
            }
            try
            {
                CloseConn();
            }
            catch { }
            Console.WriteLine("termino");
            log.Info("Termian Receive Data!");
        }

        public void CloseConn() // Close connection.
        {
            log.Info("Cerrando connection!");
            try
            {
                StreamReader.Close();
                StreamWriter.Close();
                networkStream.Close();
                tcpClient.Close();
                Console.WriteLine("[{0}] End of connection!", DateTime.Now);
                log.Info("End of connection!");
            }
            catch (Exception e) {
                log.Error("Error mientras se cerraba la conexion", e);
              //  Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
            notEnd = false;
        }
    
    }

}
