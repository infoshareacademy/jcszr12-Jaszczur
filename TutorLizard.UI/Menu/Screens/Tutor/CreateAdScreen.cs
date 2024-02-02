using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class CreateAdScreen : TutorMenuScreenBase
{
    public CreateAdScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    public override MenuNavigation Display()
    {
        string exitString = "";

        Console.WriteLine("Nowe ogłoszenie");
        Console.WriteLine();
        Console.WriteLine("Podaj tematykę, tytuł ogłoszenia i opis.");
        Console.WriteLine("Aby anulować, wciśnij Enter bez podawania wartości.");
        Console.WriteLine();
        string? subject = ConsoleParser.AskForString(new()
        {
            Message = "Tematyka: ",
            ExitString = exitString            
        });

        if (subject is null)
            return Cancel();

        string? title = ConsoleParser.AskForString(new()
        {
            Message = "Tytuł: ",
            ExitString = exitString
        });

        if (title is null)
            return Cancel();

        string? description = ConsoleParser.AskForString(new()
        {
            Message = "Opis: ",
            ExitString = exitString
        });

        if (description is null)
            return Cancel();

        Console.WriteLine();

        string[] items = [
            "Dodaj ogłoszenie", // 0
            "Zacznij od nowa", // 1
            "Anuluj" // 2
        ];

        int selected = SelectTool.SelectOne(items);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                bool successful = _tutorService.CreateAd(subject, title, description);
                if (successful)
                {
                    Console.WriteLine("Twoje ogłoszenie zostało dodane.");
                    Console.WriteLine("Wróć do menu głównego...");
                    Console.ReadKey(true);
                    return MenuNavigation.Previous;
                }
                else
                {
                    Console.WriteLine("Nie udało się dodać ogłoszenia.");
                    Console.WriteLine("Spróbuj ponownie...");
                    Console.ReadKey(true);
                    return MenuNavigation.NextOrCurrent;
                }
            case 1:
                return MenuNavigation.NextOrCurrent;
            case 2:
                return Cancel();
            default:
                return MenuNavigation.NextOrCurrent;
        }
    }

    private MenuNavigation Cancel()
    {
        Console.WriteLine();
        Console.WriteLine("Dodawanie nowego ogłoszenia anulowane.");
        Console.WriteLine("Wróć do menu głównego...");
        Console.ReadKey(true);
        return MenuNavigation.Previous;
    }
}
