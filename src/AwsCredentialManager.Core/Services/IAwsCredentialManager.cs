using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AwsCredentialManager.Core.Services
{
	public interface IAwsCredentialManager
	{
		void OpenAwsCredentialsFile();
		void UpdateAwsAccount(string awsAccountId, string awsPersonalAccountName, string tokenCode, string awsProfileToEdit);
		string AwsGetCurrentUserProfile(); // TODO: remove this method from this interface.
	}
}
