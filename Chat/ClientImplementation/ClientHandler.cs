using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using uy.edu.ort.obligatorio.Commons;
using Comunicacion;

namespace ClientImplementation
{
    public class ClientHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int puertoDNS = 2000;
        private const string ipDNS = "localhost";
        private Connection connection;
     //   public TcpClient TcpClient { get; set; }
        public string Login { get; set; }

        private static ClientHandler instance = new ClientHandler();


        private ClientHandler() { }

        public static ClientHandler GetInstance()
        {
            return instance;
        }

        public void Connect(string login)
        {
            connection = new Connection(login, new TcpClient(ipDNS, puertoDNS), new ReceiveEventHandler());
            this.Login = login;
        }

        public void CloseConnection()
        {
            if (connection != null)
            {
                this.connection.CloseConn();
            }
        }

        public void LoginClient(string login)
        {
            SendMessage(Command.REQ, OpCodeConstants.REQ_LOGIN, new Payload(login));
        }

        public void GetContactList(string login)
        {
            SendMessage(Command.REQ, OpCodeConstants.REQ_CONTACT_LIST, new Payload(login));
        }

        public void FindContact(string Login, string pattern)
        {
            SendMessage(Command.REQ, OpCodeConstants.REQ_FIND_CONTACT, new Payload(Login + "|" + pattern));
        }

        public void AddContact(string Login, string contactToAdd)
        {
            SendMessage(Command.REQ, OpCodeConstants.REQ_ADD_CONTACT, new Payload(Login + ParseConstants.SEPARATOR_PIPE + contactToAdd));
        }

        public void SendChatMessage(string clientFrom, string clientTo, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(clientFrom).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(clientTo).Append(ParseConstants.SEPARATOR_PIPE);
            sb.Append(message);
            SendMessage(Command.REQ, OpCodeConstants.REQ_SEND_CHAT_MSG, new Payload(sb.ToString()));
        }

        public event EventHandler LoginOK;

        public void OnLoginOK()
        {
            if (LoginOK != null)
                LoginOK(this, EventArgs.Empty);
        }

        public event ChatErrorEventHandler LoginFailed;
        
        public delegate void ChatErrorEventHandler(object sender, LoginErrorEventArgs e);
        
        public void OnLoginFailed(LoginErrorEventArgs e)
        {
            if (LoginFailed != null)
                LoginFailed(this, e);
        }

        public event ContactListEventHandler ContactListResponse;

        public delegate void ContactListEventHandler(object sender, ContactListEventArgs e);
        
        public void OnContactListResponse(ContactListEventArgs contactListEventArgs)
        {
            if (ContactListResponse != null)
                ContactListResponse(this, contactListEventArgs);
        }

        public delegate void FindContactsEventHandler(object sender, ContactListEventArgs e);

        public event FindContactsEventHandler FindContactResponse;

        public void OnFindContactResponse(ContactListEventArgs contactListEventArgs)
        {
            if (FindContactResponse != null)
                FindContactResponse(this, contactListEventArgs);
        }

        private void SendMessage(Command command, int opCode, Payload payload)
        {
            Data data = new Data() { Command = command, OpCode = opCode, Payload = payload };
            foreach (var item in data.GetBytes())
            {
                connection.WriteToStream(item);
            }
        }

        public delegate void AddContactEventHandler(object sender, SimpleEventArgs e);

        public event AddContactEventHandler AddContactResponse;

        public void OnAddContactResponse(SimpleEventArgs simpleEventArgs)
        {
            //el message de simpleEventArgs viene en el formato n|m|loginDestino|contactoAgregado@estado|mensaje
            string contactAddedStatus = simpleEventArgs.Message.Split('|')[3];

            if (AddContactResponse != null)
                AddContactResponse(this, simpleEventArgs);

            if (UpdateContactStatusResponse != null)
                UpdateContactStatusResponse(this, new SimpleEventArgs() { Message = contactAddedStatus });
        }

        public delegate void UpdateContactStatusEventHandler(object sender, SimpleEventArgs e);

        public event UpdateContactStatusEventHandler UpdateContactStatusResponse;

        public delegate void ChatMessageReceivedEventHandler(object sender, ChatMessageEventArgs e);

        public event ChatMessageReceivedEventHandler ChatMessageReceived;

        public void OnReceivedChatMessage(ChatMessageEventArgs chatMessageEventArgs)
        {
            if (ChatMessageReceived != null)
                ChatMessageReceived(this, chatMessageEventArgs);
        }

        public delegate void ChatMessageSentEventHandler(object sender, ChatMessageSentEventArgs e);

        public event ChatMessageSentEventHandler ChatMessageSent;

        public void OnMessageSentResponse(ChatMessageSentEventArgs chatMessageSentEventArgs)
        {
            if (ChatMessageSent != null)
                ChatMessageSent(this, chatMessageSentEventArgs);
        }

    }
}
