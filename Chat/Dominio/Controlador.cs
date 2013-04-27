using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio
{
    public class Controlador
    {

        public Controlador() { }

        public List<Usuario> ObtenerContactos()
        {
            List<Usuario> resultado = new List<Usuario>();
            resultado.Add(new Usuario ("Raquel", true, "Servidor1", "190.35.56.2"));
            resultado.Add(new Usuario("Marta", true, "Servidor2", "190.35.58.5"));
            resultado.Add(new Usuario("Rafael", false, "Servidor2", "190.35.33.17"));
            resultado.Add(new Usuario("Choriso", true, "Servidor1", "190.35.56.89"));
            resultado.Add(new Usuario("Morsiya", false, "Servidor3", "190.35.33.124"));
            return resultado;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> resultado = new List<Usuario>();
            resultado.Add(new Usuario("Raquel", false, "Servidor1", "190.35.56.2"));
            resultado.Add(new Usuario("Ronaldinho", false, "Servidor1", "190.35.77.154"));
            return resultado;
        }

        public List<Usuario> BuscarContactos(string patronBusqueda)
        {
            List<Usuario> resultado = new List<Usuario>();
            resultado.Add(new Usuario("Martita", true, "Servidor1", "186.52.36.5"));
            resultado.Add(new Usuario("Alejandro", false, "Servidor2", "186.54.43.4"));
            return resultado;
        }

        public List<Archivo> BuscarArchivos(string patronBusqueda)
        {
            List<Archivo> resultado = new List<Archivo>();
            resultado.Add(new Archivo("archivo1.txt", "Servidor1"));
            resultado.Add(new Archivo("archivo2.txt", "Servidor2"));
            resultado.Add(new Archivo("archivo3.txt", "Servidor3"));
            return resultado;
        }
    }
}
