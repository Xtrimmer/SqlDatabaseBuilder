﻿namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class DatabaseIdentifier
    {
        const int MAX_ID_LENGTH = 128;
        const int MIN_ID_LENGTH = 1;

        internal string Name { get; }

        public DatabaseIdentifier(string name)
        {
            if (name != null && !IsValidLength(name)) throw new InvalidDatabaseIdentifierException($"Identifiers must contain from {MIN_ID_LENGTH} through {MAX_ID_LENGTH} characters.");
            Name = name;
        }

        internal static bool IsValidLength(string s) => s.Length >= MIN_ID_LENGTH && s.Length <= MAX_ID_LENGTH;
    }
}
