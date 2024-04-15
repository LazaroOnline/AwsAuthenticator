namespace AwsCredentialManager;

public static class WindowStateTracker
{
	// https://github.com/anakic/Jot
	public static readonly Jot.Tracker Tracker = new();

	public static void TrackWindow(Window window)
	{
		var initialPosition = window.Position;
		var trackerNamespace = string.Join("_", window.Screens.All.Select(s => s.WorkingArea.Size.ToString()));
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
		}
		if (window.Position.X < 0 || window.Position.Y < 0)
		{
			window.Position = initialPosition;
		}
	}
}
