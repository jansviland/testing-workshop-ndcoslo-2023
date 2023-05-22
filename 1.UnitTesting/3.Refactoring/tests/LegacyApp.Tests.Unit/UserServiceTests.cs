using System;
using NSubstitute;
using Xunit;

namespace LegacyApp.Tests.Unit;

public class UserServiceTests
{
    [Fact]
    public void AddUser_ShouldNotCreateUser_WhenFirstNameIsEmpty()
    {
        // Arrange
        var clientRepository = Substitute.For<IClientRepository>();
        var userCreditService = Substitute.For<IUserCreditService>();

        var sut = new UserService(clientRepository, userCreditService);

        // Act
        var result = sut.AddUser("", "Chapsas", "mail@mail.com", new DateTime(1993, 1, 1), 4);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_ShouldNotCreateUser_WhenLastNameIsEmpty()
    {
        // Arrange
        var clientRepository = Substitute.For<IClientRepository>();
        var userCreditService = Substitute.For<IUserCreditService>();

        var sut = new UserService(clientRepository, userCreditService);

        // Act
        var result = sut.AddUser("Jan", string.Empty, "mail@mail.com", new DateTime(1993, 1, 1), 4);

        // Assert
        Assert.False(result);
    }
    
    
    [Theory]
    [InlineData("mail")]
    [InlineData("mail@mail")]
    [InlineData("mailmail.com")]
    [InlineData("mail@mailcom")]
    public void AddUser_ShouldNotCreateUser_WhenEmailIsInvalid(string mail)
    {
        // Arrange
        var clientRepository = Substitute.For<IClientRepository>();
        var userCreditService = Substitute.For<IUserCreditService>();

        var sut = new UserService(clientRepository, userCreditService);

        // Act
        var result = sut.AddUser("Jan", "Banan", mail, new DateTime(1993, 1, 1), 4);

        // Assert
        Assert.False(result);
    }
}