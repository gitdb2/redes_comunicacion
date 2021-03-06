﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;
using System.Text.RegularExpressions;

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
                    return clientsMap.Remove(login) && clientsContactsMap.Remove(login);
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

        public List<string> FindRegisteredClientByPattern(string pattern, string exclude)
        {
            lock (this)
            {
                var regex = new Regex(pattern);
                List<string> tmpKeyList = new List<string>(UsersPersistenceHandler.GetInstance().GetRegisteredUsers());
                tmpKeyList.Remove(exclude);
                return tmpKeyList.FindAll(delegate(string s) { return regex.IsMatch(s); });
            }
        }

        public void AddContactToClient(string client, string contactToAdd)
        {
            lock (this)
            {
                if (clientsContactsMap.ContainsKey(client))
                {
                    if (!clientsContactsMap[client].Contains(contactToAdd))
                        clientsContactsMap[client].Add(contactToAdd);
                }
            }
        }

        public void AddContactToClient(string login, List<string> clientContactList)
        {
            lock (this)
            {
                clientsContactsMap[login] = clientContactList;         
            }
        }

        public List<string> GetContactsOfLogin(string login)
        {
            List<string> tmpKeyList = new List<string>();
            lock (this)
            { 
                if (clientsContactsMap.ContainsKey(login))
                    tmpKeyList.AddRange(clientsContactsMap[login]);
            }
            return tmpKeyList;
        }
    }
}
