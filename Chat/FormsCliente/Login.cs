using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FormsCliente;
using Comunicacion;
using ClientImplementation;

namespace Chat
{
    public partial class Login : Form
    {
        private ClientHandler clientHandler;

        public Login()
        {
            InitializeComponent();
            clientHandler = ClientHandler.GetInstance();
            clientHandler.LoginOK += new EventHandler(EventLoginOK);
            clientHandler.LoginFailed += new ClientHandler.ChatErrorEventHandler(EventLoginFailed);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (FormUtils.TxtBoxTieneDatos(txtBoxLogin))
            {
                this.btnOK.Enabled = false;
                try 
                {
                    //intento establecer la conexion con el dns
                    clientHandler.Connect(txtBoxLogin.Text);
                    //envio el request de login
                    clientHandler.LoginClient(txtBoxLogin.Text);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Mensaje detallado " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                MessageBox.Show("Ingrese nombre de usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void EventLoginOK(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                this.clientHandler.Login = txtBoxLogin.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }));
        }

        void EventLoginFailed(object sender, LoginErrorEventArgs e)
        {
            this.BeginInvoke((Action)(delegate
            {
                MessageBox.Show("Mensaje detallado: " + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
            }));
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            clientHandler.CloseConnection();
            this.Dispose();
        }

    }
}
