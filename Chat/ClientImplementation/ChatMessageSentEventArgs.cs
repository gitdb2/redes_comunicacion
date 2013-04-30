using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientImplementation
{
    public class ChatMessageSentEventArgs : EventArgs
    {

        public string MessageTo { get; set; }
        public string MessageStatus { get; set; }

    }
}
