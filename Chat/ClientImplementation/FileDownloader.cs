using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;
using System.Net.Sockets;
using System.IO;
using Comunicacion;
using System.Threading;

namespace ClientImplementation
{
    public class FileDownloader
    {

        public string Destination { get; set; }
        public FileObject FileSelected { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public bool Cancel { get; set; }

        private TcpClient dwnldTcpClient;
        private NetworkStream dwnldNetStream;

        private StreamReader dwnldStreamReader; 
        private StreamWriter dwnldStreamWriter;

        private bool done = false;
        private int percentageDownloaded = 0;

        public void Download()
        {
            SetupConnection();
            SendDownloadRequest();
            DownloadFile();
            CloseConnection();
            this.Cancel = false;
        }

        private void DownloadFile()
        {
            Data reqDownloadResponse = DataProccessor.GetInstance().LoadObject(dwnldStreamReader);

            string[] payload = reqDownloadResponse.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE);
            string login = payload[0];
            string owner = payload[1];
            string hashfile = payload[2];
            string filename = payload[3];
            long size = long.Parse(payload[4]);

            const int BUFF_SIZE = 1024;
            byte[] buffer = new byte[BUFF_SIZE];
            
            long bytescount = 0;
            
            BinaryWriter writer = new BinaryWriter(File.Open(Destination, FileMode.Create));
            NotifyProgress("Descargando ...");

            try
            {
                while (!done && !Cancel)
                {
                    int countRead = dwnldNetStream.Read(buffer, 0, BUFF_SIZE);
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
                    {
                        //no leyo nada de la entrada (cantidad de bytes justa, en la siguiente lectura)
                        done = true;
                    }
                    if (!Cancel)
                    {
                        if (bytescount == size && size == 0)
                        {
                            percentageDownloaded = 99;
                            done = true;
                        }
                        else
                        {
                            percentageDownloaded = (int)(bytescount * 100 / size);
                        }
                        NotifyProgress((done ? "Descarga completa!" : "Descargando ..."));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                writer.Close();
            }
        }

        private void SendDownloadRequest()
        {
            NotifyProgress("Enviando peticion de descarga ...");

            StringBuilder sb = new StringBuilder();
            sb.Append(FileSelected.Name).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(FileSelected.Owner).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(FileSelected.Hash);
            Data dataRequestDownload = new Data()
            {
                Command = Command.REQ,
                OpCode = OpCodeConstants.REQ_DOWNLOAD_FILE,
                Payload = new Payload(sb.ToString())
            };
            foreach (var item in dataRequestDownload.GetBytes())
            {
                dwnldStreamWriter.Write(item);
                dwnldStreamWriter.Flush();
            }
        }

        private void SetupConnection()
        {
            NotifyProgress("Conectando con servidor ...");
            dwnldTcpClient = new TcpClient(ServerInfo.Ip, ServerInfo.TransfersPort);
            dwnldNetStream = dwnldTcpClient.GetStream();
            dwnldStreamReader = new StreamReader(dwnldNetStream, Encoding.UTF8);
            dwnldStreamWriter = new StreamWriter(dwnldNetStream, Encoding.UTF8);
        }

        private void NotifyProgress(string message)
        {
            OnDownloadProgressChanged(new ProgressBarEventArgs() {
                CurrentAction = message,
                CurrentPercentage = percentageDownloaded,
                IsCompleted = done
            });
        }

        private void CloseConnection()
        {
            NotifyProgress("Cerrando conexion ...");
            dwnldStreamReader.Dispose();
            dwnldStreamWriter.Dispose();
            dwnldNetStream.Close();
            dwnldTcpClient.Close();
            NotifyProgress("Descarga completa!");
        }

        public delegate void UpdateProgressBarEventHandler(object sender, ProgressBarEventArgs e);

        public event UpdateProgressBarEventHandler UpdateProgressBar;

        public void OnDownloadProgressChanged(ProgressBarEventArgs args)
        {
            if (UpdateProgressBar != null)
                UpdateProgressBar(this, args);
        }

        public void DownloadThread()
        {
            (new Thread(new ThreadStart(Download))).Start();
        }
    }
}
