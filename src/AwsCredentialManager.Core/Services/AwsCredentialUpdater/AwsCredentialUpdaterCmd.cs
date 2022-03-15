using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AwsCredentialManager.Core.Models;

namespace AwsCredentialManager.Core.Services
{
	public class AwsCredentialUpdaterCmd : IAwsCredentialUpdater
	{
		private AwsCliService _awsCliService = new AwsCliService();

		public void EditAwsCredsFile(string profileName, AwsCredentials? creds)
		{
			_awsCliService.SetAwsAccountTempCredentials(creds, profileName);
		}

		public string AwsGetCurrentUserProfile()
		{
			return _awsCliService.GetProfile();
		}
	}
}
