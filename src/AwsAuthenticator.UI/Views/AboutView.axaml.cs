using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace AwsAuthenticator.Views
{
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
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public void CloseDialog(object sender, RoutedEventArgs args)
		{
			this.IsVisible = false;
		}
	}
}
