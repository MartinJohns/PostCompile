using System;
using System.Collections.Generic;

namespace PostCompile
{
    [Serializable]
    public class TaskRunnerResult
    {
        public IEnumerable<string> TaskTypes { get; set; }
    }
}
