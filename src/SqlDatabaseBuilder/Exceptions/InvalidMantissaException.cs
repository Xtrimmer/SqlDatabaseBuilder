using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidMantissaException : Exception
    {
        public InvalidMantissaException()
        {
        }

        public InvalidMantissaException(string message) : base(message)
        {
        }

        public InvalidMantissaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMantissaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}