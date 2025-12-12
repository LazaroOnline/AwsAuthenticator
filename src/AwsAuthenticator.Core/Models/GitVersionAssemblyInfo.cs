namespace AwsAuthenticator.Core.Models;

public class GitVersionAssemblyInfo
{
	public string AssemblyFileName { get; set; } = "";
	public DateTime CreationDate { get; set; }
	public DateTime ModificationDate { get; set; }
	public string? Version { get; set; }
	public string? FileVersion { get; set; }
	public string? InformationalVersion { get; set; }

	public string DateFormat { get; set; } = "yyyy-MM-dd HH\\:mm zzz";

	public string GetCreationDate { get => CreationDate.ToString(DateFormat); }
	public string GetModificationDate { get => ModificationDate.ToString(DateFormat); }

	public override string ToString()
	{
		var filenameDesc = $@"{nameof(AssemblyFileName)}: {AssemblyFileName}{Environment.NewLine}";
		var hasFilename = !string.IsNullOrWhiteSpace(AssemblyFileName);
		return $@"{(hasFilename? filenameDesc : "")}"
		//	+ $@"{nameof(CreationDate)}: {GetCreationDate}{Environment.NewLine}"
		//	+ $@"{nameof(ModificationDate)}: {GetModificationDate}{Environment.NewLine}"
			+ $@"{nameof(Version)}: {Version}{Environment.NewLine}"
			+ $@"{nameof(FileVersion)}: {FileVersion}{Environment.NewLine}"
			+ $@"{nameof(InformationalVersion)}: {Environment.NewLine}{ToMultilineInformationalVersion(InformationalVersion)}" // This last line can grow a lot, split the label with a new line.
			;
	}

	public string ToShortString()
	{
		return $@"{nameof(Version)}: {Version}{Environment.NewLine}{ToMultilineInformationalVersion(InformationalVersion)}";
	}

	public const string GitVersionPendingPrefix = "--pending-"; // Same as defined in the "GitVersion.yml" file.

	public static string ToMultilineInformationalVersion(string informationalVersion)
	{
		return informationalVersion.Replace(GitVersionPendingPrefix, $"{Environment.NewLine}pending-");

	}
}
