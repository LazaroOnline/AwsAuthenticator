using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace AwsCredentialManager;

public class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow
			{
				DataContext = new AwsCredentialManagerViewModel(),
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}
