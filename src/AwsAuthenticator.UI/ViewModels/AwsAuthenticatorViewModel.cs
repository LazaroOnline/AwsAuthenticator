using System.IO;
using System.Text.RegularExpressions;
using Humanizer;
using Splat;

namespace AwsAuthenticator.ViewModels;

public partial class AwsAuthenticatorViewModel : BaseViewModel
{
	private readonly IAwsAuthenticator _awsAuthenticator;
	private readonly ITaskSchedulerService _taskSchedulerService;
	private readonly AppSettingsWriter _appSettingsWriter;

	// Constructor required by the designer tools.
	public AwsAuthenticatorViewModel()
		: this(null, null, null, null) // Call the other constructor
	{
		System.Diagnostics.Debug.WriteLine($"Default constructor used for {nameof(AwsAuthenticatorViewModel)}.");
	}

	public AwsAuthenticatorViewModel(
		 IAwsAuthenticator? awsAuthenticator
		,ITaskSchedulerService? taskSchedulerService = null
		,AppSettingsWriter? appSettingsWriter = null
		,AwsSettings? awsSettings = null
	)
	{
		_awsAuthenticator = awsAuthenticator ?? Splat.Locator.Current.GetService<IAwsAuthenticator>();
		_taskSchedulerService = taskSchedulerService ?? Splat.Locator.Current.GetService<ITaskSchedulerService>();
		_appSettingsWriter = appSettingsWriter ?? Splat.Locator.Current.GetService<AppSettingsWriter>();
		awsSettings = awsSettings ?? Splat.Locator.Current.GetService<AwsSettings>();

		//_awsAuthenticator = awsAuthenticator ?? new Core.AwsAuthenticator();
		//_taskSchedulerService = taskSchedulerService ?? new Core.TaskSchedulerService();
		//_appSettingsWriter = appSettingsWriter ?? new AppSettingsWriter();

		AwsAccountId = awsSettings?.AccountId ?? "";
		AwsUserName = awsSettings?.UserName ?? "";
		AwsProfileSource = awsSettings?.ProfileSource ?? "";
		AwsProfileToEdit = awsSettings?.Profile ?? "";
		AwsMfaGeneratorSecretKey = awsSettings?.MfaGeneratorSecretKey ?? "";
		AwsTokenCode = awsSettings?.TokenCode ?? "";

		RefreshAwsCredentialsFileLastWriteTime();
		SetWatcherForAwsCredentialsFileLastWriteTime();
		_ = ConstantRefreshAwsCredentialsFileLastWriteMessage();

		if (string.IsNullOrEmpty(AwsUserName)) {
			AwsUserName = UserInfo.GetUserFullName();
		}

		AwsLocalProfileList = _awsAuthenticator?.GetAwsLocalProfileList();

		this.WhenAnyValue(x => x.AwsMfaGeneratorSecretKey).Subscribe(x => {
			this.IsValidAwsMfaGeneratorSecretKey = GetIsValidAwsMfaGeneratorSecretKey();
		});

		this.WhenAnyValue(x => x.AwsMfaGeneratorSecretKey, x => x.IsLoading).Subscribe(x => {
			this.IsEnabledGenerateTokenButton = GetIsEnabledGenerateTokenButton();
		});

		this.WhenAnyValue(x => x.AwsTokenCode).Subscribe(x => {
			this.HasAwsTokenCode = !GetIsAwsTokenCodeEmpty();
			ClearTokenExpiration();
		});
	}

	public bool GetIsEnabledGenerateTokenButton() =>
		IsValidAwsMfaGeneratorSecretKey && !IsLoading;

	public bool GetIsAwsTokenCodeEmpty() =>
		string.IsNullOrWhiteSpace(AwsTokenCode);


	public async Task AutoUpdateCredentialsCommand()
	{
		await GenerateTokenCommand();
		await UpdateCredentialsCommand();
	}

	public void AwsProfileSourceClearCommand()
	{
		AwsProfileSource = "";
	}

	public async Task AwsProfileToEditClearCommand()
	{
		AwsProfileToEdit = "";
	}

	public async Task AwsTokenCodeClearCommand()
	{
		AwsTokenCode = "";
	}

	public async Task GenerateTokenCommand()
	{
		// This command is fast, it could run synchronously if required.
		// WithExceptionLogging(async () => {
		await ExecuteAsyncWithLoadingAndExceptionLogging(() => {
			SetGeneratedTokenFromAuthenticatorKey();
			if (!string.IsNullOrEmpty(AwsTokenCode))
			{
				LogsColorSetInfo();
				Logs = $"TOKEN-CODE GENERATED: {AwsTokenCode}    ({DateTime.Now.ToString(DATE_FORMAT)})";
			}
		});
	}

