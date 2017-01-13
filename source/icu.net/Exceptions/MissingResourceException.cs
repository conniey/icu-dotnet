using System;

namespace Icu
{
	/// <summary>
	/// Exception when icu cannot find a resource.
	/// </summary>
	public class MissingResourceException : Exception
	{
		public MissingResourceException(string message) : base(message)
		{ }
	}
}
