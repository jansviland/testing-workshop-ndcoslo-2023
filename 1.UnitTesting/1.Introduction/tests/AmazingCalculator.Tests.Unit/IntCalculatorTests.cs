using Shouldly;

namespace AmazingCalculator.Tests.Unit;

public class IntCalculatorTests
{
    [Fact]
    public void Add_ShouldAddTwoNumbers_WhenInts()
    {
        // Arrange
        // system under test
        var sut = new IntCalculator();

        // Act
        var result = sut.Add(1, 2);

        // Assert
        // if (result != 3) throw new Exception("1 + 2 should be 3");
        // Assert.Should().Equal(3, result);
        result.ShouldBe(3);
    }

    [Fact]
    public void Add_ShouldReturnZero_WhenAnOppositePositiveAndNegativeNumberAreAdded()
    {
        // Arrange
        var sut = new IntCalculator();

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
}