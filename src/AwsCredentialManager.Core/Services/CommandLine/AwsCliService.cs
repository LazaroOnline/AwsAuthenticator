using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AwsCredentialManager.Core.Models;

namespace AwsCredentialManager.Core.Services
{
	public class AwsCliService
	{
		public AwsCredentialsResponse? GetToken(string awsAccountId, string awsPersonalAccountName, string tokenCode)
		{
			var command = AwsCommands.GetAwsTokenCommand(awsAccountId, awsPersonalAccountName, tokenCode);
			var resultJson = CommandRunner.ExecuteCommand(command);
			try
			{
				var creds = System.Text.Json.JsonSerializer.Deserialize<AwsCredentialsResponse?>(resultJson);
				return creds;
			}
			catch (Exception ex)
			{
				throw new FailToGetCredentialsException("Check the token is valid, if Aws cli is installed, and if your account is properly configured in AWS.", ex);
			}
		}

		public void SetAwsAccount(AwsCredentials? creds, string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			SetAwsAccount_AccessKey(creds.SecretAccessKey, awsProfile);
			SetAwsAccount_AccessKeyId(creds.AccessKeyId, awsProfile);
			SetAwsAccount_SessionToken(creds.SessionToken, awsProfile);
		}

		public void SetAwsAccount_AccessKey(string? accessKey, string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			var command = AwsCommands.SetAwsAccountPropertyCommand(AwsCredentialsFile.Properties.AWS_SECRET_ACCESS_KEY, accessKey, awsProfile);
			CommandRunner.ExecuteCommand(command);
		}

		public void SetAwsAccount_AccessKeyId(string? accessKeyId, string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			var command = AwsCommands.SetAwsAccountPropertyCommand(AwsCredentialsFile.Properties.AWS_ACCESS_KEY_ID, accessKeyId, awsProfile);
			CommandRunner.ExecuteCommand(command);
		}

		public void SetAwsAccount_SessionToken(string? sessionToken, string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			var command = AwsCommands.SetAwsAccountPropertyCommand(AwsCredentialsFile.Properties.AWS_SESSION_TOKEN, sessionToken, awsProfile);
			CommandRunner.ExecuteCommand(command);
		}

		public void ChangeProfile(string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			Environment.SetEnvironmentVariable(AwsCredentialsFile.ENVIRONMENT_VARIABLE_NAME_AWS_PROFILE, awsProfile);
		}

		[Obsolete($"Use {nameof(Environment.SetEnvironmentVariable)}")]
		public void ChangeProfile_Cmd(string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
		{
			var command = AwsCommands.ChangeAwsProfileCommand(awsProfile);
			CommandRunner.ExecuteCommand(command);
		}

	}
}
