using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidColumnMasterKeyDefinitionException : Exception
    {
        public InvalidColumnMasterKeyDefinitionException()
        {
        }

        public InvalidColumnMasterKeyDefinitionException(string message)
            : base(message)
        {
        }

        public InvalidColumnMasterKeyDefinitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidColumnMasterKeyDefinitionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}