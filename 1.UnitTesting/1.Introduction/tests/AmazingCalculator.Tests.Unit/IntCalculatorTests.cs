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
}