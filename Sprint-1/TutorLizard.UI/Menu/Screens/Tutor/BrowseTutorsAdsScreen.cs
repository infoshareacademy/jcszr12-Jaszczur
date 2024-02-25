using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsAdsScreen : TutorMenuScreenBase
{
    private int _page = 1;
    Paginator<Ad> _paginator;
    List<AdRequest> _accepted;
    public BrowseTutorsAdsScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
        List<Ad> ads = _tutorService.GetTutorsAds();
        _accepted = _tutorService.GetTutorsAdRequests()
            .Where(r => r.IsAccepted)
            .ToList();
        PaginatorOptions<Ad> options = new()
        {
            CollectionName = "Ogłoszenia",
            DisplayItem = DisplayAd
        };
        _paginator = new(ads, options);
    }

    public override MenuNavigation Display()
    {
        Console.Write("Twoje ogłoszenia\t");
        return _paginator.DisplayPage(ref _page);
    }

    private void DisplayAd(Ad ad, int i)
    {
        List<string> studentNames = _accepted
            .Where(r => r.AdId == ad.Id)
            .Select(r => _tutorService.GetStudentUserNameByAdRequestId(r.Id))
            .ToList();

        Console.WriteLine($"{i + 1}.\tId: {ad.Id}");
        Console.WriteLine($"\tTytuł: {ad.Title}");
        Console.WriteLine($"\tTematyka: {ad.Subject}");
        Console.WriteLine($"\tOpis: {ad.Description}");
        Console.WriteLine($"\tZapisani uczniowie: {studentNames.Count}");
        foreach (var name in studentNames)
        {
            Console.WriteLine($"\t\t{name}");
        }

        Console.WriteLine();
    }
}
