using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Binary : FixedLengthCharacterSet
    {
        internal Binary() { }

        internal Binary(int n)
        {
            N = n;
        }

        protected override string TypeValue => "binary";
        public override int Size => n;
        protected override int MaxN => 8000;
    }
}
