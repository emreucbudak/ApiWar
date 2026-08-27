using ApiWar.Application.Commands;

namespace ApiWar.Infrastructure.Commands;

public sealed class CommandDispatcher : ICommandDispatcher
{
    public string Run(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var command = parts[0].ToLowerInvariant();
        var parameter = parts.Length > 1 ? parts[1] : string.Empty;

        return command switch
        {
            "/başlat" => Start(),
            "/yardım" => Help(parameter),
            _ => string.Empty,
        };
    }

    private string Start()
    {
        return string.Empty;
    }

    private string Help(string parameter)
    {
        return string.Empty;
    }
}
