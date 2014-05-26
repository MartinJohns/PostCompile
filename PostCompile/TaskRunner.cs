using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.MSBuild;
using PostCompile.Common;
using PostCompile.Extensions;

namespace PostCompile
{
    public class TaskRunner : MarshalByRefObject
    {
        public TaskRunnerResult Execute(string assemblyPath, string solutionPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);

            var tasks =
                (from type in assembly.GetTypes()
                 where typeof(IPostCompileTask).IsAssignableFrom(type)
                 where !type.IsAbstract
                 where type.HasDefaultConstructor()
                 select type).ToDictionary(x => x, x => (IPostCompileTask)Activator.CreateInstance(x));
            CheckTaskDependencies(tasks);

            var sortedTaskInstances = tasks.Values.TopologicalSort(x => x.DependsOn.Select(y => tasks[y])).ToList();

            var workspace = MSBuildWorkspace.Create();
            var solution = workspace.OpenSolutionAsync(solutionPath).Result;
            var log = new ConcreteLog(solution, Console.Out);

            foreach (var taskInstance in sortedTaskInstances)
            {
                taskInstance.Log = log;
                taskInstance.RunAsync().Wait();
            }

            // Return task types that need to be removed
            return new TaskRunnerResult
            {
                TaskTypes = tasks.Select(x => x.Key.FullName).ToList()
            };
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
