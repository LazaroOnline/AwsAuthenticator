using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AwsAuthenticator.Views;

public partial class AboutView : UserControl
{
	// https://avaloniaui.net/docs/input/events
	public static readonly RoutedEvent<RoutedEventArgs> ExitViewEvent =
		RoutedEvent.Register<AboutView, RoutedEventArgs>(nameof(ExitView), RoutingStrategies.Bubble);

	public event EventHandler<RoutedEventArgs> ExitView
	{
		add => AddHandler(ExitViewEvent, value);
		remove => RemoveHandler(ExitViewEvent, value);
	}

	public AboutView() : this(null)
	{ }

	public AboutView(AboutViewModel? viewModel)
	{
		this.InitializeComponent();
		var dataContextViewModel = viewModel ?? new AboutViewModel();
		this.DataContext = dataContextViewModel;
	InputElement.KeyDownEvent.AddClassHandler<TopLevel>(OnKeyDownParent, handledEventsToo: true);
	}

	protected void OnKeyDownParent(object sender, KeyEventArgs e)
	{
		OnKeyDown(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Key == Key.Escape && this.IsVisible)
		{
			CloseDialog();
			e.Handled = true;
		}
	}

	public void CloseDialog(object sender, RoutedEventArgs args)
	{
		CloseDialog();
	}

	public void CloseDialog()
	{
		this.IsVisible = false;
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}
