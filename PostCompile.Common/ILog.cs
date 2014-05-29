using System;
using System.Reflection;

namespace PostCompile.Common
{
    public interface ILog
    {
        void Error(string message);

        void Error(string file, string message);

        void Error(string file, int line, string message);

        void Error(string file, int line, int column, string message);

        void Error(MethodInfo methodInfo, string message);

        void Error(PropertyInfo propertyInfo, string message);

        void Error(ConstructorInfo constructorInfo, string message);

        void Error(FieldInfo fieldInfo, string message);

        void Error(TypeInfo typeInfo, string message);

        void Error(Type type, string message);

        void Warning(string message);

        void Warning(string file, string message);

        void Warning(string file, int line, string message);

        void Warning(string file, int line, int column, string message);

        void Warning(MethodInfo methodInfo, string message);

        void Warning(PropertyInfo propertyInfo, string message);

        void Warning(ConstructorInfo constructorInfo, string message);

        void Warning(FieldInfo fieldInfo, string message);

        void Warning(TypeInfo typeInfo, string message);

        void Warning(Type type, string message);

        void UsageError(MethodInfo methodInfo, string message);

        void UsageError(PropertyInfo propertyInfo, string message);

        void UsageError(ConstructorInfo constructorInfo, string message);

        void UsageWarning(MethodInfo methodInfo, string message);

        void UsageWarning(PropertyInfo propertyInfo, string message);

        void UsageWarning(ConstructorInfo constructorInfo, string message);
    }
}
