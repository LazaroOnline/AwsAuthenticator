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

namespace AwsCredentialManager.UnitTests.Core.Services.CommandLine
{
	public class AwsCliServiceTests
	{

		[Fact]
		public async Task GetAwsLocalProfileList_ReturnsList()
		{
			var service = new AwsCliService();
			var profiles = service.GetAwsLocalProfileList();
			profiles.Should().NotBeNull();
		}

		[Fact]
		public async Task EnsureAwsCredentialsFileEndsWithNewLine_DoesntThrowExceptions()
		{
			var service = new AwsCliService();
			var act = () => service.EnsureAwsCredentialsFileEndsWithNewLine();
			act.Should().NotThrow();
		}

		[Fact]
		public async Task GetProfile_ReturnsString()
		{
			var service = new AwsCliService();
			var result = service.GetProfile();
			result.Should().NotBeNullOrWhiteSpace();
		}

		[Fact]
		public async Task GetProfile_Cmd_ReturnsString()
		{
			var service = new AwsCliService();
			var result = service.GetProfile_Cmd();
			result.Should().NotBeNullOrWhiteSpace();
		}

	}
}
