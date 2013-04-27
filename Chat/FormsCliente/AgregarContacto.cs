using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dominio;

namespace Chat
{
    public partial class AgregarContacto : Form
    {
        public AgregarContacto()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string patron = txtBuscarContacto.Text;
            if (patron != null && !patron.Trim().Equals(""))
            {
                listaContactos.Items.Clear();
                Controlador controlador = new Controlador();
                List<Usuario> contactosEncontrados = controlador.BuscarContactos(patron);
                foreach (Usuario contacto in contactosEncontrados)
                {
                    ListViewItem lvi = new ListViewItem(contacto.Nombre);
                    lvi.Tag = contacto;
                    lvi.SubItems.Add(contacto.Servidor);
                    SetearEstadoContacto(lvi, contacto);
                    listaContactos.Items.Add(lvi);
                }
                FormUtils.AjustarTamanoColumnas(listaContactos);
            }
        }

        private void SetearEstadoContacto(ListViewItem lvi, Usuario contacto)
        {
            lvi.UseItemStyleForSubItems = false;
            Color colorEstado = Color.Gray;
            if (contacto.EstaConectado)
                colorEstado = Color.Green;
            lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, contacto.ObtenerEstadoEnTexto(), Color.White, colorEstado, lvi.Font));
        }

        private void btnAgregarContacto_Click(object sender, EventArgs e)
        {
            //agregar el contacto
            if (FormUtils.HayFilaElegida(listaContactos))
            {
                Usuario contacto = (Usuario) listaContactos.SelectedItems[0].Tag;
                MessageBox.Show("Agregado el contacto " + contacto.Nombre);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
