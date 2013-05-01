using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;

namespace ClientImplementation
{
    public class SearchFilesEventArgs : EventArgs
    {

        public MultiplePayloadFrameDecoded Response { get; set; }
        public Connection Connection { get; set; }
    }
}
