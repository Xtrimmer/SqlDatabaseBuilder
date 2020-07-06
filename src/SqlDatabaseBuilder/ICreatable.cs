using Microsoft.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    interface ICreatable
    {
        void Create(SqlConnection sqlConnection);
    }
}
