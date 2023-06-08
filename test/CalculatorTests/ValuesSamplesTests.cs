using FluentAssertions;
using TestingTechniques;

namespace CalculatorLibrary.Tests.Unit
{
	public class ValuesSamplesTests
	{
		private readonly ValueSamples _sut = new();

		[Fact(DisplayName = "Test FullName")]
		public void FullName_FullNameShouldBeEricClarke_WhenGivenString()
		{
			var fullName = _sut.FullName;

			fullName.Should().Be("Eric Clarke");
			fullName.Should().NotBeEmpty();
			fullName.Should().StartWith("Eric");
			fullName.Should().EndWith("Clarke");
		}

		[Fact(DisplayName = "Test Age")]
		public void Age_AgeShouldBe53_WhenGivenInteger()
		{
			var age = _sut.Age;

			age.Should().Be(53);
			age.Should().BeInRange(50, 60);
			age.Should().BePositive();
			age.Should().BeGreaterThan(21);
			age.Should().BeLessThanOrEqualTo(53);
		}

		[Fact(DisplayName = "Test Date")]
		public void DateOfBirth_DateOfBirthShouldBeJune91970_WhenGivenDateOfBirth()
		{
			var dateOfBirth = _sut.DateOfBirth;

			dateOfBirth.Should().Be(new(1970, 6, 9));
			dateOfBirth.Should().BeBefore(new(1970, 11, 04));
			dateOfBirth.Should().BeAfter(new (1970, 04, 11));
			dateOfBirth.Should().HaveDay(9);
			dateOfBirth.Should().HaveMonth(6);
			dateOfBirth.Should().HaveYear(1970);
		}

		[Fact(DisplayName = "Test User Object")]
		public void User_UserShouldBeObject_WhenGivenObject()
		{
			var expected = new User
			{
				FullName = "Eric Clarke",
				Age = 53,
				DateOfBirth = new(1970, 6, 9)
			};

			var user = _sut.AppUser;

			expected.Should().BeEquivalentTo(user);
		}

		[Fact(DisplayName = "Test IEnumerable User Object")]
		public void IEnumerableUser_IEnumerableUserShouldBeListOfUser_WhenGivenList()
		{
			var expected = new User
			{
				FullName = "Eric Clarke",
				Age = 53,
				DateOfBirth = new(1970, 6, 9)
			};

			var users = _sut.Users;

			users.Should().ContainEquivalentOf(expected);
			users.Should().HaveCount(3);
			users.Should().Contain(x => x.Age < 55 && x.FullName.StartsWith("Eric"));
			users.Should().NotBeNullOrEmpty();
		}

		[Fact(DisplayName = "Test IEnumerable Numbers")]
		public void IEnumerableNumbers_IEnumerableNumbersShouldBeListOfNumbers_WhenGivenList()
		{
			var numbers = _sut.Numbers;

			numbers.Should().HaveCount(4);
			numbers.Should().Contain(15);
			numbers.Should().NotBeNullOrEmpty();
		}

		[Fact(DisplayName = "Test Divison By Zero")]
		public void Exception_ShouldThrowException_WhenGivenDividingByZero()
		{
			var calculator = new Calculator();

			Action result = () => calculator.Divide(5, 0);

			result.Should().Throw<DivideByZeroException>().WithMessage("Attempted to divide by zero.");
		}

		[Fact(DisplayName = "Event Handler")]
		public void Event_ShouldHandleEvent_WhenGivenEventThrown()
		{
			var monitorSubject = _sut.Monitor();

			_sut.RaiseExampleEvent();

			monitorSubject.Should().Raise("ExampleEvent");
		}

		[Fact(DisplayName = "Test Internal Number")]
		public void Number_ShouldBe42_WhenGivenInt()
		{
			var number = _sut.InternalSecretNumber;

			number.Should().Be(42);
		}
	}
}
