using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class UniqueIdentifier : DataType
    {
        internal UniqueIdentifier() { }

        public override string Definition => "uniqueidentifier";

        public override int Size => 16;
    }
}
