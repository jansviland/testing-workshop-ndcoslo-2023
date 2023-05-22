using ForeignExchange.Api.Models;
using ForeignExchange.Api.Repositories;
using ForeignExchange.Api.Services;
using ForeignExchange.Api.Validation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace ForeignExchange.Api.Tests.Unit.Services;

public class QuoteServiceTests
{
    private readonly QuoteService _sut;
    private readonly IRatesRepository _ratesRepository = Substitute.For<IRatesRepository>();
    private readonly ILogger<QuoteService> _logger = Substitute.For<ILogger<QuoteService>>();

    public QuoteServiceTests()
    {
        _sut = new QuoteService(_ratesRepository, _logger);
    }

    [Fact]
    public async Task GetQuoteAsync_ShouldReturnQuote()
    {
        // Arrange
        const string fromCurrency = "EUR";
        const string toCurrency = "USD";
        const decimal amount = 100m;

        var expected = new ConversionQuote
        {
            BaseCurrency = fromCurrency,
            QuoteCurrency = toCurrency,
            BaseAmount = amount,
            QuoteAmount = 120m
        };

        _ratesRepository
            .GetRateAsync(fromCurrency, toCurrency)
            .Returns(new FxRate()
            {
                Rate = 1.2m
            });


        // Act
        var actual = await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        // Assert
        Assert.Equivalent(expected, actual);
    }

    [Theory]
    [InlineData("EUR", "USD")]
    [InlineData("USD", "EUR")]
    [InlineData("EUR", "GBP")]
    public async Task GetQuoteAsync_ShouldReturnQuote_WhenCurrenciesAreValid(string fromCurrency, string toCurrency)
    {
        // Arrange
        // const string fromCurrency = "EUR";
        // const string toCurrency = "USD";
        const decimal amount = 100m;

        var expected = new ConversionQuote
        {
            BaseCurrency = fromCurrency,
            QuoteCurrency = toCurrency,
            BaseAmount = amount,
            QuoteAmount = 120m
        };

        _ratesRepository
            .GetRateAsync(fromCurrency, toCurrency)
            .Returns(new FxRate()
            {
                Rate = 1.2m
            });

        // Act
        var actual = await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        // Assert
        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public async Task GetQuoteAsync_ShouldReturnNull_WhenNoRateExists()
    {
        // Arrange
        const string fromCurrency = "ABC";
        const string toCurrency = "DEF";
        const decimal amount = 100m;

        _ratesRepository
            .GetRateAsync(fromCurrency, toCurrency)
            .ReturnsNull();

        // Act
        var actual = await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GetQuoteAsync_ShouldThrowException_WhenNegative()
    {
        // Arrange
        const string fromCurrency = "EUR";
        const string toCurrency = "USD";
        const decimal amount = -100m;

        // Act
        var action = () => _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        var exception = await Record.ExceptionAsync(action);

        // Assert
        Assert.Equal(typeof(NegativeAmountException), exception?.GetType());

        // _logger.Received(1).LogInformation(
        //     "Retrieved quote for currencies {FromCurrency}->{ToCurrency} in {ElapsedMilliseconds}ms",
        //     fromCurrency, toCurrency, Arg.Any<long>());

        // Assert.Equal(typeof(NegativeAmountException), typeof(exception));
    }
}