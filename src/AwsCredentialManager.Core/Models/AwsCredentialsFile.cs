using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AwsCredentialManager.Core.Models
{
	public static class AwsCredentialsFile
	{
		/// <summary>Aws default profile, it does have a special meaning because the aws cli creates this.</summary>
		public const string DEFAULT_PROFILE = "default";

		/// <summary>Default profile name used by this application, it doesn't have any special meaning for aws cli, it could be any name.</summary>
		public const string DEFAULT_PROFILE_MFA = "mfa";


		public const string FILEPATH_RELATIVE = ".aws/credentials";
		/// <summary>
		/// C:/Users/%USERNAME%/.aws/credentials
		/// </summary>
		public static string FilePath => $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/{FILEPATH_RELATIVE}";


		public static class Properties
		{
			public const string AWS_ACCESS_KEY_ID = "aws_access_key_id";
			public const string AWS_SECRET_ACCESS_KEY = "aws_secret_access_key";
			public const string AWS_SESSION_TOKEN = "aws_session_token";
		}
	}
}
