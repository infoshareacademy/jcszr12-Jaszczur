using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsAdsScreen : TutorMenuScreenBase
{
    private int _page = 1;
    Paginator<Ad> _paginator;
    public BrowseTutorsAdsScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
        List<Ad> ads = _tutorService.GetUsersAds();
        PaginatorOptions<Ad> options = new()
        {
            CollectionName = "Ogłoszenia",
            DisplayItem = DisplayAd
        };
        _paginator = new(ads, options);
    }

    public override MenuNavigation Display()
    {
        Console.WriteLine("Twoje ogłoszenia");
        Console.WriteLine();
        return _paginator.DisplayPage(ref _page);
    }

    private void DisplayAd(Ad ad, int i)
    {
        Console.WriteLine($"{i + 1}. Id: {ad.Id}");
        Console.WriteLine($"Tytuł: {ad.Title}");
        Console.WriteLine($"Temat: {ad.Subject}");
        Console.WriteLine($"Opis: {ad.Description}");
        Console.WriteLine();
    }
}
