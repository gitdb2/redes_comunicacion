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

namespace Chat
{
    public partial class Login : Form
    {
        private const int puertoDNS = 2000;
        private const string ipDNS = "localhost";

        public Login()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (FormUtils.TxtBoxTieneDatos(txtBoxLogin))
            {
                ComunicationHandler commHandler = new ComunicationHandler() { Server = ipDNS, Port = puertoDNS };
                try 
                {
                    //intento establecer la conexion con el dns
                    commHandler.SetupConnection();
                    
                    //request de login
                    commHandler.SendData(Command.REQ, 1, new Payload(txtBoxLogin.Text));
                    
                    //espero respuesta SUCCESS
                    Data response = commHandler.ReceiveData();

                    if (LoginSuccessful(data))
                    { 
                    
                    }

                    VentanaPrincipalCliente vp = new VentanaPrincipalCliente() {NombreUsuario = txtBoxLogin.Text, commHandler = commHandler };
                    vp.ShowDialog();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Ocurrio un error al establecer la conexion al DNS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                MessageBox.Show("Ingrese nombre de usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
