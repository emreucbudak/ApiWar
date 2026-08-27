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
            _ => throw new ArgumentException("Invalid web test option.", nameof(selectedOption)),
        };
    }

#pragma warning disable CS1998
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
#pragma warning restore CS1998
}
