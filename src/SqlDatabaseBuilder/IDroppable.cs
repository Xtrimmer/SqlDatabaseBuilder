using Microsoft.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    interface IDroppable
    {
        void Drop(SqlConnection sqlConnection);
    }
}
