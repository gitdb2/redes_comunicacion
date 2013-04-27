using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comunicacion
{
    public class Data
    {
        const int MAX_COMMAND_LENGTH    = 3;
        const int MAX_OPCODE_LENGTH     = 2;
        const int MAX_LENGTH_LENGTH     = 5;
       

        public Command Command { get;  set; }
        public int OpCode {  get;  set; }
        public Payload Payload {  get;  set; }

        public List<char[]> GetBytes()
        {
            List<char[]> ret = new List<char[]>();
            //String comm = Enum.GetName(Command.GetType(), Command);
            String patron = "{0:D" + MAX_OPCODE_LENGTH + "}";
            String commonHeader = Command + String.Format(patron, OpCode);

            List<char[]> payloads = Payload.GetBytes();
            foreach (var item in payloads)
            {
                int length = item.Length;

              //  Console.WriteLine( ConversionUtil.GetString(item));

                String frame = commonHeader + String.Format("{0:D" + MAX_LENGTH_LENGTH + "}", length);
                char[] tmp = new char[frame.Length + length];
                char[] framArr = ConversionUtil.GetBytes(frame);
                for (int i = 0; i < framArr.Length; i++)
                {
                    tmp[i] = framArr[i];
                }

                for (int i = 0; i < item.Length; i++)
                {
                    tmp[framArr.Length + i] = item[i];
                }
                ret.Add(tmp);

            }


            return ret;
        }

    }


}
