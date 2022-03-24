using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using AwsCredentialManager.Core.Models;

namespace AwsCredentialManager.Core.Services
{
	public class GitVersionService
	{
		public static GitVersionAssemblyInfo GetGitVersionAssemblyInfo()
		{
			var appAssembly = CurrentApp.GetAssembly();
			// In "single-file" builds the assembly.Location returns null, so it is required to get the location from somewhere else:
			var appLocation = CurrentApp.GetLocation();
			return GetGitVersionAssemblyInfo(appAssembly, appLocation);
		}

		public static GitVersionAssemblyInfo GetGitVersionAssemblyInfo(Assembly appAssembly, string? assemblyLocation = null)
		{
			assemblyLocation ??= appAssembly.Location;

			var versionInfo = new GitVersionAssemblyInfo();
			versionInfo.Version = appAssembly.GetName()?.Version?.ToString();
			versionInfo.InformationalVersion = appAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

			var isValidAssemblyLocation = !string.IsNullOrWhiteSpace(assemblyLocation);
			if (isValidAssemblyLocation)
			{
				versionInfo.AssemblyFileName = System.IO.Path.GetFileName(assemblyLocation);
				versionInfo.CreationDate = System.IO.File.GetCreationTime(assemblyLocation);
				versionInfo.ModificationDate = System.IO.File.GetLastWriteTime(assemblyLocation);
				versionInfo.FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyLocation)?.FileVersion;
			}
			// return JsonConvert.SerializeObject(versionInfo);
			return versionInfo;
		}
	}
}
