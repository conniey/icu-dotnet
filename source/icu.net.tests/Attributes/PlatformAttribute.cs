using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Icu.Tests.Attributes
{
	/// <summary>
	/// PlatformAttribute is used to mark a test fixture or an
	/// individual method as applying to a particular platform only.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public class PlatformAttribute : IncludeExcludeAttribute, IApplyToTest
	{
		/// <summary>
		/// Constructor with no platforms specified, for use
		/// with named property syntax.
		/// </summary>
		public PlatformAttribute() { }

		/// <summary>
		/// Constructor taking one or more platforms
		/// </summary>
		/// <param name="platforms">Comma-delimited list of platforms</param>
		public PlatformAttribute(string platforms) : base(platforms) { }

		/// <summary>
		/// Causes a test to be skipped if this PlatformAttribute is not satisfied.
		/// </summary>
		/// <param name="test">The test to modify</param>
		public void ApplyToTest(Test test)
		{
			if (test.RunState != RunState.NotRunnable &&
				test.RunState != RunState.Ignored &&
				!IsPlatformSupported(Include, Exclude))
			{
				test.RunState = RunState.Skipped;
				test.Properties.Add(PropertyNames.SkipReason, Reason);
			}
		}
		private bool IsPlatformSupported(string include, string exclude)
		{
			try
			{
				if (include != null && !IsPlatformSupported(include))
				{
					Reason = string.Format("Only supported on {0}", include);
					return false;
				}

				if (exclude != null && IsPlatformSupported(exclude))
				{
					Reason = string.Format("Not supported on {0}", exclude);
					return false;
				}
			}
			catch (Exception ex)
			{
				Reason = ex.Message;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Test to determine if one of a collection of platforms
		/// is being used currently.
		/// </summary>
		/// <param name="platforms"></param>
		/// <returns></returns>
		public bool IsPlatformSupported(string[] platforms)
		{
			return platforms.Any(IsPlatformSupported);
		}

		/// <summary>
		/// Test to determine if the a particular platform or comma-
		/// delimited set of platforms is in use.
		/// </summary>
		/// <param name="platform">Name of the platform or comma-separated list of platform ids</param>
		/// <returns>True if the platform is in use on the system</returns>
		public bool IsPlatformSupported(string platform)
		{
			if (platform.IndexOf(',') >= 0)
				return IsPlatformSupported(platform.Split(','));

			string platformName = platform.Trim();
			bool isSupported;

			if (string.Equals(platformName, OSPlatform.Linux.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				isSupported = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
			}
			else if (string.Equals(platformName, OSPlatform.Windows.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				isSupported = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
			}
			else if (string.Equals(platformName, OSPlatform.OSX.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				isSupported = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
			}
			else
			{
				isSupported = false;
				Reason = $"Platform [{platformName}] is not one of the following: {OSPlatform.Linux}, {OSPlatform.Windows}, {OSPlatform.OSX}";
			}

			if (!isSupported)
				Reason = "Only supported on " + platform;

			return isSupported;
		}
	}
}
