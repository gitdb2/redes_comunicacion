using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientImplementation
{
    public class LoginErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }

    }
}
