namespace AwsAuthenticator;

public static class WindowStateTracker
{
	// https://github.com/anakic/Jot
	public static readonly Jot.Tracker Tracker = new();

	public static void TrackWindow(Window window)
	{
		var initialPosition = window.Position;
		var trackerNamespace = string.Join("_", window.Screens.All.Select(s => 
			$"{s.WorkingArea.Size.Width}x{s.WorkingArea.Size.Height}"));
		trackerNamespace += "__" + Environment.ProcessPath?.Replace("/", "_").Replace("\\", "_");
		Tracker.Configure<Window>()
			.Id(w => w.Name, trackerNamespace)
			.Properties(w => new { w.WindowState, w.Position, w.Width, w.Height })
			.PersistOn(nameof(Window.Closing))
			.StopTrackingOn(nameof(Window.Closing));
		Tracker.Track(window);

		if (window.WindowState == WindowState.Minimized)
		{
			window.WindowState = WindowState.Normal;

			// When the window was closed while being minimized,
			// the position is lost and what is stored are negative values off the screen
			// (example: {-31993, -32000}).
			window.Position = initialPosition;
		}
	}
}
