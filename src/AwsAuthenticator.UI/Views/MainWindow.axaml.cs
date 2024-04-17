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
		WindowStateTracker.TrackWindow(this);
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}

	protected override void OnClosing(WindowClosingEventArgs e)
	{
		base.OnClosing(e);
		var awsAuthenticatorForm = this.FindControl<AwsAuthenticatorForm>("AwsAuthenticatorForm");
		var viewModel = (AwsAuthenticatorViewModel)awsAuthenticatorForm.DataContext;
		viewModel.SaveConfig();
	}
}
