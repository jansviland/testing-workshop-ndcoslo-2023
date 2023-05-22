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
}