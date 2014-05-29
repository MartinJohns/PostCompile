using System;
using System.Collections.Generic;
using PostCompile.Extensions;
using PostCompile.Tests.Helpers;
using Xunit;

namespace PostCompile.Tests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void BuiltInTypes_ToDisplayString()
        {
            Assert.Equal("byte", typeof(byte).ToDisplayString());
            Assert.Equal("short", typeof(short).ToDisplayString());
            Assert.Equal("int", typeof(int).ToDisplayString());
            Assert.Equal("long", typeof(long).ToDisplayString());
            Assert.Equal("sbyte", typeof(sbyte).ToDisplayString());
            Assert.Equal("ushort", typeof(ushort).ToDisplayString());
            Assert.Equal("uint", typeof(uint).ToDisplayString());
            Assert.Equal("ulong", typeof(ulong).ToDisplayString());
            Assert.Equal("bool", typeof(bool).ToDisplayString());
            Assert.Equal("float", typeof(float).ToDisplayString());
            Assert.Equal("double", typeof(double).ToDisplayString());
            Assert.Equal("decimal", typeof(decimal).ToDisplayString());
            Assert.Equal("string", typeof(string).ToDisplayString());
            Assert.Equal("object", typeof(object).ToDisplayString());
            Assert.Equal("int[]", typeof(int[]).ToDisplayString());
            Assert.Equal("string[]", typeof(string[]).ToDisplayString());
        }

        [Fact]
        public void GenericTypes_ToDisplayString()
        {
            Assert.Equal("System.Collections.Generic.IEnumerable<int>", typeof(IEnumerable<int>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.IEnumerable<PostCompile.Tests.Helpers.Dummy>", typeof(IEnumerable<Dummy>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.IEnumerable<PostCompile.Tests.Helpers.Dummy.NestedDummy>", typeof(IEnumerable<Dummy.NestedDummy>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.List<string>", typeof(List<string>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.Dictionary<int,bool>", typeof(Dictionary<int, bool>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.Dictionary<int,PostCompile.Tests.Helpers.Dummy>", typeof(Dictionary<int, Dummy>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.Dictionary<PostCompile.Tests.Helpers.Dummy.NestedDummy,decimal>", typeof(Dictionary<Dummy.NestedDummy, decimal>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.Dictionary<System.Collections.Generic.IList<PostCompile.Tests.Helpers.Dummy>,System.Collections.Generic.IEnumerable<System.Collections.Generic.HashSet<bool>>>", typeof(Dictionary<IList<Dummy>, IEnumerable<HashSet<bool>>>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.IEnumerable<string[]>", typeof(IEnumerable<string[]>).ToDisplayString());
            Assert.Equal("System.Collections.Generic.IEnumerable<string>[]", typeof(IEnumerable<string>[]).ToDisplayString());
        }

        [Fact]
        public void ReflectionInfos_ToDisplayString()
        {
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method()", typeof(Dummy).GetMethod("Method", new Type[0]).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method(int)", typeof(Dummy).GetMethod("Method", new[] { typeof(int) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method(string,int)", typeof(Dummy).GetMethod("Method", new[] { typeof(string), typeof(int) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method(System.Collections.Generic.IEnumerable<string>)", typeof(Dummy).GetMethod("Method", new[] { typeof(IEnumerable<string>) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method(bool[])", typeof(Dummy).GetMethod("Method", new[] { typeof(bool[]) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method(object[])", typeof(Dummy).GetMethod("Method", new[] { typeof(object[]) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method(PostCompile.Tests.Helpers.Dummy)", typeof(Dummy).GetMethod("Method", new[] { typeof(Dummy) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.Method(PostCompile.Tests.Helpers.Dummy.NestedDummy)", typeof(Dummy).GetMethod("Method", new[] { typeof(Dummy.NestedDummy) }).ToDisplayString());

            Assert.Equal("PostCompile.Tests.Helpers.Dummy.PropertyInt", typeof(Dummy).GetProperty("PropertyInt").ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.PropertyString", typeof(Dummy).GetProperty("PropertyString").ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.NestedDummy.PropertyInt", typeof(Dummy.NestedDummy).GetProperty("PropertyInt").ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.NestedDummy.PropertyString", typeof(Dummy.NestedDummy).GetProperty("PropertyString").ToDisplayString());

            Assert.Equal("PostCompile.Tests.Helpers.Dummy._fieldInt", typeof(Dummy).GetField("_fieldInt").ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy._fieldString", typeof(Dummy).GetField("_fieldString").ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.NestedDummy._fieldInt", typeof(Dummy.NestedDummy).GetField("_fieldInt").ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.Dummy.NestedDummy._fieldString", typeof(Dummy.NestedDummy).GetField("_fieldString").ToDisplayString());

            Assert.Equal("PostCompile.Tests.Helpers.Dummy()", typeof(Dummy).GetConstructor(Type.EmptyTypes).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.DummyWithCustomConstructor(int)", typeof(DummyWithCustomConstructor).GetConstructor(new[] { typeof(int) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.DummyWithMixedConstructors()", typeof(DummyWithMixedConstructors).GetConstructor(Type.EmptyTypes).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.DummyWithMixedConstructors(int)", typeof(DummyWithMixedConstructors).GetConstructor(new[] { typeof(int) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.DummyWithMixedConstructors(string)", typeof(DummyWithMixedConstructors).GetConstructor(new[] { typeof(string) }).ToDisplayString());
            Assert.Equal("PostCompile.Tests.Helpers.DummyWithParameterlessConstructor()", typeof(DummyWithParameterlessConstructor).GetConstructor(Type.EmptyTypes).ToDisplayString());
        }

        [Fact]
        public void HasDefaultConstructor()
        {
            Assert.True(typeof(Dummy).HasDefaultConstructor());
            Assert.True(typeof(DummyWithMixedConstructors).HasDefaultConstructor());
            Assert.True(typeof(DummyWithParameterlessConstructor).HasDefaultConstructor());
            Assert.False(typeof(DummyWithCustomConstructor).HasDefaultConstructor());
        }
    }
}
