using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chat
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (FormUtils.TxtBoxTieneDatos(txtBoxUsuario))
            {
                VentanaPrincipalCliente vp = new VentanaPrincipalCliente(txtBoxUsuario.Text);
                vp.ShowDialog();
            }
            else 
            {
                MessageBox.Show("ingrese nombre de usuario");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
