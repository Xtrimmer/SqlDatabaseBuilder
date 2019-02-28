using System;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class StringValueAttribute : Attribute
    {
        public string Value { get; private set; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }
}
