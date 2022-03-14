using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AwsCredentialManager.Core.Models;

namespace AwsCredentialManager.Core.Services
{
	public interface IAwsCredentialUpdater
	{
		public void EditAwsCredsFile(string profileName, AwsCredentials? creds);

		public string AwsGetCurrentUserProfile();
	}
}
