using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientImplementation
{
    public class ChatMessageEventArgs
    {

        public string ClientFrom { get; set; }
        public string ClientTo { get; set; }
        public string Message { get; set; }

    }
}
