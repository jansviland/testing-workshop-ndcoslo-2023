using Xunit;

namespace StringCalculator.Tests.Unit;

public class StringCalculatorTests
{
    [Fact]
    public void Add_ShouldAddTwoNumbers()
    {
        // Arrange
        var numbers = "1,3";

        // Act
        var result = StringCalculator.Add(numbers);

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void Add_ShouldAllowCommaAndNewLine()
    {
        // Arrange
        var numbers = "1\n2,3";

        // Act
        var result = StringCalculator.Add(numbers);

        // Assert
        Assert.Equal(6, result);
    }
}