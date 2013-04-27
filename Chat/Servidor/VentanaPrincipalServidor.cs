using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dominio;
using Chat;

namespace Servidor
{
    public partial class VentanaPrincipalServidor : Form
    {
        public VentanaPrincipalServidor()
        {
            InitializeComponent();
            this.txtBoxDireccionIP.Text = FormUtils.ObtenerIPLocal();
        }

        private void btnIniciarServidor_Click(object sender, EventArgs e)
        {
            PopularUsuarios();
        }

        private void PopularUsuarios()
        {
            Controlador controlador = new Controlador();
            List<Usuario> usuarios = controlador.ObtenerUsuarios();
            foreach (Usuario usuario in usuarios)
            {
                ListViewItem lvi = new ListViewItem(usuario.Nombre);
                lvi.Tag = usuario;
                lvi.SubItems.Add(usuario.DireccionIP);
                SetearEstadoContacto(lvi, usuario);
                listaClientes.Items.Add(lvi);
            }
            FormUtils.AjustarTamanoColumnas(listaClientes);
        }

        private void SetearEstadoContacto(ListViewItem lvi, Usuario usuario)
        {
            lvi.UseItemStyleForSubItems = false;
            Color colorEstado = Color.Gray;
            if (usuario.EstaConectado)
                colorEstado = Color.Green;
            lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, usuario.ObtenerEstadoEnTexto(), Color.White, colorEstado, lvi.Font));
        }

    }
}
