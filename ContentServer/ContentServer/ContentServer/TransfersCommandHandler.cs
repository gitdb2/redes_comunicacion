using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using uy.edu.ort.obligatorio.Commons;
using System.IO;


namespace uy.edu.ort.obligatorio.ContentServer
{
    public class TransfersCommandHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TransfersCommandHandler instance = new TransfersCommandHandler();

        private TransfersCommandHandler() { }

        public static TransfersCommandHandler GetInstance()
        {
            return instance;
        }

        public bool Handle(Connection connection, Data dato)
        {

            if (dato.Command == Command.REQ)
            {
                return HandleREQ(connection, dato);
            }
            else
            {
                return HandleRES(connection, dato);
            }
        }

        private bool HandleRES(Connection Connection, Data dato)
        {
            Console.WriteLine("HandleRES [{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, Connection.Name, dato.ToString());
            log.DebugFormat(" connection owner: {0} ;  The data: {1} ", Connection.Name, dato.ToString());
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
                    
                    break;
            }
            return true;
        }

        private bool HandleREQ(Connection connection, Data dato)
        {
            Console.WriteLine("HandleREQ [{0}] connection owner: {1} ;  The data: {2} ", DateTime.Now, connection.Name, dato.ToString());
            log.DebugFormat(" connection owner: {0} ;  The data: {1} ", connection.Name, dato.ToString());
            bool ret = true;
            switch (dato.OpCode)
            {

                case OpCodeConstants.REQ_DOWNLOAD_FILE: //se ejecuta previo a la bajada de un archivo puntual, en el caso que el cliente no tenga el tamanio del archivo para saber cuando cortar.
                    ret = CommandDownloadFile(connection, dato);
                    log.Debug("procesé REQ download");
                    break;
                default:
                    break;
            }
            return ret;
        }
        public const string PIPE_SEPARATOR = "|";
        private bool CommandDownloadFile(Connection connection, Data dato)
        {

           // login + "|" + owner + "|"+hashfile;
            string[] payload = dato.Payload.Message.Split(new string[] { PIPE_SEPARATOR }, StringSplitOptions.None);
            string login = payload[0];
            string owner = payload[1];
            string hashfile = payload[2];

            FileInfo fi = FileOperationsSingleton.GetInstance().GetFile(hashfile, owner);
            Data retDato;
            if (fi == null)
            {
                retDato = new Data()
                {
                    Command = Command.RES,
                    OpCode = OpCodeConstants.RES_DOWNLOAD_FILE,
                    Payload = new Payload() { Message = "ERROR [wrong file or not found]" }
                };
                foreach (var item in retDato.GetBytes())
                {
                    Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                    connection.WriteToStream(item);
                }
            }
            else
            {

                long size = fi.Length;

                string message = login + PIPE_SEPARATOR + owner + PIPE_SEPARATOR + hashfile + PIPE_SEPARATOR + fi.Name + PIPE_SEPARATOR + size;

                retDato = new Data()
                {
                    Command = Command.RES,
                    OpCode = OpCodeConstants.RES_DOWNLOAD_FILE,
                    Payload = new Payload() { Message = message }
                };
                foreach (var item in retDato.GetBytes())
                {
                    Console.WriteLine("Envio :{0}", ConversionUtil.GetString(item));
                    connection.WriteToStream(item);
                }

/* 
                byte[] buffer = ConversionUtil.GetBytes2("HOLA");
                connection.WriteToNetworkStream(buffer, 0, buffer.Length);
*/
                
                FileStream fileStream = fi.OpenRead();
                const int BUFF_SIZE = 1024;
                byte[] buffer = new byte[BUFF_SIZE];
                
                bool done = false;
                while (!done)
	            {
                     int countRead = fileStream.Read(buffer,0, BUFF_SIZE);
                     if(countRead > 0){
                         if(countRead < BUFF_SIZE){
                             done = true;
                         }
                         connection.WriteToNetworkStream(buffer, 0, countRead);
                     }else{//no leyo nada de la entrada (cantidad de bytes justa, en la siguiente lectura)
                         done = true;
                     }
    
	            }
                fileStream.Close();
                
            }

            return false; //terminar la conexion

          
        }


       
    }
}
