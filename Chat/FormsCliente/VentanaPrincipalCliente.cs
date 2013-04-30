using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dominio;
using Comunicacion;
using ClientImplementation;
using uy.edu.ort.obligatorio.Commons;

namespace Chat
{
    public partial class VentanaPrincipalCliente : Form
    {
        public string Login { get; set; }
        private ClientHandler clientHandler;

        private ClientHandler.ContactListEventHandler contactListResponse;
        private ClientHandler.UpdateContactStatusEventHandler updateContactStatusResponse;
        private ClientHandler.ChatMessageReceivedEventHandler chatMessageReceived;
        private ClientHandler.ChatMessageSentEventHandler chatMessageSent;

        //las ventanas de chat activas, cuando llegue el evento de mensaje le envio el
        //mensaje a la ventana que corresponda
        private Dictionary<string, VentanaDeChat> chatWindows = new Dictionary<string, VentanaDeChat>();

        //lista de contactos temporal en la que se acumulan todas las llegadas de RES02
        //una vez que llego la ultima porcion de la lista se refresca el form y se vacia esta lista
        private Dictionary<string, bool> tmpContactList = new Dictionary<string,bool>();

        //lista de contactos actual, la uso para los cambios de estado ya que no puedo recorrer el listView
        private Dictionary<string, bool> updateContactList = new Dictionary<string, bool>();

        public VentanaPrincipalCliente()
        {
            InitializeComponent();
            clientHandler = ClientHandler.GetInstance();

            contactListResponse = new ClientHandler.ContactListEventHandler(EventContactListResponse);
            updateContactStatusResponse = new ClientHandler.UpdateContactStatusEventHandler(EventUpdateContactStatusResponse);
            chatMessageReceived = new ClientHandler.ChatMessageReceivedEventHandler(EventChatMessageReceived);
            chatMessageSent = new ClientHandler.ChatMessageSentEventHandler(EventChatMessageSent);

            clientHandler.ChatMessageReceived += chatMessageReceived;
            clientHandler.ContactListResponse += contactListResponse;
            clientHandler.UpdateContactStatusResponse += updateContactStatusResponse;
            clientHandler.ChatMessageSent += chatMessageSent;
        }

        void EventChatMessageSent(object sender, ChatMessageSentEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                if (e.MessageStatus.Equals(MessageConstants.MESSAGE_SUCCESS))
                {
                    this.chatWindows[e.MessageTo].EnableSendChatButton();
                }
                else 
                {
                    this.chatWindows[e.MessageTo].NotifyContactDisconnected();
                    this.chatWindows.Remove(e.MessageTo);
                }
            }));
        }

        void EventContactListResponse(object sender, ContactListEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                //agrego los contactos a la lista acumulada de contactos
                e.ContactList.ToList().ForEach(x => tmpContactList.Add(x.Key, x.Value));
                
                //cuando me mandaron la ultima porcion de la lista de contactos refresco el form
                if (e.IsLastPart) 
                {
                    UpdateFormContactList(tmpContactList);
                }
            }));
        }

        private void UpdateFormContactList(Dictionary<string, bool> from)
        {
            listaContactos.Items.Clear();
            foreach (KeyValuePair<string, bool> contacto in from)
            {
                ListViewItem lvi = new ListViewItem(contacto.Key);
                lvi.Tag = contacto;
                SetearEstadoContacto(lvi, contacto);
                listaContactos.Items.Add(lvi);
            }
            FormUtils.AjustarTamanoColumnas(listaContactos);
            UpdateLocalContactLists();
        }

        private void UpdateLocalContactLists()
        {
            //agrego todos los elementos a la lista de contactos para updates
            foreach (KeyValuePair<string, bool> item in tmpContactList)
            {
                updateContactList.Add(item.Key, item.Value);
            }
            //reseteo la lista de contactos temporal
            tmpContactList.Clear();
        }

        void EventUpdateContactStatusResponse(object sender, SimpleEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                string contact = e.Message.Split('@')[0];
                bool isConnected = e.Message.Split('@')[1].Equals("1");
                updateContactList[contact] = isConnected;
                UpdateFormContactList(updateContactList);
            }));
        }

        void EventChatMessageReceived(object sender, ChatMessageEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                if (this.chatWindows.ContainsKey(e.ClientFrom))
                {
                    //ya estaba chateando con ese cliente
                    chatWindows[e.ClientFrom].WriteMessage(e);
                }
                else
                { 
                    //una nueva ventana de chat
                    VentanaDeChat vt = CreateChatWindow(e.ClientFrom);
                    vt.WriteMessage(e);
                    vt.Show();
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

        private void menuArchivoOpcionSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void listaContactos_DoubleClick(object sender, EventArgs e)
        {
            KeyValuePair<string, bool> contactSelected = (KeyValuePair<string, bool>)listaContactos.SelectedItems[0].Tag;
            if (contactSelected.Value)
            {
                VentanaDeChat vt = CreateChatWindow(contactSelected.Key);
                vt.Show();
            }
            else
            {
                MessageBox.Show("No es posible chatear con " + contactSelected.Key + ", esta desconectado.",
                    "Contacto Desconectado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private VentanaDeChat CreateChatWindow(string chattingWith)
        {
            VentanaDeChat vt = new VentanaDeChat(chattingWith, this);
            chatWindows.Add(chattingWith, vt);
            return vt;
        }

        private void menuAccionesOpcionAgregarContacto_Click(object sender, EventArgs e)
        {
            AgregarContacto ac = new AgregarContacto() { Login = this.Login};
            ac.ShowDialog();
        }

        private void menuAccionesOpcionBuscarArchivo_Click(object sender, EventArgs e)
        {
            BuscarArchivo ba = new BuscarArchivo();
            ba.ShowDialog();
        }

        private void menuAccionesOpcionSubirArchivo_Click(object sender, EventArgs e)
        {
            SubirArchivo sa = new SubirArchivo();
            sa.ShowDialog();
        }

        private void VentanaPrincipalCliente_Load(object sender, EventArgs e)
        {
            clientHandler.GetContactList(Login);
        }

        private void VentanaPrincipalCliente_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientHandler.ContactListResponse -= contactListResponse;
            clientHandler.UpdateContactStatusResponse -= updateContactStatusResponse;
            clientHandler.ChatMessageReceived -= chatMessageReceived;
            clientHandler.ChatMessageSent -= chatMessageSent;
        }

        public void RemoveChatWindow(string chattingWith)
        {
            this.chatWindows.Remove(chattingWith);
        }

    }
}
