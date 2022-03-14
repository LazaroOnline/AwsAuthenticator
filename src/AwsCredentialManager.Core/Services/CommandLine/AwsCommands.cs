using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AwsCredentialManager.Core.Models;

namespace AwsCredentialManager.Core.Services
{
	public static class AwsCommands
	{
		public static string GetAwsTokenCommand(string awsAccountId, string awsPersonalAccountName, string tokenCode, int durationSeconds = 28800)
		{
			return $@"aws sts get-session-token --duration-seconds {durationSeconds} --serial-number ""arn:aws:iam::{awsAccountId}:mfa/{awsPersonalAccountName}"" --token-code {tokenCode}";
		}

		public static string SetAwsAccountPropertyCommand(string accountPropertyName, string accountPropertyValue, string awsProfileName)
		{
			return $@"aws configure set {accountPropertyName} {accountPropertyValue} --profile {awsProfileName}";
		}

		public static string ChangeAwsProfileCommand(string awsProfile = AwsCredentialsFile.DEFAULT_PROFILE) => $@"$env:AWS_PROFILE = ""{awsProfile}""";

		public static string GetAwsProfileCommand() => @"$env:AWS_PROFILE";

	}
}
