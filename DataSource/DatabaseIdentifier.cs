using Xtrimmer.SqlDatabaseBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class DatabaseIdentifier
    {
        const int MAX_ID_LENGTH = 128;
        const int MIN_ID_LENGTH = 1;

        internal string Name { get; }

        public DatabaseIdentifier(string name) {
            name.ThrowIfNull("name");
            Name = IsValidLength(name) ? name :
                throw new InvalidDatabaseIdentifierException($"Identifiers must contain from {MIN_ID_LENGTH} through {MAX_ID_LENGTH} characters.");
        }

        private bool IsValidLength(string s) => s.Length >= MIN_ID_LENGTH && s.Length <= MAX_ID_LENGTH;
    }
}
