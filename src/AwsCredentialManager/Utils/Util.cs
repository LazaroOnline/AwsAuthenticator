using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AwsCredentialManager.Utils
{
	public class Util
	{
		// https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp/43232486#43232486
		public static void OpenUrl(string url)
		{
			try
			{
				//System.Diagnostics.Process.Start(url);
				var processInfo = new ProcessStartInfo()
				{
					UseShellExecute = true,
					FileName = url,
				};
				Process.Start(processInfo);
			}
			catch
			{
				// hack because of this: https://github.com/dotnet/corefx/issues/10361
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					url = url.Replace("&", "^&");
					Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					Process.Start("xdg-open", url);
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					Process.Start("open", url);
				}
				else
				{
					throw;
				}
			}
		}
	}
}
