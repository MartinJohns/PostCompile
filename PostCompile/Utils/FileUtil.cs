using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PostCompile.Common.Utils;

namespace PostCompile.Utils
{
    public class FileUtil : IFileUtil
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public string GetFileHash(string path)
        {
            return GetStringHash(ReadAllText(path));
        }

        public string GetStringHash(string contents)
        {
            using (var sha512 = SHA512.Create())
            {
                sha512.ComputeHash(Encoding.UTF8.GetBytes(contents));
                return BitConverter.ToString(sha512.Hash).Replace("-", string.Empty).ToLower();
            }
        }
    }
}