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

        Semaphore semWrite;//= new Semaphore(0, 1);
        Semaphore semRead;//= new Semaphore(0, 1);

        public string Name { get; set; }

        public string Ip { get; set; }
        public int Port { get; set; }
        public int TransferPort { get; set; }
        public int UserCount { get; set; }
        public bool IsServer { get; set; }

        private Thread threadRead;

        public IReceiveEvent EventHandler { get; set; }

        private ConnectionDroppedDelegate onConnectionDropDelegate = null;

        //delegado para que la conexion avise a alguien cuando cae.
        public delegate void ConnectionDroppedDelegate(String idName);

        public Connection(TcpClient c, IReceiveEvent ire)
            : this("Unknown", c, ire, null)
        {
        }
        public Connection(string name, TcpClient c, IReceiveEvent ire)
            : this(name, c, ire, null)
        {
        }
        public Connection( string name, TcpClient c, IReceiveEvent ire, ConnectionDroppedDelegate dropConDelegate)
        {
           
            onConnectionDropDelegate =  dropConDelegate;
           
            IsServer = false;
            Name = name;
            tcpClient = c;
            EventHandler = ire;
            semWrite = new Semaphore(0, 1);
            semRead = new Semaphore(0, 1);
            (threadRead = new Thread(new ThreadStart(SetupConn))).Start();
        }

      

        public void WriteToStream(char[] data)
        {


            semWrite.WaitOne();
            StreamWriter.Write(data);
            StreamWriter.Flush();
            semWrite.Release();


        }

        void SetupConn()
        {
            try
            {
                Console.WriteLine("[{0}] New connection!", DateTime.Now);



                networkStream = tcpClient.GetStream();
                StreamReader = new StreamReader(networkStream, Encoding.UTF8);
                StreamWriter = new StreamWriter(networkStream, Encoding.UTF8);
                semWrite.Release();
                semRead.Release();

                ReceiveData();
            }
            finally
            {
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
                    log.Error("Catch excepcion y cerrado de conexion", e);
                    Console.WriteLine("Error: " + e.Message);
                    notEnd = false;
                }
            }
            try
            {
                log.Info("Cerrando la conexion!");
                CloseConn();
            }
            catch { }
            log.Info("Termian Receive Data!");
        }

        public void CloseConn() // Close connection.
        {
           
            try
            {
                notEnd = false;
                StreamReader.Close();
                StreamWriter.Close();
                networkStream.Close();
                tcpClient.Close();
                Console.WriteLine("[{0}] End of connection!", DateTime.Now);
                log.Info("End of connection!");

              
            }
            catch (Exception e)
            {
                log.Error("Error mientras se cerraba la conexion", e);
                //  Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);

            }
            finally
            {
                if (onConnectionDropDelegate != null)
                {
                    onConnectionDropDelegate(Name);
                }
               
            }
            notEnd = false;
        }


        public void WriteToNetworkStream(byte[] buffer, int offset, int size)
        {
            semWrite.WaitOne();
            networkStream.Write(buffer, offset, size);
            networkStream.Flush();
            semWrite.Release();
        }

    }

}
