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
    var selectedTestType = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold deepskyblue1]➜[/] ")
            .HighlightStyle(new Style(Color.DeepSkyBlue1))
            .AddChoices("API", "Web"));

    var testType = selectedTestType == "API" ? "api" : "web";
    var facade = ApiWar.Application.Testing.TestFacadeFactory.Create(testType);
    var options = facade.GetOptions();

    var selectedOption = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold deepskyblue1]➜[/] ")
            .HighlightStyle(new Style(Color.DeepSkyBlue1))
            .AddChoices(options));

    await facade.ExecuteAsync(selectedOption);
}
