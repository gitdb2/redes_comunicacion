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

namespace Chat
{
    public partial class VentanaDeChat : Form
    {
        private Usuario ChateandoCon;
        private string NombreUsuario;

        private const string patronFecha = "yyyy-MM-dd HH:mm";

        public VentanaDeChat(Usuario contacto, string nombreUsuario)
        {
            InitializeComponent();
            this.ChateandoCon = contacto;
            this.NombreUsuario = nombreUsuario;
            MostrarMensajeInicial(contacto.Nombre);
        }

        private void MostrarMensajeInicial(string nombreUsuario)
        {
            txtBoxChat.AppendText("(" + DateTime.Now.ToString(patronFecha) + ") Estas chateando con: " + nombreUsuario + "\r\n");
        }

        private void btnEnviarMensaje_Click(object sender, EventArgs e)
        {
            EnviarMensaje();
            txtBoxMensaje.Focus();
        }

        private void ActualizarVentanaDeChat(string strMessage)
        {
            txtBoxChat.AppendText("(" + DateTime.Now.ToString(patronFecha) + ") NombreUsuario: " + strMessage + "\r\n");
        }

        private void EnviarMensaje()
        {
            if (txtBoxMensaje.Lines.Length >= 1)
            {
                //enviar mensajes
                ActualizarVentanaDeChat(txtBoxMensaje.Text);

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

    }
}
