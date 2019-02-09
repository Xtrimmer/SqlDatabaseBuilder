using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class BigInt : Integer
    {
        public override string Definition { get; } = "bigint";
        public override int Size => 8;
        public static long Max = 9223372036854775807;
        public static long Min = -9223372036854775808;
    }
}
