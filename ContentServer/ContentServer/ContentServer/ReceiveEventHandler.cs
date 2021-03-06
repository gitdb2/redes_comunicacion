﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class ReceiveEventHandler : IReceiveEvent
    {

        public bool OnReceiveData(Connection connection)
        {
            Data dato = DataProccessor.GetInstance().LoadObject(connection.StreamReader);
            CommandHandler.GetInstance().Handle(connection, dato);
            return true;
        }

        public bool OnFatalError(Connection connection)
        {
           // SingletonClientConnection.GetInstance().RemoveClient(connection.Name);
            return false;
        }

    }
}
