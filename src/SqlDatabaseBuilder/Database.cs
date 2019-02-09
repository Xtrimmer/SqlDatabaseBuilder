using System;
using System.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Database : DatabaseResource
    {
        public Database(string databaseName) : base(databaseName) { }

        public override void Create(SqlConnection sqlConnection)
        {
            sqlConnection.ThrowIfNull(nameof(sqlConnection));

            string sql = $"CREATE DATABASE [{Name}]";

            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }

        public override void Drop(SqlConnection sqlConnection)
        {
            sqlConnection.ThrowIfNull(nameof(sqlConnection));

            string sql = $"DROP DATABASE [{Name}]";

            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
