namespace TutorLizard.UI.Utilities;
public static class ConsoleParser
{
    public static int? AskForInt(ParserOptions<int?> options)
    {
        Func<string?, int?> parser = s =>
        {
            bool parsed = int.TryParse(s, out int value);
            if (parsed)
                return value;
            else
                return null;
        };

        return Ask<int?>(parser, options, null);
    }

    public static string? AskForString(ParserOptions<string?> options)
    {
        Func<string?, string?> parser = s => s;
        return Ask<string?>(parser, options, null);
    }
    private static T Ask<T>(Func<string?, T?> parser, ParserOptions<T> options, T exitValue)
    {
        string? input;
        T? output;
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
                return exitValue;
            }

            output = parser.Invoke(input);

            if (output is null || options.Predicate.Invoke(output) == false)
                retry = true;
            else
                retry = false;
        } while (retry);

        return output!;
    }
}