using Shouldly;
using Xunit.Abstractions;

namespace AmazingCalculator.Tests.Unit;

public class IntCalculatorTests : IAsyncLifetime
{
    private readonly ITestOutputHelper _output;
    private Guid _guid;

    public IntCalculatorTests(ITestOutputHelper output)
    {
        _guid = Guid.NewGuid();
        _output = output;
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return new object[] { 1, 2, 3 };
        yield return new object[] { -5, 5, 0 };
        yield return new object[] { -5, -5, -10 };
    }


    // can use inline data, member data or class data
    [Theory]
    [MemberData(nameof(TestData))]
    // [InlineData(1, 2, 3)]
    // [InlineData(-5, 5, 0)]
    // [InlineData(-5, -5, -10)]
    public void Add_ShouldAddTwoNumbers_WhenInts(int left, int right, int expected)
    {
        // Arrange
        // system under test
        var sut = new IntCalculator();

        _output.WriteLine($"Guid: {_guid}");

        // Act
        var result = sut.Add(left, right);

        // Assert
        // if (result != 3) throw new Exception("1 + 2 should be 3");
        // Assert.Should().Equal(3, result);
        result.ShouldBe(expected);
    }

    [Fact]
    public void Add_ShouldReturnZero_WhenAnOppositePositiveAndNegativeNumberAreAdded()
    {
        // Arrange
        var sut = new IntCalculator();

        _output.WriteLine($"Guid: {_guid}");

        // Act
        var result = sut.Add(1, -1);

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTheNumbersAreIntegers()
    {
        // Arrange
        var sut = new IntCalculator();

        // Act
        var result = sut.Subtract(7, 5);

        // Assert
        result.ShouldBe(2);
    }


    [Fact]
    public void Multiply_ShouldMultiplyTwoNumbers_WhenTheNumbersArePositiveIntegers()
    {
        // Arrange
        var sut = new IntCalculator();

        // Act
        var result = sut.Multiply(6, 9);

        // Assert
        result.ShouldBe(54);
    }

    [Fact]
    public void Multiply_ShouldReturnZero_WhenOneOfTheNumbersIsZero()
    {
        // Arrange
        var sut = new IntCalculator();

        // Act
        var result = sut.Multiply(6, 0);

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public void Divide_ShouldDivideTwoNumbers_WhenNumbersAreDivisible()
    {
        // Arrange
        var sut = new IntCalculator();

        // Act
        var result = sut.Divide(10, 2);

        // Assert
        result.ShouldBe(5);
    }

    [Fact]
    public void Divide_ShouldReturnTheFirstNumber_WhenNumberIsDividedByOne()
    {
        // Arrange
        var sut = new IntCalculator();

        // Act
        var result = sut.Divide(10, 1);

        // Assert
        result.ShouldBe(10);
    }

    public async Task InitializeAsync()
    {
        // _guid = Guid.NewGuid();
        _output.WriteLine("InitializeAsync");
    }

    public async Task DisposeAsync()
    {
        _output.WriteLine("DisposeAsync");
    }
}