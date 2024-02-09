using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Student;
public class BrowseStudentsAcceptedScheduleScreen : StudentMenuScreenBase
{
    private int _page = 1;
    Paginator<ScheduleItem> _paginator;
    List<Ad> _ads;
    public BrowseStudentsAcceptedScheduleScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
        _ads = _studentService.GetAllAds();
        List<ScheduleItem> availableSchedule = _studentService.GetUsersAcceptedScheduleItems()
            .Where(s => s.DateTime > DateTime.Now)
            .ToList();
        PaginatorOptions<ScheduleItem> options = new()
        {
            CollectionName = "Terminy",
            DisplayItem = DisplayScheduleItem
        };

        _paginator = new(availableSchedule, options);
    }

    public override MenuNavigation Display()
    {
        Console.Write("Twoje terminy\t");
        return _paginator.DisplayPage(ref _page);
    }

    private void DisplayScheduleItem(ScheduleItem item, int i)
    {
        Ad? ad = _ads.FirstOrDefault(a => a.Id == item.AdId);

        if (ad is not null)
        {
            string tutorUserName = _studentService.GetTutorUserNameByAdId(ad.Id);
            Console.WriteLine($"{i + 1}.\tOgłoszenie Id: {ad.Id}\tTytuł: {ad.Title}");
            Console.WriteLine($"\tWystawione przez: {tutorUserName}");
        }
        else
            Console.WriteLine($"{i + 1}.\tTermin przypisany do nieistniejącego ogłoszenia.");

        Console.WriteLine($"\tTermin Id: {item.Id}\n\t{item.DateTime.ToString("dd.MM.yyyy HH:mm")}");

        Console.WriteLine();
    }
}
