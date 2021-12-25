using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AwsCredentialManager.Core.Models
{
	public class FailToGetCredentialsException : Exception
	{
		public FailToGetCredentialsException(string message = "", Exception? innerException = null) : base(message, innerException)
		{
		}

	}
}
