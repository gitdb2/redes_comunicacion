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
        public OpCode OpCode {  get;  set; }
        public Payload Payload {  get;  set; }

        public List<byte[]> GetBytes()
        {
            List<byte[]> ret = new List<byte[]>();
            //String comm = Enum.GetName(Command.GetType(), Command);
            String patron = "{0:D" + MAX_OPCODE_LENGTH + "}";
            String commonHeader = Command + String.Format(patron, (int)OpCode);
            
            List<byte[]> payloads = Payload.GetBytes();
            foreach (var item in payloads)
            {
                int length = item.Length;

              //  Console.WriteLine( ConversionUtil.GetString(item));

                String frame = commonHeader + String.Format("{0:D" + MAX_LENGTH_LENGTH + "}", length);
                byte[] tmp      = new byte[frame.Length + length];
                byte[] framArr = ConversionUtil.GetBytes(frame);
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
