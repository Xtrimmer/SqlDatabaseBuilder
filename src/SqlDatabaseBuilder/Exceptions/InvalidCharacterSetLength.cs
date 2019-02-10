using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    public class InvalidCharacterSetLength : Exception
    {
        public InvalidCharacterSetLength()
        {
        }

        public InvalidCharacterSetLength(string message) : base(message)
        {
        }

        public InvalidCharacterSetLength(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCharacterSetLength(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}