namespace AwsAuthenticator.Core.Models;

public class AwsCredentialsResponse
{
	public AwsCredentials Credentials { get; set; }
}

public class AwsCredentials
{
	public string AccessKeyId { get; set; }
	public string SecretAccessKey { get; set; }
	public string SessionToken { get; set; }
	public string Expiration { get; set; }
	public DateTime ExpirationDate { get; set; }
}
