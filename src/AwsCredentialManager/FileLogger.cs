using System.IO;

namespace AwsCredentialManager;

public static class FileLogger
{
    public const string LogFileName = "AwsCredentialManager.log";
    public static void Log(string text)
    {
        var appDir = Program.GetExecutingDir();
        var logPath = $"{appDir}/{LogFileName}";
        var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        File.AppendAllText(logPath, $"{date}  {text}\r\n");
    }
}
