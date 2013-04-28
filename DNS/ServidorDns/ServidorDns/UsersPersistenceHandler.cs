using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.LibOperations.intefaces;
using System.Xml.Serialization;
using System.IO;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class UsersPersistenceHandler
    {
        private static UsersPersistenceHandler instance = new UsersPersistenceHandler();
        private Properties users = new Properties("users.txt");

        private UsersPersistenceHandler() 
        {
        }

        public static UsersPersistenceHandler GetInstance()
        {
            return instance;
        }

        public bool RegisterLoginServer(string login, string serverName)
        {
            if (!users.ContainsKey(login))
            {
                users.Set(login, serverName);
                return true;
            }
            return false;
        }

        public string GetServerName(string login)
        {
            return users.Get(login);
        }

        public bool IsLoginRegistered(string login)
        {
            return users.ContainsKey(login);
        }

        public void SaveUsers()
        {
            users.Save();
        }

        public void LoadUsers()
        {
            users.Reload();
            Console.WriteLine("Users:\r\n{0}", users.ToString());
        }

    }
   
}
