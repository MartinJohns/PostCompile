using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PostCompile.Common;

namespace TypeScriptGenerator.Data
{
    public class TypeScriptGeneratorTask : PostCompileTask
    {
        private const string TargetFile = @"..\TypeScriptGenerator.Web\Scripts\Data.ts";

        private readonly StringBuilder _sb = new StringBuilder();

        public override Task RunAsync()
        {
            var types =
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where type.Namespace == "TypeScriptGenerator.Data.Models"
                select type;
            foreach (var type in types)
            {
                GenerateType(type);
            }

            File.WriteAllText(TargetFile, _sb.ToString());

            return Task.FromResult(0);
        }

        private void GenerateType(Type type)
        {
            _sb.AppendLine(string.Format("export interface I{0} {{", type.Name));

            foreach (var property in type.GetProperties())
            {
                _sb.AppendLine(string.Format("    {0}: {1};", ToCamelCase(property.Name), GetTypeScriptType(property.PropertyType)));
            }

            _sb.AppendLine("}");
        }

        private string GetTypeScriptType(Type type)
        {
            if (type.Namespace == "TypeScriptGenerator.Data.Models")
                return "I" + type.Name;

            switch (type.FullName)
            {
                case "System.Int32": return "number";
                case "System.String": return "string";
                case "System.Boolean": return "boolean";
                default: throw new ArgumentOutOfRangeException("type", "Unknown typescript type.");
            }
        }

        private string ToCamelCase(string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
    }
}
