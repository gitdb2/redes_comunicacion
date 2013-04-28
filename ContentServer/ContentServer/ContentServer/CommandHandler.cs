using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class CommandHandler
    {
        private static CommandHandler instance = new CommandHandler();

        private CommandHandler() { }

        public static CommandHandler GetInstance()
        {
            return instance;
        }

        public void Handle(Connection Connection, Data dato)
        {

            if (dato.Command == Command.REQ)
            {
                HandleREQ(Connection, dato);
            }
            else
            {
                HandleRES(Connection, dato);
            }
        }

        private void HandleRES(Connection Connection, Data dato)
        {
            switch (dato.OpCode)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                   
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 99:
                    break;
                default:
                    Console.WriteLine("default RES    --->" + ConversionUtil.GetString(dato.GetBytes()[0]));

                    break;
            }
        }

        private void HandleREQ(Connection Connection, Data dato)
        {
            switch (dato.OpCode)
            {
                case 0:
                    break;
                case 2: //viene el obtener lista de contactos
                    CommandGetContactList(Connection, dato);
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 99:
                    break;
                default:
                    Console.WriteLine("default REQ    --->" + ConversionUtil.GetString(dato.GetBytes()[0]));
                   
                    break;
            }
        }

        const string CONTACT_DELIMITER = "|";
        const string STATUS_DELIMITER = "@";
        private void CommandGetContactList(Connection Connection, Data dato)
        {
            string login = dato.Payload.Message;
            List<string> contacts = UsersContactsPersistenceHandler.GetInstance().GetContacts(login);

            StringBuilder message = new StringBuilder();
            bool first = true;
            foreach (var item in contacts)
            {
               if (first)
                {
                    first = false;
                }
                else
                {
                    message.Append(CONTACT_DELIMITER);
                }
                message.Append(item).Append(STATUS_DELIMITER).Append("0");
            }
            Data retDato = new Data() { Command = Command.RES, OpCode = 2, Payload = new MultiplePayload() { Message = message.ToString(), Destination=login } };
            foreach (var item in retDato.GetBytes())
	        {
                Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                Connection.WriteToStream(item);
	        }
            Console.WriteLine("termina :CommandGetContactList");
        }

       

      

       
    }
}
