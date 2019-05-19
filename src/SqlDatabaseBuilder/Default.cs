using System;
namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Default : DatabaseObject
    {
        private object value;

        public Default(object value) : this(null, value) { }

        public Default(string name, object value) : base(name)
        {
            this.value = value;
        }

        internal override string SqlDefinition
        {
            get
            {
                string defaultValue = value == null ? "" : $" DEFAULT {value}";
                string defaultName = Name != null && value != null ? $" CONSTRAINT {Name}" : "";
                return $"{defaultName}{defaultValue}";
            }
        }
    }
}
