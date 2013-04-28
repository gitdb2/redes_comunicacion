using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.LibOperations.intefaces;

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

        public void Handle(Connection clientConnection, Data dato)
        {
            if (dato.Command == Command.REQ)
            {
                HandleREQ(clientConnection, dato);
            }
            else
            {
                HandleRES(clientConnection, dato);
            }
       }

        private void HandleRES(Connection clientConnection, Data dato)
        {
            switch (dato.OpCode)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    CommandRESContactList(clientConnection, dato);
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 99:
                    break;
                default:
                    break;
            }
        }

        private void CommandRESContactList(Connection clientConnection, Data entryData)
        {
            //construyo un diccionario donde cada entrada es un login y el value indica si esta o no conectado
            Dictionary<string, bool> tmpContactList = UtilContactList.ContactListFromString(entryData.Payload.Message);
            var keys = new List<string>(tmpContactList.Keys);
            foreach (string key in keys)
            {
                tmpContactList[key] = SingletonClientConnection.GetInstance().ClientIsConnected(key);
            }
            //obtengo el login del payload y le envio la trama actualizada con los contactos activos
            string login = UtilContactList.ExtractLogin(entryData.Payload.Message);
            Data outData = new Data() { Command = Command.RES, OpCode = 2, Payload = new Payload(UtilContactList.StringFromContactList(tmpContactList, entryData.Payload.Message)) };
            foreach (var item in outData.GetBytes())
            {
                clientConnection.WriteToStream(item);
            }
        }

        private void HandleREQ(Connection clientConnection, Data dato)
        {
            switch (dato.OpCode)
            {
                case 0:
                    break;
                case 1: //viene el comando login
                    CommandREQLogin(clientConnection, dato);
                    break;
                case 2: //un login pide su lista de contactos
                    CommandREQContactList(clientConnection, dato);
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 99:
                    break;
                default:
                    break;
            }
        }

        private void CommandREQContactList(Connection clientConnection, Data dato)
        {
            string login = dato.Payload.Message;
            string serverName = UsersPersistenceHandler.GetInstance().GetServerName(login);
            if (serverName != null) 
            {
                Connection serverConnection = SingletonServerConnection.GetInstance().GetServer(serverName);
                foreach (var item in dato.GetBytes())
                {
                    Console.WriteLine("Enviando peticion de contactos al servidor");
                    //FIXME faltan servidores, comentado por ahora
                    //serverConnection.WriteToStream(item);
                }
            }
        }

        private void CommandREQLogin(Connection clientConnection, Data dato)
        {
            string login = dato.Payload.Message;
            bool ret = false;
            if (!UsersPersistenceHandler.GetInstance().IsLoginRegistered(login))
            {
                string serverName = FindAGoodServer();
                bool ok = AddUserToServer(login, serverName);
                if (ok)
                {
                    //aumenta el contador de usuarios por servidor, luego que el server agreaga el usuario
                    ret = UsersPersistenceHandler.GetInstance().RegisterLoginServer(login, serverName);
                }
                else
                {
                    ret = false;
                }
            }

            if (ret)//si esta registrado
            {
                SingletonClientConnection scc = SingletonClientConnection.GetInstance();
                Connection oldConnection = scc.GetClient(login);
                if (oldConnection != null)
                {
                    scc.RemoveClient(login);
                    oldConnection.CloseConn();
                }

               scc.AddClient(login, clientConnection);

               Data data = new Data() { Command = Command.RES, OpCode = 1, Payload = new Payload("SUCCESS") };

               foreach (var item in data.GetBytes())
	            {
                    clientConnection.WriteToStream(item);
	            }
               
            }
            else
            {
                Data data = new Data() { Command = Command.RES, OpCode = 2, Payload = new Payload("ERROR REGISTRO") };

                foreach (var item in data.GetBytes())
                {
                    clientConnection.WriteToStream(item);
                }
               
            }
        }

        /// <summary>
        /// Que lo arregle boris
        /// </summary>
        /// <param name="login"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        private bool AddUserToServer(string login, string serverName)
        {
            return true;
        }

        private string FindAGoodServer()
        {
            return "server1";
        }
    }
}
