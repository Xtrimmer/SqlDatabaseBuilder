using System;
using System.Runtime.Serialization;

namespace Xtrimmer.SqlDatabaseBuilder
{
    [Serializable]
    internal class InvalidCharacterSetength : Exception
    {
        public InvalidCharacterSetength()
        {
        }

        public InvalidCharacterSetength(string message) : base(message)
        {
        }

        public InvalidCharacterSetength(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCharacterSetength(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}