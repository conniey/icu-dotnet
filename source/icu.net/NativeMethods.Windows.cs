using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Icu
{
	internal class NativeMethodsWindowsHelper
	{
		private static readonly PackageCompilationAssemblyResolver _resolver;
		private static readonly string _nugetPackageDirectory;
		private static bool _haveDependencyPathsBeenSet;

		static NativeMethodsWindowsHelper()
		{
			// HACK: This is a hack from
			// https://github.com/dotnet/core-setup/blob/master/src/Microsoft.Extensions.DependencyModel/Resolution/PackageCompilationAssemblyResolver.cs
			// to try and fetch the user's NuGet cache since that logic is hidden.
			// This is fairly fragile since the field could be renamed, etc.
			_resolver = new PackageCompilationAssemblyResolver();
			var packageDirectoryProperty = typeof(PackageCompilationAssemblyResolver)
				.GetTypeInfo()
				.GetField("_nugetPackageDirectory", BindingFlags.NonPublic);

			_nugetPackageDirectory = packageDirectoryProperty?.GetValue(_resolver) as string;
		}

		public static bool TrySetIcuPathsOnWindows()
		{
			var runtimeId = "win7-x64";
			var defaultContext = DependencyContext.Default;
			var runtimeLib = defaultContext.RuntimeLibraries.ToArray();
			var runtimelibNames = runtimeLib.Select(x => x.Name).ToArray();

			var nativeAssets = defaultContext.GetRuntimeNativeAssets(runtimeId).ToArray();
			var defaultNativeAssets = defaultContext.GetDefaultNativeAssets().ToArray();
			var winx64NativeAssets = nativeAssets.Except(defaultNativeAssets).ToArray();

			var runtimeIdGraph = defaultContext.RuntimeGraph;
			var graphNames = runtimeIdGraph.Select(x => x.Runtime).ToArray();

			if (string.IsNullOrEmpty(_nugetPackageDirectory))
			{
				// that's odd.. I guess we use normal search paths from %PATH% then.
				return false;
			}

			var nunit = defaultContext.CompileLibraries
				.Where(x => x.Name.ToLowerInvariant().Equals("nunit", StringComparison.OrdinalIgnoreCase))
				.First();
			var nunitRuntimeLib = runtimeLib.Where(x => x.Name.Equals("nunit", StringComparison.OrdinalIgnoreCase)).First();

			var icuRuntimeLib = runtimeLib.Where(x => x.Name.StartsWith("icu4c", StringComparison.OrdinalIgnoreCase)).ToArray();

			List<string> assemblyPaths = new List<string>();
			bool resolved = _resolver.TryResolveAssemblyPaths(nunit, assemblyPaths);

			if (_haveDependencyPathsBeenSet)
			{
				return true;
			}

			var dependencyModel = DependencyContext.Default;

			var allNativeAssets = dependencyModel.RuntimeLibraries;
		}

		/// <summary>
		/// Tries to fetch the default package directory for NuGet packages.
		/// </summary>
		/// <param name="osPlatform"></param>
		/// <param name="environment"></param>
		/// <returns></returns>
		internal static string GetDefaultPackageDirectory(OSPlatform osPlatform)
		{
			var packageDirectory = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

			if (!string.IsNullOrEmpty(packageDirectory))
			{
				return packageDirectory;
			}

			string basePath;
			if (osPlatform == OSPlatform.Windows)
			{
				basePath = Environment.GetEnvironmentVariable("USERPROFILE");
			}
			else
			{
				basePath = Environment.GetEnvironmentVariable("HOME");
			}
			if (string.IsNullOrEmpty(basePath))
			{
				return null;
			}
			return Path.Combine(basePath, ".nuget", "packages");
		}

		/// <summary>
		/// Given a CompilationLibrary and a base path, tries to construct the
		/// nuget package location and returns true if it exists.
		///
		/// Taken from: https://github.com/dotnet/core-setup/blob/master/src/Microsoft.Extensions.DependencyModel/Resolution/ResolverUtils.cs#L12
		/// </summary>
		/// <param name="library">Compilation library to try to get the rooted
		/// path from.</param>
		/// <param name="basePath">Rooted base path to try and get library from.</param>
		/// <param name="packagePath">The path for the library if it exists;
		/// null otherwise.</param>
		/// <returns></returns>
		internal static bool TryResolvePackagePath(CompilationLibrary library, string basePath, out string packagePath)
		{
			var path = library.Path;
			if (string.IsNullOrEmpty(path))
			{
				path = Path.Combine(library.Name, library.Version);
			}

			packagePath = Path.Combine(basePath, path);

			return Directory.Exists(packagePath);
		}
	}
}
