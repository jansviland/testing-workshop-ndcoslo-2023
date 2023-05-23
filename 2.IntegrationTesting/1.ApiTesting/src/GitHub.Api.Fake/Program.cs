
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

var settings = new WireMockServerSettings
{
    Urls = new[] { "https://localhost:9095/" },
    StartAdminInterface = true,
    ProxyAndRecordSettings = new ProxyAndRecordSettings
    {
        Url = "https://api.github.com",
        SaveMapping = true,
        SaveMappingToFile = true,
        SaveMappingForStatusCodePattern = "2xx"
    }
};

var wiremockServer = WireMockServer.Start(settings);

Console.WriteLine($"Server started at: {wiremockServer.Url}");

Console.ReadKey();

wiremockServer.Dispose();
