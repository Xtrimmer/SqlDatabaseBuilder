using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class ConstraintCollection : DatabaseObjectCollection<Constraint>
    { 
        public override DatabaseObjectCollection<Constraint> Add(Constraint constraint)
        {
            constraint.ThrowIfNull(nameof(constraint));
            if (constraint is PrimaryKeyConstraint && ContainsPrimaryKey())
            {
                throw new MultiplePrimaryKeyException("A constraint collection can have only one PrimaryKeyConstraint");
            }
            else
            {
                list.Add(constraint);
            }
            return this;
        }

        private bool ContainsPrimaryKey()
        {
            return list.OfType<PrimaryKeyConstraint>().Any();
        }
    }
}
