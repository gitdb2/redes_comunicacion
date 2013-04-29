using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;
using System.IO;
using System.Security.Cryptography;

namespace uy.edu.ort.obligatorio.ContentServer
{
    public class FileOperationsSingleton
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Singleton
        private static FileOperationsSingleton instance = new FileOperationsSingleton();
        private FileOperationsSingleton()
        {
        }
        public static FileOperationsSingleton GetInstance()
        {
            return instance;
        }
        #endregion

       /// <summary>
       /// Crea el espacio compartido de un usuario en el filesystem
       /// </summary>
       /// <param name="login"></param>
       /// <returns></returns>
        public bool CreateDiskSharedSpace(string login)
        {
            string dir = Settings.GetInstance().GetProperty("base.shared.dir.path", @"c:/shared") + "/" + login;
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }

            }
            return true;
        }

        private string BasePath()
        {
            return Settings.GetInstance().GetProperty("base.shared.dir.path", @"c:\shared");
        }

        public FileInfo GetFile(string hash)
        {
            return GetFile(hash, "");
        }

        public FileInfo GetFile(string hash, string owner)
        {
            FileInfo ret = null;


            return ret;

        }

        private FileObject SearchFilesByHash(string hash, string owner)
        {
            FileObject ret = null;

            try
            {
                //  string[] filePaths = Directory.GetFiles(@"c:\MyDir\", "*.bmp");
                string basePath = BasePath();

                if (owner.Length > 0)
                {
                    basePath += @"\" + owner;
                }

                string[] filePaths = Directory.GetFiles(basePath, "*", SearchOption.AllDirectories);


                foreach (var filename in filePaths)
                {
                    FileObject tmp = CreateFileObject(filename);
                    if (tmp.Hash.Equals(hash))
                    {
                        ret = tmp;
                        break;
                    }

                }
            }
            catch (Exception)
            {

                ret = null;
            }
            return ret;
        }


        public List<FileObject> SearchFilesMatching(string pattern)
        {
            List<FileObject> ret = new List<FileObject>();

          //  string[] filePaths = Directory.GetFiles(@"c:\MyDir\", "*.bmp");
            string basePath = BasePath();
            string[] filePaths = Directory.GetFiles(basePath, pattern, SearchOption.AllDirectories);


            foreach (var filename in filePaths)
            {
                ret.Add(CreateFileObject(filename));                
            }
            return ret;
        }

        private FileObject CreateFileObject(string fileName)
        {
           using (var md5 = MD5.Create())
            {
                FileInfo fi = new FileInfo(fileName);
                using (var stream = fi.OpenRead())
                {
                   return  new FileObject()
                   {
                       Name= fi.Name, 
                       Hash=BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower(), 
                       Size= fi.Length, 
                       Owner=fi.Directory.Name
                   };
                 }
            }
        }


        private string CalculateMD5(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
        }
        

    }
}
