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
    public class FileUploader
    {
        public string Destination { get; set; }
        public FileObject FileSelected { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public bool Cancel { get; set; }

        private TcpClient uploadTcpClient;
        private NetworkStream uploadNetStream;

        private StreamReader uploadStreamReader; 
        private StreamWriter uploadStreamWriter;

        private bool done = false;
        private int percentageUploaded = 0;

        public void Upload()
        {
            SetupConnection();
            SendUploadRequest();
            UploadFile();
            CloseConnection();
            this.Cancel = false;
        }

        private void SendUploadRequest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FileSelected.Owner).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(FileSelected.Name).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(FileSelected.Size);
            Data dataRequestDownload = new Data()
            {
                Command = Command.REQ,
                OpCode = OpCodeConstants.REQ_UPLOAD_FILE,
                Payload = new Payload(sb.ToString())
            };
            foreach (var item in dataRequestDownload.GetBytes())
            {
                uploadStreamWriter.Write(item);
                uploadStreamWriter.Flush();
            }
        }

        private void UploadFile()
        {
            Data reqUploadResponse = DataProccessor.GetInstance().LoadObject(uploadStreamReader);
            if (reqUploadResponse.Payload.Message.Equals(MessageConstants.MESSAGE_SERVER_READY))
            {
                FileInfo fileInfo = new FileInfo(FileSelected.FullName);
                FileStream fileStream = fileInfo.OpenRead();

                const int BUFF_SIZE = 1024;
                byte[] buffer = new byte[BUFF_SIZE];

                long bytesCount = 0;

                while (!done && !Cancel)
                {
                    int countRead = fileStream.Read(buffer, 0, BUFF_SIZE);
                    bytesCount += countRead;
                    if (countRead > 0)
                    {
                        if (countRead < BUFF_SIZE)
                        {
                            percentageUploaded = 100;
                            done = true;
                        }
                        uploadNetStream.Write(buffer, 0, countRead);
                        uploadNetStream.Flush();
                    }
                    else
                    {
                        //no leyo nada de la entrada (cantidad de bytes justa, en la siguiente lectura)
                        done = true;
                    }
                    if (!Cancel)
                    {
                        if (bytesCount == FileSelected.Size && FileSelected.Size == 0)
                        {
                            percentageUploaded = 100;
                            done = true;
                        }
                        else
                        {
                            percentageUploaded = (int)(bytesCount * 100 / FileSelected.Size);
                        }
                    }
                    NotifyProgress(done ? "Subida completa !" : "Subiendo ...");
                }
                fileStream.Close();
            }
            else
            {
                throw new Exception("El servidor no esta disponible para descargas");
            }
        }

        private void SetupConnection()
        {
            uploadTcpClient = new TcpClient(ServerInfo.Ip, ServerInfo.TransfersPort);
            uploadNetStream = uploadTcpClient.GetStream();
            uploadStreamReader = new StreamReader(uploadNetStream, Encoding.UTF8);
            uploadStreamWriter = new StreamWriter(uploadNetStream, Encoding.UTF8);
        }

        public delegate void UpdateProgressBarEventHandler(object sender, ProgressBarEventArgs e);

        public event UpdateProgressBarEventHandler UpdateProgressBar;

        public void OnDownloadProgressChanged(ProgressBarEventArgs args)
        {
            if (UpdateProgressBar != null)
                UpdateProgressBar(this, args);
        }

        private void NotifyProgress(string message)
        {
            OnDownloadProgressChanged(new ProgressBarEventArgs()
            {
                CurrentAction = message,
                CurrentPercentage = percentageUploaded,
                IsCompleted = done
            });
        }

        private void CloseConnection()
        {
            uploadStreamReader.Dispose();
            uploadStreamWriter.Dispose();
            uploadNetStream.Close();
            uploadTcpClient.Close();
        }

        public void UploadThread()
        {
            (new Thread(new ThreadStart(Upload))).Start();
        }
        
    }
}
