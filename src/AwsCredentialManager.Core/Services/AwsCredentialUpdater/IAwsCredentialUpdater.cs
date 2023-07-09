namespace AwsCredentialManager.Core.Services;

public interface IAwsCredentialUpdater
{
	public void EditAwsCredsFile(string profileName, AwsCredentials? creds);

	public string AwsGetCurrentUserProfile();
}
