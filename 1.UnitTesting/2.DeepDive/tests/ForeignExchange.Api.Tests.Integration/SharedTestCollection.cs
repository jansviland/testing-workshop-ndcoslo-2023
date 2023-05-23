using Xunit;

namespace ForeignExchange.Api.Tests.Integration;

[CollectionDefinition("Shared collection")]
public class SharedTestCollection : ICollectionFixture<ForeignExchangeApiFactory>
{
}