using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using System.IO;

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

        public void SaveContacts()
        {
            contacts.Save();
        }


        public int Count { get { return contacts.Count; } }

        //solo se permite ageagar un usuario nuevo a la vez, crea el area compartida y lo da de alta en la lista de contactos con 0 contactos y salva el archivo
        public bool AddNewUser(string login)
        {
            lock (contacts)
            {
                if (!contacts.ContainsKey(login))
                {

                    bool ok = CreateDiskSharedSpace(login);

                    if (ok)
                    {
                        contacts.Set(login, "");
                        contacts.Save();
                    }
                    return ok;
                }
                return false;
            }
        }
        /*
         base.shared.dir.path=c:/shared
listen.ip=ANY

server.ip=192.168.0.242
server.port=2001
server.name=rodrigo-nb

dns.ip=127.0.0.1
dns.port=2000
         
         */
        private bool CreateDiskSharedSpace(string login)
        {
            string dir = Settings.GetInstance().GetProperty("base.shared.dir.path", @"c:/shared")+ "/"+login;
            if (!Directory.Exists(dir))
            {
                try 
	            {
                    Directory.CreateDirectory(dir);
                    return true;
	            }
	            catch (Exception)
	            {
		
		            return false;
	            }
              
            }
             return false;
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
