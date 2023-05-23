using System.Net;
using System.Net.Http.Json;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Customers.Api.Tests.Integration;

public class CustomerEndpointsTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<IApiMarker> _waf = new();
    private readonly List<Guid> _customerIds = new();

    public CustomerEndpointsTests()
    {
        _client = _waf.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldCreateCustomer_WhenDetailsAreValid()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Email = "nick@chapsas.com",
            FullName = "Nick Chapsas",
            DateOfBirth = new DateTime(1993, 01, 01),
            GitHubUsername = "nickchapsas"
        };

        var expectedResponse = new CustomerResponse
        {
            Email = "nick@chapsas.com",
            FullName = "Nick Chapsas",
            DateOfBirth = new DateTime(1993, 01, 01),
            GitHubUsername = "nickchapsas"
        };

        // Act
        var response = await _client.PostAsJsonAsync("customers", request);
        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        _customerIds.Add(customerResponse!.Id);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().Be($"http://localhost/customers/{customerResponse.Id}");
        customerResponse.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(x => x.Id));
        customerResponse.Id.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Get_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Email = "nick@chapsas.com",
            FullName = "Nick Chapsas",
            DateOfBirth = new DateTime(1993, 01, 01),
            GitHubUsername = "nickchapsas"
        };

        var createdCustomerResponse = await _client.PostAsJsonAsync("customers", request);
        var createdCustomer = await createdCustomerResponse.Content.ReadFromJsonAsync<CustomerResponse>();
        _customerIds.Add(createdCustomer!.Id);

        // Act
        var response = await _client.GetAsync($"customers/{createdCustomer.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedCustomer = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        retrievedCustomer.Should().BeEquivalentTo(createdCustomer);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        foreach (var customerId in _customerIds)
        {
            await _client.DeleteAsync($"customers/{customerId}");
        }
    }
}
