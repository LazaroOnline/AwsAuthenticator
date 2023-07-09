namespace AwsCredentialManager.Core.Services;

public interface IAwsCredentialManager
{
	void OpenAwsCredentialsFile();
	void UpdateAwsAccount(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileSource, string awsProfileToEdit);
	string AwsGetCurrentUserProfile(); // TODO: remove this method from this interface.
	List<string> GetAwsLocalProfileList();
}
