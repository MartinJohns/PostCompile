using System.IO;
using PostCompile.Common.Utils;

namespace PostCompile.Utils
{
    public class LogUtil : ILogUtil
    {
        private readonly TextWriter _writer;

        public LogUtil(TextWriter writer)
        {
            _writer = writer;
        }

        #region Error Methods

        public void Error(string message)
        {
            _writer.WriteLine("PostCompile: error: {0}", message);
        }

        public void Error(string file, string message)
        {
            _writer.WriteLine("{0}: error: {1}", file, message);
        }

        public void Error(string file, int line, string message)
        {
            _writer.WriteLine("{0}({1}): error: {2}", file, line, message);
        }

        public void Error(string file, int line, int column, string message)
        {
            _writer.WriteLine("{0}({1},{2}): error: {3}", file, line, column, message);
        }

        #endregion

        #region Warning Methods

        public void Warning(string message)
        {
            _writer.WriteLine("PostCompile: warning: {0}", message);
        }

        public void Warning(string file, string message)
        {
            _writer.WriteLine("{0}: warning: {1}", file, message);
        }

        public void Warning(string file, int line, string message)
        {
            _writer.WriteLine("{0}({1}): warning: {2}", file, line, message);
        }

        public void Warning(string file, int line, int column, string message)
        {
            _writer.WriteLine("{0}({1},{2}): warning: {3}", file, line, column, message);
        }

        #endregion
    }
}
