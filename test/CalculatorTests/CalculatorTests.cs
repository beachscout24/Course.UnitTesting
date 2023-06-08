using CalculatorLibrary;
using FluentAssertions;
using System.Collections;

namespace CalculatorTests
{
	public class CalculatorTests : IDisposable
	{
		private Calculator _sut;

		public CalculatorTests()
		{
			_sut = new();
		}

		[Theory(DisplayName = "Testing Add Functionality")]
		[MemberData(nameof(AddTestData))]
		
		public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
		{

			//Act
			var result = _sut.Add(a, b);

			//Assert
			//Assert.Equal(expected, result);
			result.Should().Be(expected);
		}

		[Theory(DisplayName = "Testing Subtract Functionality")]
		[ClassData(typeof(CalculatorSubtractTestData))]
		public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
		{
			var result = _sut.Subtract(a, b);

			result.Should().Be(expected);
		}

		[Theory(DisplayName = "Testing Multiply Functionality")]
		[InlineData(5, 5, 25)]
		[InlineData(50, 0, 0)]
		[InlineData(-5, 5, -25)]
		public void Multiply_ShouldMultiplyTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int expected)
		{
			var result = _sut.Multiply(a, b);

			result.Should().Be(expected);
		}

		[Theory(DisplayName = "Testing Divide Functionality")]
		[InlineData(5, 5, 1)]
		[InlineData(15, 5, 3)]
		[InlineData(-5, 0, -25, Skip = "Ignore Mathematically impossible cases such as divide by zero being impossible")]
		public void Divide_ShouldDivideTwoNumbers_WhenTwoNumbersAreFloats(float a, float b, float expected)
		{
			var result = _sut.Divide(a, b);

			result.Should().Be(expected);
		}

		public void Dispose()
		{
			_sut = null!;
			GC.SuppressFinalize(this);
		}

		public static IEnumerable<object[]> AddTestData => new List<object[]>
		{
			new object[]{5, 5, 10 },
			new object[]{ -5, 5, 0},
			new object[]{ -15, -5, -20}
		};
	}

	public class CalculatorSubtractTestData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			yield return new object[] { 5, 5, 0 };
			yield return new object[] { 15, 5, 10 };
			yield return new object[] { -5, -5, 0 };
			yield return new object[] { -15, -5, -10 };
			yield return new object[] { 5, 10, -5 };
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
