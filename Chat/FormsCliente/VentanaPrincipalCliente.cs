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

namespace Chat
{
    public partial class VentanaPrincipalCliente : Form
    {
        public string NombreUsuario { get; set; }
        public ComunicationHandler commHandler { get; set; }

        public VentanaPrincipalCliente()
        {
            InitializeComponent();
            PopularListaContactos();
        }

        private void PopularListaContactos()
        {
            //request de la lista de contactos
            commHandler.SendData(Command.REQ, 2, new Payload(NombreUsuario));
            string rawContactList = commHandler.ReceiveData();

            foreach (Usuario contacto in contactos)
            {
                ListViewItem lvi = new ListViewItem(contacto.Nombre);
                lvi.Tag = contacto;
                lvi.SubItems.Add(contacto.Servidor);
                SetearEstadoContacto(lvi, contacto);
                listaContactos.Items.Add(lvi);
            }
            FormUtils.AjustarTamanoColumnas(listaContactos);
        }

        private void SetearEstadoContacto(ListViewItem lvi, Usuario contacto)
        {
            lvi.UseItemStyleForSubItems = false;
            Color colorEstado = Color.Gray;
            if (contacto.EstaConectado)
                colorEstado = Color.Green;
            lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, contacto.ObtenerEstadoEnTexto(), Color.White, colorEstado, lvi.Font));
        }

        private void menuArchivoOpcionSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void listaContactos_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem seleccion = listaContactos.SelectedItems[0];
            Usuario usuario = (Usuario) seleccion.Tag;
            if (usuario.EstaConectado)
            {
                VentanaDeChat vt = new VentanaDeChat(usuario, this.NombreUsuario);
                vt.Show();
            }
            else 
            {
                MessageBox.Show("No es posible chatear con " + usuario.Nombre + ", esta desconectado." ,
                    "Contacto Desconectado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuAccionesOpcionAgregarContacto_Click(object sender, EventArgs e)
        {
            AgregarContacto ac = new AgregarContacto();
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
    }
}
