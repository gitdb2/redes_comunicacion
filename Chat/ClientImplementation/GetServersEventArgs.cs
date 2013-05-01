using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;

namespace ClientImplementation
{
    public class GetServersEventArgs : EventArgs
    {

        public MultiplePayloadFrameDecoded Response { get; set; }


    }
}
