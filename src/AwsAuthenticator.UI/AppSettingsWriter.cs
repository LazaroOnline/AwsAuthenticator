using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AwsAuthenticator;

// https://www.newtonsoft.com/json
// https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
public class AppSettingsWriter
{
	public void Save(AppSettings appSettings, string fileName = AppSettings.FILENAME)
	{
		var folderPath = CurrentApp.GetFolder();
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

}
