using Bogus;
using ForeignExchange.Api.Models;
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

    private readonly Faker<FxRate> _FxRateGenerator =
        new Faker<FxRate>()
            .RuleFor(x => x.FromCurrency, f => f.Finance.Currency().Code)
            .RuleFor(x => x.ToCurrency, f => f.Finance.Currency().Code)
            .RuleFor(x => x.Rate, f => f.Random.Decimal(0.1m, 100m))
            .RuleFor(x => x.TimestampUtc, f => f.Date.Past(1));


    // private readonly Faker<string> _currencyGenerator =
    //     new Faker<string>()
    //         .RuleFor(x => x., f => f.Finance.Currency().Symbol);

    public ForeignExchangeEndpointsTests(ForeignExchangeApiFactory waf)
    {
        _client = waf.Client;
        _dbReset = waf.ResetDatabaseAsync;
        // _setupGitHubUser = waf.GitHubApiServer.SetupUser;
    }

    [Fact]
    public void GetQuote_ShouldReturnQuote_WhenCurrencyPairIsSupported()
    {
        // Arrange
        var randomCurrencies = _FxRateGenerator.Generate();
        
        var fromCurrency = randomCurrencies.FromCurrency;
        var toCurrency = randomCurrencies.ToCurrency;
        var amount = 100;

        Assert.True(false);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _dbReset();
    }
}