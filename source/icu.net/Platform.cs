using System;
#if NETSTANDARD
using System.Runtime.InteropServices;
#endif
namespace Icu
{
	/// <summary>
	/// Taken from LibGit2Sharp, Platform.cs
	/// </summary>
	internal enum OperatingSystemType
	{
		Windows,
		Unix,
		MacOSX
	}

	/// <summary>
	/// Taken from LibGit2Sharp, Platform.cs
	/// </summary>
	internal static class Platform
	{
		public static string ProcessorArchitecture
		{
			get {
#if NETSTANDARD
				return RuntimeInformation.OSArchitecture.ToString();
#else
				return Environment.Is64BitProcess ? "x64" : "x86";
#endif
			}
		}

		public static OperatingSystemType OperatingSystem
		{
			get
			{
#if NETSTANDARD
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					return OperatingSystemType.Windows;
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					return OperatingSystemType.Unix;
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					return OperatingSystemType.MacOSX;
				else
					throw new NotSupportedException("Cannot get OperatingSystemType from: " + RuntimeInformation.OSDescription);
#else
				// See http://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform
				switch ((int)Environment.OSVersion.Platform)
				{
					case 4:
					case 128:
						return OperatingSystemType.Unix;

					case 6:
						return OperatingSystemType.MacOSX;

					default:
						return OperatingSystemType.Windows;
				}
#endif
			}
		}
	}
}
