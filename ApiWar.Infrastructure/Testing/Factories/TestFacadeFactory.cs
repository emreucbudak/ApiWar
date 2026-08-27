using ApiWar.Application.Testing.Facades;
using ApiWar.Application.Testing.Factories;
using ApiWar.Infrastructure.Testing.Facades;

namespace ApiWar.Infrastructure.Testing.Factories;

public sealed class TestFacadeFactory : ITestFacadeFactory
{
    public ITestFacade Create(string testType)
    {
        ArgumentNullException.ThrowIfNull(testType);

        return testType.ToLowerInvariant() switch
        {
            "api" => new ApiTestFacade(),
            "web" => new WebTestFacade(),
            _ => throw new ArgumentException("Invalid test type.", nameof(testType)),
        };
    }
}
