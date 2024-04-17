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


namespace AwsAuthenticator.UnitTests.Core.Services
{
	public class GitVersionServiceTests
	{
		[Fact]
		public void GitVersionServiceTest()
		{
			var result = GitVersionService.GetGitVersionAssemblyInfo();
			result.Should().NotBeNull();
		}
	}
}
