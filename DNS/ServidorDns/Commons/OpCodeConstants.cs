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

        public const int REQ_ADD_CONTACT = 6;
        public const int RES_ADD_CONTACT = REQ_ADD_CONTACT;

        public const int REQ_SEARCH_FILES = 7;
        public const int RES_SEARCH_FILES = REQ_SEARCH_FILES;

        public const int REQ_GET_TRANSFER_INFO = 8;
        public const int RES_GET_FILE = REQ_GET_TRANSFER_INFO;

        public const int REQ_SEND_CHAT_MSG = 9;
        public const int RES_SEND_CHAT_MSG = REQ_SEND_CHAT_MSG;

        public const int REQ_DOWNLOAD_FILE = 10;
        public const int RES_DOWNLOAD_FILE = REQ_DOWNLOAD_FILE;

        public const int REQ_GET_SERVERS = 11;
        public const int RES_GET_SERVERS = REQ_GET_SERVERS;

        public const int REQ_SERVER_INFO = 12;
        public const int RES_SERVER_INFO = REQ_SERVER_INFO;

        public const int REQ_UPLOAD_FILE = 13;
        public const int RES_UPLOAD_FILE = REQ_UPLOAD_FILE;


        public const int REQ_CHANGE_STATUS = 14;
        public const int RES_CHANGE_STATUS = REQ_CHANGE_STATUS;
    }
}
