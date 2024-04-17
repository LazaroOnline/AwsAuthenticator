using System.Reflection;

namespace AwsAuthenticator.Core.Services;

public static class CurrentApp
{

	public static string GetLocation()
	{
		return Environment.ProcessPath;

		/*
		// This works both during debugging and when compiled to "single file":
		// https://github.com/dotnet/runtime/issues/13531
		var currentProcess = Process.GetCurrentProcess();
		return currentProcess.MainModule.FileName;

		
		// This doesn't work if compiled to "single file" (but at least it is faster):
		// https://rules.sonarsource.com/csharp/RSPEC-3902
		Assembly currentAssembly = typeof(AppSettingsWriter).Assembly;
		return currentAssembly.Location;


		// This doesn't work if compiled to "single file" AND it is slow:
		var executingAssembly = Assembly.GetExecutingAssembly();
		return executingAssembly.Location;
		*/
	}

	public static string GetFolder()
	{
		return AppContext.BaseDirectory;
		// return Path.GetDirectoryName(GetLocation());
	}

	public static Assembly GetExecutingAssembly()
	{
		return Assembly.GetExecutingAssembly();
	}

	public static Assembly GetAssembly<T>()
	{
		return typeof(T).Assembly;
	}

	public static Assembly GetAssembly()
	{
		return typeof(CurrentApp).Assembly;
	}

}
