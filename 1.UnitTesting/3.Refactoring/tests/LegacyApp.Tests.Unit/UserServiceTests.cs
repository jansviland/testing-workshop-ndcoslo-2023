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

    [Theory]
    [InlineData(2020)] // user is 20
    [InlineData(2019)] // user is 19
    [InlineData(2001)] // user is 1
    [InlineData(2000)] // user is 0
    public void AddUser_ShouldNotCreateUser_WhenUserIsUnder21(int year)
    {
        // Arrange
        var clientRepository = Substitute.For<IClientRepository>();
        var userCreditService = Substitute.For<IUserCreditService>();
        var clock = Substitute.For<IClock>();

        // year is 2000
        clock.Now.Returns(new DateTime(2000, 1, 1));

        var sut = new UserService(clientRepository, userCreditService, clock);

        // Act
        var result = sut.AddUser(
            "Jan",
            "Banan",
            "mail@mail.com",
            new DateTime(year, 1, 1), 4);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_ShouldNotCreateUser_WhenUserHasCreditLimitAndLimitIsLessThan500()
    {
        // Arrange
        var clientRepository = Substitute.For<IClientRepository>();
        var userCreditService = Substitute.For<IUserCreditService>();
        var clock = Substitute.For<IClock>();

        // year is 2000
        clock.Now.Returns(new DateTime(2000, 1, 1));

        var clientResponse = new Client
        {
            Id = 123,
            Name = "Test",
            ClientStatus = ClientStatus.Gold
        };

        clientRepository.GetById(Arg.Any<int>()).Returns(clientResponse);

        // credit limit is 499
        userCreditService.GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(499);

        var sut = new UserService(clientRepository, userCreditService, clock);

        // Act
        var result = sut.AddUser(
            "Jan",
            "Banan",
            "mail@mail.com",
            new DateTime(1970, 1, 1), 4);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_ShouldCreateUser_WhenUserDetailsAreValid()
    {
        // Arrange
        var clientRepository = Substitute.For<IClientRepository>();
        var userCreditService = Substitute.For<IUserCreditService>();
        var clock = Substitute.For<IClock>();

        // year is 2000
        clock.Now.Returns(new DateTime(1970, 1, 1));

        var clientResponse = new Client
        {
            Id = 123,
            Name = "VeryImportantClient", // skip credit check
            ClientStatus = ClientStatus.Gold
        };

        clientRepository.GetById(Arg.Any<int>()).Returns(clientResponse);

        // credit limit is 501
        // userCreditService.GetCreditLimit(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(501);

        var sut = new UserService(clientRepository, userCreditService, clock);

        // Act
        var result = sut.AddUser(
            "Jan",
            "Banan",
            "mail@mail.com",
            new DateTime(2000, 1, 1), 4);

        // Assert
        Assert.True(result);
    }
}