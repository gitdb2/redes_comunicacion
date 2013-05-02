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
        private FileUploader.UploadCancelledEventHandler uploadCancelledEventHandler;
        private ClientHandler.ServerInfoResponseEventHandler serverInfoResponseEventHandler;

        public SubirArchivo()
        {
            InitializeComponent();
            clientHandler = ClientHandler.GetInstance();
            fileUploader = new FileUploader();
            updateProgressBarEventHandler = new FileUploader.UpdateProgressBarEventHandler(UpdateProgressBarEvent);
            uploadCancelledEventHandler = new FileUploader.UploadCancelledEventHandler(UploadCancelledEvent);
            serverInfoResponseEventHandler = new ClientHandler.ServerInfoResponseEventHandler(ServerInfoEvent);
            fileUploader.UpdateProgressBar += updateProgressBarEventHandler;
            fileUploader.UploadCancelled += uploadCancelledEventHandler;
            clientHandler.ServerInfoResponse += serverInfoResponseEventHandler;

            this.btnCerrar.Text = "Cancelar";
        }

        private void UploadCancelledEvent(object sender, SimpleEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                MessageBox.Show(e.Message, "Descarga de archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.lblStatus.Text = "Ocurrio un error";
                this.progressBar.Value = 0;
                this.btnCerrar.Text = "Cerrar";
            }));
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
            fileUploader.UploadCancelled -= uploadCancelledEventHandler;
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
                try
                {
                    this.lblStatus.Text = "Iniciando Subida";
                    clientHandler.GetServerInfo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Descarga de archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SubirArchivo_FormClosing(object sender, FormClosingEventArgs e)
        {
            fileUploader.Cancel = true;
            fileUploader.UpdateProgressBar -= updateProgressBarEventHandler;
            clientHandler.ServerInfoResponse -= serverInfoResponseEventHandler;
            fileUploader.UploadCancelled -= uploadCancelledEventHandler;
            this.Dispose();
        }

    }
}
