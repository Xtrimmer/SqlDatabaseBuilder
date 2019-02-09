using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class SmallInt : Integer
    {
        public override string Definition => "smallint";
        public override int Size => 2;
        public static long Max = 32767;
        public static long Min = -32768;
    }
}
