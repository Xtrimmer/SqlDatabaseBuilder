using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidDatabaseIdentifierException : ArgumentException
    {
        public InvalidDatabaseIdentifierException()
        {
        }

        public InvalidDatabaseIdentifierException(string message)
            : base(message)
        {
        }

        public InvalidDatabaseIdentifierException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidDatabaseIdentifierException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
