using System;

namespace Icu
{
	/// <summary>
	/// Exceptions indicating that there was a warning returned from icu.
	/// </summary>
    public class WarningException : Exception
    {
        public WarningException(string message) : base(message)
        { }
    }
}
