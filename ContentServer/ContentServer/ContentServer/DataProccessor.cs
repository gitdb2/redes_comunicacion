﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Comunicacion;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class DataProccessor
    {

        private static DataProccessor instance = new DataProccessor();

        private DataProccessor() { }

        public static DataProccessor GetInstance()
        {
            return instance;
        }



        public Data LoadObject(StreamReader br)
        {

            char[] buffer = new char[10];
            int readQty = br.Read(buffer, 0, 10);//REQ99000050101A


            if (readQty < 10) throw new Exception("Errror en trama largo fijo");

            Command type = (Command)Enum.Parse(typeof(Command), ArrayToString(buffer, 0, 3));
            int opCode = int.Parse(ArrayToString(buffer, 3, 2));
            int payloadLength = int.Parse(ArrayToString(buffer, 5, 5));
            //int partsTotal          = int.Parse(ArrayToString(buffer, 10, 2));
            //int partsCurrent        = int.Parse(ArrayToString(buffer, 12, 2));

       


            buffer = new char[payloadLength];
            readQty = br.Read(buffer, 0, payloadLength);
            if (readQty < payloadLength) throw new Exception("Errror en trama largo fijo leyendo payload");

            String payloadTmp = ArrayToString(buffer, 0, readQty);
            Console.WriteLine(type + " " + opCode + " " + payloadLength + " " + payloadTmp);//+ " " + partsTotal + " " + partsCurrent);
            Data ret = new Data() { Command = type, OpCode = opCode, Payload = new Payload(payloadTmp) };

            return ret;
        }

        private static string ArrayToString(char[] buffer, int startIndex, int length)
        {
            return new string(buffer).Substring(startIndex, length);
        }
    }
}
