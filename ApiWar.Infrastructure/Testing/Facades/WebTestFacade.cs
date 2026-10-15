using ApiWar.Application.Testing.Facades;

namespace ApiWar.Infrastructure.Testing.Facades;

public sealed class WebTestFacade : ITestFacade
{
    private static readonly IReadOnlyList<string> Options =
    [
        "Form",
        "Login",
        "Input",
        "Session",
        "SQL Injection",
    ];

    public IReadOnlyList<string> GetOptions() => Options;

    public Task ExecuteAsync(string selectedOption)
    {
        return selectedOption switch
        {
            "Form" => FormAsync(),
            "Login" => LoginAsync(),
            "Input" => InputAsync(),
            "Session" => SessionAsync(),
            "SQL Injection" => SqlInjectionAsync(),
            _ => throw new ArgumentException("Invalid web test option.", nameof(selectedOption)),
        };
    }

    public async Task FormAsync()
    {
    }

    public async Task LoginAsync()
    {
    }

    public async Task InputAsync()
    {
    }

    public async Task SessionAsync()
    {
    }

    public async Task SqlInjectionAsync()
    {
    }
}
