using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidIndexDefinitionException : Exception
    {
        public InvalidIndexDefinitionException()
        {
        }

        public InvalidIndexDefinitionException(string message) : base(message)
        {
        }

        public InvalidIndexDefinitionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidIndexDefinitionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}