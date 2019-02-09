using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class TinyInt : Integer
    {
        public override string Definition => "tinyint";
        public override int Size => 1;
        public static long Max = 255;
        public static long Min = 0;
    }
}
