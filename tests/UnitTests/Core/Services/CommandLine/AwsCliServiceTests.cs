using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AwsAuthenticator.Core.Services;

namespace AwsAuthenticator.UnitTests.Core.Services.CommandLine
{
	public class AwsCliServiceTests
	{

		[Fact]
		public void GetAwsLocalProfileList_ReturnsList()
		{
			var service = new AwsCliService();
			var profiles = service.GetAwsLocalProfileList();
			profiles.Should().NotBeNull();
		}

		[Fact]
		public void EnsureAwsCredentialsFileEndsWithNewLine_DoesntThrowExceptions()
		{
			var service = new AwsCliService();
			var act = () => service.EnsureAwsCredentialsFileEndsWithNewLine();
			act.Should().NotThrow();
		}

		[Fact]
		public void GetProfile_ReturnsString()
		{
			var service = new AwsCliService();
			var result = service.GetProfile();
			result.Should().NotBeNullOrWhiteSpace();
		}

		[Fact]
		public void GetProfile_Cmd_ReturnsString()
		{
			var service = new AwsCliService();
			var result = service.GetProfile_Cmd();
			result.Should().NotBeNullOrWhiteSpace();
		}

	}
}
