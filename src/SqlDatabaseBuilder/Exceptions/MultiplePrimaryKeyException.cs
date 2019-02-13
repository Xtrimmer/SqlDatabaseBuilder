using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class MultiplePrimaryKeyException : Exception
    {
        public MultiplePrimaryKeyException()
        {
        }

        public MultiplePrimaryKeyException(string message) : base(message)
        {
        }

        public MultiplePrimaryKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MultiplePrimaryKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}