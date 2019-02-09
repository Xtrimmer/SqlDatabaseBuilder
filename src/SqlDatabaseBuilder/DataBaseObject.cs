using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public abstract class DatabaseObject
    {
        private DatabaseIdentifier databaseIdentifier;

        public string Name
        {
            get => databaseIdentifier.Name;
            set => databaseIdentifier = new DatabaseIdentifier(value);
        }

        protected DatabaseObject(string name)
        {
            Name = name;
        }

        internal abstract string SqlDefinition {get;}
    }
}
