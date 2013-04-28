using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class SingletonClientConnection
    {
        Dictionary<string, Connection> clientsMap = new Dictionary<string, Connection>();

        Dictionary<string, List<string>> clientsContactsMap = new Dictionary<string, List<string>>();

        private static SingletonClientConnection instance = new SingletonClientConnection();

        private SingletonClientConnection() { }

        public static SingletonClientConnection GetInstance(){
            return instance;
        }

        public void AddClient(string login, Connection connection)
        {
            lock (this)
            {
                connection.Name = login;
                clientsMap.Add(login, connection);
            }
        }

        public bool RemoveClient(string login)
        {
            lock (this)
            {
                if (login != null)
                {
                    return clientsMap.Remove(login);
                }
                else 
                {
                    return false;
                }
            }
        }

        public Connection GetClient(string login)
        {
            Connection ret = null;
            lock (this)
            {
                try
                {
                    clientsMap.TryGetValue(login, out ret);//[login];
                }
                catch
                {
                    ret = null;
                }
            }
            return ret;
        }

        public bool ClientIsConnected(string login)
        {
            return clientsMap.ContainsKey(login);
        }
    }
}
