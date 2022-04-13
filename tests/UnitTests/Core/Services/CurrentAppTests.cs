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
	public class CurrentAppTests
	{
		[Fact]
		public async Task GetLocation_ReturnsString()
		{
			var result = CurrentApp.GetLocation();
			result.Should().NotBeNullOrWhiteSpace();
		}

		[Fact]
		public async Task GetLocation_ReturnsExistingFile()
		{
			var result = CurrentApp.GetLocation();
			var exist = File.Exists(result);
			exist.Should().BeTrue();
		}

		[Fact]
		public async Task GetFolder_ReturnsExistingFolder()
		{
			var result = CurrentApp.GetFolder();
			var exist = Directory.Exists(result);
			exist.Should().BeTrue();
		}

		[Fact]
		public async Task GetExecutingAssembly_ReturnsAssembly()
		{
			var result = CurrentApp.GetExecutingAssembly();
			result.Should().NotBeNull();
		}

		[Fact]
		public async Task GetAssembly_ReturnsAssembly()
		{
			var result = CurrentApp.GetAssembly();
			result.Should().NotBeNull();
		}
	}
}
