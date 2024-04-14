using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AwsCredentialManager.Core;

public static class StringExtensions
{
    public static bool TryOpenUrlInBrowser(this string url)
    {
        try {
            // validate the text is a valid url:
            var uri = new Uri(url);
            Process.Start(uri.ToString());
            return true;
        }
        catch {
            try {
                // https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp/43232486#43232486
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                    Process.Start("open", url);
                }
                else {
                    return false;
                }
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
