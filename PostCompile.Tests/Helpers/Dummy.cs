using System.Collections.Generic;

namespace PostCompile.Tests.Helpers
{
    public class Dummy
    {
        public void Method()
        {
        }

        public void Method(int i)
        {
        }

        public void Method(string s, int i)
        {
        }

        public void Method(IEnumerable<string> s)
        {
        }

        public void Method(bool[] b)
        {
        }

        public void Method(params object[] o)
        {
        }

        public void Method(Dummy d)
        {
        }

        public void Method(NestedDummy nd)
        {
        }

        public int PropertyInt { get; set; }

        public int PropertyString { get; set; }

        public int _fieldInt;

        public string _fieldString;

        public class NestedDummy
        {
            public int PropertyInt { get; set; }

            public int PropertyString { get; set; }

            public int _fieldInt;

            public string _fieldString;
        }
    }
}