	public async Task CopyTokenCodeCommand()
	{
		// This command is fast, it could run synchronously if required.
		// WithExceptionLogging(async () => {
		await ExecuteAsyncWithLoadingAndExceptionLogging(async () => {
			await Clipboard.SetTextAsync(AwsTokenCode);
		});
	}

	public async Task GenerateTokenAndCopyToClipboardCommand()
	{
		SetGeneratedTokenFromAuthenticatorKey();
		var isValidToken = !string.IsNullOrWhiteSpace(AwsTokenCode);
		if (isValidToken) {
			// https://docs.avaloniaui.net/docs/input/clipboard
			await Clipboard.SetTextAsync(AwsTokenCode);
		}
		else
		{
			// Show the errors from the logs:
			await Clipboard.SetTextAsync(Logs);
		}
	}

	public async Task SetGeneratedTokenFromAuthenticatorKey(bool keepRegeneratingTokens = false)
	{
		do
		{
			AwsTokenCode = GenerateTokenFromAuthenticatorKey();
			SetTokenExpiration();
			var refreshExpirationTask = RefreshTokenExpirationInfoUntilExpires();
			if (keepRegeneratingTokens)
			{
				await refreshExpirationTask;
			}
		}
		while (keepRegeneratingTokens);
	}

	public string GenerateTokenFromAuthenticatorKey()
	{
		return GenerateTokenFromAuthenticatorKey(AwsMfaGeneratorSecretKey);
	}

	public string GenerateTokenFromAuthenticatorKey(string mfaGeneratorSecretKey)
	{
		var validationErrors = ValidateMfaGeneratorSecretKey(mfaGeneratorSecretKey);
		if (validationErrors.Any())
		{
			LogsColorSetWarning();
			Logs = "Validation error:\r\n" + string.Join("\r\n", validationErrors);
			return "";
		}
		var authenticator = new TwoStepsAuthenticator.TimeAuthenticator();
		var tokenCode = authenticator.GetCode(mfaGeneratorSecretKey);
		return tokenCode;
	}

	public void SetTokenExpiration()
	{
		this.TokenExpirationTime = _awsAuthenticator.GetNextTokenExpirationTime();
	}

	public async Task RefreshTokenExpirationInfoUntilExpires()
	{
		do
		{
			RefreshTokenExpirationInfo();
			if (TokenSecondsToExpire != null)
			{
				await Task.Delay(1000);
			}
		}
		while (TokenSecondsToExpire != null);
		Console.WriteLine("Refresh ended");
	}

	public void RefreshTokenExpirationInfo()
	{
		var utcNow = DateTime.UtcNow;
		if (TokenExpirationTime != null && TokenExpirationTime > utcNow)
		{
			this.TokenSecondsToExpire = (TokenExpirationTime.Value - utcNow).Seconds;
			this.TokenTimeLeftPercentage = 100 * (TokenSecondsToExpire.Value / 30);
		}
		else
		{
			ClearTokenExpiration();
		}
	}

	public void ClearTokenExpiration()
	{
		this.TokenExpirationTime = null;
		this.TokenSecondsToExpire = null;
		this.TokenTimeLeftPercentage = 0;
	}

	public bool GetIsValidAwsMfaGeneratorSecretKey() => !ValidateMfaGeneratorSecretKey().Any();

	public IList<string> ValidateMfaGeneratorSecretKey() => ValidateMfaGeneratorSecretKey(AwsMfaGeneratorSecretKey);

	public IList<string> ValidateMfaGeneratorSecretKey(string? mfaSecretKey)
	{
		var errors = new List<string>();

		var fieldName = "Authenticator secret key";

		if (string.IsNullOrWhiteSpace(mfaSecretKey))
		{
			return [$"'{fieldName}' is empty."];
		}

		/*
		// Length validation removed to allow using the MFA generator for other providers.
		if (mfaSecretKey.Length != Core.Services.AwsAuthenticator.MFA_DEVICE_GENERATOR_SECRET_KEY_LENGTH)
		{
			return [$"'{fieldName}' length was {mfaSecretKey.Length}, length should be {Core.Services.AwsAuthenticator.MFA_DEVICE_GENERATOR_SECRET_KEY_LENGTH}."];
		}
		*/

			try
			{
			// Test the Base32 encoder required during token generation:
			// https://github.com/glacasa/TwoStepsAuthenticator/blob/main/TwoStepsAuthenticator/Authenticator.cs#L10
			var key = TwoStepsAuthenticator.Base32Encoding.ToBytes(mfaSecretKey);
		}
		catch (ArgumentException ex)
		{
			if (ex.Message.Contains("Character is not a Base32 character"))
			{
				errors.Add($"Invalid '{fieldName}'. It should be a Base32 set of characters.");
			}
		}
		catch (Exception ex)
		{
			errors.Add($"Invalid '{fieldName}'. Failed converting to Base32.\r\n{ex.GetType()}: {ex.Message}");
		}
		return errors;
	}

