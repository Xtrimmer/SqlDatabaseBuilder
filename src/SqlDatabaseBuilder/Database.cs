using System;
using System.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Database : DatabaseResource
    {
        public Database(string databaseName) : base(databaseName)
        {
            if (databaseName == null) throw new InvalidDatabaseIdentifierException("Database name cannot be null");
        }

        public override void Create(SqlConnection sqlConnection)
        {
            sqlConnection.ThrowIfNull(nameof(sqlConnection));

            string sql = $"CREATE {SqlDefinition}";

            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }

        public override void Drop(SqlConnection sqlConnection)
        {
            sqlConnection.ThrowIfNull(nameof(sqlConnection));

            string sql = $"DROP {SqlDefinition}";

            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }

        internal override string SqlDefinition
        {
            get
            {
                return $"DATABASE [{Name}]";
            }
        }
    }
}
