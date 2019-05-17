namespace Xtrimmer.SqlDatabaseBuilder
{
    public abstract class Constraint : DatabaseObject
    {
        protected Constraint(string name) : base(name) { }
    }
}
