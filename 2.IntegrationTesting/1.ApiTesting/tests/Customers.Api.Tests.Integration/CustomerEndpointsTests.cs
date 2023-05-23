using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Customers.Api.Tests.Integration;

public class CustomerEndpointsTests : IAsyncLifetime
{
    private readonly List<Guid> _customerIds = new();

    private readonly HttpClient _client;

    private readonly WebApplicationFactory<IApiMarker> _ref = new();

    public CustomerEndpointsTests()
    {
        _client = _ref.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost:5001")
        });
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    [Fact]
    public async Task Create_ShouldCreateCustomer_WhenDetailsAreValid()
    {
        // Arrange
        var request = new CustomerRequest()
        {
            GitHubUsername = "johnsmith",
            FullName = "John Smith",
            Email = "john@email.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        var expected = new CustomerRequest()
        {
            GitHubUsername = "johnsmith",
            FullName = "John Smith",
            Email = "john@email.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/customers", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        response.Headers.Location.Should().Be($"https://localhost:5001/customers/{content!.Id}");

        content.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
        content!.Id.Should().NotBeEmpty();

        _customerIds.Add(content.Id);
    }

    [Fact]
    public async Task Get_ShouldGetCustomer_WhenCustomerExist()
    {
        // Arrange
        var request = new CustomerRequest()
        {
            GitHubUsername = "johnsmith",
            FullName = "John Smith",
            Email = "john@email.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        var expected = new CustomerRequest()
        {
            GitHubUsername = "johnsmith",
            FullName = "John Smith",
            Email = "john@email.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var createdCustomerResponse = await _client.PostAsJsonAsync("/customers", request);
        var createdCustomer = await createdCustomerResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        _customerIds.Add(createdCustomer!.Id);

        var response = await _client.GetAsync($"/customers/{createdCustomer!.Id}");
        var content = await response.Content.ReadFromJsonAsync<CustomerResponse>();

        // Assert
        createdCustomerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        content.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenTheEmailIsInvalid()
    {
        // Arrange
        var request = new CustomerRequest()
        {
            GitHubUsername = "johnsmith",
            FullName = "John Smith",
            Email = "invalid",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        // Act
        var createdCustomerResponse = await _client.PostAsJsonAsync("/customers", request);
        var response = await createdCustomerResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        // Assert
        response.Should().NotBeNull();

        Assert.NotNull(response);
        Assert.NotNull(response.Errors);

        response.Status.Should().Be(StatusCodes.Status400BadRequest);
        response.Errors.Should().ContainKey(nameof(CustomerRequest.Email));
    }

    public async Task DisposeAsync()
    {
        // Remove all customers
        foreach (var customerId in _customerIds)
        {
            await _client.DeleteAsync($"/customers/{customerId}");
        }
    }
}