using Spectre.Console;

using ApiWar.Application.Testing.Factories;
using ApiWar.Infrastructure.Testing.Factories;

ITestFacadeFactory testFacadeFactory = new TestFacadeFactory();

AnsiConsole.WriteLine();

AnsiConsole.Write(
    new FigletText("API WAR")
    {
        Color = Color.DeepSkyBlue1,
        Justification = Justify.Center,
    });

AnsiConsole.WriteLine();

AnsiConsole.Write(
    new Align(
        new Markup("[bold]ApiWar'a hoş geldiniz![/]"),
        HorizontalAlignment.Center));

AnsiConsole.Write(
    new Align(
        new Markup("[grey]ApiWar, web sitelerinizi ve API'lerinizi test etmek için geliştirilmiş bir araçtır.[/]"),
        HorizontalAlignment.Center));

AnsiConsole.Write(
    new Align(
        new Markup("[dim]Yalnızca izinli testlerde kullanılmalıdır. Yasa dışı kullanımlardan geliştirici sorumlu değildir.[/]"),
        HorizontalAlignment.Center));

AnsiConsole.Write(
    new Align(
        new Markup("Başlamak için [bold deepskyblue1]/başlat[/] yazın."),
        HorizontalAlignment.Center));

AnsiConsole.WriteLine();

while (true)
{
    var selectedTestType = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold deepskyblue1]➜[/] ")
            .HighlightStyle(new Style(Color.DeepSkyBlue1))
            .AddChoices("API", "Web"));

    var testType = selectedTestType == "API" ? "api" : "web";
    var facade = testFacadeFactory.Create(testType);
    var options = facade.GetOptions();

    var selectedOption = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold deepskyblue1]➜[/] ")
            .HighlightStyle(new Style(Color.DeepSkyBlue1))
            .AddChoices(options));

    await facade.ExecuteAsync(selectedOption);
}
