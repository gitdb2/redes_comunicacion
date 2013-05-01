using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientImplementation;
using uy.edu.ort.obligatorio.Commons;

namespace Chat
{
    public partial class AgregarContacto : Form
    {
        public string Login { get; set; }
        private ClientHandler clientHandler;
        private ClientHandler.FindContactsEventHandler findContactsEventHandler;
        private ClientHandler.AddContactEventHandler addContactsResponse;

        //lista de contactos temporal en la que se acumulan todas las llegadas de RES02
        //una vez que llego la ultima porcion de la lista se refresca el form y se vacia esta lista
        private Dictionary<string, bool> tmpContactList = new Dictionary<string, bool>();

        public AgregarContacto()
        {
            InitializeComponent();
            clientHandler = ClientHandler.GetInstance();
            findContactsEventHandler = new ClientHandler.FindContactsEventHandler(EventFindContactsResponse);
            addContactsResponse = new ClientHandler.AddContactEventHandler(EventAddContactResponse);
            clientHandler.FindContactResponse += findContactsEventHandler;
            clientHandler.AddContactResponse += addContactsResponse;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string pattern = txtBuscarContacto.Text;
            if (pattern != null && !pattern.Trim().Equals(""))
            {
                clientHandler.FindContact(Login, pattern);
            }
        }

        private void EventFindContactsResponse(object sender, ContactListEventArgs e)
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
                KeyValuePair<string, bool> contactSelected = (KeyValuePair<string, bool>)listaContactos.SelectedItems[0].Tag;
                clientHandler.AddContact(Login, contactSelected.Key);
            }
        }

        private void EventAddContactResponse(object sender, SimpleEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                string opResult = e.Message.Split('|')[4];
                string message;
                MessageBoxIcon icon;

                if (opResult.Equals(MessageConstants.MESSAGE_SUCCESS))
                {
                    message = "Contacto agregado con exito";
                    icon = MessageBoxIcon.Information;
                }
                else
                {
                    message = "Ocurrio un error al agregar el contacto";
                    icon = MessageBoxIcon.Error;
                }
                MessageBox.Show(message, "Alta de Contacto", MessageBoxButtons.OK, icon);
            }));
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void AgregarContacto_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientHandler.FindContactResponse -= findContactsEventHandler;
            clientHandler.AddContactResponse -= addContactsResponse;
        }

    }
}
