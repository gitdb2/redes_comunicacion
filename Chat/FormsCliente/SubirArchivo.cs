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
    public partial class SubirArchivo : Form
    {
        public SubirArchivo()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnElegirArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Elija un archivo";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxArchivo.Text = ofd.FileName;
            }
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            if (FormUtils.TxtBoxTieneDatos(txtBoxArchivo))
            {
                MessageBox.Show("Hay que subir el archivo: " + txtBoxArchivo.Text);
            }
        }

    }
}
