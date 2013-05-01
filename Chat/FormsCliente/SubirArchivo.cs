using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using uy.edu.ort.obligatorio.Commons;
using ClientImplementation;
using System.IO;

namespace Chat
{
    public partial class SubirArchivo : Form
    {
        ClientHandler clientHandler;
        FileUploader fileUploader;
        FileObject fileToUpload;

        private FileUploader.UpdateProgressBarEventHandler updateProgressBarEventHandler;
        private ClientHandler.ServerInfoResponseEventHandler serverInfoResponseEventHandler;

        public SubirArchivo()
        {
            InitializeComponent();
            clientHandler = ClientHandler.GetInstance();
            fileUploader = new FileUploader();
            updateProgressBarEventHandler = new FileUploader.UpdateProgressBarEventHandler(UpdateProgressBarEvent);
            serverInfoResponseEventHandler = new ClientHandler.ServerInfoResponseEventHandler(ServerInfoEvent);
            fileUploader.UpdateProgressBar += updateProgressBarEventHandler;
            clientHandler.ServerInfoResponse += serverInfoResponseEventHandler;

            this.btnCerrar.Text = "Cancelar";
        }

        private void UpdateProgressBarEvent(object sender, ProgressBarEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                if (e.CurrentAction != null)
                    this.lblStatus.Text = e.CurrentAction;

                this.progressBar.Value = e.CurrentPercentage;

                if (e.IsCompleted)
                {
                    this.progressBar.Value = 100;
                    this.lblStatus.Text = "Subida Completa!";
                    this.btnCerrar.Text = "Cerrar";
                }
            }));
        }

        private void ServerInfoEvent(object sender, ServerInfoEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                fileUploader.ServerInfo = e.ServerInfo;
                fileUploader.FileSelected = fileToUpload;
                fileUploader.UploadThread();
            }));
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            fileUploader.Cancel = true;
            fileUploader.UpdateProgressBar -= updateProgressBarEventHandler;
            clientHandler.ServerInfoResponse -= serverInfoResponseEventHandler;
            this.Dispose();
        }

        private void btnElegirArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Elija un archivo";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxArchivo.Text = ofd.FileName;
                fileToUpload = new FileObject() {
                    Owner = ClientHandler.GetInstance().Login,
                    FullName = ofd.FileName,
                    Name = ofd.SafeFileName,
                    Size = new FileInfo(ofd.FileName).Length
                };
            }
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            if (FormUtils.TxtBoxTieneDatos(txtBoxArchivo))
            {
                this.lblStatus.Text = "Iniciando Subida";
                clientHandler.GetServerInfo();
            }
        }

        private void SubirArchivo_FormClosing(object sender, FormClosingEventArgs e)
        {
            fileUploader.Cancel = true;
            fileUploader.UpdateProgressBar -= updateProgressBarEventHandler;
            clientHandler.ServerInfoResponse -= serverInfoResponseEventHandler;
            this.Dispose();
        }

    }
}
