using System;
using System.Linq;
using System.Reflection;

namespace PostCompile.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasDefaultConstructor(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static string ToDisplayString(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsGenericType)
            {
                return string.Format(
                    "{0}<{1}>",
                    type.FullName.Split('`')[0].Replace("+", "."),
                    string.Join(",", type.GetGenericArguments().Select(ToDisplayString)));
            }

            switch (type.FullName)
            {
                case "System.Int16": return "short";
                case "System.Int32": return "int";
                case "System.In64": return "long";
                case "System.Boolean": return "bool";
                case "System.String": return "string";
            }

            return type.FullName.Replace("+", ".");
        }

        public static string ToDisplayString(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");
            
            return string.Format(
                "{0}.{1}({2})",
                methodInfo.DeclaringType.ToDisplayString(),
                methodInfo.Name,
                string.Join(",", methodInfo.GetParameters().Select(x => x.ParameterType.ToDisplayString())));
        }

        public static string ToDisplayString(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            return string.Format(
                "{0}.{1}",
                propertyInfo.DeclaringType.ToDisplayString(),
                propertyInfo.Name);
        }

        public static string ToDisplayString(this ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                throw new ArgumentNullException("constructorInfo");

            return string.Format(
                "{0}({1})",
                constructorInfo.DeclaringType.ToDisplayString(),
                string.Join(",", constructorInfo.GetParameters().Select(x => x.ParameterType.ToDisplayString())));
        }

        public static string ToDisplayString(this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            return string.Format(
                "{0}.{1}",
                fieldInfo.DeclaringType.ToDisplayString(),
                fieldInfo.Name);
        }

        public static string ToDisplayString(this TypeInfo typeInfo)
        {
            if (typeInfo == null)
                throw new ArgumentNullException("typeInfo");

            return typeInfo.AsType().ToDisplayString();
        }
    }
}
