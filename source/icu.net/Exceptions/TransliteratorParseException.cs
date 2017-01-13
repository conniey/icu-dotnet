using System;

namespace Icu
{
	/// <summary>
	/// Exceptions for Transliterator errors.
	/// </summary>
    public class TransliteratorParseException : Exception
    {
        public TransliteratorParseException(string message) : base(message)
        { }
    }
}
