using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.LibOperations.intefaces;
using uy.edu.ort.obligatorio.Commons;

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

                default:
                   
                    break;
            }
        }

        private void CommandRESUserCreated(Connection connection, Data dato)
        {
            Console.WriteLine("[{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, connection.Name, dato.ToString());
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
            Connection loginConnection = SingletonClientConnection.GetInstance().GetClient(login);
            if (loginConnection != null)
            {
                Data outData = new Data() { Command = Command.RES, OpCode = OpCodeConstants.RES_CONTACT_LIST, Payload = new Payload(UtilContactList.StringFromContactList(tmpContactList, entryData.Payload.Message)) };
                foreach (var item in outData.GetBytes())
                {
                    loginConnection.WriteToStream(item);
                }
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
               
                default:
                    
                    break;
            }
        }

        private void CommandREQServerConnect(Connection newConnection, Data dato)
        {
            string[] tmp = dato.Payload.Message.Split(':');//viene name: ip : port : user count
            string serverName   = tmp[0];
            string serverIp     = tmp[1];
            int serverPort      = int.Parse(tmp[2]);
            int userCount       = int.Parse(tmp[3]);

            newConnection.Ip        = serverIp;
            newConnection.Name      = serverName;
            newConnection.Port      = serverPort;
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

        private void SendMessage(Connection connection, Command command, int opCode, Payload payload)
        {
            Data data = new Data() { Command = command, OpCode = opCode, Payload = payload };
            foreach (var item in data.GetBytes())
            {
                connection.WriteToStream(item);
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
                if (oldConnection != null)
                {
                    scc.RemoveClient(login);
                    oldConnection.CloseConn();
                }

                scc.AddClient(login, clientConnection);

                SendMessage(clientConnection, Command.RES, OpCodeConstants.REQ_LOGIN, new Payload("SUCCESS"));
            }
            else
            {
                SendMessage(clientConnection, Command.RES, OpCodeConstants.REQ_LOGIN, new Payload("ERROR REGISTRO"));
                clientConnection.CloseConn();
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
    }
}
