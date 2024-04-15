using Avalonia.Markup.Xaml;

namespace AwsCredentialManager.Views;

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
		var awsCredentialManagerForm = this.FindControl<AwsCredentialManagerForm>("AwsCredentialManagerForm");
		var viewModel = (AwsCredentialManagerViewModel)awsCredentialManagerForm.DataContext;
		viewModel.SaveConfig();
	}
}
