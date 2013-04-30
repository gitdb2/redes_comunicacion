using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace uy.edu.ort.obligatorio.Commons
{
    public class StringUtils
    {

        static public string CalculateMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(input)))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
        }
    }
}
