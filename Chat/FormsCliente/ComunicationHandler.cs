using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using System.Net.Sockets;
using System.IO;

namespace Chat
{
    public class ComunicationHandler
    {
        public string Server { get; set; }
        public int Port { get; set; }

        public const int METADATA_OPTYPE_LENGTH = 3;
        public const int METADATA_OPCODE_LENGTH = 2;
        public const int METADATA_PAYLOAD_LENGTH = 5;
        private const int METADATA_TOTAL_LENGTH = METADATA_OPTYPE_LENGTH + METADATA_OPCODE_LENGTH + METADATA_PAYLOAD_LENGTH;

        private TcpClient TCPClient;
        private NetworkStream NetStream;

        private StreamReader StrReader;
        private StreamWriter StrWriter;

        public void SetupConnection()
        {
            try
            {
                this.TCPClient = new TcpClient(Server, Port);
                this.NetStream = TCPClient.GetStream();
                this.StrReader = new StreamReader(NetStream, Encoding.UTF8);
                this.StrWriter = new StreamWriter(NetStream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CloseConnection()
        {
            StrReader.Dispose();
            StrWriter.Dispose();
            NetStream.Close();
            TCPClient.Close();
        }

        public void SendData(Command command, int opcode, Payload payload)
        {
            Data data = new Data() { Command = Command.REQ, OpCode = opcode, Payload = payload };
            foreach (var item in data.GetBytes())
            {
                this.StrWriter.Write(item);
                this.StrWriter.Flush();
            }
        }

        public Data ReceiveData()
        {
            return LoadObject();
        }

        private Data LoadObject()
        {
            //leo la metadata de la trama
            char[] buffer = new char[10];
            int readQty = this.StrReader.Read(buffer, 0, METADATA_TOTAL_LENGTH);

            if (readQty < METADATA_TOTAL_LENGTH) 
                throw new Exception("Errror en trama largo fijo");

            Command type = (Command)Enum.Parse(typeof(Command), ArrayToString(buffer, 0, METADATA_OPTYPE_LENGTH));
            int opCode = int.Parse(ArrayToString(buffer, METADATA_OPTYPE_LENGTH, METADATA_OPCODE_LENGTH));
            int payloadLength = int.Parse(ArrayToString(buffer, METADATA_OPTYPE_LENGTH + METADATA_OPCODE_LENGTH, METADATA_PAYLOAD_LENGTH));

            //leo el payload de la trama
            buffer = new char[payloadLength];
            readQty = this.StrReader.Read(buffer, 0, payloadLength);
            
            if (readQty < payloadLength) 
                throw new Exception("Errror en trama largo fijo leyendo payload");

            return new Data() { Command = type, OpCode = opCode, Payload = new Payload(ArrayToString(buffer, 0, readQty)) };
        }

        private string ArrayToString(char[] buffer, int startIndex, int length)
        {
            return new string(buffer).Substring(startIndex, length);
        }

    }
}
