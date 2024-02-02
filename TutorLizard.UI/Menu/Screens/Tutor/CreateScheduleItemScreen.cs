using System.ComponentModel;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class CreateScheduleItemScreen : TutorMenuScreenBase
{
    public CreateScheduleItemScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    public override MenuNavigation Display()
    {
        string exitString = "";

        Console.WriteLine("Nowy termin zajęć");
        Console.WriteLine();
        Console.WriteLine("Aby anulować, wciśnij Enter bez podawania wartości.");
        Console.WriteLine();

        int? adId = ConsoleParser.AskForInt(new()
        {
            ExitString = exitString,
            Message = "Podaj id ogłoszenia: ",
            Predicate = id => _tutorService.UserCanEditAdSchedule((int)id!),
            RetryMessage = "Nieprawidłowe id. Spróbuj ponownie."
        });

        if (adId is null)
            return Cancel();

        Ad ad = _tutorService.GetAdById((int)adId);

        Console.WriteLine();
        DisplayAdSummary(ad);
        Console.WriteLine();

        DateTime? dateTime = ConsoleParser.AskForDateAndHour(new()
        {
            ExitString = exitString,
            Message = "Podaj termin zajęć i godzinę w formacie dd.MM.yyyy HH:mm:\n",
            Predicate = d => d >= DateTime.Now,
            RetryMessage = "Nieprawidłowa data. Spróbuj ponownie."
        });

        if (dateTime is null)
            return Cancel();

        Console.WriteLine();

        string[] items = [
            "Dodaj termin", // 0
            "Zacznij od nowa", // 1
            "Anuluj" // 2
        ];

        int selected = SelectTool.SelectOne(items);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                bool successful = _tutorService.CreateScheduleItem((int)adId, (DateTime)dateTime);
                if (successful)
                {
                    Console.WriteLine("Termin zajęć został dodany.");
                    Console.WriteLine("Wróć do menu głównego...");
                    Console.ReadKey(true);
                    return MenuNavigation.Previous;
                }
                else
                {
                    Console.WriteLine("Nie udało się dodać terminu.");
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

    private void DisplayAdSummary(Ad ad)
    {
        Console.WriteLine($"Ogłoszenie Id: {ad.Id}");
        Console.WriteLine($"Tytuł: {ad.Title}");
    }

    private MenuNavigation Cancel()
    {
        Console.WriteLine();
        Console.WriteLine("Dodawanie nowego terminu anulowane.");
        Console.WriteLine("Wróć do menu głównego...");
        Console.ReadKey(true);
        return MenuNavigation.Previous;
    }
}
