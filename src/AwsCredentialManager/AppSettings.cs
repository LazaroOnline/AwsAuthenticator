using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AwsCredentialManager
{
	public class AppSettings
	{
        public const string FILENAME = "AppSettings.json";

		public AwsSettings Aws { get; set; } = new AwsSettings();
	}

	public class AwsSettings
	{
		public string AccountId { get; set; } = "";

		public string UserName { get; set; } = "";

        public string MfaGeneratorSecretKey { get; set; } = "";

        /// <summary>
        /// Aws Profile name with the source permanent access keys.
        /// </summary>
        public string? ProfileSource { get; set; }

		/// <summary>
		/// Aws Profile name to edit.
		/// </summary>
		public string? Profile { get; set; }

		/// <summary>
		/// MFA Multi-Factor Authentication token code.
		/// It is a 6 digits number.
		/// </summary>
		public string? TokenCode { get; set; }
	}
}
