namespace AwsAuthenticator.Core.Services;

[Obsolete($"Use {nameof(AwsCredentialUpdaterCmd)} instead, since it is managed by AWS CLI and thus more reliable.")]
public class AwsCredentialUpdaterRegex : IAwsCredentialUpdater
{

	public void EditAwsCredsFile(string profileName, AwsCredentials? creds)
	{
		var filePath = AwsCredentialsFile.FilePath;
		var fileContent = File.ReadAllText(filePath);
		var fileContentEdited = EditAwsCredsFile(fileContent, profileName, creds);
		File.WriteAllText(filePath, fileContentEdited);
	}

	public string EditAwsCredsFile(string credentialsFileContent, string profileName, AwsCredentials? creds)
	{
		var edited = credentialsFileContent;
		var fileContentProfileSection = GetFileContentProfileSection(credentialsFileContent, profileName);
		var fileContentProfileSectionEdited = fileContentProfileSection;

		fileContentProfileSectionEdited = ReplaceAwsCredProperty(AwsCredentialsFile.Properties.AWS_ACCESS_KEY_ID,     creds?.AccessKeyId, fileContentProfileSectionEdited);
		fileContentProfileSectionEdited = ReplaceAwsCredProperty(AwsCredentialsFile.Properties.AWS_SECRET_ACCESS_KEY, creds?.SecretAccessKey, fileContentProfileSectionEdited);
		fileContentProfileSectionEdited = ReplaceAwsCredProperty(AwsCredentialsFile.Properties.AWS_SESSION_TOKEN,     creds?.SessionToken, fileContentProfileSectionEdited);

		edited = edited.Replace(fileContentProfileSection, fileContentProfileSectionEdited);

		return edited;
	}

	public string GetFileContentProfileSection(string credentialsFileContent, string profileName)
	{
		var match = Regex.Match(credentialsFileContent,
			$@"\[{Regex.Escape(profileName)}\].*(\[|$)",
			RegexOptions.IgnoreCase |
			RegexOptions.Singleline
		);

		return match.Value;
	}

	public string ReplaceAwsCredProperty(string propertyName, string newValue, string fileContentProfileSection)
	{
		return Regex.Replace(
			fileContentProfileSection,
			$@"\s*{Regex.Escape(propertyName)}\s*=\s*(.*)([\r\n])",
			$"\r\n{propertyName} = {newValue}\r\n",
			RegexOptions.IgnoreCase |
			RegexOptions.CultureInvariant
		);
	}

	public string AwsGetCurrentUserProfile()
	{
		throw new NotImplementedException();
	}

}
