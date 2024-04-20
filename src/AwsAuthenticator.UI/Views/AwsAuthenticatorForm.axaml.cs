using Avalonia.Interactivity;

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

	private void AwsProfileSourceOptionsExpander_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		AutoCompleteBox_AwsProfileSource.IsDropDownOpen = !AutoCompleteBox_AwsProfileSource.IsDropDownOpen;
		AutoCompleteBox_AwsProfileSource.Focus();
	}

	private void AwsProfileToEditOptionsExpander_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		AutoCompleteBox_AwsProfileToEdit.IsDropDownOpen = !AutoCompleteBox_AwsProfileToEdit.IsDropDownOpen;
		AutoCompleteBox_AwsProfileToEdit.Focus();
	}

	public void OpenAboutDialog(object sender, RoutedEventArgs args)
	{
		this.AboutViewDialog.IsVisible = true;
	}
}
