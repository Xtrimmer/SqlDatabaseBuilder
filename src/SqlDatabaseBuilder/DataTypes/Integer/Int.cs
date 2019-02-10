using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Int : Integer
    {
        public override string Definition => "int";
        public override int Size => 4;
    }
}
