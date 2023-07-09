namespace AwsCredentialManager.Core.Models;

public class FailToGetCredentialsException : Exception
{
	public FailToGetCredentialsException(string message = "", Exception? innerException = null) : base(message, innerException)
	{
	}

}
