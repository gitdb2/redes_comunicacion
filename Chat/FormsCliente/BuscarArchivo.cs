using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dominio;
using ClientImplementation;
using uy.edu.ort.obligatorio.Commons;
using FormsCliente;
using System.Threading;

namespace Chat
{
    public partial class BuscarArchivo : Form
    {

   

     /*   private ClientHandler.FindFilesEventHandler findFilesEventHandler;
        private ClientHandler.AddContactEventHandler addContactsResponse;
        */
        public BuscarArchivo()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {




            //if (FormUtils.TxtBoxTieneDatos(txtBuscarArchivo))
            //{
            //    string patron = txtBuscarArchivo.Text;
            //    listaArchivos.Items.Clear();
            //    Controlador controlador = new Controlador();
            //    List<Archivo> archivosEncontrados = controlador.BuscarArchivos(patron);
            //    foreach (Archivo archivo in archivosEncontrados)
            //    {
            //        ListViewItem lvi = new ListViewItem(archivo.Nombre);
            //        lvi.Tag = archivo;
            //        lvi.SubItems.Add(archivo.Servidor);
            //        listaArchivos.Items.Add(lvi);
            //    }
            //    FormUtils.AjustarTamanoColumnas(listaArchivos);
            //}
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            //FileObject fileSelected = (FileObject)listaArchivos.SelectedItems[0].Tag;
            //lo voy a tomar de un diccionario
            //ServerInfo serverInfo = new ServerInfo();
            
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Descargar Archivo";
            sfd.FileName = "imagen.jpg"; //fileSelected.Name;
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string destino = @"c:\shared\mauricio\res.iso";
                FileObject fo = new FileObject() { 
                    Name = ClientHandler.GetInstance().Login, 
                    // imagen.png Hash = "f4f1a7a2a9f6284dd0bfa7558fa134da",
                    // exe Hash = "eefc05a7ff11a84d350d561a63014a47",
                    Hash = "e1e8c17baf81af6722feb8987269f22e",
                    Owner = "mauricio",
                    Server = "server1"
                };

                ServerInfo si = new ServerInfo() { Ip = "127.0.0.1", Name = "server1", TransfersPort = 20001 };

                FileDownloader fd = new FileDownloader()
                {
                    Destination = destino,
                    FileSelected = fo,
                    ServerInfo = si
                };
                
                DownloadProgress dp = new DownloadProgress(fd);
                dp.Show();

                //esto tendria que ser en una nueva ventana que se updatee con delegados
                fd.DownloadThread();

                //FileDownloader fd = new FileDownloader() { 
                //    Destination = sfd.FileName
                //    , FileSelected = fileSelected
                //    , ServerInfo = serverInfo
                //};
                
            }
        }

        //void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    this.BeginInvoke((MethodInvoker)delegate
        //    {
        //        double bytesIn = double.Parse(e.BytesReceived.ToString());
        //        double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
        //        double percentage = bytesIn / totalBytes * 100;
        //        label2.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
        //        progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        //    });
        //}
        //void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        //{
        //    this.BeginInvoke((MethodInvoker)delegate
        //    {
        //        label2.Text = "Completed";
        //    });
        //}


    }
}
