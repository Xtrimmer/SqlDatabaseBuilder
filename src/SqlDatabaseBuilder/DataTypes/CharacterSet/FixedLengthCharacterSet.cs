using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    abstract class FixedLengthCharacterSet : CharacterSet
    {
        protected override string NSpecification => n == 1 ? "" : $"({n.ToString()})";
    }
}
