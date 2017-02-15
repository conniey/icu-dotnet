using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;

namespace Icu.Tests
{
	/// <summary>
	/// NUnit clone for SetCultureAttribute because it does not exist in .NET Standard.
	/// </summary>
	/// <remarks>
	/// See: https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Attributes/SetCultureAttribute.cs#L24
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
	public class SetCultureAttribute : PropertyAttribute, IApplyToContext
	{
		private string _culture;

		/// <summary>
		/// Construct given the name of a culture
		/// </summary>
		/// <param name="culture"></param>
		public SetCultureAttribute(string culture) : base(PropertyNames.SetCulture, culture)
		{
			_culture = culture;
		}

		public void ApplyToContext(TestExecutionContext context)
		{
			context.CurrentCulture = new System.Globalization.CultureInfo(_culture);
		}
	}
}
