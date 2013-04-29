using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientImplementation
{
    public class ContactListEventArgs : EventArgs
    {

        public Dictionary<string, bool> ContactList { get; set; }

    }
}
