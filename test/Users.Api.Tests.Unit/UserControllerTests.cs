using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NSubstitute.ReturnsExtensions;
using Users.Api.Contracts;
using Users.Api.Controllers;
using Users.Api.Mappers;
using Users.Api.Models;
using Users.Api.Services;

namespace Users.Api.Tests.Unit
{
    public class UserControllerTests
    {
        private readonly UserController _sut;
        private readonly IUserService _userService = Substitute.For<IUserService>();


        public UserControllerTests()
        {
            _sut = new UserController(_userService);
        }

        [Fact]
        public async Task GetById_ReturnsOkAndObject_WhenUserExist()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Test"
            };

            _userService.GetByIdAsync(user.Id).Returns(user);
            var userResponse = user.ToUserResponse();
            // Act
            var result = (OkObjectResult) await _sut.GetById(user.Id);
            // Assert
            result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(userResponse);
        }

		[Fact]
		public async Task GetById_ReturnsNotFound_WhenNoUserExist()
		{
            // Arrange
            var userId = Arg.Any<Guid>();
			_userService.GetByIdAsync(userId).ReturnsNull();
			// Act
			var result = (NotFoundResult)await _sut.GetById(userId);
			// Assert
			result.StatusCode.Should().Be(404);
		}

		[Fact]
		public async Task GetAll_ReturnsOkEmptyList_WhenNoUsersExist()
		{
            // Arrange
            _userService.GetAllAsync().Returns(Enumerable.Empty<User>());
			// Act
			var result = (OkObjectResult)await _sut.GetAll();
			// Assert
			result.StatusCode.Should().Be(200);
            result.Value.As<IEnumerable<UserResponse>>().Should().BeEmpty();
		}

		[Fact]
		public async Task GetAll_ReturnUserResponse_WhenUsersExist()
		{
			// Arrange
			var users = new[]
			{
				new User
				{
					Id = Guid.NewGuid(),
					FullName = "Test",
				}
			};

			var usersResponse = users.Select(u => u.ToUserResponse());
			_userService.GetAllAsync().Returns(users);
			
			// Act
			var result = (OkObjectResult)await _sut.GetAll();
			// Assert
			result.StatusCode.Should().Be(200);
			result.Value.Should().BeEquivalentTo(usersResponse);
		}

		[Fact]
		public async Task Create_ReturnsBadrequest_WhenUsersWasntCreated()
		{
			// Arrange
			_userService.CreateAsync(Arg.Any<User>()).Returns(false);
			// Act
			var result = (BadRequestResult)await _sut.Create(new CreateUserRequest());
			// Assert
			result.StatusCode.Should().Be(400);
		}

		[Fact]
		public async Task Create_ShouldReturnUserResponse_WhenUserCreated()
		{
			// Arrange
			var createdUser = new CreateUserRequest
			{
				FullName = "Test",
			};

			var user = new User {
				Id = Guid.NewGuid(),
				FullName = createdUser.FullName,
			};
			_userService.CreateAsync(Arg.Do<User>(x => user = x)).Returns(true);
			
			// Act
			var result = (CreatedAtActionResult)await _sut.Create(createdUser);
			// Assert
			var userResponse = user.ToUserResponse();
			result.StatusCode.Should().Be(201);
			result.Value.As<UserResponse>().Should().BeEquivalentTo(userResponse);
			result.RouteValues!["id"].Should().BeEquivalentTo(user.Id);
		}

		[Fact]
		public async Task DeleteById_ReturnsOk_WhenUserDeleted()
		{
			// Arrange
			_userService.DeleteByIdAsync(Arg.Any<Guid>()).Returns(true);
			
			// Act
			var result = (OkResult)await _sut.DeleteById(Guid.NewGuid());
			// Assert
			result.StatusCode.Should().Be(200);
		}

		[Fact]
		public async Task DeleteById_ReturnsNotFound_WhenNotDeleted()
		{
			// Arrange
			var userId = Arg.Any<Guid>();
			_userService.DeleteByIdAsync(userId).Returns(false);
			// Act
			var result = (NotFoundResult)await _sut.DeleteById(Guid.NewGuid());
			// Assert
			result.StatusCode.Should().Be(404);
		}
	}
}
