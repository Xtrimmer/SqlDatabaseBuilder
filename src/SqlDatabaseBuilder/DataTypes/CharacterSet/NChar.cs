using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class NChar : FixedLengthCharacterSet
    {
        internal NChar() { }

        internal NChar(int n)
        {
            this.n = n;
        }

        protected override int MaxN => 4000;
        protected override string TypeValue => "nchar";
        public override int Size => 2 * n;
    }
}
