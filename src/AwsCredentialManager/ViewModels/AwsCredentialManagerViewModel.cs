using System;
using System.Linq;
using System.Text;
using System.Reactive;
using ReactiveUI;
using Splat;
using AwsCredentialManager.Core.Models;
using AwsCredentialManager.Core.Services;

namespace AwsCredentialManager.ViewModels
{
	public class AwsCredentialManagerViewModel : ViewModelBase
	{
		private IAwsCredentialManager _awsCredentialManager;
		private AppSettingsWriter _appSettingsWriter;

		public string AwsAccountId { get; set; }

		public string AwsUserName { get; set; }

		public string AwsProfileSource { get; set; } = AwsCredentialsFile.DEFAULT_PROFILE;

		public string AwsProfileToEdit { get; set; }

		public string AwsProfileToEdit_Default { get; set; } = "mfa";

		public string AwsTokenCode { get; set; }

		public string AwsCurrentProfileName { get; set; }

		private bool _isAboutVisible;
		public bool IsAboutVisible
		{
			get => _isAboutVisible;
			set => this.RaiseAndSetIfChanged(ref _isAboutVisible, value);
		}

		private string _logs;
		public string Logs
		{
			get => _logs;
			set => this.RaiseAndSetIfChanged(ref _logs, value);
		}

		private bool _isLogEmpty;
		public bool IsLogEmpty
		{
			get => _isLogEmpty;
			set => this.RaiseAndSetIfChanged(ref _isLogEmpty, value);
		}

		public bool GetIsLogEmpty() => string.IsNullOrWhiteSpace(Logs);

		// https://avaloniaui.net/docs/controls/button
		public ReactiveCommand<Unit, Unit> OnOpenAboutWindow { get; }
		public ReactiveCommand<Unit, Unit> OnCloseAboutPopup { get; }


		// Constructor required by the designer tools.
		public AwsCredentialManagerViewModel()
			: this(null, null, null) // Call the other constructor
		{
			System.Diagnostics.Debug.WriteLine($"Default constructor used for {nameof(AwsCredentialManagerViewModel)}.");
		}

		public AwsCredentialManagerViewModel(
			IAwsCredentialManager? awsCredentialManager = null
			,AppSettingsWriter? appSettingsWriter = null
			,AwsSettings? awsSettings = null
		)
		{
			var secret = "asdf";
			var authenticator = new TwoStepsAuthenticator.TimeAuthenticator();
			var code = authenticator.GetCode(secret);



			_awsCredentialManager = awsCredentialManager ?? Splat.Locator.Current.GetService<IAwsCredentialManager>();
			_appSettingsWriter = appSettingsWriter ?? Splat.Locator.Current.GetService<AppSettingsWriter>();
			awsSettings = awsSettings ?? Splat.Locator.Current.GetService<AwsSettings>();

			//_awsCredentialManager = awsCredentialManager ?? new Core.AwsCredentialManager();
			//_appSettingsWriter = appSettingsWriter ?? new AppSettingsWriter();

			AwsAccountId = awsSettings?.AccountId;
			AwsUserName = awsSettings?.UserName; // ?? Utils.UserInfo.GetUserFullName(); // TODO: try to get the current user email.
			AwsProfileSource = awsSettings?.ProfileSource;
			AwsProfileToEdit = awsSettings?.Profile;
			AwsTokenCode = awsSettings?.TokenCode;

			this.WhenAnyValue(x => x.Logs).Subscribe(x => {
				this.IsLogEmpty = GetIsLogEmpty();
			});


			OnOpenAboutWindow = ReactiveCommand.Create(() => {
				this.IsAboutVisible = true;
			});
			OnCloseAboutPopup = ReactiveCommand.Create(() => {
				this.IsAboutVisible = false;
			});
		}

		public void AutoUpdateCredentialsCommand()
		{
			// TODO:
			throw new NotImplementedException("The feature to automatically get the MFA token is not yet implemented.");
		}

		public string GetAwsProfileToEdit()
		{
			return string.IsNullOrWhiteSpace(AwsProfileToEdit) ? AwsProfileToEdit_Default : AwsProfileToEdit;
		}

		public void UpdateCredentialsCommand()
		{
			try
			{
				Logs = "Updating...";
				_awsCredentialManager.UpdateAwsAccount(AwsAccountId, AwsUserName, AwsTokenCode, AwsProfileSource, GetAwsProfileToEdit());
				Logs = $"Updated credentials successfully at {DateTime.Now.ToString("yyy-M-d HH:mm")}.";
			}
			catch (Exception ex)
			{
				Logs = $"ERROR: {DateTime.Now.ToString("yyy-M-d HH:mm:ss")} {ex}";
			}
		}

		public void OpenAwsCredentialsFileCommand()
		{
			_awsCredentialManager.OpenAwsCredentialsFile();
		}

		public void ReadAwsCurrentProfileName()
		{
			AwsCurrentProfileName = _awsCredentialManager.AwsGetCurrentUserProfile();
		}

		public void ClearLogsCommand()
		{
			Logs = "";
		}

		public void SaveConfig()
		{
			var newAppSettings = new AppSettings()
			{
				Aws = new AwsSettings
				{
					AccountId = AwsAccountId,
					UserName = AwsUserName,
					ProfileSource = AwsProfileSource,
					Profile = AwsProfileToEdit,
				},
			};
			_appSettingsWriter.Save(newAppSettings);
		}

	}
}
