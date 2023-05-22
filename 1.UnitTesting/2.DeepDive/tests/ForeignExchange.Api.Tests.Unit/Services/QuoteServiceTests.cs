using FluentAssertions;
using ForeignExchange.Api.Logging;
using ForeignExchange.Api.Models;
using ForeignExchange.Api.Repositories;
using ForeignExchange.Api.Services;
using ForeignExchange.Api.Validation;
using NSubstitute;
using Xunit;

namespace ForeignExchange.Api.Tests.Unit.Services;

public class QuoteServiceTests
{
    private readonly QuoteService _sut;
    private readonly IRatesRepository _ratesRepository = Substitute.For<IRatesRepository>();
    private readonly ILoggerAdapter<QuoteService> _logger = Substitute.For<ILoggerAdapter<QuoteService>>();

    public QuoteServiceTests()
    {
        _sut = new QuoteService(
            _ratesRepository,
            _logger);
    }
    
    [Fact]
    public async Task GetQuoteAsync_ShouldReturnQuote_WhenCurrenciesAreValid()
    {
        // Arrange
        var fromCurrency = "USD";
        var toCurrency = "GBP";
        var amount = 100;

        var expectedQuote = new ConversionQuote
        {
            BaseCurrency = fromCurrency,
            QuoteCurrency = toCurrency,
            BaseAmount = amount,
            QuoteAmount = 160
        };

        _ratesRepository.GetRateAsync(fromCurrency, toCurrency)
            .Returns(new FxRate
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                TimestampUtc = DateTime.UtcNow,
                Rate = 1.6m
            });

        // Act
        var result = await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        // Assert
        result.Should().BeEquivalentTo(expectedQuote);
    }

    [Fact]
    public async Task GetQuoteAsync_ShouldThrowSameCurrencyException_WhenAmountIsNegative()
    {
        // Arrange
        var fromCurrency = "USD";
        var toCurrency = "USD";
        var amount = 100;

        // Act
        var action = () => _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        // Assert
        await action.Should()
            .ThrowAsync<SameCurrencyException>()
            .WithMessage($"You cannot convert currency {fromCurrency} to itself");
    }

    [Fact]
    public async Task GetQuoteAsync_ShouldThrowNegativeAmountException_WhenAmountIsNegative()
    {
        // Arrange
        var fromCurrency = "USD";
        var toCurrency = "GBP";
        var amount = 0;

        // Act
        var action = () => _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        // Assert
        await action.Should()
            .ThrowAsync<NegativeAmountException>()
            .WithMessage("You can only convert a positive amount of money");
    }

    [Fact]
    public async Task GetQuoteAsync_ShouldLogMessage_WhenQuoteIsCalculated()
    {
        // Arrange
        var fromCurrency = "USD";
        var toCurrency = "GBP";
        var amount = 100;

        _ratesRepository.GetRateAsync(fromCurrency, toCurrency)
            .Returns(new FxRate
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                TimestampUtc = DateTime.UtcNow,
                Rate = 1.6m
            });

        // Act
        await _sut.GetQuoteAsync(fromCurrency, toCurrency, amount);

        // Assert
        _logger.Received(1).LogInformation(
            Arg.Is("Retrieved quote for currencies {FromCurrency}->{ToCurrency} in {ElapsedMilliseconds}ms"),
            Arg.Is<object[]>(x => 
                x[0].ToString() == fromCurrency &&
                x[1].ToString() == toCurrency));
    }
}
