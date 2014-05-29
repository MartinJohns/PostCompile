using System;
using System.Linq;
using System.Text;
using PostCompile.Common;
using PostCompile.Extensions;
using PostCompile.Tests.Helpers;
using Xunit;

namespace PostCompile.Tests
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void TopologicalSort_Tasks()
        {
            var types = new[] { typeof(DummyTaskA), typeof(DummyTaskB), typeof(DummyTaskC), typeof(DummyTaskD), typeof(DummyTaskE), typeof(DummyTaskF) };
            var tasks = types.ToDictionary(x => x, x => (IPostCompileTask)Activator.CreateInstance(x));

            var sortedTasks = tasks.Values.TopologicalSort(x => x.DependsOn.Select(y => tasks[y]));
            var sortedTasksStr = sortedTasks.Aggregate(new StringBuilder(), (ag, n) => ag.Append(n.GetType().Name.Last()), ag => ag.ToString());

            Assert.Equal("ABCDEF", sortedTasksStr);
        }
    }
}
