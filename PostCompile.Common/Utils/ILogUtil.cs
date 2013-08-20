
namespace PostCompile.Common.Utils
{
    public interface ILogUtil
    {
        #region Error Methods

        void Error(string message);

        void Error(string file, string message);

        void Error(string file, int line, string message);

        void Error(string file, int line, int column, string message);

        #endregion

        #region Warning Methods

        void Warning(string message);

        void Warning(string file, string message);

        void Warning(string file, int line, string message);

        void Warning(string file, int line, int column, string message);

        #endregion
    }
}
