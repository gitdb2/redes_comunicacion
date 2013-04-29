using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.Commons
{
    public class OpCodeConstants
    {
        public const int REQ_LOGIN = 1;
        public const int RES_LOGIN = REQ_LOGIN;

        public const int REQ_CONTACT_LIST = 2;
        public const int RES_CONTACT_LIST = REQ_CONTACT_LIST;
        
        public const int REQ_SERVER_CONNECT = 3;
        public const int RES_SERVER_CONNECT = REQ_SERVER_CONNECT;
        
        public const int REQ_CREATE_USER = 4;
        public const int RES_CREATE_USER = REQ_CREATE_USER;

        public const int REQ_FIND_CONTACT = 5;
        public const int RES_FIND_CONTACT = REQ_FIND_CONTACT;
    }
}
