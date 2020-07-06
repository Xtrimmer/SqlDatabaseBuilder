using System.Collections.Generic;
using Microsoft.Data.SqlClient;
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

        public static bool IsColumnMasterKeyPresentInDatabase(this ColumnMasterKey columnMasterKey, SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                string cmkName = columnMasterKey.Name;
                string sql = "SELECT column_master_key_id from sys.column_master_keys where name = @cmkName";
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.Add(new SqlParameter("cmkName", cmkName));
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public static bool IsColumnEncryptionKeyPresentInDatabase(this ColumnEncryptionKey columnEncryptionKey, SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                string cekName = columnEncryptionKey.Name;
                string sql = "SELECT column_encryption_key_id from sys.column_encryption_keys where name = @cekName";
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.Add(new SqlParameter("cekName", cekName));
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        internal static readonly Dictionary<DataType, object> DataTypeValues = new Dictionary<DataType, object>
        {
            {DataType.BigInt(), (long)1},
            {DataType.SmallInt(), (short)2},
            {DataType.Int(), 3},
            {DataType.TinyInt(), (byte)4},
            {DataType.Bit(), 0},
            {DataType.Money(), (decimal)5.0000},
            {DataType.SmallMoney(), (decimal)6.0000},
            {DataType.Decimal(), (decimal)7.0000},
            {DataType.Numeric(), (decimal)8.0000},
            {DataType.Float(), 9.0000},
            {DataType.Real(), (float)10.0000},
            {DataType.VarChar(7), "'VarChar'"},
            {DataType.Char(4), "'Char'"},
            {DataType.VarBinary(1), (byte)255},
            {DataType.Binary(1), (byte)128},
            {DataType.NVarChar(8), "'NVarChar'"},
            {DataType.NChar(5), "'Nchar'"},
            {DataType.Date(), "'2019-05-12T00:00:00.0000000'"},
            {DataType.DateTime(), "'2019-05-12T12:30:20'"},
            {DataType.DateTime2(), "'2018-04-23T08:20:40.5'"},
            {DataType.Time(), "'05:40:00'"},
            {DataType.UniqueIdentifier(), "'00000000-0000-0000-0000-000000000000'"},
        };
    }
}
