using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class TableShould
    {
        [Fact]
        public void ThrowExceptionWhenCreatingWithNoColumns()
        {
            string tableName = nameof(ThrowExceptionWhenCreatingWithNoColumns);
            Table table = new Table(tableName);
            using (SqlConnection sqlConnection = new SqlConnection(""))
            {
                table.Create(sqlConnection);
            }
        }
    }
}
