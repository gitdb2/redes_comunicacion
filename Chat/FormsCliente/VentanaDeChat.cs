using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dominio;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using ClientImplementation;

namespace Chat
{
    public partial class VentanaDeChat : Form
    {
        private string ChattingWith;
        private ClientHandler clientHandler;
        private VentanaPrincipalCliente mainWindow;

        private const string patronFecha = "yyyy-MM-dd HH:mm:ss";

        public VentanaDeChat(string contacto,  VentanaPrincipalCliente mainWindow)
        {
            InitializeComponent();
            this.ChattingWith = contacto;
            this.mainWindow = mainWindow;
            this.clientHandler = ClientHandler.GetInstance();
            SetupChatWindow(contacto);
        }

        public void WriteMessage(ChatMessageEventArgs e) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(DateTime.Now.ToString(patronFecha)).Append(") ");
            sb.Append(e.ClientFrom).Append(": ");
            sb.Append(e.Message).Append("\r\n");
            txtBoxChat.AppendText(sb.ToString());
        }

        private void SetupChatWindow(string nombreUsuario)
        {
            this.Text = clientHandler.Login +" Chateando Con " + nombreUsuario;
            txtBoxChat.AppendText("(" + DateTime.Now.ToString(patronFecha) + ") Estas Chateando Con: " + nombreUsuario + "\r\n");
        }

        private void btnEnviarMensaje_Click(object sender, EventArgs e)
        {
            EnviarMensaje();
            txtBoxMensaje.Focus();
        }

        private void EnviarMensaje()
        {
            if (txtBoxMensaje.Lines.Length >= 1)
            {
                //envio el mensaje al destinatario
                clientHandler.SendChatMessage(clientHandler.Login, ChattingWith, txtBoxMensaje.Text);

                //imprimo el mensaje enviado en la ventana
                WriteMessage(new ChatMessageEventArgs() { ClientFrom = "Tu", Message = txtBoxMensaje.Text });

                //bloqueo el boton de enviar hasta que llegue el ok del mensaje enviado
                //se habilita en el evento que recibe dicha notificacion
                this.btnEnviarMensaje.Enabled = false;

                txtBoxMensaje.Lines = null;
            }
            txtBoxMensaje.Text = "";
        }

        private void txtBoxMensaje_KeyPress(object sender, KeyPressEventArgs e)
        {
            //si apretan enter enviar el mensaje
            if (e.KeyChar == (char)Keys.Enter)
            {
                EnviarMensaje();
            }
        }

        public void EnableSendChatButton()
        {
            this.btnEnviarMensaje.Enabled = true;
        }

        public void NotifyContactDisconnected()
        {
            MessageBox.Show("No es posible continuar chateando con " + ChattingWith + ", ya que perdio su conexion con el servidor.",
            "Contacto Desconectado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.btnEnviarMensaje.Enabled = false;
            this.txtBoxMensaje.Enabled = false;
        }

        private void VentanaDeChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainWindow.RemoveChatWindow(this.ChattingWith);
        }

    }
}
