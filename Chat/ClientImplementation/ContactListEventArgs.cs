using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientImplementation
{
    public class ContactListEventArgs : EventArgs
    {

        public Dictionary<string, bool> ContactList { get; set; }

        //la lista de contactos puede venir separada en varios RES
        //esta variable esta en true cuando es el ultimo
        public bool IsLastPart { get; set; }

    }
}
