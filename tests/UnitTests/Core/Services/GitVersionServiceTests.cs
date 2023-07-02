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
