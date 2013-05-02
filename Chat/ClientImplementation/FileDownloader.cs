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

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Destination { get; set; }
        public FileObject FileSelected { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public bool Cancel { get; set; }

        private TcpClient dwnldTcpClient;
        private NetworkStream dwnldNetStream;
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



        private void SendDownloadRequest()
        {
            try
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
            catch (Exception e)
            {
                Cancel = true;
                done = false;
                FatalError("Error al iniciar descarga ");
            }
        }

        private void DownloadFile()
        {
            BinaryWriter writer = new BinaryWriter(File.Open(Destination, FileMode.Create));

            try
            {
                log.InfoFormat("Inicio descarga del archivo {0}, tamanio {1}", FileSelected.Name, FileSelected.Size);

                long total = 0;
                long remaining = FileSelected.Size;
                int BUFFER_SIZE = 10000;
                byte[] buffer = new byte[BUFFER_SIZE];

                while (remaining > 0 && !Cancel)
                {
                    int read = dwnldNetStream.Read(buffer, 0, BUFFER_SIZE);
                    total += read;
                    if (read <= 0)
                    {
                        percentageDownloaded = 100;
                        done = true;
                    }
                    writer.Write(buffer, 0, read);
                    log.InfoFormat("Lei {0} bytes de {1}", total, FileSelected.Size);
                    percentageDownloaded = (int)(total * 100 / FileSelected.Size);
                    NotifyProgress((done ? "Descarga completa!" : "Descargando ..."));
                    remaining -= read;
                }

                if (remaining == 0)
                    NotifyProgress((done ? "Descarga completa!" : "Descargando ..."));
            }
            catch (Exception e)
            {
                Cancel = true;
                done = false;
                FatalError("Error al descargar");
            }
            finally
            {
                writer.Close();
            }
        }

        private void SetupConnection()
        {
            try
            {
                NotifyProgress("Conectando con servidor ...");
                dwnldTcpClient = new TcpClient(ServerInfo.Ip, ServerInfo.TransfersPort);
                dwnldNetStream = dwnldTcpClient.GetStream();
                dwnldStreamWriter = new StreamWriter(dwnldNetStream, Encoding.UTF8);

            }
            catch
            {
                Cancel = true;
            }
        }

        private void NotifyProgress(string message)
        {
            OnDownloadProgressChanged(new ProgressBarEventArgs()
            {
                CurrentAction = message,
                CurrentPercentage = percentageDownloaded,
                IsCompleted = done
            });
        }

        private void CloseConnection()
        {
            try
            {
                NotifyProgress("Cerrando conexion ...");
                dwnldStreamWriter.Dispose();
                dwnldNetStream.Close();
                dwnldTcpClient.Close();
                NotifyProgress("Descarga completa!");
            }
            catch (Exception e)
            {
                log.Error("closeConnection", e);
            }
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


        #region delegados errores


        public delegate void DonwloadCancelledEventHandler(object sender, SimpleEventArgs e);

        public event DonwloadCancelledEventHandler DownloadCancelled;

        public void OnDownloadCancelled(SimpleEventArgs args)
        {
            if (DownloadCancelled != null)
                DownloadCancelled(this, args);
        }


        private void FatalError()
        {
            Cancel = true;
            done = false;
            OnDownloadCancelled(new SimpleEventArgs() { Message = "Ocurrio un error durante la subida del archivo, se cancelo el proceso." });
        }

        private void FatalError(string message)
        {
            Cancel = true;
            done = false;
            OnDownloadCancelled(new SimpleEventArgs() { Message = message });
        }

        #endregion
    }
}
