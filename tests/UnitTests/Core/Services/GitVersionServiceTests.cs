namespace AwsAuthenticator.UnitTests.Core.Services;

public class GitVersionServiceTests
{
	[Fact]
	public void GitVersionServiceTest()
	{
		var result = GitVersionService.GetGitVersionAssemblyInfo();
		result.Should().NotBeNull();
	}
}
