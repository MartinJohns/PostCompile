using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostCompile.Common;

namespace PostCompile.Tests.Helpers
{
    //
    // Hierarchiy:
    // 
    //   A
    //  / \
    // B   C
    // | \ |
    // D   E
    // |
    // F
    //
    // Expected order:
    // A, B, C, D, E, F

    public class DummyTaskA : PostCompileTask
    {
        public override Task RunAsync()
        {
            throw new NotSupportedException();
        }
    }

    public class DummyTaskB : PostCompileTask
    {
        public override IEnumerable<Type> DependsOn
        {
            get { return new[] {typeof (DummyTaskA)}; }
        }

        public override Task RunAsync()
        {
            throw new NotSupportedException();
        }
    }

    public class DummyTaskC : PostCompileTask
    {
        public override IEnumerable<Type> DependsOn
        {
            get { return new[] { typeof(DummyTaskA) }; }
        }

        public override Task RunAsync()
        {
            throw new NotSupportedException();
        }
    }

    public class DummyTaskD : PostCompileTask
    {
        public override IEnumerable<Type> DependsOn
        {
            get { return new[] { typeof(DummyTaskB) }; }
        }

        public override Task RunAsync()
        {
            throw new NotSupportedException();
        }
    }
    public class DummyTaskE : PostCompileTask
    {
        public override IEnumerable<Type> DependsOn
        {
            get { return new[] { typeof(DummyTaskB), typeof(DummyTaskC) }; }
        }

        public override Task RunAsync()
        {
            throw new NotSupportedException();
        }
    }
    public class DummyTaskF : PostCompileTask
    {
        public override IEnumerable<Type> DependsOn
        {
            get { return new[] { typeof(DummyTaskD) }; }
        }

        public override Task RunAsync()
        {
            throw new NotSupportedException();
        }
    }
}
