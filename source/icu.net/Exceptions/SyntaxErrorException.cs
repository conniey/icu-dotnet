using System;

namespace Icu
{
	/// <summary>
	/// Exception for syntax errors in a format pattern (ie. Number, exponent patterns.)
	/// </summary>
	public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string message) : base(message)
        { }
    }
}
