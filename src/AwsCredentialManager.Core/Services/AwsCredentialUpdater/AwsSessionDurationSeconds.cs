namespace AwsCredentialManager.Core.Services.AwsCredentialUpdater;

/// <summary>https://awscli.amazonaws.com/v2/documentation/api/latest/reference/sts/get-session-token.html</summary>
public static class AwsSessionDurationSeconds
{
	/// <summary>Max duration seconds (15 minutes).</summary>
	public const int Min = 900;
	/// <summary>Default duration seconds (12 hours).</summary>
	public const int Default = 43200;
	/// <summary>Max duration (36 hours).</summary>
	public const int Max = 129600;
}
