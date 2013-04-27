using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chat;
using System.Net;

namespace FormsDNS
{

    public partial class VentanaPrincipalDNS : Form
    {
        public VentanaPrincipalDNS()
        {
            InitializeComponent();
            this.txtBoxDireccionIP.Text = FormUtils.ObtenerIPLocal();
        }

        private void btnIniciarServidor_Click(object sender, EventArgs e)
        {
            IPAddress direccionIP = IPAddress.Parse(txtBoxDireccionIP.Text);
            int puerto = Int16.Parse(txtBoxPuerto.Text);
            txtBoxMensajes.AppendText("Escuchando conexiones ...\r\n");
        }

    }
}
