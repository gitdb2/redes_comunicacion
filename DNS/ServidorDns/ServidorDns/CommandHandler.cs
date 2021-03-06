﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.Commons;
using System.Text.RegularExpressions;

namespace uy.edu.ort.obligatorio.ServidorDns
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

        private void HandleRES(Connection connection, Data dato)
        {
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, connection.Name, dato.ToString());
            switch (dato.OpCode)
            {
                case OpCodeConstants.REQ_LOGIN:
                    break;
                case OpCodeConstants.RES_CONTACT_LIST:
                    CommandRESContactList(connection, dato);
                    break;
                case OpCodeConstants.RES_CREATE_USER:
                    CommandRESUserCreated(connection, dato);
                    break;
                case OpCodeConstants.RES_ADD_CONTACT:
                    CommandRESAddContact(connection, dato);
                    break;
                default:
                    break;
            }
        }

        private void CommandRESAddContact(Connection connection, Data dato)
        {
            //la respuesta viene en el formato n|m|loginDestino|contactoAgregado@estado|mensaje_success_o_error
            string[] payLoadSplitted = dato.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE);
            string login = payLoadSplitted[2];
            string contactAddedInfo = payLoadSplitted[3];
            string opStatus = payLoadSplitted[4];
            string contactAdded = contactAddedInfo.Split(ParseConstants.SEPARATOR_AT)[0];

            //si el servidor agrego el contacto, lo agrego a la lista local de contactos
            if (opStatus.Equals(MessageConstants.MESSAGE_SUCCESS))
            {
                //agrego a los usuarios como contactos mutuamente
                SingletonClientConnection.GetInstance().AddContactToClient(login, contactAdded);
                SingletonClientConnection.GetInstance().AddContactToClient(contactAdded, login);

                //si el usuario esta conectado actualizo la trama
                if (SingletonClientConnection.GetInstance().ClientIsConnected(contactAdded))
                {
                    dato.Payload.Message = CreateUserIsConnectedMessage(payLoadSplitted, login, contactAdded);
                }
            }

            //notifico al login para que actualice su lista de contactos
            Connection loginConnection = SingletonClientConnection.GetInstance().GetClient(login);
            SendMessage(loginConnection, Command.RES, dato.OpCode, dato.Payload);

            //si el contacto que fue agregado esta online tambien lo notifico
            Connection contactAddedConnection = SingletonClientConnection.GetInstance().GetClient(contactAdded);
            if (contactAddedConnection != null)
            {
                dato.Payload.Message = CreateUserIsConnectedMessage(payLoadSplitted, contactAdded, login);
                SendMessage(contactAddedConnection, Command.RES, dato.OpCode, dato.Payload);
            }
        }

        private string CreateUserIsConnectedMessage(string[] originalPayload, string loginRequester, string contactAdded)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(originalPayload[0]).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(originalPayload[1]).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(loginRequester).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(contactAdded).Append(ParseConstants.SEPARATOR_AT).Append(MessageConstants.STATUS_ONLINE).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(originalPayload[4]);
            return sb.ToString();
        }

        private void CommandRESUserCreated(Connection connection, Data dato)
        {
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, connection.Name, dato.ToString());
        }

        private void CommandRESContactList(Connection clientConnection, Data entryData)
        {
            //obtengo el login del payload
            string login = UtilContactList.ExtractLogin(entryData.Payload.Message);

            //construyo un diccionario donde cada entrada es un login y el value indica si esta o no conectado
            Dictionary<string, bool> tmpContactList = UtilContactList.ContactListFromString(entryData.Payload.Message);

            //agrego la lista de contactos del usuario al cache local del DNS
            SingletonClientConnection.GetInstance().AddContactToClient(login, tmpContactList.Keys.ToList<string>());

            //marco los contactos que estan conectados
            var keys = new List<string>(tmpContactList.Keys);
            foreach (string key in keys)
            {
                tmpContactList[key] = SingletonClientConnection.GetInstance().ClientIsConnected(key);
            }

            //envio la trama actualizada con los contactos conectados
            Connection loginConnection = SingletonClientConnection.GetInstance().GetClient(login);
            if (loginConnection != null)
            {
                Data outData = new Data() { Command = Command.RES, OpCode = OpCodeConstants.RES_CONTACT_LIST, Payload = new Payload(UtilContactList.StringFromContactList(tmpContactList, entryData.Payload.Message)) };
                foreach (var item in outData.GetBytes())
                {
                    loginConnection.WriteToStream(item);
                }
                //notifico que el usuario se conecto
                NotifyUserChangedStatus(login, MessageConstants.STATUS_ONLINE);
            }
            else
            {
                Console.WriteLine("Tengo que descartar respuesta para {0} que no tiene Conexion", login);
            }
        }

        private void HandleREQ(Connection clientConnection, Data dato)
        {
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, clientConnection.Name, dato.ToString());
            switch (dato.OpCode)
            {
                case OpCodeConstants.REQ_LOGIN: //viene el comando login
                    CommandREQLogin(clientConnection, dato);
                    break;
                case OpCodeConstants.REQ_CONTACT_LIST: //un login pide su lista de contactos
                    CommandREQContactList(clientConnection, dato);
                    break;
                case OpCodeConstants.REQ_SERVER_CONNECT: //un servidor se conecta y registra en el dns
                    CommandREQServerConnect(clientConnection, dato);
                    break;
                case OpCodeConstants.REQ_FIND_CONTACT: //un login hace una busqueda de contactos
                    CommandREQFindContacts(clientConnection, dato);
                    break;
                case OpCodeConstants.REQ_ADD_CONTACT: //un login quiere agregar un contacto nuevo
                    CommandREQADDContact(clientConnection, dato);
                    break;
                case OpCodeConstants.REQ_GET_SERVERS: //Obtiene la lista de servidores online
                    CommandREQGetServers(clientConnection, dato);
                    break;
                case OpCodeConstants.REQ_SEND_CHAT_MSG: //un login le envia un mensaje de chat a otro
                    CommandREQSendChatMessage(clientConnection, dato);
                    break;
                case OpCodeConstants.REQ_SERVER_INFO: //un login pide los datos de su servidor
                    CommandREQServerInfo(clientConnection, dato);
                    break;
                default:
                    break;
            }
        }

        private void CommandREQServerInfo(Connection clientConnection, Data dato)
        {
            //en el payload viene el login que solicita la info
            string serverName = UsersPersistenceHandler.GetInstance().GetServerName(dato.Payload.Message);
            Connection serverConnection = SingletonServerConnection.GetInstance().GetServer(serverName);
            StringBuilder sb = new StringBuilder();
            if (serverConnection == null)
            {
                //el servidor no esta online, respondo con error
                sb.Append(MessageConstants.MESSAGE_ERROR);          
            }
            else
            {
                //serverName|serverIp|serverPort|transfersPort
                sb.Append(serverConnection.Name).Append(ParseConstants.SEPARATOR_PIPE);
                sb.Append(serverConnection.Ip).Append(ParseConstants.SEPARATOR_PIPE);
                sb.Append(serverConnection.Port).Append(ParseConstants.SEPARATOR_PIPE);
                sb.Append(serverConnection.TransferPort);
            }
            SendMessage(clientConnection, Command.RES, OpCodeConstants.RES_SERVER_INFO, new Payload(sb.ToString()));
        }

        private void CommandREQGetServers(Connection connection, Data dato)
        {
            //string login = dato.Payload.Message.Split('|')[0];
            List<ServerInfo> servers = SingletonServerConnection.GetInstance().GetServersWithUsers();
            StringBuilder message = new StringBuilder();
            if (servers.Count == 0)
            {
                message.Append(MessageConstants.MESSAGE_ERROR).Append(ParseConstants.SEPARATOR_PIPE).Append("No hay Servidores en linea");
            }
            else
            {
                bool first = true;
                foreach (var item in servers)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        message.Append(ParseConstants.SEPARATOR_PIPE);
                    }
                    message.Append(item.ToNetworkString());
                }
            }
            Data outData = new Data()
            {
                Command = Command.RES,
                OpCode = OpCodeConstants.RES_GET_SERVERS,
                Payload = new MultiplePayload() { Message = message.ToString(), Destination = dato.Payload.Message }
            };
            foreach (var item in outData.GetBytes())
            {
                connection.WriteToStream(item);
            }
        }

        private void CommandREQSendChatMessage(Connection clientConnection, Data dato)
        {
            string[] payloadSplitted = dato.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE);
            string clientFrom = payloadSplitted[0];
            string clientTo = payloadSplitted[1];
            
            Connection clientToConnection = SingletonClientConnection.GetInstance().GetClient(clientTo);
            Connection clientFromConnection = SingletonClientConnection.GetInstance().GetClient(clientFrom);

            if (clientToConnection != null)
            {
                //envio el mensaje al destinatario
                SendMessage(clientToConnection, Command.REQ, dato.OpCode, dato.Payload);
                //aviso al remitente que envie su mensaje
                string messageSuccess = clientTo + ParseConstants.SEPARATOR_PIPE + MessageConstants.MESSAGE_SUCCESS;
                SendMessage(clientFromConnection, Command.RES, OpCodeConstants.RES_SEND_CHAT_MSG, new Payload() { Message = messageSuccess });
            }
            else 
            {
                //aviso que se perdio la conexion con el destinatario
                string messageError = clientTo + ParseConstants.SEPARATOR_PIPE + MessageConstants.MESSAGE_ERROR;
                SendMessage(clientFromConnection, Command.RES, OpCodeConstants.RES_SEND_CHAT_MSG, new Payload() { Message = messageError });
            }
            

        }

        private void CommandREQADDContact(Connection clientConnection, Data dato)
        {
            //en la trama viene: login que hace el request|contacto a agregar
            string login = dato.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE)[0];
            string contacto = dato.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE)[1];
            string serverName = UsersPersistenceHandler.GetInstance().GetServerName(login);
            if (serverName != null)
            {
                Connection serverConnection = SingletonServerConnection.GetInstance().GetServer(serverName);
                foreach (var item in dato.GetBytes())
                {
                    Console.WriteLine("Enviando peticion de agregar contacto al servidor");
                    serverConnection.WriteToStream(item);
                }
            }
            //agrega login a la lista de contacots de contacto
            serverName = UsersPersistenceHandler.GetInstance().GetServerName(contacto);
            dato.Payload.Message = contacto + ParseConstants.SEPARATOR_PIPE + login;
            if (serverName != null)
            {
                Connection serverConnection = SingletonServerConnection.GetInstance().GetServer(serverName);
                foreach (var item in dato.GetBytes())
                {
                    Console.WriteLine("Enviando peticion de agregar contacto al servidor");
                    serverConnection.WriteToStream(item);
                }
            }

        }

        private void CommandREQFindContacts(Connection clientConnection, Data dato)
        {
            string[] payloadSplitted = dato.Payload.Message.Split('|');
            string login = payloadSplitted[0];
            string pattern = payloadSplitted[1];

            //me fijo que contactos de los registrados matchea con el patron recibido
            List<string> contactsFound = SingletonClientConnection.GetInstance().FindRegisteredClientByPattern(pattern, login);

            //construyo el diccionario resultado, marcando los contactos que esten conectados
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (string key in contactsFound)
            {
                result.Add(key, SingletonClientConnection.GetInstance().ClientIsConnected(key));
            }

            //armo la lista resultado y devuelvo
            if (clientConnection != null)
            {
                Data outData = new Data() { 
                    Command = Command.RES, 
                    OpCode = OpCodeConstants.RES_FIND_CONTACT, 
                    Payload = new MultiplePayload() {Message = UtilContactList.StringFromContactList(result), Destination = login}
                };
                foreach (var item in outData.GetBytes())
                {
                    clientConnection.WriteToStream(item);
                }
            }
            else
            {
                Console.WriteLine("Tengo que descartar respuesta para {0} que no tiene Conexion", login);
            }

        }

        private void CommandREQServerConnect(Connection newConnection, Data dato)
        {
            string[] tmp = dato.Payload.Message.Split(':');//viene name: ip : port : user count
            string serverName   = tmp[0];
            string serverIp     = tmp[1];
            int serverPort      = int.Parse(tmp[2]);
            int serverTxPort    = int.Parse(tmp[3]);
            int userCount       = int.Parse(tmp[4]);

            newConnection.IsServer  = true;
            newConnection.Ip        = serverIp;
            newConnection.Name      = serverName;
            newConnection.Port      = serverPort;
            newConnection.TransferPort = serverTxPort;
            newConnection.UserCount = userCount;
          

            SingletonServerConnection ssc = SingletonServerConnection.GetInstance();
            Connection oldConnection = ssc.GetServer(serverName);
            if (oldConnection != null)
            {
                ssc.RemoveServer(serverName);
                oldConnection.CloseConn();
            }

            ssc.AddServer(serverName, newConnection);
            log.InfoFormat("Agregado nuevo servidor: {0}:{2} , name:{1}, userCount:{3}", 
                newConnection.Ip,  newConnection.Name, newConnection.Port, newConnection.UserCount);
            SendMessage(newConnection, Command.RES, OpCodeConstants.REQ_SERVER_CONNECT, new Payload("SUCCESS"));
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
                    Console.WriteLine("Enviando peticion de lista de contactos al servidor");
                    serverConnection.WriteToStream(item);
                }
            }
        }

        private void CommandREQLogin(Connection clientConnection, Data dato)
        {
            string login = dato.Payload.Message;
            bool ret = true;

            if (!UsersPersistenceHandler.GetInstance().IsLoginRegistered(login))
            {
                try
                {
                    string serverName = FindAGoodServer();
                    //agrega el usuario y el server al registro de usuario-server
                    bool ok = UsersPersistenceHandler.GetInstance().RegisterLoginServer(login, serverName);
                    if (ok)
                    {
                        //aumenta el contador de usuarios por servidor, luego que el server agreaga el usuario
                        //ret = UsersPersistenceHandler.GetInstance().RegisterLoginServer(login, serverName);
                        AddUserToServer(login, serverName);
                    }
                    else
                    {
                        ret = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error : {0}", e.Message);
                    ret = false;
                }
                
            }

            if (ret)//si esta registrado
            {
                SingletonClientConnection scc = SingletonClientConnection.GetInstance();
                Connection oldConnection = scc.GetClient(login);
                if (oldConnection == null)
                {

                    if (SingletonServerConnection.GetInstance().GetServer(UsersPersistenceHandler.GetInstance().GetServerName(login)) != null)
                    {
                        scc.AddClient(login, clientConnection);
                        SendMessage(clientConnection, Command.RES, OpCodeConstants.REQ_LOGIN, new Payload("SUCCESS"));
                    }
                    else
                    {
                        SendMessage(clientConnection, Command.RES, OpCodeConstants.REQ_LOGIN, new Payload("ERROR Servidor Offline"));
                        clientConnection.CloseConn();
                        ret = false;
                    }
                }
                else
                {
                    SendMessage(clientConnection, Command.RES, OpCodeConstants.REQ_LOGIN, new Payload("ERROR Login en uso"));
                    clientConnection.CloseConn();
                    ret = false;
                }
            }
            else
            {
                SendMessage(clientConnection, Command.RES, OpCodeConstants.REQ_LOGIN, new Payload("ERROR No se pudo registrar el login nuevo (server offline??)"));
                clientConnection.CloseConn();
            }
        }

        public void Logout(Connection clientConnection)
        {
            if (clientConnection.IsServer)
            {
                log.InfoFormat("Desconectando el servidor {0}", clientConnection.Name);
                Console.WriteLine("Desconectando el servidor {0}", clientConnection.Name);
                SingletonServerConnection.GetInstance().RemoveServer(clientConnection.Name);
            }
            else
            {
                log.InfoFormat("Desconectando el cliente {0}", clientConnection.Name);
                Console.WriteLine("Desconectando el cliente {0}", clientConnection.Name);
                NotifyUserChangedStatus(clientConnection.Name, MessageConstants.STATUS_OFFLINE);
                SingletonClientConnection.GetInstance().RemoveClient(clientConnection.Name);
            }
        }

        private void NotifyUserChangedStatus(string user, string newStatus)
        {
            StringBuilder sb;
            Connection contactConnection;
            foreach (string contact in SingletonClientConnection.GetInstance().GetContactsOfLogin(user))
            {
                if (SingletonClientConnection.GetInstance().ClientIsConnected(contact))
                {
                    sb = new StringBuilder();
                    sb.Append("01").Append(ParseConstants.SEPARATOR_PIPE);
                    sb.Append("01").Append(ParseConstants.SEPARATOR_PIPE);
                    sb.Append(contact).Append(ParseConstants.SEPARATOR_PIPE);
                    sb.Append(user).Append("@").Append(newStatus).Append(ParseConstants.SEPARATOR_PIPE);
                    sb.Append(MessageConstants.MESSAGE_SUCCESS);

                    contactConnection = SingletonClientConnection.GetInstance().GetClient(contact);
                    SendMessage(contactConnection, Command.RES, OpCodeConstants.RES_ADD_CONTACT, new Payload() { Message = sb.ToString() });
                }
            }
        }

        private bool AddUserToServer(string login, string serverName)
        {
            
            try
            {
                int count = SingletonServerConnection.GetInstance().IncUserCount(serverName);
                if (count > 0)
                {
                    //si se agrego en el dns, entonces pido al server que lo agregue, pero no espero confirmacion., las operaciones de un usuario sobe el server van a chequear que exista el usuario, y si no existe, lo va a crear.
                    SendMessage(SingletonServerConnection.GetInstance().GetServer(serverName), Command.REQ, 
                                                        OpCodeConstants.REQ_CREATE_USER, new Payload(login));
                    Console.WriteLine("El server {0} queda con {1} usuarios registrados", serverName, count);
                }
                else
                {
                    throw new Exception("No se encontro el sevidor en la lista de Online");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al agregar usuario al servidor: {0}", e.Message);
                
            }
            
            return true;
        }

        private string FindAGoodServer()
        {
            return SingletonServerConnection.GetInstance().FindBestServerForNewUser();
        }

        private void SendMessage(Connection connection, Command command, int opCode, Payload payload)
        {
            Data data = new Data() { Command = command, OpCode = opCode, Payload = payload };
            foreach (var item in data.GetBytes())
            {
                connection.WriteToStream(item);
            }
        }

    }
}
