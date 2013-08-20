using PostCompile.Common.Utils;

namespace PostCompile.Common
{
    public interface IUtils
    {
        ILogUtil Log { get; }

        IFileUtil File { get; }
    }
}
