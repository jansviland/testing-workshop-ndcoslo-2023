using System.Net.Http.Headers;
using System.Net.Http.Json;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Customers.Api.Tests.Integration;

public class CustomerEndpointsTests : IAsyncLifetime
{
    private List<Guid> _customerIds = new();

    private readonly HttpClient client;

    private readonly WebApplicationFactory<IApiMarker> _ref = new();

    public CustomerEndpointsTests()
    {
        client = _ref.CreateClient();
        // client.BaseAddress = new Uri("https://localhost:5001");
        // client.DefaultRequestHeaders.Accept.Clear();
        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

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
        var response = await client.PostAsJsonAsync("/customers", request);

        // Assert
        // response.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());

        // response.StatusCode.Should().Be(StatusCodes.Status201Created);

        var content = await response.Content.ReadFromJsonAsync<CustomerResponse>();

        content.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
        content!.Id.Should().NotBeEmpty();

        _customerIds.Add(content.Id);

        // Cleanup
        // await client.DeleteAsync($"/customers/{response.Id}");
    }

    public async Task InitializeAsync() => await Task.CompletedTask;

    public async Task DisposeAsync()
    {
        // Remove all customers
        foreach (var customerId in _customerIds)
        {
            await client.DeleteAsync($"/customers/{customerId}");
        }
    }
}