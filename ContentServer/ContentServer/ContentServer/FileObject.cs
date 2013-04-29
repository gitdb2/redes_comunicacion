using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class FileObject
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
        public string Owner { get; set; }

        public string FullName { get; set; }

        public override string ToString()
        {
            return String.Format("name:{0}, hash:{1}, size: {2}, owner:{3}, fullName:{4}", Name, Hash, Size, Owner, FullName);
        }
    }
}
