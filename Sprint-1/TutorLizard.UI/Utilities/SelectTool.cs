namespace TutorLizard.UI.Utilities;
public static class SelectTool
{
    public static int SelectOne(IList<string> items, SelectOneOptions options)
    {
        int originalRow = Console.GetCursorPosition().Top;

        foreach (string item in items)
        {
            Console.WriteLine($"{options.NotSelected} {item}");
        }
        
        int finalRow = Console.GetCursorPosition().Top;

        if (finalRow < originalRow + items.Count)
            originalRow = finalRow - items.Count;

        Console.CursorVisible = false;

        int selected = 0;

        DisplaySelector(selected, originalRow, finalRow, options.Selected);

        bool selecting = true;

        while (selecting)
        {
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selected == 0 && options.LoopSelection == false)
                        break;
                    DisplaySelector(selected, originalRow, finalRow, options.NotSelected);
                    if (selected == 0)
                        selected = items.Count - 1;
                    else
                        selected--;
                    DisplaySelector(selected, originalRow, finalRow, options.Selected);
                    break;
                case ConsoleKey.DownArrow:
                    if (selected == items.Count - 1 && options.LoopSelection == false)
                        break;
                    DisplaySelector(selected, originalRow, finalRow, options.NotSelected);
                    if (selected == items.Count - 1)
                        selected = 0;
                    else
                        selected++;
                    DisplaySelector(selected, originalRow, finalRow, options.Selected);
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    selecting = false;
                    break;
                default:
                    break;
            }
        }

        Console.CursorVisible = true;
        return selected;
    }
    public static int SelectOne(IList<string> items) => SelectOne(items, new());

    private static void DisplaySelector(int index, int originalRow, int finalRow, string selector)
    {
        int row = originalRow + index;
        Console.SetCursorPosition(0, row);
        Console.Write(selector);
        Console.SetCursorPosition(0, finalRow);
    }
}