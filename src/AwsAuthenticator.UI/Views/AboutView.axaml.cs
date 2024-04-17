using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using AwsAuthenticator.ViewModels;

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
			var dataContextViewModel = viewModel ?? new AboutViewModel();

			dataContextViewModel.OnCloseView.Subscribe(x => {
				var eventArgs = new RoutedEventArgs { RoutedEvent = ExitViewEvent, Source = this };
				this.RaiseEvent(eventArgs);

			});
			this.DataContext = dataContextViewModel;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

	}
}
