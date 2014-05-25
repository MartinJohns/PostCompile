using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.MSBuild;
using PostCompile.Common;
using PostCompile.Extensions;

namespace PostCompile
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Critical error: " + ex.InnerException.Message);
            }
        }

        public static async Task MainAsync(string[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException("Invalid of amount arguments passed.", "args");

            var assemblyPath = args[0];
            var solutionPath = args[1];

            if (!File.Exists(assemblyPath))
                throw new FileNotFoundException("Failed to locate assembly.");

            if (!File.Exists(solutionPath))
                throw new FileNotFoundException("Failed to locate solution.");

            var assembly = Assembly.LoadFrom(assemblyPath);

            var tasks =
                (from type in assembly.GetTypes()
                 where typeof (IPostCompileTask).IsAssignableFrom(type)
                 where !type.IsAbstract
                 where type.HasDefaultConstructor()
                 select type).ToDictionary(x => x, x => (IPostCompileTask)Activator.CreateInstance(x));
            CheckTaskDependencies(tasks);

            var sortedTaskInstances = tasks.Values.TopologicalSort(x => x.DependsOn.Select(y => tasks[y])).ToList();


            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(solutionPath);
            var log = new ConcreteLog(solution, Console.Out);

            foreach (var taskInstance in sortedTaskInstances)
            {
                taskInstance.Log = log;
                await taskInstance.RunAsync();
            }
        }

        private static void CheckTaskDependencies(Dictionary<Type, IPostCompileTask> tasks)
        {
            var invalidDependencies =
                (from task in tasks
                 from dependency in task.Value.DependsOn
                 where !typeof(IPostCompileTask).IsAssignableFrom(dependency)
                 select new { Dependent = task.Key, Dependency = dependency }).ToList();
            foreach (var id in invalidDependencies)
            {
                Error("Invalid dependency '{0}' for task '{1}'.", id.Dependency.FullName, id.Dependent.FullName);
            }

            var missingDependencies =
                (from task in tasks
                 from dependency in task.Value.DependsOn
                 where !tasks.ContainsKey(task.Key)
                 where typeof(IPostCompileTask).IsAssignableFrom(dependency)
                 select new { Dependent = task.Key, Dependency = dependency }).ToList();

            foreach (var md in missingDependencies)
            {
                Error("Missing dependency '{0}' for task '{1}'.", md.Dependency.FullName, md.Dependent.FullName);
            }

            if (invalidDependencies.Any() || missingDependencies.Any())
            {
                throw new Exception("Aborted to invalid or missing dependencies.");
            }
        }

        private static void Error(string format, params object[] args)
        {
            Console.WriteLine("Error: " + format, args);
        }
    }
}
