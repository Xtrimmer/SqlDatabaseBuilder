using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Date : Temporal
    {
        internal Date() { }

        public override string Definition => "date";

        public override int Size => 3;
    }
}
