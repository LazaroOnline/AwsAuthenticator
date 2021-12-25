using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Management;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using DS = System.DirectoryServices;

namespace AwsCredentialManager.Utils
{
	public static class UserInfo
	{
		// TODO: get the current windows user email.

		public static string GetUserFullName()
		{
			// https://stackoverflow.com/questions/1240373/how-do-i-get-the-current-username-in-net-using-c
			// https://stackoverflow.com/questions/7357123/get-current-users-email-address-in-net
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
			ManagementObjectCollection collection = searcher.Get();
			string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];

			string UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; // Gives NT AUTHORITY\SYSTEM

			var xxxx = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;

			var test = System.Security.Principal.WindowsIdentity.GetCurrent();
			// var test2= UserPrincipal.Current.EmailAddress;
			UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
			UserName = Environment.UserName;

			string CurrUsrEMail = string.Empty;
			CurrUsrEMail = DS.AccountManagement.UserPrincipal.Current.EmailAddress;

			return username;
		}
	}
}