	public IList<string> ValidateTokenAws() => ValidateTokenAws(AwsTokenCode);

	public IList<string> ValidateTokenAws(string? token)
	{
		var fieldName = "TokenCode";
		if (string.IsNullOrWhiteSpace(token))
		{
			return [$"{fieldName} is empty."];
		}
		if (token.Length < Core.Services.AwsAuthenticator.TOKEN_AWS_MIN_LENGTH)
		{
			return [$"Invalid length for parameter {fieldName}, value: {token.Length}, valid min length: {Core.Services.AwsAuthenticator.TOKEN_AWS_MIN_LENGTH}."];
		}
		return [];
	}

	public IList<string> ValidateAwsAccountId() => ValidateAwsAccountId(AwsAccountId);

	public IList<string> ValidateAwsAccountId(string? awsAccountId)
	{
		var fieldName = "Aws Account Id";
		if (string.IsNullOrWhiteSpace(awsAccountId))
		{
			return [$"The field '{fieldName}' is required."];
		}
		if (awsAccountId.Length != Core.Services.AwsAuthenticator.AWS_ACCOUNT_ID_LENGTH)
		{
			return [$"Invalid length for parameter {fieldName}, value: {awsAccountId.Length}, valid length: {Core.Services.AwsAuthenticator.AWS_ACCOUNT_ID_LENGTH}."];
		}

		var awsAccountIdValidationRegeex = new Regex(@"^\d{12}$");
		var isValidPattern = awsAccountIdValidationRegeex.IsMatch(awsAccountId);
		if (!isValidPattern)
		{
			return [$"The field '{fieldName}' must be a 12 digits number."];
		}

		return [];
	}

	public IList<string> ValidateAwsUserName() => ValidateAwsUserName(AwsUserName);

	public IList<string> ValidateAwsUserName(string? awsUserName)
	{
		var fieldName = "Aws User Name";
		if (string.IsNullOrWhiteSpace(awsUserName))
		{
			return [$"The field '{fieldName}' is required."];
		}
		return [];
	}

	public IList<string> ValidateUpdateCredentialsCommand()
	{
		var errors = new List<string>();

		var hasEmptyToken = string.IsNullOrWhiteSpace(AwsTokenCode);

		if (hasEmptyToken)
		{
			var errorsInMfaGeneratorSecretKey = ValidateMfaGeneratorSecretKey();
			errors.AddRange(errorsInMfaGeneratorSecretKey);
		}
		else
		{
			var errorsInTokenCode = ValidateTokenAws();
			errors.AddRange(errorsInTokenCode);
		}

		var errorsInAwsAccountId = ValidateAwsAccountId();
		errors.AddRange(errorsInAwsAccountId);

		var errorsInAwsUserName = ValidateAwsUserName();
		errors.AddRange(errorsInAwsUserName);

		return errors;
	}

	public string GetAwsProfileToEdit()
	{
		return string.IsNullOrWhiteSpace(AwsProfileToEdit) ? AwsProfileToEdit_Default : AwsProfileToEdit;
	}

	public async Task UpdateCredentialsCommand()
	{
		await ExecuteAsyncWithLoadingAndExceptionLogging(() => {
			var validationErrors = ValidateUpdateCredentialsCommand();
			if (validationErrors.Any()) {
				LogsColorSetWarning();
				Logs = "Validation error:\r\n" + string.Join("\r\n", validationErrors);
				return;
			}
			var hasEmptyToken = string.IsNullOrWhiteSpace(AwsTokenCode);
			if (hasEmptyToken && IsValidAwsMfaGeneratorSecretKey)
			{
				SetGeneratedTokenFromAuthenticatorKey();
			}
			LogsColorSetInfo();
			Logs = "Updating...";
			// System.Threading.Thread.Sleep(6000); // Use this line for testing the loading spinner.
			var profileToEdit = GetAwsProfileToEdit();
			try
			{
				_awsAuthenticator.UpdateAwsAccount(AwsAccountId, AwsUserName, AwsTokenCode, AwsProfileSource, profileToEdit);
				LogsColorSetSuccess();
				Logs = $"Updated credentials successfully at {DateTime.Now.ToString(DATE_FORMAT)}.";
			}
			catch (FailToGetCredentialsException ex) {
				LogsColorSetError();
				Logs = $"ERROR UPDATING CREDENTIALS ({DateTime.Now.ToString(DATE_FORMAT)})\r\n{ex.Message}";
			}
			RefreshAwsCredentialsFileLastWriteTime();
		});
	}
	
