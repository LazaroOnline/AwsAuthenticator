namespace AwsAuthenticator;

public static class WindowStateTracker
{
	// https://github.com/anakic/Jot
	public static readonly Jot.Tracker Tracker = new Jot.Tracker(new Jot.Storage.JsonFileStore(@$"{AppContext.BaseDirectory}\window-state\"));

	public static void TryTrackWindow(Window window)
	{
		try {
			TrackWindow(window);
		} catch (Exception ex) {
			// This may occur when using assembly trimming during the dotnet publish.
			FileLogger.Log($"Error in '{nameof(TrackWindow)}'.{Environment.NewLine}{ex}");
		}
	}

	public static void TrackWindow(Window window)
	{
		var initialPosition = window.Position;
		var trackerNamespace = string.Join("_", window.Screens.All.Select(s => 
			$"{s.WorkingArea.Size.Width}x{s.WorkingArea.Size.Height}"));
		trackerNamespace += "__" + Environment.ProcessPath?.Replace("/", "_").Replace("\\", "_");
		Tracker.Configure<Window>()
			.Id(w => w.Name, trackerNamespace)
			.Properties(w => new { w.WindowState, w.Position, w.Width, w.Height })
			// This uses reflection, which won't work with trimming/AOT.
			//.PersistOn(nameof(Window.Closing))
			//.StopTrackingOn(nameof(Window.Closing))
			;
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
