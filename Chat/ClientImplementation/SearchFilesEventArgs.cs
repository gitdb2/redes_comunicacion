using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.ContentServer;

namespace ClientImplementation
{
    public class SearchFilesEventArgs : EventArgs
    {

        public List<FileObject> FilesFound { get; set; }

        //la lista de contactos puede venir separada en varios RES
        //esta variable esta en true cuando es el ultimo
        public bool IsLastPart { get; set; }

    }
}
