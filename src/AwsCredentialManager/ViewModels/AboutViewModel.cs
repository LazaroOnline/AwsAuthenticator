using System;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AwsCredentialManager.Utils;
using AwsCredentialManager.Core.Services;
using ReactiveUI;
using Splat;

namespace AwsCredentialManager.ViewModels
{
	public class AboutViewModel
	{

		private const string URL_LICENSE = "https://opensource.org/licenses/MIT";
		private const string URL_GITHUB = "https://github.com/LazaroOnline/AwsCredentialManager";

		public string GitVersion { get; set; }

		public ReactiveCommand<Unit, Unit> OnOpenAboutWindow { get; }
		public ReactiveCommand<Unit, Unit> OnCloseView { get; }

		public AboutViewModel()
		{
			try
			{
				GitVersion = GitVersionService.GetGitVersionAssemblyInfo().ToString().TrimEnd('\n').TrimEnd('\r');
			}
			catch
			{

			}
			OnCloseView = ReactiveCommand.Create(() => { });
		}

		public void OpenLinkLicense()
		{
			Util.OpenUrl(URL_LICENSE);
		}

		public void OpenLinkGitHub()
		{
			Util.OpenUrl(URL_GITHUB);
		}

	}
}
