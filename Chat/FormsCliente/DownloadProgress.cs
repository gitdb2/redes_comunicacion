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

        public DownloadProgress(FileDownloader fd)
        {
            InitializeComponent();
            this.fileDownloader = fd;
            updateProgressBarEventHandler = new FileDownloader.UpdateProgressBarEventHandler(UpdateProgressBarEvent);
            fileDownloader.UpdateProgressBar += updateProgressBarEventHandler;
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
                    this.lblStatus.Text = "Descarga Completa!";
                    this.btnCancelar.Text = "Cerrar";
                }
            }));
        }

        private void DownloadProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            fileDownloader.UpdateProgressBar -= updateProgressBarEventHandler;
            fileDownloader.Cancel = true;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            fileDownloader.UpdateProgressBar -= updateProgressBarEventHandler;
            fileDownloader.Cancel = true;
            this.Dispose();
        }

    }
}
