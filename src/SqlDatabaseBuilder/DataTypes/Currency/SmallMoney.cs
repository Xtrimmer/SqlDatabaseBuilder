using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class SmallMoney : Currency
    {
        public override string Definition { get; } = "smallmoney";
        public override int Size => 4;
        public static double Max = 2147483647 / 1000;
        public static double Min = -2147483648 / 1000;
    }
}
