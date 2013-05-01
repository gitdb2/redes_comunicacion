using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class ReceiveTransfersEventHandler : IReceiveEvent
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool OnReceiveData(Connection connection)
        {
            Data dato = DataProccessor.GetInstance().LoadObject(connection.StreamReader);
            return TransfersCommandHandler.GetInstance().Handle(connection, dato);
        }

        public bool OnFatalError(Connection connection)
        {
           // SingletonClientConnection.GetInstance().RemoveClient(connection.Name);
            return false;
        }

    }
}
