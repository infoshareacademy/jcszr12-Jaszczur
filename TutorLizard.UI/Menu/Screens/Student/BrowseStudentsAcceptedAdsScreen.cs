using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using static System.Net.Mime.MediaTypeNames;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Student;
public class BrowseStudentsAcceptedAdsScreen : StudentMenuScreenBase
{
    private int _page = 1;
    Paginator<Ad> _paginator;
    public BrowseStudentsAcceptedAdsScreen(IMenuService menuService, IStudentService studentService) : base(menuService, studentService)
    {
        List<Ad> acceptedAds = _studentService.GetStudentsAcceptedAds();
        PaginatorOptions<Ad> options = new()
        {
            CollectionName = "Ogłoszenia",
            DisplayItem = DisplayAd
        };
        _paginator = new(acceptedAds, options);
    }
    public override MenuNavigation Display()
    {
        Console.Write("Ogłoszenia, na które jesteś zapisany/a \t");
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

        Console.WriteLine();
    }
}
