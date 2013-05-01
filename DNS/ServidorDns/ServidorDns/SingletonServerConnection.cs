using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;
using System.Text.RegularExpressions;

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
            lock (serversMap)
            {
                connection.Name = serverName;
                //   connection.Port = serverPort;
                serversMap.Add(serverName, connection);
            }
        }

        public bool RemoveServer(string serverName)
        {
            lock (serversMap)
            {
                return serversMap.Remove(serverName);
            }
        }

        public Connection GetServer(string serverName)
        {
            Connection ret = null;
            lock (serversMap)
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

        //lockea el mapa para que nadie escriba o remueva mientras se busca el mejor server para el usuario
        public string FindBestServerForNewUser()
        {
            string ret = null;
            lock (serversMap)
            {
                if (serversMap.Count != 0)
                {
                    int lower = int.MaxValue;
                    foreach (var item in serversMap.Values)
                    {
                        if (lower > item.UserCount)
                        {
                            ret = item.Name;
                            lower = item.UserCount;
                        }
                    }
                    return ret;
                }
                else
                {
                    throw new Exception("No hay servidores ONLINE");
                }
            }
        }

        public int IncUserCount(string serverName)
        {
            int ret = -1;
            lock (serversMap)
            {
                if (serversMap.ContainsKey(serverName))
                {
                    serversMap[serverName].UserCount++;
                    ret = serversMap[serverName].UserCount;
                }
                else throw new Exception("El server no se encuentra ONLINE");
            }
            return ret;
        }

        public List<ServerInfo> GetServersWithUsers()
        {
            List<ServerInfo> ret = new List<ServerInfo>();
            lock (this)
            {
                foreach (var item in serversMap.Values)
                {
                    if (item.UserCount > 0)
                    {
                        ret.Add(new ServerInfo() { Ip = item.Ip, Name = item.Name, Port = item.Port, TransfersPort = item.TransferPort });
                    }
                }
            }
            return ret;
        }

    }
}