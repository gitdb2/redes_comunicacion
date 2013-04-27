using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class CommandHandler
    {
        private static CommandHandler instance = new CommandHandler();

        private CommandHandler() { }

        public static CommandHandler GetInstance()
        {
            return instance;
        }

        internal void Handle(ClientConnection clientConnection, Data dato)
        {
            throw new NotImplementedException();
        }
    }
}
