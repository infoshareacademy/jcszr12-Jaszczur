using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Student;
public class CreateAdRequestScreen : StudentMenuScreenBase
{
    public CreateAdRequestScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
    }

    public override MenuNavigation Display()
    {
        string exitString = "";

        Console.WriteLine("Nowe zgłoszenie do ogłoszenia");
        Console.WriteLine();
        Console.WriteLine("Aby anulować, wciśnij Enter bez podawania wartości.");
        Console.WriteLine();

        int? adId = ConsoleParser.AskForInt(new()
        {
            Message = "Podaj id ogłoszenia: ",
            ExitString = exitString,
            Predicate = id => _studentService.GetAdById((int)id!) is not null,
            RetryMessage = "Nieprawidłowe id. Spróbuj ponownie."
        });
        if (adId is null)
            return Cancel();

        Console.WriteLine();

        Ad ad = _studentService.GetAdById((int)adId)!;
        DisplayAdSummary(ad);

        Console.WriteLine();

        string? message = ConsoleParser.AskForString(new()
        {
            ExitString = exitString,
            Message = "Napisz wiadomość do prowadzącego:\n"
        });

        if (message is null)
            return Cancel();

        Console.WriteLine();

        int selected = SelectTool.SelectOne([
            "Wyślij zgłoszenie",
            "Zacznij od nowa",
            "Anuluj"
        ]);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                AdRequest createdRequest = _studentService.CreateAdRequest(ad.Id, message);
                if (createdRequest.Id != 0)
                {
                    Console.WriteLine("Zgłoszenie wysłane.");
                    Console.Write("Wróć do menu głównego...");
                    Console.ReadKey(true);
                    return MenuNavigation.Previous;
                }
                else
                {
                    Console.WriteLine("Nie udało się wysłać zgłoszenia.");
                    Console.Write("Spróbuj ponownie...");
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
        Console.WriteLine("Tworzenie zgłoszenia do ogłoszenia anulowane.");
        Console.Write("Wróć do menu głównego...");
        Console.ReadKey(true);
        return MenuNavigation.Previous;
    }

    private void DisplayAdSummary(Ad ad)
    {
        string tutorUserName = _studentService.GetTutorUserNameByAdId(ad.Id);
        Console.WriteLine($"Ogłoszenie Id: {ad.Id}");
        Console.WriteLine($"Wystawione przez: {tutorUserName}");
        Console.WriteLine($"Tytuł: {ad.Title}");
        Console.WriteLine($"Tematyka: {ad.Subject}");
        Console.WriteLine($"Opis: {ad.Description}");
    }
}
