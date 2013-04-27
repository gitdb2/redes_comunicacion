using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio
{
    public class Usuario
    {

        public string Nombre { get; set; }
        public bool EstaConectado { get; set; }
        public string Servidor { get; set; }
        public string DireccionIP { get; set; }

        public Usuario() { }

        public Usuario(string nombre, bool conectado, string servidor, string ip)
        {
            this.Nombre = nombre;
            this.EstaConectado = conectado;
            this.Servidor = servidor;
            this.DireccionIP = ip;
        }

        public string ObtenerEstadoEnTexto()
        {
            if (EstaConectado)
                return "Conectado";
            else
                return "Desconectado";
        }

        public override string ToString()
        {
            return this.Nombre;
        }
    }
}
