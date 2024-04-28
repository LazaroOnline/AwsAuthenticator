using System.Diagnostics;

namespace AwsAuthenticator.Core.Services;

public interface IAwsAuthenticator
{
	void OpenAwsCredentialsFile();
	void UpdateAwsAccount(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileSource, string awsProfileToEdit);
	List<string> GetAwsLocalProfileList();
	DateTime GetNextTokenExpirationTime();
}

public class AwsAuthenticator : IAwsAuthenticator
{
	// Each MFA provider may use different key lengths.
	// For example GitHub's MFA secret key is shorter with 16 alphanumeric characters.
	/// <summary>Length of the alpha-numeric secret key used by AWS to link a device as MFA token generator.</summary>
	public const int MFA_DEVICE_GENERATOR_SECRET_KEY_LENGTH = 64;
	public const int TOKEN_AWS_MIN_LENGTH = 6;
	public const int AWS_ACCOUNT_ID_LENGTH = 12;

	private readonly IAwsCredentialUpdater _awsCredentialUpdater;
	private readonly AwsCliService _awsCliService;

	public AwsAuthenticator(
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

	public List<string> GetAwsLocalProfileList()
	{
		return _awsCliService.GetAwsLocalProfileList();
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
		var currentAwsProfile = Environment.GetEnvironmentVariable(EnvironmentVariables.AWS_PROFILE);
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

	public DateTime GetNextTokenExpirationTime()
	{
		var now = DateTime.UtcNow;
		var isMinutesFirstHalf = now.Second < 30;
		if (isMinutesFirstHalf)
		{
			return now.AddSeconds(30 - now.Second);
		}
		else
		{
			return now.AddSeconds(60 - now.Second);
		}
	}
}
