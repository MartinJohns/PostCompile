using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using PostCompile.Extensions;
using PostCompile.Tests.Helpers;
using Xunit;

namespace PostCompile.Tests
{
    public class RoslynExtensionsTests
    {
        private readonly Solution _solution;

        private readonly Project _testProject;

        private readonly Compilation _testProjectCompilation;

        public RoslynExtensionsTests()
        {
            var workspace = MSBuildWorkspace.Create();
            _solution = workspace.OpenSolutionAsync(@"..\..\..\PostCompile.sln").Result;
            _testProject = _solution.Projects.First(x => x.Name == "PostCompile.Tests");
            _testProjectCompilation = _testProject.GetCompilationAsync().Result;
        }


        [Fact]
        public void GetTypeSymbols()
        {
            var typeSymbols = _testProjectCompilation.GlobalNamespace.GetTypeSymbols();
            var hashSet = new HashSet<string>(typeSymbols.Select(x => x.ToDisplayString()));

            Assert.Contains("PostCompile.Tests.Helpers.Dummy", hashSet);
            Assert.Contains("PostCompile.Tests.Helpers.Dummy.NestedDummy", hashSet);
            Assert.Contains("PostCompile.Tests.Helpers.DummyTaskA", hashSet);
            Assert.Contains("PostCompile.Tests.Helpers.DummyTaskB", hashSet);
            Assert.Contains("PostCompile.Tests.Helpers.DummyTaskC", hashSet);
            Assert.Contains("PostCompile.Tests.Helpers.DummyTaskD", hashSet);
            Assert.Contains("PostCompile.Tests.Helpers.DummyTaskE", hashSet);
        }

        [Fact]
        public void GetSymbols()
        {
            Assert.NotNull(_solution.GetSymbol(typeof(Dummy)));
            Assert.NotNull(_solution.GetSymbol(typeof(Dummy.NestedDummy)));
            Assert.NotNull(_solution.GetSymbol(typeof(Dummy.NestedDummy.DeeplyNested)));
            Assert.NotNull(_solution.GetSymbol(typeof(DummyTaskA)));

            Assert.NotNull(typeof(Dummy).GetMethod("Method", new Type[0]).ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetMethod("Method", new[] { typeof(int) }).ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetMethod("Method", new[] { typeof(string), typeof(int) }).ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetMethod("Method", new[] { typeof(IEnumerable<string>) }).ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetMethod("Method", new[] { typeof(bool[]) }).ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetMethod("Method", new[] { typeof(object[]) }).ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetMethod("Method", new[] { typeof(Dummy) }).ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetMethod("Method", new[] { typeof(Dummy.NestedDummy) }).ToDisplayString());

            Assert.NotNull(typeof(Dummy).GetProperty("PropertyInt").ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetProperty("PropertyString").ToDisplayString());
            Assert.NotNull(typeof(Dummy.NestedDummy).GetProperty("PropertyInt").ToDisplayString());
            Assert.NotNull(typeof(Dummy.NestedDummy).GetProperty("PropertyString").ToDisplayString());

            Assert.NotNull(typeof(Dummy).GetField("_fieldInt").ToDisplayString());
            Assert.NotNull(typeof(Dummy).GetField("_fieldString").ToDisplayString());
            Assert.NotNull(typeof(Dummy.NestedDummy).GetField("_fieldInt").ToDisplayString());
            Assert.NotNull(typeof(Dummy.NestedDummy).GetField("_fieldString").ToDisplayString());

            Assert.NotNull(typeof(Dummy).GetConstructor(Type.EmptyTypes).ToDisplayString());
            Assert.NotNull(typeof(DummyWithCustomConstructor).GetConstructor(new[] { typeof(int) }).ToDisplayString());
            Assert.NotNull(typeof(DummyWithMixedConstructors).GetConstructor(Type.EmptyTypes).ToDisplayString());
            Assert.NotNull(typeof(DummyWithMixedConstructors).GetConstructor(new[] { typeof(int) }).ToDisplayString());
            Assert.NotNull(typeof(DummyWithMixedConstructors).GetConstructor(new[] { typeof(string) }).ToDisplayString());
            Assert.NotNull(typeof(DummyWithParameterlessConstructor).GetConstructor(Type.EmptyTypes).ToDisplayString());
        }
    }
}
