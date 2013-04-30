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
using System.Threading;

namespace Chat
{
    public partial class Login : Form
    {

        private bool connected = false;

        public Login()
        {
            InitializeComponent();
            ClientHandler.GetInstance().LoginOK += new EventHandler(EventLoginOK);
            ClientHandler.GetInstance().LoginFailed += new ClientHandler.ChatErrorEventHandler(EventLoginFailed);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            
            if (FormUtils.TxtBoxTieneDatos(txtBoxLogin))
            {
                if (!connected)
                {

                    try
                    {
                        //intento establecer la conexion con el dns
                        this.btnOK.Enabled = false;
                        ClientHandler.GetInstance().Connect(txtBoxLogin.Text);
                        connected = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No se pudo conectar al servidor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.btnOK.Enabled = true;
                        this.connected = false;
                    }

                }
                if (connected) 
                {
                    try
                    {
                        //envio el request de login
                        ClientHandler.GetInstance().LoginClient(txtBoxLogin.Text);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Error: " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.btnOK.Enabled = true;
                    }

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
                ClientHandler.GetInstance().Login = txtBoxLogin.Text;
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
            ClientHandler.GetInstance().CloseConnection();
            this.Dispose();
        }

    }
}
