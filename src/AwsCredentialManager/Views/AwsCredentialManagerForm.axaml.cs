using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AwsCredentialManager.ViewModels;

namespace AwsCredentialManager.Views
{
	public partial class AwsCredentialManagerForm : UserControl
	{
		public AwsCredentialManagerForm()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		// TODO: try to bind "OnCloseAboutPopup" directly from XAML instead from this code-behind:
		public void AboutViewExitHandler(object sender, RoutedEventArgs e)
		{
			var viewModel = (AwsCredentialManagerViewModel)this.DataContext;
			viewModel.IsAboutVisible = false;
			e.Handled = true;
		}

	}
}
