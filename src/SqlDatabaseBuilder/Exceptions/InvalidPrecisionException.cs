using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidPrecisionException : Exception
    {
        public InvalidPrecisionException()
        {
        }

        public InvalidPrecisionException(string message) : base(message)
        {
        }

        public InvalidPrecisionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPrecisionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}