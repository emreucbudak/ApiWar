namespace ApiWar.Application.Testing.Facades;

public interface ITestFacade
{
    IReadOnlyList<string> GetOptions();

    Task ExecuteAsync(string selectedOption);
}
