using Avalonia.Controls;
using Avalonia.Threading;

namespace AwsCredentialManager;

public static class Clipboard
{
	public static async Task SetTextAsync(string text, Control? control = null)
	{
		await Dispatcher.UIThread.Invoke(() =>
		{
			var controlForClipboard = control ?? new Window();
			return TopLevel.GetTopLevel(controlForClipboard)?.Clipboard?.SetTextAsync(text) ?? Task.CompletedTask;
		});
	}
}
