namespace ApiWar.Application.Testing;

public interface ITestFacade
{
    IReadOnlyList<string> GetOptions();

    Task ExecuteAsync(string selectedOption);
}