	public void RefreshAwsCredentialsFileLastWriteTime()
	{
		try {
			AwsCredentialsFileLastWriteTime = File.GetLastWriteTimeUtc(AwsCredentialsFile.FilePath);
		} catch {
			AwsCredentialsFileLastWriteTime = null;
		}
		RefreshAwsCredentialsFileLastWriteMessage();
	}

	public void RefreshAwsCredentialsFileLastWriteMessage()
	{
		if (AwsCredentialsFileLastWriteTime == null) {
			AwsCredentialsFileLastWriteMessage = "";
		} else {
			var timeSinceLastWrite = DateTime.UtcNow - AwsCredentialsFileLastWriteTime;
			AwsCredentialsFileLastWriteMessage = $"Aws credentials file last updated:\r\n{AwsCredentialsFileLastWriteTime?.ToLocalTime().ToString("yyy-M-d HH:mm")}\r\n{timeSinceLastWrite?.Humanize()} ago.";
		}
	}

	public async Task ConstantRefreshAwsCredentialsFileLastWriteMessage()
	{
		do
		{
			RefreshAwsCredentialsFileLastWriteMessage();
			var timeToNextRefresh = CalculateTimeToNextRefreshAwsCredentialsFileLastWriteMessage();
			await Task.Delay(timeToNextRefresh);
		} while (true);
	}

	public TimeSpan CalculateTimeToNextRefreshAwsCredentialsFileLastWriteMessage()
	{
		var timeSinceLastWrite = DateTime.UtcNow - AwsCredentialsFileLastWriteTime;
		if (timeSinceLastWrite < TimeSpan.FromMinutes(1)) { return TimeSpan.FromSeconds(1); }
		if (timeSinceLastWrite < TimeSpan.FromHours(1)) { return TimeSpan.FromMinutes(1); }
		if (timeSinceLastWrite < TimeSpan.FromDays(1)) { return TimeSpan.FromHours(1); }
		return TimeSpan.FromDays(1);
	}

	private FileSystemWatcher? watcher;

	public void SetWatcherForAwsCredentialsFileLastWriteTime()
	{
		try
		{
			// https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-8.0
			var filePath = AwsCredentialsFile.FilePath;
			watcher = new FileSystemWatcher();
			var fileDir = Path.GetDirectoryName(filePath);
			var fileName = Path.GetFileName(filePath);
			watcher.Path = fileDir ?? "";
			watcher.Filter = fileName;
			watcher.IncludeSubdirectories = false;
			watcher.EnableRaisingEvents = true;
			watcher.NotifyFilter = NotifyFilters.LastWrite;
			watcher.Changed += OnChangedAwsCredentialsFile;
			// watcher.Changed += (_, __) => RefreshAwsCredentialsFileLastWriteTime();
		}
		catch (Exception ex)
		{
			// Could not watch the file.
			Console.WriteLine($"Could not set file watcher for: {AwsCredentialsFile.FilePath} \r\n{ex}");
		}
	}

	private void OnChangedAwsCredentialsFile(object sender, FileSystemEventArgs e)
	{
		// if (e.ChangeType == WatcherChangeTypes.Changed)
		RefreshAwsCredentialsFileLastWriteTime();
	}

	public async Task AddToTaskSchedulerCommand()
	{
		await ExecuteAsyncWithLoadingAndExceptionLogging(() => {
			var alreadyExist = _taskSchedulerService?.ExistsDailyTask() ?? false;
			LogsColorSetInfo();
			Logs = $"{(alreadyExist ? "Overwritting" : "Creating new")} Windows Task-Scheduler task '{TaskSchedulerService.DailyTaskName}'";
			_taskSchedulerService?.AddDailyTask();
			Logs += $" done.";
		});
	}

	public void OpenAwsCredentialsFileCommand()
	{
		_awsAuthenticator.OpenAwsCredentialsFile();
	}

	private const string AwsUserSecurityCredentialsWebPage = "https://console.aws.amazon.com/iam/home#/security_credentials?section=IAM_credentials";
	public void OpenAwsUserSecurityCredentialsWebPageCommand()
	{
		AwsUserSecurityCredentialsWebPage.TryOpenUrlInBrowser();
	}

	public void ReadAwsCurrentProfileName()
	{
		AwsCurrentProfileName = _awsAuthenticator.AwsGetCurrentUserProfile();
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
				MfaGeneratorSecretKey = AwsMfaGeneratorSecretKey,
			},
		};
		_appSettingsWriter.Save(newAppSettings);
	}
}
