using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Reflection;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace AwsCredentialManager
{
	// https://www.newtonsoft.com/json
	// https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
	public class AppSettingsWriter
	{
		public void Save(AppSettings appSettings, string fileName = AppSettings.FILENAME)
		{
			var folderPath = GetAppFolder();
			var settingsFullPath = $"{folderPath}/{fileName}";

			var jsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				//PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				Converters = {
					 new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
				},
			};

			string appSettingsJson = JsonSerializer.Serialize(appSettings, jsonSerializerOptions);
			File.WriteAllText(settingsFullPath, appSettingsJson);
		}

		private string GetAppFolder()
		{
			/*
			// This doesn't work if compiled to "single file" AND it is slow:
			var executingAssembly = Assembly.GetExecutingAssembly();
			var folderPath = Path.GetDirectoryName(executingAssembly.Location);
			return folderPath;

			// This doesn't work if compiled to "single file" (but at least it is faster):
			// https://rules.sonarsource.com/csharp/RSPEC-3902
			Assembly currentAssembly = typeof(AppSettingsWriter).Assembly;
			var folderPath = Path.GetDirectoryName(currentAssembly.Location);
			return folderPath;
			*/

			// This should work both during debugging and when compiled to "single file":
			// https://github.com/dotnet/runtime/issues/13531
			var filename = Process.GetCurrentProcess().MainModule.FileName;
			var folderPath = Path.GetDirectoryName(filename);
			return folderPath;
		}
	}
}
