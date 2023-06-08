using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using System.Diagnostics;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Tests.Unit
{
    public class UserServiceTests
    {
        private readonly IUserService _sut;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();
        public UserServiceTests()
        {
            _sut = new UserService(_userRepository, _logger);
        }

        [Fact]
        public async Task GetAllSync_ShouldReturnEmptyList_WhenNoUserExist()
        {
            //arrange
            _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

            //act
            var users = await _sut.GetAllAsync();
            //assert
            users.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllSync_ShouldReturnList_WhenUsersExist()
        {
            //arrange
            var users = new[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Test",
                }
            };

            _userRepository.GetAllAsync().Returns(users);

            //act
            var result = await _sut.GetAllAsync();
            //assert
            result.Should().BeEquivalentTo(users);
            result.Should().ContainSingle(x => x.FullName == "Test");
        }

        [Fact]
        public async Task GetAllSync_ShouldLogMessages_WhenInvoked()
        {
            //arrange
            _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

            //act
            await _sut.GetAllAsync();
            //assert
            _logger.Received(1).LogInformation("Retrieving all users");
            _logger.Received(1).LogInformation("All users retrieved in {0}ms", Arg.Any<long>());
        }

        [Fact]
        public async Task GetAllSync_ShouldLogMessageAndException_WhenExceptionIsThrown()
        {
            //arrange
            var sqlLiteException = new SqliteException("Something Went Wrong", 500);

            _userRepository.GetAllAsync().Throws(sqlLiteException);
            //accept
            var resultAction = async () => await _sut.GetAllAsync();
            //assert
            await resultAction.Should()
                .ThrowAsync<SqliteException>()
                .WithMessage("Something went wrong");

            _logger.Received(1).LogError(sqlLiteException, Arg.Is("Something went wrong while retrieving all users"));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExist()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Test",
            };
			

			_userRepository.GetByIdAsync(user.Id).Returns(user);
            // Act
            var result = await _sut.GetByIdAsync(user.Id);
            // Assert
            result.Should().BeEquivalentTo(user);
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnNull_WhenNoUserExist()
		{
            // Arrange
            var id = Arg.Any<Guid>();
			_userRepository.GetByIdAsync(id).ReturnsNull();
			// Act
			var result = await _sut.GetByIdAsync(id);
			// Assert
			result.Should().BeNull();
		}

        [Fact]
        public async Task GetByIdSync_ShouldLogMessages_WhenInvoked()
        {
			// Arrange
			var id = Guid.NewGuid();
			_userRepository.GetByIdAsync(id).ReturnsNull();
			// Act
			await _sut.GetByIdAsync(id);
            // Assert
            _logger.Received(1).LogInformation("Retrieving user with id: {0}", id);
			_logger.Received(1).LogInformation(Arg.Is("User with id {0} retrieved in {1}ms"), Arg.Is(id), Arg.Any<long>());
		}

		[Fact]
		public async Task GetByIdSync_ShouldLogMessagesAndThrowException_WhenExceptionThrown()
		{
			// Arrange
			var sqlLiteException = new SqliteException("Something Went Wrong", 500);
			var id = Arg.Any<Guid>();
			_userRepository.GetByIdAsync(id).Throws(sqlLiteException);
			// Act
			var resultAction = async () => await _sut.GetByIdAsync(id);
			// Assert
			await resultAction.Should()
				.ThrowAsync<SqliteException>()
				.WithMessage("Something went wrong");
			_logger.Received(1).LogError(sqlLiteException, "Something went wrong while retrieving user with id {0}", id);
		}

		[Fact]
		public async Task CreateAsync_ShouldCreateUser_WhenUserIsValid()
		{
			// Arrange

			var user = new User
			{
				Id = Guid.NewGuid(),
				FullName = "Test",
			};


			_userRepository.CreateAsync(user).Returns(true);
			// Act
			var result = await _sut.CreateAsync(user);
			// Assert
			result.Should().BeTrue();
		}

		[Fact]
		public async Task CreateAsync_ShouldLogMessage_WhenInvoked()
		{
			// Arrange

			var user = new User
			{
				Id = Guid.NewGuid(),
				FullName = "Test",
			};

			_userRepository.CreateAsync(user).Returns(true);
			// Act
			await _sut.CreateAsync(user);
            // Assert
            _logger.Received(1).LogInformation("Creating user with id {0} and name: {1}", Arg.Any<Guid>(), Arg.Any<string>());
			_logger.Received(1).LogInformation(Arg.Is("User with id {0} created in {1}ms"), Arg.Any<Guid>(), Arg.Any<long>());
		}

		[Fact]
		public async Task CreateSync_ShouldLogMessagesAndThrowException_WhenExceptionThrown()
		{
			// Arrange
			var sqlLiteException = new SqliteException("Something Went Wrong", 500);
			var user = new User
			{
				Id = Guid.NewGuid(),
				FullName = null!,
			};

			_userRepository.CreateAsync(user).Throws(sqlLiteException);
			// Act
			var resultAction = async () => await _sut.CreateAsync(user);
			// Assert
			await resultAction.Should()
				.ThrowAsync<SqliteException>()
				.WithMessage("Something went wrong");
			_logger.Received(1).LogError(sqlLiteException, "Something went wrong while creating a user");
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldReturnUser_WhenUserExist()
		{
			// Arrange
			var userId = Guid.NewGuid();

			_userRepository.DeleteByIdAsync(userId).Returns(true);
			// Act
			var result = await _sut.DeleteByIdAsync(userId);
			// Assert
			result.Should().BeTrue();
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldReturnNull_WhenNoUserExist()
		{
			// Arrange
			var id = Arg.Any<Guid>();
			_userRepository.DeleteByIdAsync(id).Returns(false);
			// Act
			var result = await _sut.DeleteByIdAsync(id);
			// Assert
			result.Should().BeFalse();
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldLogMessages_WhenInvoked()
		{
			// Arrange
			var id = Arg.Any<Guid>();
			_userRepository.DeleteByIdAsync(id).Returns(true);
			// Act
			await _sut.DeleteByIdAsync(id);
			// Assert
			_logger.Received(1).LogInformation("Deleting user with id: {0}", id);
			_logger.Received(1).LogInformation("User with id {0} deleted in {1}ms", id, Arg.Any<long>());
		}

		[Fact]
		public async Task DeleteByIdAsync_ShouldLogMessagesAndThrowException_WhenExceptionThrown()
		{
			// Arrange
			var sqlLiteException = new SqliteException("Something Went Wrong", 500);
			var id = Arg.Any<Guid>();
			_userRepository.DeleteByIdAsync(id).Throws(sqlLiteException);
			// Act
			var resultAction = async () => await _sut.DeleteByIdAsync(id);
			// Assert
			await resultAction.Should()
				.ThrowAsync<SqliteException>()
				.WithMessage("Something went wrong");
			_logger.Received(1).LogError(sqlLiteException, "Something went wrong while deleting user with id {0}", id);
		}
	}
}
