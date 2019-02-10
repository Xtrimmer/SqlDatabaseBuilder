using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidCharacterSetLengthException : Exception
    {
        public InvalidCharacterSetLengthException()
        {
        }

        public InvalidCharacterSetLengthException(string message) : base(message)
        {
        }

        public InvalidCharacterSetLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCharacterSetLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}