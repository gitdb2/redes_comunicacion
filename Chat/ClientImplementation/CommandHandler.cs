﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;
using Comunicacion;

namespace ClientImplementation
{
    public class CommandHandler
    {
        private const string MESSAGE_SUCCESS = "SUCCESS";
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
                    CommandRESLogin(clientConnection, dato);
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

        private void CommandRESLogin(Connection clientConnection, Data dato)
        {
            if (dato.Payload.Message.Equals(MESSAGE_SUCCESS))
            {
                ClientHandler.GetInstance().OnLoginOK();
            } 
            else 
            {
                ClientHandler.GetInstance().OnLoginFailed(new LoginErrorEventArgs(dato.Payload.Message));
            }
        }

        private void CommandRESContactList(Connection clientConnection, Data dato)
        {
            throw new NotImplementedException();
        }

        private void HandleREQ(Connection clientConnection,Data dato)
        {
             switch (dato.OpCode)
            {
                case 0:
                    break;
                case 1: //el cliente se quiere loguear
                    CommandREQLogin(clientConnection, dato);
                    break;
                case 2: //el cliente pide su lista de contactos
                    CommandREQContactList(clientConnection, dato);
                    break;
                case 3: //mensaje de chat
                    CommandREQChatMessage(clientConnection, dato);
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    break;
            }
        }

        private void CommandREQChatMessage(Connection clientConnection, Data dato)
        {
            throw new NotImplementedException();
        }

        private void CommandREQContactList(Connection clientConnection, Data dato)
        {
            throw new NotImplementedException();
        }

        private void CommandREQLogin(Connection clientConnection, Data dato)
        {

        }

    }
}
