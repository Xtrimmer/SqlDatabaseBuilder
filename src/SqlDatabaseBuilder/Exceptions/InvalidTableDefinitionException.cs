using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidTableDefinitionException : Exception
    {
        public InvalidTableDefinitionException()
        {
        }

        public InvalidTableDefinitionException(string message) 
            : base(message)
        {
        }

        public InvalidTableDefinitionException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected InvalidTableDefinitionException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}