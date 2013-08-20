using System.Text;

namespace PostCompile.Common.Utils
{
    public interface IFileUtil
    {
        bool Exists(string path);

        string ReadAllText(string path);
        
        string ReadAllText(string path, Encoding encoding);
        
        void WriteAllText(string path, string contents);
        
        void WriteAllText(string path, string contents, Encoding encoding);

        void Delete(string path);

        string GetFileHash(string path);

        string GetStringHash(string contents);
    }
}
