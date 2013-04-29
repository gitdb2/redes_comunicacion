﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.Commons;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class CommandHandler
    {
        private static CommandHandler instance = new CommandHandler();

        private CommandHandler() { }

        public static CommandHandler GetInstance()
        {
            return instance;
        }

        public void Handle(Connection Connection, Data dato)
        {

            if (dato.Command == Command.REQ)
            {
                HandleREQ(Connection, dato);
            }
            else
            {
                HandleRES(Connection, dato);
            }
        }

        private void HandleRES(Connection Connection, Data dato)
        {
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, "USAR CONNECTION DE COMMONS", dato.ToString());
          
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

        private void HandleREQ(Connection Connection, Data dato)
        {
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, "USAR CONNECTION DE COMMONS", dato.ToString());
                   
            switch (dato.OpCode)
            {
                case OpCodeConstants.REQ_CONTACT_LIST: //viene el obtener lista de contactos
                    CommandGetContactList(Connection, dato);
                    break;
                case OpCodeConstants.REQ_CREATE_USER:
                    CommandCreateNewUser(Connection, dato);
                    break;
                case OpCodeConstants.REQ_ADD_CONTACT:
                    CommandAddContact(Connection, dato);
                    break;
                default:
                    break;
            }
        }

        private void CommandAddContact(Connection Connection, Data dato)
        {
            string[] payloadSplitted = dato.Payload.Message.Split('|');
            string login = payloadSplitted[0];
            string contactToAdd = payloadSplitted[1];
            
            bool ok = UsersContactsPersistenceHandler.GetInstance().AddContact(login, contactToAdd);

            string statusMessage = ok ? "SUCCESS" : "ERROR";
            string message = contactToAdd + STATUS_DELIMITER + "0" + CONTACT_DELIMITER + statusMessage;

            Data retDato = new Data()
            {
                Command = Command.RES,
                OpCode = OpCodeConstants.RES_ADD_CONTACT,
                Payload = new MultiplePayload() { Message = message, Destination = login }
            };
            foreach (var item in retDato.GetBytes())
            {
                Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                Connection.WriteToStream(item);
            }
        }

        private void CommandCreateNewUser(Connection Connection, Data dato)
        {
            string login = dato.Payload.Message;
            bool ok = UsersContactsPersistenceHandler.GetInstance().RegisterNewUser(login);
            Data retDato = new Data() { Command = Command.RES, 
                                        OpCode = OpCodeConstants.RES_CREATE_USER,
                                        Payload = new MultiplePayload() { Message = (ok? "SUCCESS" : "ERROR"), Destination = login }
            };
            foreach (var item in retDato.GetBytes())
            {
                Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                Connection.WriteToStream(item);
            }
        }

        const string CONTACT_DELIMITER = "|";
        const string STATUS_DELIMITER = "@";
        private void CommandGetContactList(Connection Connection, Data dato)
        {
            string login = dato.Payload.Message;
            List<string> contacts = UsersContactsPersistenceHandler.GetInstance().GetContacts(login);

            StringBuilder message = new StringBuilder();
            bool first = true;
            foreach (var item in contacts)
            {
               if (first)
                {
                    first = false;
                }
                else
                {
                    message.Append(CONTACT_DELIMITER);
                }
                message.Append(item).Append(STATUS_DELIMITER).Append("0");
            }
            Data retDato = new Data() { Command = Command.RES, OpCode = OpCodeConstants.RES_CONTACT_LIST, Payload = new MultiplePayload() { Message = message.ToString(), Destination = login } };
            foreach (var item in retDato.GetBytes())
	        {
                Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                Connection.WriteToStream(item);
	        }
            Console.WriteLine("termina :CommandGetContactList");
        }
       
    }
}
