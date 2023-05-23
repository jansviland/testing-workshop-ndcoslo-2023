using System.Data.Common;
using ForeignExchange.Api.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;

namespace ForeignExchange.Api.Tests.Integration;

public class ForeignExchangeApiFactory : WebApplicationFactory<IExchangeApiMarker>, IAsyncLifetime
{
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    // public GitHubApiServer GitHubApiServer { get; } = new();

    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithDatabase("mydb")
            .WithUsername("workshop")
            .WithPassword("changeme")
            .Build();

    public HttpClient Client { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbConnectionFactory));
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(_dbContainer.GetConnectionString()));

            // services.AddHttpClient("GitHub", httpClient =>
            // {
            //     httpClient.BaseAddress = new Uri(GitHubApiServer.Url);
            //     httpClient.DefaultRequestHeaders.Add(
            //         HeaderNames.Accept, "application/vnd.github.v3+json");
            //     httpClient.DefaultRequestHeaders.Add(
            //         HeaderNames.UserAgent, $"Workshop-{Environment.MachineName}");
            // });
        });
    }

    public async Task InitializeAsync()
    {
        // GitHubApiServer.Start();
        await _dbContainer.StartAsync();
        Client = CreateClient();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new []{ "public" }
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        // GitHubApiServer.Dispose();
    }
}