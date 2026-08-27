using Spectre.Console;

AnsiConsole.WriteLine();

AnsiConsole.Write(
    new FigletText("API WAR")
    {
        Color = Color.DeepSkyBlue1,
        Justification = Justify.Center,
    });

AnsiConsole.WriteLine();

while (true)
{
    _ = AnsiConsole.Prompt(
        new TextPrompt<string>("[bold deepskyblue1]➜[/] ")
            .PromptStyle(new Style(Color.White))
            .AllowEmpty());
}
