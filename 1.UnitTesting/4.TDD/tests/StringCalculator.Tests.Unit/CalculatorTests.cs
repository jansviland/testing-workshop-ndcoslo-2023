using FluentAssertions;
using Xunit;

namespace StringCalculator.Tests.Unit;

public class CalculatorTests
{
    [Fact]
    public void Add_ShouldReturnZero_WhenStringIsEmpty()
    {
        // Arrange
        var calculator = new Calculator();
        
        // Act
        var result = calculator.Add(string.Empty);

        // Assert
        result.Should().Be(0);
    }
    
    [Fact]
    public void Add_ShouldComputeTheSumOfASingleNumber_WhenTheNumberIsAnInteger()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        int result = calculator.Add("1");

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void Add_ShouldComputeTheSumOfUpToTwoNumbers_WhenNumbersAreIntegers()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        int result = calculator.Add("1,2");

        // Assert
        result.Should().Be(3);
    }
    
    [Theory]
    [InlineData("1,2,3", 6)]
    [InlineData("5,5,5,5", 20)]
    public void Add_ShouldComputeTheSumOfAnyAmountOfNumbers_WhenNumbersAreIntegers(
        string numbers, int expectedResult)
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        int result = calculator.Add(numbers);

        // Assert
        result.Should().Be(expectedResult);
    }
}
