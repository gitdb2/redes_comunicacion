using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comunicacion
{

   
    public class Payload
    {
        public const int MAX_PAYLOAD_LENGTH = 10000;
        public String Message { get; set; }


        public virtual List<byte[]> GetBytes()
        {
            var ret =new List<byte[]>();
            ret.Add(ConversionUtil.GetBytes(Message));
            return ret;
        }

        /// <summary>
        /// el payload por defecto asume que el mensaje no se parte, o sea que el payload tiene menos o 10000 caracteres
        /// </summary>
        /// <param name="bytes"></param>
        public  Payload(byte[] bytes)
        {
            Message = ConversionUtil.GetString(bytes);
        }
        public Payload()
        {
        }
        public Payload(String menssage)
        {
            Message = menssage;
        }

        public virtual void AppernMessage(String appender)
        {
            Message += appender;
        }

          
         /// <summary>
         /// Este metodo es sobreescrito por las distintas implementaciones, ej payload de lista de usuarios que va a enviar la metadata de usuarios pro sin partir el contenido, para ello debera aumentar la cantidad de partes si es necesario
         /// </summary>
         /// <returns></returns>
        //public virtual  int GetPatrs()
        //{
        //    if (message.Length < MAX_PAYLOAD_LENGTH)
        //    {
        //        return 1;
        //    }
        //    else
        //    { //
        //        int parts = message.Length / MAX_PAYLOAD_LENGTH;
        //        int rest = message.Length % MAX_PAYLOAD_LENGTH;

        //        return rest>0 ? parts +1 : parts;

        //    }
        //}
    }
}
