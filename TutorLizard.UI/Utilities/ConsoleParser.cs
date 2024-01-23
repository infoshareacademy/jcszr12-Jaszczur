namespace TutorLizard.UI.Utilities;
public static class ConsoleParser
{
    public static int? AskForInt(ParserOptions<int> options)
    {
        string? input;
        int output;
        bool retry = false;

        if (string.IsNullOrEmpty(options.StartMessage) == false)
            Console.WriteLine(options.StartMessage);

        do
        {
            if (retry && string.IsNullOrEmpty(options.RetryMessage) == false)
                Console.WriteLine(options.RetryMessage);
            
            if (string.IsNullOrEmpty(options.Message) == false)
                Console.Write(options.Message);

            input = Console.ReadLine();

            if (input is not null && input.Trim().ToLower() == options.ExitString)
            {
                return null;
            }

            bool parsed = int.TryParse(input, out output);

            if (parsed == false || options.Predicate.Invoke(output) == false)
                retry = true;
            else
                retry = false;
        } while (retry);

        return output;
    }
}