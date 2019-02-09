using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    interface IDroppable
    {
        void Drop(SqlConnection sqlConnection);
    }
}
