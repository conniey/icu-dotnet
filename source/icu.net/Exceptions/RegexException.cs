using System;

namespace Icu
{
	/// <summary>
	/// Exceptions indicating Regexp failures
	/// </summary>
	public class RegexException : Exception
    {
        public RegexException(string message) : base(message)
        { }
    }
}
