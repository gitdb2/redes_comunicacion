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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        /*
        public bool RegisterLoginServer(string login, string serverName)
        {
            lock (contacts)
            {
                if (!contacts.ContainsKey(login))
                {
                    contacts.Set(login, serverName);
                    return true;
                }
                return false;
            }
        }
        */
        public bool IsLoginRegistered(string login)
        {
            lock (contacts)
            {
                return contacts.ContainsKey(login);
            }
        }

        public void SaveContacts()
        {
            lock (contacts)
            {
                contacts.Save();
            }
        }


        public int Count
        {
            get
            {
                lock (contacts)
                { return contacts.Count; }
            }
        }

        public bool RegisterNewUser(string login)
        {
            lock (contacts)
            {
                return AddNewUser(login);
            }
        }
        //solo se permite ageagar un usuario nuevo a la vez, crea el area compartida y lo da de alta en la lista de contactos con 0 contactos y salva el archivo
        private bool AddNewUser(string login)
        {

            if (!contacts.ContainsKey(login))
            {
                //ok si el directorio se cra o si ya existe
                bool ok = FileOperationsSingleton.GetInstance().CreateDiskSharedSpace(login);

                if (ok)
                {
                    contacts.Set(login, "");
                    contacts.Save();
                }
                return ok;
            }
            return false;
        }


       

        public List<string> GetContacts(string login)
        {
            lock (contacts)
            {
                return GetContactsInternal(login);
            }
        }
        private List<string> GetContactsInternal(string login)
        {
            if (contacts.ContainsKey(login))
            {
                string contactStr = contacts.Get(login);
                return new List<string>(contactStr.Split(new string[] { CONTACT_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                AddNewUser(login);
            }
            return new List<string>();
        }

    
        public bool AddContact(string login, string contact)
        {
            lock (contacts)
            {
                bool ret = false;

                if (!contacts.ContainsKey(login))
                {
                    AddNewUser(login);

                }

                List<string> someContacts = GetContactsInternal(login);
                
                if (!someContacts.Contains(contact))
                {
                    someContacts.Add(contact);
                    string newContactList = String.Join(CONTACT_SEPARATOR, someContacts);
                    contacts.Set(login, newContactList);
                    contacts.Save();
                    ret = true;
                }

               return ret;
            }
        }

        public void LoadContacts()
        {
            lock (contacts)
            {
                contacts.Reload();
                Console.WriteLine("Contacts :\r\n{0}", contacts.ToString());
            }
        }
    }
}
