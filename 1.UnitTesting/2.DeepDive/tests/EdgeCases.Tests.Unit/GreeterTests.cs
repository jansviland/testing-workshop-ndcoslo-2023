using NSubstitute;
using Xunit;

namespace EdgeCases.Tests.Unit;

public class GreeterTests
{
    [Fact]
    public void GenerateGreetText_WhenCalled_ReturnsGreetText()
    {
        // Arrange
        var clock = Substitute.For<IClock>();
        clock.Now.Returns(new DateTime(2021, 1, 1, 20, 0, 0));

        var greeter = new Greeter(clock);

        // Act
        var result = greeter.GenerateGreetText();

        // Assert
        Assert.Equal("Good evening", result);
    }
}