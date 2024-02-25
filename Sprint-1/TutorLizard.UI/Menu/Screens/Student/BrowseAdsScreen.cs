using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Student;
public class BrowseAdsScreen : StudentMenuScreenBase
{
    private int _page = 1;
    Paginator<Ad> _paginator;
    public BrowseAdsScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
        List<Ad> ads = _studentService.GetAllAds();
        PaginatorOptions<Ad> options = new()
        {
            CollectionName = "Ogłoszenia",
            DisplayItem = DisplayAd
        };
        _paginator = new(ads, options);
    }

    public override MenuNavigation Display()
    {
        Console.Write("Wszystkie ogłoszenia \t");
        return _paginator.DisplayPage(ref _page);
    }

    private void DisplayAd(Ad ad, int i)
    {
        string tutorUserName = _studentService.GetTutorUserNameByAdId(ad.Id);
        Console.WriteLine($"{i + 1}.\tId: {ad.Id}");
        Console.WriteLine($"\tWystawione przez: {tutorUserName}");
        Console.WriteLine($"\tTytuł: {ad.Title}");
        Console.WriteLine($"\tTematyka: {ad.Subject}");
        Console.WriteLine($"\tOpis: {ad.Description}");

        var requests = _studentService
            .GetStudentsAdRequests()
            .Where(r => r.AdId == ad.Id);
        bool accepted = requests.Any(r => r.IsAccepted);
        if (accepted)
        {
            Console.WriteLine("\tJesteś zapisany/a.");
        }
        else if (requests.Any())
        {
            Console.WriteLine("\tWysłałeś/aś zgłoszenie.");
        }

        Console.WriteLine();
    }
}
