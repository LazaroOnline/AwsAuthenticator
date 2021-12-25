using System.Diagnostics;
using AwsCredentialManager.Core.Models;

namespace AwsCredentialManager.Core.Services
{
	public class AwsCredentialManager : IAwsCredentialManager
	{
		private IAwsCredentialUpdater _awsCredentialUpdater;
		private AwsCliService _awsCliService;

		public AwsCredentialManager(
			IAwsCredentialUpdater? awsCredentialUpdater = null
			,AwsCliService? awsCliService = null
		)
		{
			_awsCredentialUpdater = awsCredentialUpdater ?? new AwsCredentialUpdaterCmd();
			_awsCliService = awsCliService ?? new AwsCliService();
		}

		public void OpenAwsCredentialsFile()
		{
			OpenWithDefaultProgram(AwsCredentialsFile.FilePath, "notepad");
		}

		public static void OpenWithDefaultProgram(string path, string app = "explorer")
		{
			using Process fileopener = new Process();

			fileopener.StartInfo.FileName = app;
			fileopener.StartInfo.Arguments = "\"" + path + "\"";
			fileopener.Start();
		}

		public void UpdateAwsAccount(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileToEdit)
		{
			var creds = AwsGetToken(awsAccountId, awsPersonalAccountName, tokenCode);
			_awsCredentialUpdater.EditAwsCredsFile(awsProfileToEdit, creds?.Credentials);
		}

		public AwsCredentialsResponse? AwsGetToken(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileName = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			var currentAwsProfile = Environment.GetEnvironmentVariable(AwsCredentialsFile.ENVIRONMENT_VARIABLE_NAME_AWS_PROFILE);
			_awsCliService.ChangeProfile(awsProfileName);

			var creds = _awsCliService.GetToken(awsAccountId, awsPersonalAccountName, tokenCode);

			_awsCliService.ChangeProfile(currentAwsProfile);

			return creds;
		}

	}
}
