using Avalonia.Markup.Xaml;

namespace AwsAuthenticator.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
#if DEBUG
		this.AttachDevTools();
#endif
		WindowStateTracker.TryTrackWindow(this);
		Console.WriteLine($"WindowState: {this.WindowState}, Position: {this.Position}, Width: {this.Width}, Height: {this.Height}");

		this.Closing += (_, _) => {
			WindowStateTracker.Tracker.Persist(this);
			WindowStateTracker.Tracker.StopTracking(this);
			Console.WriteLine($"WindowState: {this.WindowState}, Position: {this.Position}, Width: {this.Width}, Height: {this.Height}");
		};
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}
