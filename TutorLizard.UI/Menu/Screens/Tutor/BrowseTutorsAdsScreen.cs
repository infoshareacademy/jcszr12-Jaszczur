using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsAdsScreen : TutorMenuScreenBase
{
    private int _page = 0;
    private int _adsPerPage = 4;
    public BrowseTutorsAdsScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    public override MenuNavigation Display()
    {
        List<Ad> ads = _tutorService.GetUsersAds();

        int pagesTotal = ads.Count / _adsPerPage;
        if (ads.Count % _adsPerPage != 0)
            pagesTotal++;

        int firstItem = _page * _adsPerPage;
        int lastItem = Math.Min((_page + 1) * _adsPerPage, ads.Count);

        Console.WriteLine("Twoje ogłoszenia");
        Console.WriteLine();
        Console.WriteLine($"Strona {_page + 1} z {pagesTotal}\tOgłoszenia {firstItem + 1}-{lastItem} z {ads.Count}");
        Console.WriteLine();

        for (int i = firstItem; i < lastItem; i++)
        {
            DisplayAd(ads[i], i);
        }

        if (pagesTotal == 1)
            return OnlyPageOptions();
        else if (_page == 0)
            return FirstPageOptions();
        else if (_page + 1 < pagesTotal)
            return MiddlePageOptions();
        else
            return LastPageOptions();
    }

    private void DisplayAd(Ad ad, int i)
    {
        Console.WriteLine($"{i + 1}. Id: {ad.Id}");
        Console.WriteLine($"Tytuł: {ad.Title}");
        Console.WriteLine($"Temat: {ad.Subject}");
        Console.WriteLine($"Opis: {ad.Description}");
        Console.WriteLine();
    }

    private MenuNavigation FirstPageOptions()
    {
        int selected = SelectTool.SelectOne(["Kolejna strona", "Wróc do menu głównego"]);
        switch (selected)
        {
            case 0:
                _page++;
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
    private MenuNavigation MiddlePageOptions()
    {
        int selected = SelectTool.SelectOne(["Kolejna strona", "Poprzednia strona", "Wróc do menu głównego"]);
        switch (selected)
        {
            case 0:
                _page++;
                return MenuNavigation.NextOrCurrent;
            case 1:
                _page--;
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
    private MenuNavigation LastPageOptions()
    {
        int selected = SelectTool.SelectOne(["Poprzednia strona", "Wróc do menu głównego"]);
        switch (selected)
        {
            case 0:
                _page--;
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
    private MenuNavigation OnlyPageOptions()
    {
        int selected = SelectTool.SelectOne(["Wróc do menu głównego"]);
        return MenuNavigation.Previous;
    }
}
