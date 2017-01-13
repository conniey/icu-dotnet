using System;

namespace Icu
{
	/// <summary>
	/// Exceptions related to BreakIterator functionality.
	/// </summary>
    public class BreakException : Exception
    {
        public BreakException(string message) : base(message)
        { }
    }
}
