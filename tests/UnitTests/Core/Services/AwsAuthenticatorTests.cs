﻿namespace AwsAuthenticator.UnitTests.Core.Services;

public class AwsAuthenticatorTests
{
	[Fact]
	public void GetAwsLocalProfileList_ReturnsList()
	{
		var service = GetServiceInstance();
		var profiles = service.GetAwsLocalProfileList();
		profiles.Should().NotBeNull();
	}

	private readonly Mock<IAwsCredentialUpdater> _mockAwsCredentialUpdater = new Mock<IAwsCredentialUpdater>();
	private readonly Mock<AwsCliService> _mockAwsCliService = new Mock<AwsCliService>(); // Consider creating an interface for IAwsCliService.

	private AwsAuthenticator.Core.Services.AwsAuthenticator GetServiceInstance()
	{
		var service = new AwsAuthenticator.Core.Services.AwsAuthenticator(
			 _mockAwsCredentialUpdater.Object
			,_mockAwsCliService.Object
		);
		return service;
	}

}
