using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AwsCredentialManager.Core.Services
{
	public static class CommandRunner
	{
		public static string ExecuteCommand(object command)
		{
			// https://www.codeproject.com/Articles/25983/How-to-Execute-a-Command-in-C
			try
			{
				// /c parameter tells cmd that we want it to execute the command that follows and then exit.
				System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

				// The following commands are needed to redirect the standard output.
				// This means that it will be redirected to the Process.StandardOutput StreamReader.
				procStartInfo.RedirectStandardOutput = true;
				procStartInfo.RedirectStandardError = true;
				procStartInfo.UseShellExecute = false;
				procStartInfo.CreateNoWindow = true;
				var proc = new System.Diagnostics.Process();
				proc.StartInfo = procStartInfo;
				proc.Start();
				string resultOutput = proc.StandardOutput.ReadToEnd();

				if (string.IsNullOrEmpty(resultOutput)) {
					string resultError = proc.StandardError.ReadToEnd();
					return resultError ?? "";
				}
				return resultOutput;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex}");
			}
			return "";
		}

	}
}
