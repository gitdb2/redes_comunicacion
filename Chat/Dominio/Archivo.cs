using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio
{
    public class Archivo
    {
        public string Nombre { get; set; }
        public string MD5 { get; set; }
        public string Servidor { get; set; }
        public string Usuario { get; set; }
        public string RutaAbsoluta { get; set; }

        public Archivo(string nombre, string servidor)
        {
            this.Nombre = nombre;
            this.Servidor = servidor;
        }

    }
}
