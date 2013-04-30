using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.Commons
{
    public class ServerInfo
    {
        public const string DELIMITER = "@";
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }

        public string ToNetworkString()
        {
            return Name + DELIMITER + Ip + DELIMITER + Port;
        }

        private const int PARTS = 3;
        public static ServerInfo Parse(string item)
        {
            if(item ==null){
                throw new  System.ArgumentNullException();
            }
            string[] payload = item.Split(new string[] { DELIMITER }, StringSplitOptions.None);
            if (payload.Length == PARTS)
            {
                return new ServerInfo() { Name = payload[0], Ip = payload[1], Port = int.Parse(payload[2]) };
            }else{
                throw new System.FormatException("el elemento no tiene exactamente " + PARTS + " partes");
            }
            
        }
    }
}
