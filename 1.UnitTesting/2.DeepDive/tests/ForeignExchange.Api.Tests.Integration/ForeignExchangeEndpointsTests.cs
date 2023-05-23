using Xunit;

namespace ForeignExchange.Api.Tests.Integration;

[Collection("Shared collection")]
public class ForeignExchangeEndpointsTests : IAsyncLifetime
{
    private readonly HttpClient _client;

    private readonly Func<Task> _dbReset;
    // private readonly Action<string> _setupGitHubUser;

    // private readonly Faker<CustomerRequest> _customerGenerator =
    //     new Faker<CustomerRequest>()
    //         .RuleFor(x => x.FullName, f => f.Person.FullName)
    //         .RuleFor(x => x.Email, f => f.Person.Email)
    //         .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date)
    //         .RuleFor(x => x.GitHubUsername, f => f.Person.UserName.Replace(".", "").Replace("-", "").Replace("_", ""))
    //         .UseSeed(1000);

    public ForeignExchangeEndpointsTests(ForeignExchangeApiFactory waf)
    {
        _client = waf.Client;
        _dbReset = waf.ResetDatabaseAsync;
        // _setupGitHubUser = waf.GitHubApiServer.SetupUser;
    }

    [Fact]
    public void Test()
    {
        Assert.True(false);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _dbReset();
    }
}