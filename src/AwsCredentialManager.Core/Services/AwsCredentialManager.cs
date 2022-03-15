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

		public void UpdateAwsAccount(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileSource, string awsProfileToEdit)
		{
			var isInvalidAwsProfile = awsProfileSource.Equals(awsProfileToEdit);
			if (isInvalidAwsProfile) {
				throw new ArgumentException($"The parameters: '{nameof(awsProfileSource)}' and '{nameof(awsProfileSource)}' should not be the same because that would override the permanent 'aws_access_key_id'/'aws_secret_access_key' with the temporal one.");
			}
			var creds = AwsGetToken(awsAccountId, awsPersonalAccountName, tokenCode, awsProfileSource);
			_awsCredentialUpdater.EditAwsCredsFile(awsProfileToEdit, creds?.Credentials);
		}

		public AwsCredentialsResponse? AwsGetToken(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileSource = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			var currentAwsProfile = Environment.GetEnvironmentVariable(AwsCredentialsFile.ENVIRONMENT_VARIABLE_NAME_AWS_PROFILE);
			// No need to set at the user level, the process and it's child process inherits the env vars.
			var envVarLevel = EnvironmentVariableTarget.Process; // EnvironmentVariableTarget.User
			_awsCliService.ChangeProfile(awsProfileSource, envVarLevel);

			try
			{
				var creds = _awsCliService.GetToken(awsAccountId, awsPersonalAccountName, tokenCode, awsProfileSource);
				
				return creds;
			}
			finally {
				// Reset the AWS profile to the value it had before this operation.
				_awsCliService.ChangeProfile(currentAwsProfile, envVarLevel);
			}
		}

		public string AwsGetCurrentUserProfile()
		{
			return _awsCliService.GetProfile();
		}
	}
}
