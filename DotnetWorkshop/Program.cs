using DotnetWorkshop;
using Spectre.Console;

const string exitKey = "[red]Exit[/]";
const string addGptKey = "[blue]Add GPT[/]";

var gpts = new List<(string Name, string Prompt)>();
var apiKey = File.ReadAllText("apiKey.txt");

if (!apiKey.StartsWith("sk-"))
    throw new ArgumentException("Pass valid OpenAI Api key to apikey.txt!!!");

while (true)
{
    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[green]Main menu[/]")
            .HighlightStyle(new Style(Color.Yellow))
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
            .AddChoices([
                exitKey, addGptKey, ..gpts.Select(x => x.Name)
            ])
    );

    var result = choice switch
    {
        exitKey => throw new Exception("Terminated"),
        addGptKey => AddGptHandler(),
        _ => await RunGptHandler(choice)
    };
    
    AnsiConsole.WriteLine($"\n{result}");

    var restart = AnsiConsole.Confirm("[red]Continue?[/]");
    if (!restart) break;

    AnsiConsole.Clear();
}

return;

string AddGptHandler()
{
    var name = AnsiConsole.Ask<string>("Enter GPT name:");
    var prompt = AnsiConsole.Ask<string>("Enter GPT prompt:");
    gpts.Add((name, prompt));
    return "Success";
}

async Task<string> RunGptHandler(string gptName)
{
    var gptPrompt = gpts.FirstOrDefault(x => x.Name == gptName).Prompt;
    if (gptPrompt == null) return "Invalid GPT selection";

    var question = AnsiConsole.Ask<string>("Enter your question:");

    var response = await PromptRunner.Run(gptPrompt, question, apiKey);
    return response;
}