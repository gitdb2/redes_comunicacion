using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class SingletonClientConnection
    {
        Dictionary<string, ClientConnection> clientsMap = new Dictionary<string, ClientConnection>();

        private static SingletonClientConnection instance = new SingletonClientConnection();

        private SingletonClientConnection() { }

        public static SingletonClientConnection GetInstance(){
            return instance;
        }

        public void AddClient(string login, ClientConnection connection)
        {
            lock (this)
            {
                clientsMap.Add(login, connection); 
            }
        }

        public bool RemoveClient(string login)
        {
            lock (this)
            {
                return clientsMap.Remove(login);
            }
        }

        public ClientConnection GetClient(string login)
        {
            ClientConnection ret = null;
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
    }
}
