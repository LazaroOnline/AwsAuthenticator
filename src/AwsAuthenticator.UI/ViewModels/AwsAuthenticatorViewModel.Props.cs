namespace AwsAuthenticator.ViewModels;

public partial class AwsAuthenticatorViewModel : BaseViewModel
{

	public string AwsAccountId { get; set; }

	public string AwsUserName { get; set; }

	/// <summary>
	/// Token generator secret key of 64 alpha-numeric characters code from aws console in the 'Manage MFA device' section.
	/// </summary>
	private string _awsMfaGeneratorSecretKey = "";
	public string AwsMfaGeneratorSecretKey
	{
		get => _awsMfaGeneratorSecretKey;
		set => this.RaiseAndSetIfChanged(ref _awsMfaGeneratorSecretKey, value);
	}

	private string _awsProfileSource = AwsCredentialsFile.DEFAULT_PROFILE;
	public string AwsProfileSource
	{
		get => _awsProfileSource;
		set => this.RaiseAndSetIfChanged(ref _awsProfileSource, value);
	}

	private string _awsProfileToEdit = "";
	public string AwsProfileToEdit
	{
		get => _awsProfileToEdit;
		set => this.RaiseAndSetIfChanged(ref _awsProfileToEdit, value);
	}

	public string AwsProfileToEdit_Default { get; set; } = AwsCredentialsFile.DEFAULT_PROFILE_MFA;

	private string _awsTokenCode = "";
	public string AwsTokenCode
	{
		get => _awsTokenCode;
		set => this.RaiseAndSetIfChanged(ref _awsTokenCode, value);
	}

	private List<string> _awsLocalProfileList = new List<string>();
	public List<string> AwsLocalProfileList
	{
		get => _awsLocalProfileList;
		set => this.RaiseAndSetIfChanged(ref _awsLocalProfileList, value);
	}

	public string AwsCurrentProfileName { get; set; } = "";

	private bool _isValidAwsMfaGeneratorSecretKey;
	public bool IsValidAwsMfaGeneratorSecretKey
	{
		get => _isValidAwsMfaGeneratorSecretKey;
		set => this.RaiseAndSetIfChanged(ref _isValidAwsMfaGeneratorSecretKey, value);
	}

	private bool _isEnabledGenerateTokenButton;
	public bool IsEnabledGenerateTokenButton
	{
		get => _isEnabledGenerateTokenButton;
		set => this.RaiseAndSetIfChanged(ref _isEnabledGenerateTokenButton, value);
	}

	private bool _hasAwsTokenCode;
	public bool HasAwsTokenCode
	{
		get => _hasAwsTokenCode;
		set => this.RaiseAndSetIfChanged(ref _hasAwsTokenCode, value);
	}

	private DateTime? _tokenExpirationTime;
	public DateTime? TokenExpirationTime
	{
		get => _tokenExpirationTime;
		set => this.RaiseAndSetIfChanged(ref _tokenExpirationTime, value);
	}

	private double? _tokenSecondsToExpire;
	public double? TokenSecondsToExpire
	{
		get => _tokenSecondsToExpire;
		set => this.RaiseAndSetIfChanged(ref _tokenSecondsToExpire, value);
	}

	private double _tokenTimeLeftPercentage;
	public double TokenTimeLeftPercentage
	{
		get => _tokenTimeLeftPercentage;
		set => this.RaiseAndSetIfChanged(ref _tokenTimeLeftPercentage, value);
	}

	private DateTime? _awsCredentialsFileLastWriteTime;
	public DateTime? AwsCredentialsFileLastWriteTime
	{
		get => _awsCredentialsFileLastWriteTime;
		set => this.RaiseAndSetIfChanged(ref _awsCredentialsFileLastWriteTime, value);
	}

	private string _awsCredentialsFileLastWriteMessage = "";
	public string AwsCredentialsFileLastWriteMessage
	{
		get => _awsCredentialsFileLastWriteMessage;
		set => this.RaiseAndSetIfChanged(ref _awsCredentialsFileLastWriteMessage, value);
	}

}
