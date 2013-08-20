using System;
using PostCompile.Common;
using PostCompile.Common.Utils;
using PostCompile.Utils;

namespace PostCompile
{
    public class ConcreteUtils : IUtils
    {
        public ConcreteUtils()
        {
            Log = new LogUtil(Console.Out);
            File = new FileUtil();
        }

        public ILogUtil Log { get; private set; }

        public IFileUtil File { get; private set; }
    }
}
