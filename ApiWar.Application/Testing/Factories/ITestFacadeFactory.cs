using ApiWar.Application.Testing.Facades;

namespace ApiWar.Application.Testing.Factories;

public interface ITestFacadeFactory
{
    ITestFacade Create(string testType);
}
