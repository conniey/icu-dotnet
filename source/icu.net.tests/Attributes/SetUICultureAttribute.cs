using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;

namespace Icu.Tests
{
	/// <summary>
	/// NUnit clone for [SetUICulture] because it does not exist in .NET Standard.
	/// </summary>
	/// <remarks>
	/// See: https://github.com/nunit/nunit/blob/master/src/NUnitFramework/framework/Attributes/SetUICultureAttribute.cs#L24
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
	public class SetUICultureAttribute : PropertyAttribute, IApplyToContext
	{
		private const string SetUICulture = nameof(SetUICulture);
		private readonly string _culture;

		public SetUICultureAttribute(string culture) 
			: base(SetUICulture, culture)
		{
			_culture = culture;
		}

		public void ApplyToContext(TestExecutionContext context)
		{
			context.CurrentUICulture = new System.Globalization.CultureInfo(_culture);
		}
	}
}
