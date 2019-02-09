
using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Char : FixedLengthCharacterSet
    {
        internal Char() { }

        internal Char(int n)
        {
            this.n = n;
        }

        protected override int MaxN => 8000;
        protected override string TypeValue => "char";
        public override int Size => n;
    }
}
