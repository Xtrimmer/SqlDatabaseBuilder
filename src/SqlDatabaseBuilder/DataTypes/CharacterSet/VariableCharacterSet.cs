using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    abstract class VariableCharacterSet : CharacterSet
    {        
        protected const int MAX_SIZE = 2147483647;
        protected bool isMax = false;
        protected override string NSpecification => isMax ? "(max)" : (n == 1 ? "" : $"({n.ToString()})");
    }
}
