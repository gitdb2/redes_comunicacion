using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comunicacion
{
    public class MultiplePayload :Payload
    {
        public const String SEPARATOR   = "|";
        const int SIZES_LENGTH          = 2; //2 prara total, 2 para la cuenta y 1x2 para separadores : TOTAL|COUNT|
        const int SEPARATOR_LENGTH      = 1;
        const int TOTAL_LENGTH          = 2 * SIZES_LENGTH + 2 * SEPARATOR_LENGTH;
        const int MAX_CHUNK = MAX_PAYLOAD_LENGTH - TOTAL_LENGTH;
         public override List<byte[]> GetBytes()
        {
            var ret =new List<byte[]>();
            if (Message.Length < MAX_CHUNK)
            {

                String subFrame = String.Format("{0:D" + SIZES_LENGTH + "}"+SEPARATOR+"{1:D" + SIZES_LENGTH + "}|", 1, 1);
                ret.Add(ConversionUtil.GetBytes(subFrame + Message));
            }
            else
            {
                int parts = Message.Length / MAX_CHUNK;
                int rest = Message.Length % MAX_CHUNK;

                if(rest>0){
                    parts++;
                }

                for (int i = 0; i < parts; i++)
                {
                    String subFrame = String.Format("{0:D" + SIZES_LENGTH + "}"+SEPARATOR+"{1:D" + SIZES_LENGTH + "}|", parts, i+1);

                    if (Message.Length - i * MAX_CHUNK > MAX_CHUNK)
                    {
                        ret.Add(ConversionUtil.GetBytes(subFrame + Message.Substring(i * MAX_CHUNK, MAX_CHUNK)));
                    }
                    else
                    {
                        ret.Add(ConversionUtil.GetBytes(subFrame + Message.Substring(i * MAX_CHUNK, Message.Length - i * MAX_CHUNK)));
                    }
                    

                }

                

            }
            return ret;
        }

     
        public  MultiplePayload(byte[] bytes){
            Message = ConversionUtil.GetString(bytes);
        }
        public MultiplePayload()
        {
            Message = "";
        }
        public MultiplePayload(String menssage)
        {
          
            Message = menssage;
        }

        public virtual void SetMessage(List<String> elements)
        {
           Message = string.Join(SEPARATOR, elements);
           
        }
        public override void AppernMessage(String text)
        {
            if(!Message.EndsWith(SEPARATOR)){
                Message += SEPARATOR;
            }
            Message += text;
        }



    }
}
