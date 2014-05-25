using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostCompile.Common
{
    public abstract class PostCompileTask : IPostCompileTask
    {
        public virtual IEnumerable<Type> DependsOn
        {
            get { return Enumerable.Empty<Type>(); }
        }

        public ILog Log { set; get; }

        public abstract Task RunAsync();
    }
}
