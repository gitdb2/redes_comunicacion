using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class SingletonServerConnection
    {
        Dictionary<string, Connection> serversMap = new Dictionary<string, Connection>();

        private static SingletonServerConnection instance = new SingletonServerConnection();

        private SingletonServerConnection() { }

        public static SingletonServerConnection GetInstance()
        {
            return instance;
        }

        public void AddServer(string serverName, Connection connection)
        {
            lock (this)
            {
                serversMap.Add(serverName, connection); 
            }
        }

        public bool RemoveServer(string serverName)
        {
            lock (this)
            {
                return serversMap.Remove(serverName);
            }
        }

        public Connection GetServer(string serverName)
        {
            Connection ret = null;
            lock (this)
            {
                try
                {
                    serversMap.TryGetValue(serverName, out ret);
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
