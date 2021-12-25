using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AwsCredentialManager.Core.Models
{
	public static class AwsCredentialsFile
	{
		public const string ENVIRONMENT_VARIABLE_NAME_AWS_PROFILE = "AWS_PROFILE";
		public const string DEFAULT_PROFILE = "default";

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
