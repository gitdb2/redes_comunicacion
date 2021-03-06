﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comunicacion
{
    public class ConversionUtil
    {
        public static char[] GetBytes(string str)
        {
           
            return  str.ToCharArray();//Encoding.UTF8.GetBytes(str);
            //byte[] bytes = new byte[str.Length * sizeof(char)];
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            //return bytes;
        }
        public static byte[] GetBytes2(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string GetString(byte[] bytes)
        {
            return  Encoding.UTF8.GetString(bytes);
        }

        public static string GetString(char[] bytes)
        {
            return new string(bytes);
         //  return  Encoding.UTF8.GetString(bytes);

            //char[] chars = new char[bytes.Length / sizeof(char)];
            //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            //return new string(chars);
        }
        public static string GetString(char[] bytes, int bytesread)
        {
            return new string(bytes).Substring(0, bytesread);
            //return //Encoding.UTF8.GetString(bytes, 0, bytesread);

            //char[] chars = new char[bytes.Length / sizeof(char)];
            //System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            //return new string(chars);
        }
        public static string GetString(byte[] bytes, int bytesread)
        {

            return Encoding.UTF8.GetString(bytes, 0, bytesread);
        }
    }
}
