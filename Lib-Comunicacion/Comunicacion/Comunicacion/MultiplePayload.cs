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
        const int MAX_CHUNK             = MAX_PAYLOAD_LENGTH - TOTAL_LENGTH;

        public MultiplePayload()
        {
            Destination = "NONE";
            Message = "";
        }

        public String Destination { get; set; }

         public override List<char[]> GetBytes()
        {

            int destinationLength = Destination.Length == 0 ? 0 : 1 + Destination.Length;
            string destinationStr = (destinationLength > 0 ? (Destination + SEPARATOR) : ("NONE" + SEPARATOR));

            int NEW_MAX_CHUNK  = MAX_CHUNK- destinationLength;
            var ret =new List<char[]>();
            if (Message.Length < NEW_MAX_CHUNK)
            {

                String subFrame = String.Format("{0:D" + SIZES_LENGTH + "}"+SEPARATOR+"{1:D" + SIZES_LENGTH + "}"+SEPARATOR+ destinationStr, 1, 1);
                ret.Add(ConversionUtil.GetBytes(subFrame  + Message));
            }
            else
            {
                int parts = (Message.Length) / NEW_MAX_CHUNK;
                int rest = (Message.Length) % NEW_MAX_CHUNK;

                if(rest>0){
                    parts++;
                }

                for (int i = 0; i < parts; i++)
                {
                    String subFrame = String.Format("{0:D" + SIZES_LENGTH + "}" + SEPARATOR + "{1:D" + SIZES_LENGTH + "}" + SEPARATOR + 
                                                                                                                destinationStr, parts, i + 1);


                    if (Message.Length - i * NEW_MAX_CHUNK > NEW_MAX_CHUNK)
                    {
                        ret.Add(ConversionUtil.GetBytes(subFrame + Message.Substring(i * NEW_MAX_CHUNK, NEW_MAX_CHUNK)));
                    }
                    else
                    {
                        ret.Add(ConversionUtil.GetBytes(subFrame + Message.Substring(i * NEW_MAX_CHUNK, Message.Length - i * NEW_MAX_CHUNK)));
                    }
                    

                }

                

            }
            return ret;
        }

     
        public  MultiplePayload(char[] bytes){
            Message = ConversionUtil.GetString(bytes);
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
