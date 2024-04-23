namespace AwsAuthenticator.UnitTests.Core.Services;

public class CurrentAppTests
{
	[Fact]
	public void GetLocation_ReturnsString()
	{
		var result = CurrentApp.GetLocation();
		result.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public void GetLocation_ReturnsExistingFile()
	{
		var result = CurrentApp.GetLocation();
		var exist = File.Exists(result);
		exist.Should().BeTrue();
	}

	[Fact]
	public void GetFolder_ReturnsExistingFolder()
	{
		var result = CurrentApp.GetFolder();
		var exist = Directory.Exists(result);
		exist.Should().BeTrue();
	}

	[Fact]
	public void GetExecutingAssembly_ReturnsAssembly()
	{
		var result = CurrentApp.GetExecutingAssembly();
		result.Should().NotBeNull();
	}

	[Fact]
	public void GetAssembly_ReturnsAssembly()
	{
		var result = CurrentApp.GetAssembly();
		result.Should().NotBeNull();
	}
}
