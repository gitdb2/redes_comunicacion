using System;
using Comunicacion;
using System.Text;
using System.Collections.Generic;
using System.IO;

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
            Console.WriteLine("Done Multiple Payload");



            Console.WriteLine("|"+Settings.GetInstance().GetProperty("REQ01") +"|");
            Console.WriteLine("Done reading property");



            Consola();




            Console.WriteLine("Done");
            Console.ReadLine();
        }



        static public void Consola()
        {
            Stream inputStream = Console.OpenStandardInput();
            byte[] bytes = new byte[10000];
            Console.WriteLine("Escriba un coando del protocolo (se hace touppercase automaticco)");
            Console.WriteLine("(Example: REQ0200015Hola soy Cli 1.");
            Boolean end = false;
            String buffer1  ="";
            Boolean buffered = false;
            while (!end)
            {
                
                int outputLength = inputStream.Read(bytes, 0, 30);
                Console.WriteLine("leido: " + outputLength);
                //if(outputLength < 10 || buffered){
                //    buffer1 +=ConversionUtil.GetString(bytes, outputLength);
                //    buffered =true;
                //    Console.WriteLine("buffer--->"+buffer1);
                //}else{
                    String inString = ConversionUtil.GetString(bytes, outputLength);
                    //if(buffered){
                    //    buffered =  false;
                    //    inString=  buffer1 + inString;
                    //    buffer1 ="";
                    //}
                      Console.WriteLine("inString--->"+inString);

                //}
               
              
                
              // Enum.Parse(typeof(Command), 

            }
           
            //char[] chars = Encoding.UTF7.GetChars(bytes, 0, outputLength);
            Console.WriteLine("Decoded string:");
            //Console.WriteLine(new string(chars));
        }
    }
}
