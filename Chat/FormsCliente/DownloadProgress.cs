using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientImplementation;

namespace FormsCliente
{
    public partial class DownloadProgress : Form
    {
        private FileDownloader.UpdateProgressBarEventHandler updateProgressBarEventHandler;
        private FileDownloader fileDownloader;
        private FileDownloader.DonwloadCancelledEventHandler donwloadCancelledEventHandler;
        public DownloadProgress(FileDownloader fd)
        {
            InitializeComponent();
            this.fileDownloader = fd;
            updateProgressBarEventHandler = new FileDownloader.UpdateProgressBarEventHandler(UpdateProgressBarEvent);
            fileDownloader.UpdateProgressBar += updateProgressBarEventHandler;
            donwloadCancelledEventHandler = new FileDownloader.DonwloadCancelledEventHandler(DownloadCancelledEvent);
            fileDownloader.DownloadCancelled += donwloadCancelledEventHandler;
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
                    this.lblStatus.Text = "Descarga Completa!";
                    this.btnCancelar.Text = "Cerrar";
                }
            }));
        }

        private void DownloadCancelledEvent(object sender, SimpleEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                MessageBox.Show(e.Message, "Descarga de archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.lblStatus.Text = "Ocurrio un error";
                this.progressBar.Value = 0;
                this.btnCancelar.Text = "Cerrar";
            }));
        }


        private void DownloadProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            fileDownloader.UpdateProgressBar -= updateProgressBarEventHandler;
            fileDownloader.DownloadCancelled -= donwloadCancelledEventHandler;
        
            fileDownloader.Cancel = true;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            fileDownloader.UpdateProgressBar -= updateProgressBarEventHandler;
            fileDownloader.DownloadCancelled -= donwloadCancelledEventHandler;
            fileDownloader.Cancel = true;
            this.Dispose();
        }

    }
}
