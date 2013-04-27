using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.LibOperations.intefaces;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    public class UsersPersistenceHandler
    {
        private static UsersPersistenceHandler instance = new UsersPersistenceHandler();

        Dictionary<string, string> users = new Dictionary<string,string>();


        private UsersPersistenceHandler() { }

        public static UsersPersistenceHandler GetInstance()
        {
            return instance;
        }

      public bool RegisterLoginServer(string login, string serverName){
          
          if(!users.ContainsKey(login)){
              users.Add(login, serverName);
              return true;
          }
          return false;

      }
        public bool IsLoginRegistered(string login){
            return users.ContainsKey(login);
        }

   
}
