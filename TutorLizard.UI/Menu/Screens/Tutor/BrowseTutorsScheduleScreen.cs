using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsScheduleScreen : TutorMenuScreenBase
{
    private int _page = 1;
    Paginator<ScheduleItem> _paginator;
    List<Ad> _ads;
    List<ScheduleItemRequest> _accepted;
    public BrowseTutorsScheduleScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
        List<ScheduleItem> schedule = _tutorService.GetTutorsScheduleItems();
        PaginatorOptions<ScheduleItem> options = new()
        {
            CollectionName = "Terminy",
            DisplayItem = DisplayScheduleItem
        };
        _paginator = new(schedule, options);

        _ads = _tutorService.GetTutorsAds();
        _accepted = _tutorService.GetTutorsScheduleItemRequests()
            .Where(r => r.IsAccepted)
            .ToList();
    }

    public override MenuNavigation Display()
    {
        Console.Write("Twoje terminy\t");
        return _paginator.DisplayPage(ref _page);
    }

    private void DisplayScheduleItem(ScheduleItem item, int i)
    {
        Ad? ad = _ads.FirstOrDefault(a => a.Id == item.AdId);
        ScheduleItemRequest? acceptedRequest = _accepted
            .FirstOrDefault(r => r.ScheduleItemId == item.Id);

        if (ad is not null)
            Console.WriteLine($"{i + 1}.\tOgłoszenie Id: {ad.Id}\tTytuł: {ad.Title}");
        else
            Console.WriteLine($"{i + 1}.\tTermin przypisany do nieistniejącego ogłoszenia.");

        Console.WriteLine($"\tTermin Id: {item.Id}\n\t{item.DateTime.ToString("dd.MM.yyyy HH:mm")}");
            
        if (acceptedRequest is not null)
        {
            string studentUserName = _tutorService.GetStudentUserNameByScheduleItemRequestId(acceptedRequest.Id);
            Console.WriteLine($"\tUczeń zapisany na termin: {studentUserName}");
        }
        else
        {
            Console.WriteLine("\tTermin wolny");
        }

        Console.WriteLine();
    }
}
