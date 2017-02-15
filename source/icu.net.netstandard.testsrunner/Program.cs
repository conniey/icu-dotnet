using Icu.Tests;
using NUnit.Common;
using NUnitLite;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace icu.net.netstandard.testsrunner
{
	public class Program
	{
		public static int Main(string[] args)
		{
			var result = new AutoRun(typeof(IcuWrapperTests).GetTypeInfo().Assembly)
				.Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
			return result;
		}
	}
}
