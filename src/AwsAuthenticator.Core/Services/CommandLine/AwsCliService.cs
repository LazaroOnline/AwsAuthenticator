﻿namespace AwsAuthenticator.Core.Services;

public class AwsCliService
{
	public AwsCredentialsResponse? GetToken(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileToEdit = AwsCredentialsFile.DEFAULT_PROFILE)
	{
		awsProfileToEdit = string.IsNullOrWhiteSpace(awsProfileToEdit) ? AwsCredentialsFile.DEFAULT_PROFILE : awsProfileToEdit;
		var command = AwsCommands.SetAwsProfileCommand(awsProfileToEdit);
		command += "&& " + AwsCommands.GetAwsTokenCommand(awsAccountId, awsPersonalAccountName, tokenCode);
		var resultJson = CommandRunner.ExecuteCommand(command);
		try
		{
			var creds = System.Text.Json.JsonSerializer.Deserialize<AwsCredentialsResponse?>(resultJson);
			return creds;
		}
		catch (Exception ex)
		{
			throw new FailToGetCredentialsException(
				// "Check the token is valid, if Aws cli is installed, and if your account is properly configured in AWS.\r\n" +
				"The command: \r\n  " + command +
				"\r\nReturned: \r\n" + resultJson + "", ex);
		}
	}

	public List<string> GetAwsLocalProfileList()
	{
		var command = AwsCommands.GetAwsLocalProfileListCommand();
		var profiles = CommandRunner.ExecuteCommand(command);
		var profileList = profiles.Trim('\r', '\n').Split(Environment.NewLine)?.ToList() ?? new List<string>();
		return profileList;
	}

	/// <summary>
	/// This sets up "aws_access_key_id" and "aws_secret_access_key" and "aws_session_token" to use the temporal credentials.123456
	/// It is important not to overwrite your permanent "aws_access_key_id" and "aws_secret_access_key" 
	/// used to get the temporal credentials with the temporal ones.
	/// </summary>
	public void SetAwsAccountTempCredentials(AwsCredentials? creds, string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
	{
		if (creds == null) { return; }
		SetAwsAccount_AccessKey(creds.SecretAccessKey, awsProfile);
		SetAwsAccount_AccessKeyId(creds.AccessKeyId, awsProfile);
		SetAwsAccount_SessionToken(creds.SessionToken, awsProfile);
	}

	public bool EnsureAwsCredentialsFileEndsWithNewLine()
	{
		var filePath = AwsCredentialsFile.FilePath;
		var fileContent = File.ReadAllText(filePath);
		var fileEndsWithNewLine = Regex.IsMatch(fileContent, @"\r\n$");
		var fileContentEdited = fileContent;
		var fileWasEdited = false;
		if (!fileEndsWithNewLine)
		{
			fileContentEdited += "\r\n\r\n";
			fileWasEdited = true;
		}
		File.WriteAllText(filePath, fileContentEdited);
		return fileWasEdited;
	}

	public void SetAwsAccount_AccessKey(string? accessKey, string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
	{
		// The command "aws configure set..." doesn't work properly if the credentials file doesn't end with a new line,
		// it would start writing the new profile at the end of the file without adding a newline itself, thus generating an invalid credentials file.
		// See: https://github.com/aws/aws-cli/issues/6874
		EnsureAwsCredentialsFileEndsWithNewLine();
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

	public void ChangeProfile(string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
	{
		Environment.SetEnvironmentVariable(EnvironmentVariables.AWS_PROFILE, awsProfile, target);
	}

	public string GetProfile()
	{
		return Environment.GetEnvironmentVariable(EnvironmentVariables.AWS_PROFILE);
	}

	[Obsolete($"Use {nameof(Environment.SetEnvironmentVariable)}")]
	public void ChangeProfile_Cmd(string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE)
	{
		var command = AwsCommands.SetAwsProfileCommand(awsProfile);
		CommandRunner.ExecuteCommand(command);
	}

	[Obsolete($"Use {nameof(Environment.GetEnvironmentVariable)}")]
	public string GetProfile_Cmd()
	{
		var command = AwsCommands.GetAwsProfileCommand();
		var result = CommandRunner.ExecuteCommand(command);
		// "echo" command introduces an extra new line at the end, so lets remove it:
		var resultCleaned = Regex.Replace(result ?? "", "\r\n$", "");
		return resultCleaned;
	}

}
