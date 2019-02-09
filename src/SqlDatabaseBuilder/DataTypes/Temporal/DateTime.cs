using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    class DateTime : Temporal
    {
        internal DateTime() { }

        public override string Definition => "datetime";

        public override int Size => 8;
    }
}
