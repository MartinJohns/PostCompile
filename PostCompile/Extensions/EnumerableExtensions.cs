using System;
using System.Collections.Generic;

namespace PostCompile.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> TopologicalSort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle = false)
            where T : class
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dependencies == null)
                throw new ArgumentNullException("dependencies");

            var sorted = new List<T>();
            var visited = new HashSet<T>();

            foreach (var item in source)
                Visit(item, visited, sorted, dependencies, throwOnCycle);

            return sorted;
        }

        private static void Visit<T>(T item, HashSet<T> visited, List<T> sorted, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle)
            where T : class
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (visited == null)
                throw new ArgumentNullException("visited");
            if (sorted == null)
                throw new ArgumentNullException("sorted");
            if (dependencies == null)
                throw new ArgumentNullException("dependencies");

            if (!visited.Contains(item))
            {
                visited.Add(item);

                foreach (var dep in dependencies(item))
                    Visit(dep, visited, sorted, dependencies, throwOnCycle);

                sorted.Add(item);
            }
            else
            {
                if (throwOnCycle)
                    throw new Exception("Cyclic dependency found");
            }
        }
    }
}
