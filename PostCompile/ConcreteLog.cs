using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using PostCompile.Common;
using PostCompile.Extensions;
using TypeInfo = System.Reflection.TypeInfo;

namespace PostCompile
{
    public class ConcreteLog : ILog
    {
        private readonly Solution _solution;
        private readonly TextWriter _writer;

        public ConcreteLog(Solution solution, TextWriter writer)
        {
            if (solution == null)
                throw new ArgumentNullException("solution");
            if (writer == null)
                throw new ArgumentNullException("writer");

            _solution = solution;
            _writer = writer;
        }

        #region Error Interface

        public void Error(string message)
        {
            _writer.WriteLine("PostCompile: error: {0}", message);
        }

        public void Error(string file, string message)
        {
            _writer.WriteLine("{0}: error: {1}", file, message);
        }

        public void Error(string file, int line, string message)
        {
            _writer.WriteLine("{0}({1}): error: {2}", file, line, message);
        }

        public void Error(string file, int line, int column, string message)
        {
            _writer.WriteLine("{0}({1},{2}): error: {3}", file, line, column, message);
        }

        public void Error(MethodInfo methodInfo, string message)
        {
            var symbol = _solution.GetSymbol(methodInfo);
            if (symbol == null)
            {
                Error(string.Format("{0}: Method {1}", methodInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for method '{0}'.", methodInfo));
            }
            else
            {
                Error(symbol, message);
            }
        }

        public void Error(PropertyInfo propertyInfo, string message)
        {
            var symbol = _solution.GetSymbol(propertyInfo);
            if (symbol == null)
            {
                Error(string.Format("{0}: Property {1}", propertyInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for property '{0}'.", propertyInfo));
            }
            else
            {
                Error(symbol, message);
            }
        }

        public void Error(ConstructorInfo constructorInfo, string message)
        {
            var symbol = _solution.GetSymbol(constructorInfo);
            if (symbol == null)
            {
                Error(string.Format("{0}: Constructor {1}", constructorInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for constructor '{0}'.", constructorInfo));
            }
            else
            {
                Error(symbol, message);
            }
        }

        public void Error(FieldInfo fieldInfo, string message)
        {
            var symbol = _solution.GetSymbol(fieldInfo);
            if (symbol == null)
            {
                Error(string.Format("{0}: Field {1}", fieldInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for field '{0}'.", fieldInfo));
            }
            else
            {
                Error(symbol, message);
            }
        }

        public void Error(TypeInfo typeInfo, string message)
        {
            Error(typeInfo.AsType(), message);
        }

        public void Error(Type type, string message)
        {
            var symbol = _solution.GetSymbol(type);
            if (symbol == null)
            {
                Error(string.Format("{0}: Type {1}", type.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for type '{0}'.", type));
            }
            else
            {
                Error(symbol, message);
            }
        }

        #endregion

        #region Error Private

        private void Error(ISymbol symbol, string message)
        {
            var location = symbol.Locations.FirstOrDefault();
            if (location == null)
                throw new Exception("Unexpected: Symbol contains no location.");

            Error(location.GetLineSpan(), message);
        }

        private void Error(SymbolCallerInfo callerInfo, string message)
        {
            foreach (var location in callerInfo.Locations.Reverse())
            {
                Error(location.GetLineSpan(), message);
            }
        }

        private void Error(FileLinePositionSpan lineSpan, string message)
        {
            Error(lineSpan.Path, lineSpan.StartLinePosition.Line + 1, lineSpan.StartLinePosition.Character + 1, message);
        }

        #endregion

        #region Warning Interface 

        public void Warning(string message)
        {
            _writer.WriteLine("PostCompile: warning: {0}", message);
        }

        public void Warning(string file, string message)
        {
            _writer.WriteLine("{0}: warning: {1}", file, message);
        }

        public void Warning(string file, int line, string message)
        {
            _writer.WriteLine("{0}({1}): warning: {2}", file, line, message);
        }

        public void Warning(string file, int line, int column, string message)
        {
            _writer.WriteLine("{0}({1},{2}): warning: {3}", file, line, column, message);
        }

        public void Warning(MethodInfo methodInfo, string message)
        {
            var symbol = _solution.GetSymbol(methodInfo);
            if (symbol == null)
            {
                Warning(string.Format("{0}: Method {1}", methodInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for method '{0}'.", methodInfo));
            }
            else
            {
                Warning(symbol, message);
            }
        }

        public void Warning(PropertyInfo propertyInfo, string message)
        {
            var symbol = _solution.GetSymbol(propertyInfo);
            if (symbol == null)
            {
                Warning(string.Format("{0}: Property {1}", propertyInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for property '{0}'.", propertyInfo));
            }
            else
            {
                Warning(symbol, message);
            }
        }

        public void Warning(ConstructorInfo constructorInfo, string message)
        {
            var symbol = _solution.GetSymbol(constructorInfo);
            if (symbol == null)
            {
                Warning(string.Format("{0}: Constructor {1}", constructorInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for constructor '{0}'.", constructorInfo));
            }
            else
            {
                Warning(symbol, message);
            }
        }

        public void Warning(FieldInfo fieldInfo, string message)
        {
            var symbol = _solution.GetSymbol(fieldInfo);
            if (symbol == null)
            {
                Warning(string.Format("{0}: Field {1}", fieldInfo.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for field '{0}'.", fieldInfo));
            }
            else
            {
                Warning(symbol, message);
            }
        }

        public void Warning(TypeInfo typeInfo, string message)
        {
            Warning(typeInfo.AsType(), message);
        }

        public void Warning(Type type, string message)
        {
            var symbol = _solution.GetSymbol(type);
            if (symbol == null)
            {
                Warning(string.Format("{0}: Type {1}", type.ToDisplayString(), message));
                Warning(string.Format("Failed to locate symbol for type '{0}'.", type));
            }
            else
            {
                Warning(symbol, message);
            }
        }

        #endregion

        #region Warning Private

        private void Warning(ISymbol symbol, string message)
        {
            var location = symbol.Locations.FirstOrDefault();
            if (location == null)
                throw new Exception("Unexpected: Symbol contains no location.");

            Warning(location.GetLineSpan(), message);
        }

        private void Warning(SymbolCallerInfo callerInfo, string message)
        {
            foreach (var location in callerInfo.Locations.Reverse())
            {
                Warning(location.GetLineSpan(), message);
            }
        }

        private void Warning(FileLinePositionSpan lineSpan, string message)
        {
            Warning(lineSpan.Path, lineSpan.StartLinePosition.Line + 1, lineSpan.StartLinePosition.Character + 1, message);
        }

        #endregion

        #region Usage Error Interface

        public void UsageError(MethodInfo methodInfo, string message)
        {
            var symbol = _solution.GetSymbol(methodInfo);
            if (symbol == null)
            {
                Warning(string.Format("Failed to locate symbol for method '{0}'.", methodInfo));
            }
            else
            {
                var callers = SymbolFinder.FindCallersAsync(symbol, _solution).Result;
                foreach (var caller in callers)
                {
                    Error(caller, message);
                }
            }
        }

        public void UsageError(PropertyInfo propertyInfo, string message)
        {
            var symbol = _solution.GetSymbol(propertyInfo);
            if (symbol == null)
            {
                Warning(string.Format("Failed to locate symbol for property '{0}'.", propertyInfo));
            }
            else
            {
                var callers = SymbolFinder.FindCallersAsync(symbol, _solution).Result;
                foreach (var caller in callers)
                {
                    Error(caller, message);
                }
            }
        }

        public void UsageError(ConstructorInfo constructorInfo, string message)
        {
            var symbol = _solution.GetSymbol(constructorInfo);
            if (symbol == null)
            {
                Warning(string.Format("Failed to locate symbol for constructor '{0}'.", constructorInfo));
            }
            else
            {
                var callers = SymbolFinder.FindCallersAsync(symbol, _solution).Result;
                foreach (var caller in callers)
                {
                    Error(caller, message);
                }
            }
        }

        #endregion

        #region Usage Warning Interface

        public void UsageWarning(MethodInfo methodInfo, string message)
        {
            var symbol = _solution.GetSymbol(methodInfo);
            if (symbol == null)
            {
                Warning(string.Format("Failed to locate symbol for method '{0}'.", methodInfo));
            }
            else
            {
                var callers = SymbolFinder.FindCallersAsync(symbol, _solution).Result;
                foreach (var caller in callers)
                {
                    Warning(caller, message);
                }
            }
        }

        public void UsageWarning(PropertyInfo propertyInfo, string message)
        {
            var symbol = _solution.GetSymbol(propertyInfo);
            if (symbol == null)
            {
                Warning(string.Format("Failed to locate symbol for property '{0}'.", propertyInfo));
            }
            else
            {
                var callers = SymbolFinder.FindCallersAsync(symbol, _solution).Result;
                foreach (var caller in callers)
                {
                    Warning(caller, message);
                }
            }
        }

        public void UsageWarning(ConstructorInfo constructorInfo, string message)
        {
            var symbol = _solution.GetSymbol(constructorInfo);
            if (symbol == null)
            {
                Warning(string.Format("Failed to locate symbol for constructor '{0}'.", constructorInfo));
            }
            else
            {
                var callers = SymbolFinder.FindCallersAsync(symbol, _solution).Result;
                foreach (var caller in callers)
                {
                    Warning(caller, message);
                }
            }
        }

        #endregion
    }
}
