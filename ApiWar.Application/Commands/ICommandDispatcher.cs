namespace ApiWar.Application.Commands;

public interface ICommandDispatcher
{
    string Run(string input);
}
