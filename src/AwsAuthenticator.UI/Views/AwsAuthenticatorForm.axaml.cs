using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AwsAuthenticator.Views;

public partial class AwsAuthenticatorForm : UserControl
{
	public AwsAuthenticatorForm()
	{
		InitializeComponent();
		AwsProfileSourceOptionsExpander.Click += AwsProfileSourceOptionsExpander_Click;
		AwsProfileToEditOptionsExpander.Click += AwsProfileToEditOptionsExpander_Click;

		if (AwsTokenTextBox != null)
		{
			AwsTokenTextBox.AttachedToVisualTree += (s, e) => AwsTokenTextBox.Focus();
		}
	}


	// TODO: try to bind "OnCloseAboutPopup" directly from XAML instead from this code-behind:
	public void AboutViewExitHandler(object sender, RoutedEventArgs e)
	{
		var viewModel = (AwsAuthenticatorViewModel?)this.DataContext;
		if (viewModel != null) {
			viewModel.IsAboutVisible = false;
		}
		e.Handled = true;
	}

	private void AwsProfileSourceOptionsExpander_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		AutoCompleteBox_AwsProfileSource.IsDropDownOpen = !AutoCompleteBox_AwsProfileSource.IsDropDownOpen;
	}

	private void AwsProfileToEditOptionsExpander_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		AutoCompleteBox_AwsProfileToEdit.IsDropDownOpen = !AutoCompleteBox_AwsProfileToEdit.IsDropDownOpen;
	}

}
