using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.obligatorio.Commons;
using Comunicacion;

namespace ClientImplementation
{
    public class CommandHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string MESSAGE_SUCCESS = "SUCCESS";
        private static CommandHandler instance = new CommandHandler();

        private CommandHandler() { }

        public static CommandHandler GetInstance()
        {
            return instance;
        }

        public bool Handle(Connection clientConnection, Data dato)
        {
            if (dato.Command == Command.REQ)
            {
                return  HandleREQ(clientConnection, dato);
            }
            else
            {
               return  HandleRES(clientConnection, dato);
            }
        }

        private bool HandleRES(Connection clientConnection, Data dato)
        {
            bool ret = true;
            switch (dato.OpCode)
            {
                case 0:
                    break;
                case OpCodeConstants.RES_LOGIN:
                    CommandRESLogin(clientConnection, dato);
                    break;
                case OpCodeConstants.RES_CONTACT_LIST:
                    CommandRESContactList(clientConnection, dato);
                    break;
                case OpCodeConstants.RES_FIND_CONTACT:
                    CommandRESFindContact(clientConnection, dato);
                    break;
                case OpCodeConstants.RES_ADD_CONTACT:
                    CommandRESAddContact(clientConnection, dato);
                    break;
                case OpCodeConstants.RES_SEND_CHAT_MSG:
                    CommandRESMessageSent(clientConnection, dato);
                    break;
                case OpCodeConstants.RES_GET_SERVERS:
                    CommandRESGetServers(clientConnection, dato);
                    break;
                case OpCodeConstants.RES_SEARCH_FILES:
                    ret = CommandRESSearchFiles(clientConnection, dato);
                    break;
                case OpCodeConstants.RES_SERVER_INFO:
                    CommandRESServerInfo(clientConnection, dato);
                    break;
                default:
                    break;
            }
            return ret;
        }

        private void CommandRESServerInfo(Connection clientConnection, Data dato)
        {
            //la respuesta viene en el formato 
            //serverName|serverIp|serverPort|transfersPort o ERROR
            string[] payloadSplitted = dato.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE);

            ServerInfo serverInfo = null;

            if (!payloadSplitted[0].Equals(MessageConstants.MESSAGE_ERROR))
            {
                serverInfo = new ServerInfo() { 
                    Name = payloadSplitted[0]
                    , Ip = payloadSplitted[1]
                    , Port = int.Parse(payloadSplitted[2])
                    , TransfersPort = int.Parse(payloadSplitted[3])
                };
            }

            ClientHandler.GetInstance().OnServerInfoResponse(new ServerInfoEventArgs() { ServerInfo = serverInfo });
        }

        private void CommandRESMessageSent(Connection clientConnection, Data dato)
        {
            //la respuesta viene en el formato loginMessageTo|success o error
            string[] payloadSplitted = dato.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE);
            ClientHandler.GetInstance().OnMessageSentResponse(new ChatMessageSentEventArgs() { MessageTo = payloadSplitted[0], MessageStatus = payloadSplitted[1] });
        }

        private void CommandRESAddContact(Connection clientConnection, Data dato)
        {
            //la respuesta viene en el formato n|m|loginDestino|contactoAgregado@estado|mensaje
            ClientHandler.GetInstance().OnAddContactResponse(new SimpleEventArgs() { Message = dato.Payload.Message });
        }

        private void CommandRESFindContact(Connection clientConnection, Data dato)
        {
            Dictionary<string, bool> contactList = UtilContactList.ContactListFromString(dato.Payload.Message);
            bool isLastPart = UtilContactList.IsLastPart(dato.Payload.Message);
            ClientHandler.GetInstance().OnFindContactResponse(new ContactListEventArgs() { ContactList = contactList, IsLastPart = isLastPart });
        }

        private void CommandRESLogin(Connection clientConnection, Data dato)
        {
            if (dato.Payload.Message.Equals(MESSAGE_SUCCESS))
            {
                ClientHandler.GetInstance().OnLoginOK();
            } 
            else 
            {
                ClientHandler.GetInstance().OnLoginFailed(new LoginErrorEventArgs() { ErrorMessage = dato.Payload.Message });
            }
        }

        private void CommandRESContactList(Connection clientConnection, Data dato)
        {
            Dictionary<string, bool> contactList = UtilContactList.ContactListFromString(dato.Payload.Message);
            bool isLastPart = UtilContactList.IsLastPart(dato.Payload.Message);
            ClientHandler.GetInstance().OnContactListResponse(new ContactListEventArgs() { ContactList = contactList, IsLastPart = isLastPart });
        }

        private void CommandRESGetServers(Connection connection, Data dato)
        {
            MultiplePayloadFrameDecoded decoded = MultiplePayloadFrameDecoded.Parse(dato.Payload.Message);
            ClientHandler.GetInstance().OnGetServersResponse(new GetServersEventArgs() { Response = decoded});

        }

        private bool CommandRESSearchFiles(Connection connection, Data dato)
        {
            MultiplePayloadFrameDecoded decoded = MultiplePayloadFrameDecoded.Parse(dato.Payload.Message);
            ClientHandler.GetInstance().OnSearchFilesResponse(new SearchFilesEventArgs() { Response = decoded , Connection = connection});
            return !decoded.IsLastpart();
        }

        private bool HandleREQ(Connection clientConnection, Data dato)
        {
            bool ret = true;
             switch (dato.OpCode)
            {
                case OpCodeConstants.REQ_SEND_CHAT_MSG:
                    CommandREQSendChatMessage(clientConnection, dato);
                    break;
                case 1: 
                    break;
                default:
                    break;
            }
             return ret;
        }

        private void CommandREQSendChatMessage(Connection clientConnection, Data dato)
        {
            string[] payloadSplitted = dato.Payload.Message.Split(ParseConstants.SEPARATOR_PIPE);
            ClientHandler.GetInstance().OnReceivedChatMessage(
                new ChatMessageEventArgs()
                {
                    ClientFrom = payloadSplitted[0],
                    ClientTo = payloadSplitted[1],
                    Message = payloadSplitted[2]
                }
            );
        }

    }
}
