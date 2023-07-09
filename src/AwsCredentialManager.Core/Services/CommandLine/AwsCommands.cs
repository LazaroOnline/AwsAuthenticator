using System;
namespace AwsCredentialManager.Core.Services;

public static class EnvironmentVariables
{
	public const string AWS_PROFILE = "AWS_PROFILE";
}

public static class AwsCommands
{
	public static string DefaultAwsProfileIfEmpty(string? awsProfile) =>
		string.IsNullOrWhiteSpace(awsProfile) ? AwsCredentialsFile.DEFAULT_PROFILE : awsProfile;

		// https://awscli.amazonaws.com/v2/documentation/api/latest/reference/sts/get-session-token.html
		public static string GetAwsTokenCommand(string awsAccountId, string awsPersonalAccountName, string tokenCode, int durationSeconds = AwsSessionDurationSeconds.Max) =>
		$@"aws sts get-session-token --duration-seconds {durationSeconds} --serial-number ""arn:aws:iam::{awsAccountId}:mfa/{awsPersonalAccountName}"" --token-code {tokenCode}";

	public static string SetAwsAccountPropertyCommand(string accountPropertyName, string accountPropertyValue, string? awsProfile) =>
		$@"aws configure set {accountPropertyName} {accountPropertyValue} --profile {DefaultAwsProfileIfEmpty(awsProfile)}";

	// https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html
	public static string GetAwsLocalProfileListCommand() =>
		$@"aws configure list-profiles";

	public static string SetAwsProfileCommand(string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE) =>
		SetEnvironmentVariableCommand(EnvironmentVariables.AWS_PROFILE, DefaultAwsProfileIfEmpty(awsProfile));

	public static string SetAwsProfileCommandPowerShell(string? awsProfile = AwsCredentialsFile.DEFAULT_PROFILE, EnvironmentVariableTarget target = EnvironmentVariableTarget.User) =>
		SetEnvironmentVariableCommandPowerShell(EnvironmentVariables.AWS_PROFILE, DefaultAwsProfileIfEmpty(awsProfile), target);

	public static string SetEnvironmentVariableCommand(string envVariableName, string value) =>
		$@"set {envVariableName}={value}";

	public static string SetEnvironmentVariableCommandPowerShellLocalProcess(string envVariableName, string value) =>
		$@"$env:{envVariableName} = '{EscapePowerShellString(value, "'")}'";

	// https://www.tachytelic.net/2019/03/powershell-environment-variables/
	public static string SetEnvironmentVariableCommandPowerShell(string envVariableName, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.User) =>
		$@"[System.Environment]::SetEnvironmentVariable('{EscapePowerShellString(envVariableName,"'")}','{EscapePowerShellString(value, "'")}',[System.EnvironmentVariableTarget]::{target})";

	public static string GetAwsProfileCommand() =>
		GetEnvironmentVariableCommand(EnvironmentVariables.AWS_PROFILE);

	public static string GetAwsProfileCommandPowerShell() =>
		GetEnvironmentVariableCommandPowerShell(EnvironmentVariables.AWS_PROFILE);

	public static string GetEnvironmentVariableCommand(string envVariableName) =>
		$@"echo %{envVariableName}%";

	public static string GetEnvironmentVariableCommandPowerShell(string envVariableName) =>
		$@"$env:{envVariableName}";


	public static string? EscapePowerShellString(string? value, string delimiter = "'")
	{
		return value?.Replace(delimiter ?? "", delimiter + delimiter);
	}

}
