using ApiWar.Application.Testing.Facades;

namespace ApiWar.Infrastructure.Testing.Facades;

public sealed class ApiTestFacade : ITestFacade
{
    private static readonly IReadOnlyList<string> Options =
    [
        "Authentication",
        "Load",
        "Rate Limit",
        "Endpoint",
    ];

    public IReadOnlyList<string> GetOptions() => Options;

    public Task ExecuteAsync(string selectedOption)
    {
        return selectedOption switch
        {
            "Authentication" => AuthenticationAsync(),
            "Load" => LoadAsync(),
            "Rate Limit" => RateLimitAsync(),
            "Endpoint" => EndpointAsync(),
            _ => throw new ArgumentException("Invalid API test option.", nameof(selectedOption)),
        };
    }

    public async Task AuthenticationAsync()
    {
    }

    public async Task LoadAsync()
    {
    }

    public async Task RateLimitAsync()
    {
    }

    public async Task EndpointAsync()
    {
    }
}
