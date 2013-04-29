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


        public virtual List<char[]> GetBytes()
        {
            var ret = new List<char[]>();
            ret.Add(ConversionUtil.GetBytes(Message));
            return ret;
        }

        /// <summary>
        /// el payload por defecto asume que el mensaje no se parte, o sea que el payload tiene menos o 10000 caracteres
        /// </summary>
        /// <param name="bytes"></param>
        public Payload(char[] bytes)
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


        public override string ToString()
        {
            return Message;
        }
    }
}
