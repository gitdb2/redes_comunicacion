using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class UsersContactsPersistenceHandler
    {

        const string CONTACT_SEPARATOR = ",";
        private static UsersContactsPersistenceHandler instance = new UsersContactsPersistenceHandler();
      
        private Properties contacts = new Properties("contactos.txt");

        private UsersContactsPersistenceHandler() 
        {
        }

        public static UsersContactsPersistenceHandler GetInstance()
        {
            return instance;
        }

        public bool RegisterLoginServer(string login, string serverName)
        {
            if (!contacts.ContainsKey(login))
            {
                contacts.Set(login, serverName);
                return true;
            }
            return false;
        }

        public bool IsLoginRegistered(string login)
        {
            return contacts.ContainsKey(login);
        }

        public void SaveUsers()
        {
            contacts.Save();
        }

      

        public List<string> GetContacts(string login)
        {
            if (IsLoginRegistered(login))
            {
                string contactStr = contacts.Get(login);
                return new List<string>(contactStr.Split(new string[] { CONTACT_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries));
            }
            return new List<string>();
        }

        /// <summary>
        /// si el login no existe 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool AddContact(string login, string contact)
        {
           bool ret = false;

           if (IsLoginRegistered(login))
           {
               List<string> someContacts = GetContacts(login);
               ret = true;
               if (!someContacts.Contains(contact))
               {
                   someContacts.Add(contact);
               }
           }

           return ret;
        }

        public void LoadContacts()
        {
            contacts.Reload();
            Console.WriteLine("Contacts :\r\n{0}", contacts.ToString());
        }
    }
}
