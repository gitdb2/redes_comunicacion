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

namespace Chat
{
    public partial class BuscarArchivo : Form
    {

        private string HashQuery {get;set;}

        private ClientHandler.ServerListReceivedEventHandler serverListReceivedEventHandler;
        

        private string GenerateHashQuery(string pattern){
             HashQuery = StringUtils.CalculateMD5Hash(String.Format("{0}|{1}|{2}", ClientHandler.GetInstance().Login, pattern, "" + DateTime.Now));
             return HashQuery;
       }

        public BuscarArchivo()
        {
            InitializeComponent();
            GenerateHashQuery("INIT");

            serverListReceivedEventHandler = new ClientHandler.ServerListReceivedEventHandler(EventServerListReceivedResponse);
          
            ClientHandler.GetInstance().ServerListReceived += serverListReceivedEventHandler;
           
        }



        private void EventServerListReceivedResponse(object sender, GetServersEventArgs arg)
        {
            this.BeginInvoke((Action)(delegate
            {



                MultiplePayloadFrameDecoded payload = arg.Response;
/*
                //agrego los contactos a la lista acumulada de contactos
                e.ContactList.ToList().ForEach(x => tmpContactList.Add(x.Key, x.Value));

                //cuando me mandaron la ultima porcion de la lista de contactos refresco el form
                if (e.IsLastPart)
                {
                    listaContactos.Items.Clear();
                    foreach (KeyValuePair<string, bool> contacto in tmpContactList)
                    {
                        ListViewItem lvi = new ListViewItem(contacto.Key);
                        lvi.Tag = contacto;
                        SetearEstadoContacto(lvi, contacto);
                        listaContactos.Items.Add(lvi);
                    }
                    FormUtils.AjustarTamanoColumnas(listaContactos);

                    //reseteo la lista de contactos temporal
                    tmpContactList.Clear();
                }
 * */
            }));
        }

        

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            

            string pattern = txtBuscarArchivo.Text;
            if (pattern != null && !pattern.Trim().Equals(""))
            {
                btnBuscar.Enabled = false;
                ClientHandler.GetInstance().REQGetServerList(GenerateHashQuery(pattern));// .FindContact(Login, pattern);
            }

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

      

        private void BuscarArchivo_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientHandler.GetInstance().ServerListReceived -= serverListReceivedEventHandler;
            //clientHandler.AddContactResponse -= addContactsResponse;
        }

    }
}
