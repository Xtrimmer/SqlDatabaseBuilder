using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Money : Currency
    {
        public override string Definition { get; } = "money";
        public override int Size => 8;
        public static double Max = 9223372036854775807 / 1000;
        public static double Min = -9223372036854775808 / 1000;
    }
}
