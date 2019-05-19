using System.Linq;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class ConstraintCollection : DatabaseObjectCollection<Constraint>
    {
        public override DatabaseObjectCollection<Constraint> Add(Constraint item)
        {
            item.ThrowIfNull(nameof(item));
            if (item is PrimaryKeyConstraint && ContainsPrimaryKey())
            {
                throw new MultiplePrimaryKeyException("A constraint collection can have only one PrimaryKeyConstraint");
            }
            else
            {
                list.Add(item);
            }
            return this;
        }

        private bool ContainsPrimaryKey()
        {
            return list.OfType<PrimaryKeyConstraint>().Any();
        }
    }
}
