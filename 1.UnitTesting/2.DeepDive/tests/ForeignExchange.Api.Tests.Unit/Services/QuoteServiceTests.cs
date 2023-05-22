using ForeignExchange.Api.Models;
using ForeignExchange.Api.Repositories;
using ForeignExchange.Api.Services;
using ForeignExchange.Api.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
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
        var fromCurrency = "EUR";
        var toCurrency = "USD";
        var amount = 100m;

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
    public async Task GetQuoteAsync_ShouldThrowException_WhenNegative()
    {
        // Arrange
        var fromCurrency = "EUR";
        var toCurrency = "USD";
        var amount = -100m;

        // var expected = new ConversionQuote
        // {
        //     BaseCurrency = fromCurrency,
        //     QuoteCurrency = toCurrency,
        //     BaseAmount = amount,
        //     QuoteAmount = 120m
        // };

        // _ratesRepository
        //     .GetRateAsync(fromCurrency, toCurrency)
        //     .Returns(new FxRate()
        //     {
        //         Rate = 1.2m
        //     });


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