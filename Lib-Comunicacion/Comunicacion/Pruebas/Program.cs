using System;
using Comunicacion;
using System.Text;
using System.Collections.Generic;

namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {

            Data data = new Data();
            data.OpCode = OpCode.HELLO;
            data.Command = Command.REQ;
            data.Payload = new Payload("Hola soy Cli 1");


            List<byte[]> lista = data.GetBytes();
            foreach (var item in lista)
	        {
		            Console.WriteLine(ConversionUtil.GetString(item));
	        }
            Console.WriteLine("Done Payload");


            data = new Data();
            data.OpCode = OpCode.HELLO;
            data.Command = Command.REQ;
            data.Payload = new MultiplePayload();

            for (int i = 0; i < MultiplePayload.MAX_PAYLOAD_LENGTH; i++)
            {
                ((MultiplePayload)data.Payload).Message+="A";
            }


            lista = data.GetBytes();
            foreach (var item in lista)
            {
                Console.WriteLine(ConversionUtil.GetString(item));
            }


            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
