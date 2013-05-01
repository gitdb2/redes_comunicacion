using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.Commons
{
    public class FileObject
    {
        public const string DATA_SEPARATOR = "@";
        public string Name { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
        public string Owner { get; set; }
        public string Server { get; set; }


        public string FullName { get; set; }


        public override string ToString()
        {
            return String.Format("name:{0}, hash:{1}, size: {2}, owner:{3}, fullName:{4}", Name, Hash, Size, Owner, FullName);
        }

        public string ToNetworkString()
        {
            return String.Format("{0}@{1}@{2}@{3}", Name, Hash, Size, Owner);
        }

        public static FileObject FromNetworkString(string payload)
        {
            FileObject ret = new FileObject();

            string[] arr = payload.Split(new string[] { DATA_SEPARATOR }, StringSplitOptions.None);
            if (arr.Length < 4 || arr.Length > 4)//si faltan datos o hay mas
            {
                return null;
            }
            else
            {
                ret.Name = arr[0];
                ret.Hash = arr[1];
                ret.Size = long.Parse(arr[2]);
                ret.Owner = arr[3];
            }
            return ret;

        }
    }
}
