namespace TutorLizard.UI.Utilities;

public class ParserOptions
{
    public string? StartMessage { get; set; }
    public string? Message { get; set; }
    public string? RetryMessage { get; set; }
    public string ExitString { get; set; } = "exit";
    public Predicate<int> Predicate { get; set; } = x => true;

}