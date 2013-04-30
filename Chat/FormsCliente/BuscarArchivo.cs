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
            Archivo archivo = (Archivo)listaArchivos.SelectedItems[0].Tag;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Descargar Archivo";
            sfd.FileName = archivo.Nombre;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Guardo el archivo en la ruta: " + sfd.FileName);
                //System.IO.FileStream fs = (System.IO.FileStream)sfd.OpenFile();
            }
        }

    }
}
