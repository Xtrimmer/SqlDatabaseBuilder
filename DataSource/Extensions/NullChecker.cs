using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public static class NullChecker
    {
        public static T ThrowIfNull<T>(this T obj, string name)
        {
            if (obj == null) throw new ArgumentNullException(string.Concat(name, " [", typeof(T), "]"));
            return obj;
        }
    }
}
