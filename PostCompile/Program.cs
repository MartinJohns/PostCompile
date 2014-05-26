using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace PostCompile
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                    throw new ArgumentException("Invalid of amount arguments passed.", "args");

                var assemblyPath = args[0];
                var solutionPath = args[1];

                if (!File.Exists(assemblyPath))
                    throw new FileNotFoundException("Failed to locate assembly.");

                if (!File.Exists(solutionPath))
                    throw new FileNotFoundException("Failed to locate solution.");

                TaskRunnerResult result;
                using (var isolated = new Isolated<TaskRunner>())
                {
                    result = isolated.Value.Execute(assemblyPath, solutionPath);
                }

                var module = ModuleDefinition.ReadModule(assemblyPath);
                var types = module.Types.Where(x => result.TaskTypes.Contains(x.FullName)).ToList();
                foreach (var t in types)
                {
                    module.Types.Remove(t);
                }

                module.Write(assemblyPath);
                File.Delete(Path.Combine(Path.GetDirectoryName(assemblyPath), "PostCompile.Common.dll"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical error: " + ex.Message);
            }
        }
    }
}
