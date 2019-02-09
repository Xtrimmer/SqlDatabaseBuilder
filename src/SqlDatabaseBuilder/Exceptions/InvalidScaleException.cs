using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidScaleException : Exception
    {
        public InvalidScaleException()
        {
        }

        public InvalidScaleException(string message) : base(message)
        {
        }

        public InvalidScaleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidScaleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}