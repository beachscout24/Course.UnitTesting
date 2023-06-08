using FluentAssertions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using UnderstandingDependencies.Api.Models;
using UnderstandingDependencies.Api.Repositories;
using UnderstandingDependencies.Api.Services;
using Xunit;

namespace UnderstandingDependencies.Api.Tests.Unit;

public class UserServiceTests
{
	private readonly UserService _sut;
	private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    public UserServiceTests()
    {
		_sut = new UserService(_userRepository);
    }

    [Fact]
	public async Task GetAllAsync_ShouldreturnEmptyList_WhenNoUsersExist()
	{
		// Arrange
		_userRepository.GetAllAsync().Returns(Array.Empty<User>());
		//Act
		var users = await _sut.GetAllAsync();

		//Assert
		users.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnListOfUsers_WhenUsersExist()
	{
		// Arrange
		var user = new []
		{
			new User
			{
				Id = Guid.NewGuid(),
				FullName = "Eric Clarke",
			}
		};

		_userRepository.GetAllAsync().Returns(user);

		//Act
		var users = await _sut.GetAllAsync();

		//Assert
		users.Should().Contain(user);
		users.Should().ContainSingle(x => x.FullName == "Eric Clarke");
	}
}
