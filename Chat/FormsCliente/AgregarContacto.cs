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
    public partial class AgregarContacto : Form
    {
        public string Login { get; set; }
        private ClientHandler clientHandler;

        //lista de contactos temporal en la que se acumulan todas las llegadas de RES02
        //una vez que llego la ultima porcion de la lista se refresca el form y se vacia esta lista
        private Dictionary<string, bool> tmpContactList = new Dictionary<string, bool>();

        public AgregarContacto()
        {
            InitializeComponent();
            clientHandler = ClientHandler.GetInstance();
            clientHandler.FindContactResponse += new ClientHandler.FindContactsEventHandler(EventFindContactsResponse);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string pattern = txtBuscarContacto.Text;
            if (pattern != null && !pattern.Trim().Equals(""))
            {
                clientHandler.FindContact(Login, pattern);
            }
        }

        void EventFindContactsResponse(object sender, ContactListEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
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
            }));
        }

        private void SetearEstadoContacto(ListViewItem lvi, KeyValuePair<string, bool> contacto)
        {
            lvi.UseItemStyleForSubItems = false;
            Color colorEstado = contacto.Value ? Color.Green : Color.Gray;
            string estado = contacto.Value ? "Conectado" : "Desconectado";
            lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, estado, Color.White, colorEstado, lvi.Font));
        }

        private void btnAgregarContacto_Click(object sender, EventArgs e)
        {
            //agregar el contacto
            if (FormUtils.HayFilaElegida(listaContactos))
            {
                Usuario contacto = (Usuario) listaContactos.SelectedItems[0].Tag;
                MessageBox.Show("Agregado el contacto " + contacto.Nombre);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
