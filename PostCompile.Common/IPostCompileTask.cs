using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostCompile.Common
{
    public interface IPostCompileTask
    {
        IEnumerable<Type> DependsOn { get; }

        ILog Log { set; }

        Task RunAsync();
    }
}
