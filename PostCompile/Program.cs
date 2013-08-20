using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PostCompile.Common;

namespace PostCompile
{
    public class Program
    {
        #region Constants and Fields

        private static readonly HashSet<string> Methods = new HashSet<string> { "clean", "build" };

        #endregion

        public static void Main(string[] args)
        {
            if (args.Length != 2) throw new ArgumentException("Invalid amount of arguments passed.", "args");
            if (!Methods.Contains(args[0], StringComparer.OrdinalIgnoreCase)) throw new ArgumentException("Unknown build method in first argument.", "args");
            
            var method = args[0].ToLowerInvariant();
            if (method == "clean" && !File.Exists(args[1]))
            {
                // In cleaning-mode, when the assembly file does not exist we assume it has been cleaned already.
                return;
            }
            
            if (!File.Exists(args[1])) throw new FileNotFoundException("Failed to locate assembly from second argument.", args[1]);
            
            var assemblyPath = args[1];
            var assembly = Assembly.LoadFrom(assemblyPath);
            
            var taskTypes =
                (from type in assembly.GetTypes()
                 where typeof(PostCompileTask).IsAssignableFrom(type)
                 where type.GetConstructor(Type.EmptyTypes) != null
                 select type).ToList();
            var taskInstances =
                (from taskType in taskTypes
                 let taskInstance = (PostCompileTask) Activator.CreateInstance(taskType)
                 orderby taskInstance.Order
                 select taskInstance).ToList();
            
            var utils = new ConcreteUtils();
            foreach (var taskInstance in taskInstances)
            {
                switch (method)
                {
                    case "clean":
                        taskInstance.Clean(assembly, utils);
                        break;
                    case "build":
                        taskInstance.Run(assembly, utils);
                        break;
                }
            }
        }
    }
}
