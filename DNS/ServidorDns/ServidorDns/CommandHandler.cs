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

        public void Handle(ClientConnection clientConnection, Data dato)
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

        private void HandleRES(ClientConnection clientConnection, Data dato)
        {
            switch (dato.OpCode)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
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

        private void HandleREQ(ClientConnection clientConnection, Data dato)
        {
            switch (dato.OpCode)
            {
                case 0:
                    break;
                case 1: //viene el comando login
                    CommandLogin(clientConnection, dato);
                    break;
                case 2:
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

        private void CommandLogin(ClientConnection clientConnection, Data dato)
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
                ClientConnection oldConnection = scc.GetClient(login);
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

        private bool AddUserToServer(string login, string serverName)
        {
            throw new NotImplementedException();
        }

        private string FindAGoodServer()
        {
            return "server1";
        }
    }
}
