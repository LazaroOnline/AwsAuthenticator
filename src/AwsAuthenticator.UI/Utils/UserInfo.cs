using System.Threading.Tasks;
using System.Management;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using DS = System.DirectoryServices;

namespace AwsAuthenticator.Utils;

public static class UserInfo
{
	public static string GetUserFullName()
	{
		try {
			// Get the user info and email from Active Directory:
			// This requires connection to the company Active Directory servers,
			// for some users this means they need the VPC connected.
			// https://stackoverflow.com/questions/7357123/get-current-users-email-address-in-net
			return System.DirectoryServices.AccountManagement.UserPrincipal.Current.EmailAddress;
		} catch {
			// Connection failed.
		}
		/*
		// Trying other ways to get the current windows user email.
		// https://stackoverflow.com/questions/1240373/how-do-i-get-the-current-username-in-net-using-c
		ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
		ManagementObjectCollection collection = searcher.Get();
		string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];

		string UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; // Gives NT AUTHORITY\SYSTEM

		var test = System.Security.Principal.WindowsIdentity.GetCurrent();
		UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
		UserName = Environment.UserName;

		string CurrUsrEMail = string.Empty;
		CurrUsrEMail = DS.AccountManagement.UserPrincipal.Current.EmailAddress;

		return username;
		*/
		return "";
	}
}
