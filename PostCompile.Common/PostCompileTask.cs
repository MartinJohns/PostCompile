using System.Reflection;

namespace PostCompile.Common
{
    public abstract class PostCompileTask
    {
        public virtual int Order { get { return 0; } }

        public abstract void Clean(Assembly assembly, IUtils utils);

        public abstract void Run(Assembly assembly, IUtils utils);
    }
}
