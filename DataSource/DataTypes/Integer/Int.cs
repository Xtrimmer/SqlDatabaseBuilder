using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Int : Integer
    {
        public override string Definition => "int";
        public override int Size => 4;
        public static long Max = 2147483647;
        public static long Min = -2147483648;
    }
}
