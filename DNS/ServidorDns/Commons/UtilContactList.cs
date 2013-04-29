using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.Commons
{
    public class UtilContactList
    {

        //formato de la trama
        //01|01|login|contacto1@0|contacto2@0|....
        private const char EXTERNAL_SEPARATOR = '|';
        private const char INTERNAL_SEPARATOR = '@';
        
        private const int POS_LOGIN = 2;
        private const int START_POS_CONTACT_LIST = 3;

        private static string ExtractStringAtPos(string payload, int pos)
        {
            return payload.Split(EXTERNAL_SEPARATOR)[pos];
        }

        public static string ExtractLogin(string payload)
        {
            return ExtractStringAtPos(payload, POS_LOGIN);
        }

        public static bool IsLastPart(string payload)
        {
            string[] payloadSplitted = payload.Split(EXTERNAL_SEPARATOR);
            return payloadSplitted[0].Equals(payloadSplitted[1]);
        }
        
        public static Dictionary<string, bool> ContactListFromString(string payload)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            string[] payloadSplitted = payload.Split(EXTERNAL_SEPARATOR);
            string[] tmp;
            int cont = 0;
            foreach (string item in payloadSplitted)
            {
                if (cont >= START_POS_CONTACT_LIST)
                {
                    tmp = item.Split(INTERNAL_SEPARATOR);
                    if (tmp[0].Length > 0)
                        result.Add(tmp[0], tmp[1].Equals("1"));
                }
                cont++;
            }
            return result;
        }

        public static String StringFromContactList(Dictionary<string, bool> list, string originalPayload)
        {
            StringBuilder result = new StringBuilder();

            //appendeo el total de tramas y la trama actual
            result.Append(ExtractStringAtPos(originalPayload, 0)).Append(EXTERNAL_SEPARATOR);
            result.Append(ExtractStringAtPos(originalPayload, 1)).Append(EXTERNAL_SEPARATOR);
            
            //appendeo el login
            result.Append(ExtractStringAtPos(originalPayload, POS_LOGIN)).Append(EXTERNAL_SEPARATOR);

            //appendeo la lista de contactos
            bool first = true;
            foreach (KeyValuePair<string, bool> entry in list)
            {
                if (first)
                {
                    first = false;
                }
                else 
                {
                    result.Append(EXTERNAL_SEPARATOR);
                }
                result.Append(entry.Key).Append(INTERNAL_SEPARATOR);
                result.Append(entry.Value ? "1" : "0");
            }
            return result.ToString();
        }

    }
}
