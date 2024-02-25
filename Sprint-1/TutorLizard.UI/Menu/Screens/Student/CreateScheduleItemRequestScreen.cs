using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Student;
public class CreateScheduleItemRequestScreen : StudentMenuScreenBase
{
    public CreateScheduleItemRequestScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
    }

    public override MenuNavigation Display()
    {
        string exitString = "";

        Console.WriteLine("Nowe zgłoszenie na termin zajęć");
        Console.WriteLine();
        Console.WriteLine("Aby anulować, wciśnij Enter bez podawania wartości.");
        Console.WriteLine();

        List<ScheduleItem> availableSchedule = _studentService.GetAllScheduleItemsForStudentsAcceptedAds()
            .Where(s => s.DateTime > DateTime.Now && _studentService.IsScheduleItemFree(s))
            .ToList();

        int? scheduleItemId = ConsoleParser.AskForInt(new()
        {
            Message = "Podaj id terminu: ",
            ExitString = exitString,
            Predicate = id => availableSchedule.Any(s => s.Id == id),
            RetryMessage = "Nieprawidłowe id. Spróbuj ponownie."
        });
        if (scheduleItemId is null)
            return Cancel();

        Console.WriteLine();

        ScheduleItem scheduleItem = availableSchedule.First(s => s.Id == scheduleItemId);
        DisplayScheduleItemSummary(scheduleItem);

        bool alreadySentRequest = _studentService
            .GetStudentsScheduleItemRequests()
            .Any(r => r.ScheduleItemId == scheduleItemId);

        Console.WriteLine();

        if (alreadySentRequest)
        {
            Console.WriteLine("Już wysłałeś/aś zgłoszenie na ten termin.");
            Console.Write("Spróbuj ponownie...");
            Console.ReadKey(true);
            return MenuNavigation.NextOrCurrent;
        }

        int selected = SelectTool.SelectOne([
            "Wyślij zgłoszenie",
            "Zacznij od nowa",
            "Anuluj"
        ]);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                ScheduleItemRequest createdRequest = _studentService.CreateScheduleItemRequest(scheduleItem.Id);
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
        Console.WriteLine("Tworzenie zgłoszenia na termin zajęć anulowane.");
        Console.Write("Wróć do menu głównego...");
        Console.ReadKey(true);
        return MenuNavigation.Previous;
    }

    private void DisplayScheduleItemSummary(ScheduleItem scheduleItem)
    {
        Ad? ad = _studentService.GetAdById(scheduleItem.AdId);
        Console.WriteLine($"Termin Id: {scheduleItem.Id}");
        if (ad is null)
            Console.WriteLine($"Termin do nieistniejącego ogłoszenia");
        else
        {
            string tutorUserName = _studentService.GetTutorUserNameByAdId(ad.Id);
            Console.WriteLine($"Termin do ogłoszenia Id: {ad.Id}");
            Console.WriteLine($"Tytuł ogłoszenia: {ad.Title}");
            Console.WriteLine($"Wystawione przez: {tutorUserName}");
        }
        Console.WriteLine($"{scheduleItem.DateTime.ToString("dd.MM.yyyy HH:mm")}");
    }
}
