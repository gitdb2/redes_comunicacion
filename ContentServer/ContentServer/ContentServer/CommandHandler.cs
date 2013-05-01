using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.Commons;
using System.IO;


namespace uy.edu.ort.obligatorio.ContentServer
{
    public class CommandHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, Connection.Name, dato.ToString());
            log.DebugFormat(" connection owner: {0} ;  The data: {1} ", Connection.Name, dato.ToString());
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

        private void HandleREQ(Connection connection, Data dato)
        {
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, connection.Name, dato.ToString());
            log.DebugFormat(" connection owner: {0} ;  The data: {1} ", connection.Name, dato.ToString());
                   
            switch (dato.OpCode)
            {
                case OpCodeConstants.REQ_CONTACT_LIST: //viene el obtener lista de contactos
                    CommandGetContactList(connection, dato);
                    log.Debug("procesé REQ LISTA DE CONTACTOS");
                    break;
                case OpCodeConstants.REQ_CREATE_USER:
                    CommandCreateNewUser(connection, dato);
                    log.Debug("procesé REQ Crear USUARIO");
                    break;
                case OpCodeConstants.REQ_SEARCH_FILES:
                    CommandSearchFiles(connection, dato);
                    log.Debug("procesé REQ Buscar Archivos");
                    break;
                case OpCodeConstants.REQ_ADD_CONTACT:
                    CommandAddContact(connection, dato);
                    log.Debug("procesé REQ Add CONTACTOS");
                    break;

                case OpCodeConstants.REQ_GET_TRANSFER_INFO: //se ejecuta previo a la bajada de un archivo puntual, en el caso que el cliente no tenga el tamanio del archivo para saber cuando cortar.
                    CommandGetFile(connection, dato);
                    log.Debug("procesé REQ GetFile");
                    break;
                default:
                    break;
            }
        }

        private void CommandGetFile(Connection connection, Data dato)
        {
            string[] payload = dato.Payload.Message.Split(new string[] { PIPE_SEPARATOR }, StringSplitOptions.None);
            string login = payload[0];
            string owner = payload[1];
            string hashfile = payload[2];

            FileInfo fi =  FileOperationsSingleton.GetInstance().GetFile(hashfile, login);
            Data retDato;
            if (fi == null)
            {
                retDato = new Data()
                {
                    Command = Command.RES,
                    OpCode = OpCodeConstants.RES_SEARCH_FILES,
                    Payload = new Payload() { Message = "ERROR [wrong file or not found]" }
                };
            }
            else
            {

                string port = Settings.GetInstance().GetProperty("server.transfers.port", "20001");
                string ip = Settings.GetInstance().GetProperty("server.ip", "127.0.0.1");

                long size = fi.Length;

                string message = login + PIPE_SEPARATOR + owner + PIPE_SEPARATOR + hashfile + PIPE_SEPARATOR + ip + PIPE_SEPARATOR + port+ PIPE_SEPARATOR +size;

                retDato = new Data()
                {
                    Command = Command.RES,
                    OpCode = OpCodeConstants.RES_SEARCH_FILES,
                    Payload = new Payload() { Message = message }
                };
            }

            foreach (var item in retDato.GetBytes())
            {
                Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                connection.WriteToStream(item);
            }

        }







        public const string PIPE_SEPARATOR = "|";
        const string ARROBA_SEPARATOR = "@";
        private void CommandSearchFiles(Connection connection, Data dato)
        {//REQ07

           // login + "|" + hashQuery + "|" + pattern;


            string[] payload = dato.Payload.Message.Split(new string[] { PIPE_SEPARATOR }, StringSplitOptions.None);
            string login        = payload[0];
            string queryHash    = payload[1];
            string pattern      = payload[2];
            List<FileObject> results = FileOperationsSingleton.GetInstance().SearchFilesMatching(pattern);

            StringBuilder message = new StringBuilder();
            string destination = login + ARROBA_SEPARATOR + queryHash + ARROBA_SEPARATOR + Settings.GetInstance().GetProperty("server.name", "DEFAULT_SERVER");

            bool first = true;
            foreach (var item in results)
            {
               if (first)
                {
                    first = false;
                }
                else
                {
                    message.Append(PIPE_SEPARATOR);
                }
                message.Append(item.ToNetworkString());
            }

            string tmp = message.ToString();

            Data retDato = new Data()
            {
                Command = Command.RES,
                OpCode = OpCodeConstants.RES_SEARCH_FILES,
                Payload = new MultiplePayload() { Message = tmp, Destination = destination }
            };

            foreach (var item in retDato.GetBytes())
            {
                Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                connection.WriteToStream(item);
            }
        }

        private void CommandAddContact(Connection Connection, Data dato)
        {
            string[] payloadSplitted = dato.Payload.Message.Split('|');
            string login = payloadSplitted[0];
            string contactToAdd = payloadSplitted[1];

            bool ok = UsersContactsPersistenceHandler.GetInstance().AddContact(login, contactToAdd)
                      && UsersContactsPersistenceHandler.GetInstance().AddContact(contactToAdd, login);

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
