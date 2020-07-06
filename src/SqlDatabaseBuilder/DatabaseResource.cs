﻿using Microsoft.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public abstract class DatabaseResource : DatabaseObject, ICreatable, IDroppable
    {
        protected DatabaseResource(string resourceName) : base(resourceName)
        {
        }

        public abstract void Create(SqlConnection sqlConnection);
        public abstract void Drop(SqlConnection sqlConnection);
    }
}
