using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AwsCredentialManager.Core.Services;

namespace AwsCredentialManager.UnitTests.Core.Services
{
	public class AwsCredentialManagerTests
	{
		[Fact]
		public async Task GetAwsLocalProfileList_ReturnsList()
		{
			var service = GetServiceInstance();
			var profiles = service.GetAwsLocalProfileList();
			profiles.Should().NotBeNull();
		}

		private readonly Mock<IAwsCredentialUpdater> _mockAwsCredentialUpdater = new Mock<IAwsCredentialUpdater>();
		private readonly Mock<AwsCliService> _mockAwsCliService = new Mock<AwsCliService>(); // Consider creating an interface for IAwsCliService.

		private AwsCredentialManager.Core.Services.AwsCredentialManager GetServiceInstance()
		{
			var service = new AwsCredentialManager.Core.Services.AwsCredentialManager(
				 _mockAwsCredentialUpdater.Object
				,_mockAwsCliService.Object
			);
			return service;
		}

	}
}
