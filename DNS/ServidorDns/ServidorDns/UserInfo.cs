using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.ServidorDns
{
    [Serializable]
    public class UserInfo
    {
        public string UserName;
        public string Password;
        [NonSerialized]
        public bool LoggedIn;      // Is logged in and connected?
        [NonSerialized]
        public DnsServer Connection;  // Connection info

        public UserInfo(string user, string pass)
        {
            this.UserName = user;
            this.Password = pass;
            this.LoggedIn = false;
        }
        public UserInfo(string user, string pass, DnsServer conn)
        {
            this.UserName = user;
            this.Password = pass;
            this.LoggedIn = true;
            this.Connection = conn;
        }
    }
}
