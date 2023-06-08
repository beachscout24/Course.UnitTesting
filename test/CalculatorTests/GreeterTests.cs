using FluentAssertions;
using NSubstitute;

namespace CalculatorLibrary.Tests.Unit
{
	public class GreeterTests
	{
		private readonly Greeter _sut;

		private readonly IDateTimeProvider _timeProvider = Substitute.For<IDateTimeProvider>();

        public GreeterTests()
        {
			_sut = new Greeter(_timeProvider);
        }

        [Fact]
		public void GenerateGreetMessage_ShouldSayGoodEvening_WhenItsEvening()
		{
			// Arrange
			_timeProvider.DateTimeNow.Returns(new DateTime(2023, 1, 1, 20, 0, 0));
			// Act
			var result = _sut.GenerateGreetMessage();
			// Assert
			result.Should().Be("Good Evening");
		}

		[Fact]
		public void GenerateGreetMessage_ShouldSayGoodMorning_WhenItsMorning()
		{
			// Arrange
			_timeProvider.DateTimeNow.Returns(new DateTime(2023, 1, 1, 8, 0, 0));
			// Act
			var result = _sut.GenerateGreetMessage();
			// Assert
			result.Should().Be("Good Morning");
		}

		[Fact]
		public void GenerateGreetMessage_ShouldSayGoodAfternoon_WhenItsAfternoon()
		{
			// Arrange
			_timeProvider.DateTimeNow.Returns(new DateTime(2023, 1, 1, 20, 0, 0));
			// Act
			var result = _sut.GenerateGreetMessage();
			// Assert
			result.Should().Be("Good Afternoon");
		}
	}
}
