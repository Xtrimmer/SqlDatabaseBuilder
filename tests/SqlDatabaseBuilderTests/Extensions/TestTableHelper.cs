using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;

namespace Xtrimmer.SqlDatabaseBuilderTests
{
    internal static class TestTableHelper
    {
        public static bool IsTablePresentInDatabase(this Table table, SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                string tableName = table.Name;
                string sql = "SELECT object_id FROM sys.tables WHERE name = @tableName";
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.Add(new SqlParameter("tableName", tableName));
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }
    }
}
