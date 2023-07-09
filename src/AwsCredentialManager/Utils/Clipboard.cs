namespace AwsCredentialManager;

public static class Clipboard
{
	public static Task SetTextAsync(string text)
	{
		return GetApp().Clipboard?.SetTextAsync(text) ?? Task.CompletedTask;
	}

	private static Application GetApp()
	{
		if (Application.Current != null)
		{
			return Application.Current;
		}
		//var appBuilder = AppBuilder.Configure<App>();
		var appBuilder = AppBuilder.Configure<Avalonia.Application>()
			.UsePlatformDetect()
			.SetupWithoutStarting();
		return appBuilder.Instance;
	}

}
