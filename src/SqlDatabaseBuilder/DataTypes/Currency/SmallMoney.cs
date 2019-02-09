using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class SmallMoney : Currency
    {
        public override string Definition { get; } = "smallmoney";
        public override int Size => 4;
    }
}
