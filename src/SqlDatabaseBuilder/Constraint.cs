using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public abstract class Constraint : DatabaseObject
    {
        protected Constraint(string name) : base(name) { }
    }
}
