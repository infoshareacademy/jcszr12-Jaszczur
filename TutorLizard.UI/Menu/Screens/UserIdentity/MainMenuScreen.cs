using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.UserIdentity;
public class MainMenuScreen : UserIdentityMenuScreenBase
{
    public MainMenuScreen(IMenuService menuService, IUserIdentityService userIdentityService) : base(menuService, userIdentityService)
    {
    }

    public override MenuNavigation Display()
    {
        //var userType = _userIdentityService.GetUserType();
        UserType? userType = null;

        switch (userType)
        {
            case BusinessLogic.Models.UserType.Tutor:
                return DisplayTutorMenu();
            case BusinessLogic.Models.UserType.Student:
                return DisplayStudentMenu();
            case null:
            default:
                return DisplayNotLoggedInMenu();
        }
    }

    private MenuNavigation DisplayNotLoggedInMenu()
    {
        int selected = SelectTool.SelectOne(["Zaloguj się", "Zarejestruj się",  "Wyjdź"], new SelectOneOptions()
        {
        });

        switch (selected)
        {
            case 0:
                // _menuService.AddNextScreen(MenuScreenName.Login);
                return MenuNavigation.NextOrCurrent;
            case 1:
                // _menuService.AddNextScreen(MenuScreenName.Register);
                return MenuNavigation.NextOrCurrent;
            case 2:
                return MenuNavigation.QuitProgram;
            default:
                return MenuNavigation.NextOrCurrent;
        }
    }

    private MenuNavigation DisplayStudentMenu()
    {
        throw new NotImplementedException();
        // - Przeglądaj wszystkie ogłoszenia
        // - Przeglądaj swoje ogłoszenia (zakceptowane)
        // - Zgłoś się na termin
        // - Przeglądaj swoje terminy (zaakceptowane)
        // - Wyloguj
        //      _userIdentityService.Logout();
        //      return MenuNavigation.NextOrCurrent;
        // - Wyjdź
    }

    private MenuNavigation DisplayTutorMenu()
    {
        string[] items = [
            "Dodaj ogłoszenie", // 0
            "Dodaj termin zajęć do ogłoszenia", // 1
            "Przeglądaj swoje ogłoszenia", // 2
            "Przeglądaj swoje terminy zajęć", // 3
            "Przeglądaj zgłoszenia do twoich ofert", //4
            "Przeglądaj zgłoszenia na terminy zajęć", //5
            "Wyloguj", // 6
            "Wyjdź" // 7
            ];

        int selected = SelectTool.SelectOne(items, new());

        switch (selected)
        {
            case 0:
                _menuService.AddNextScreen(MenuScreenName.CreateAd);
                break;
            case 1:
                _menuService.AddNextScreen(MenuScreenName.CreateScheduleItem);
                break;
            case 2:
                _menuService.AddNextScreen(MenuScreenName.BrowseTutorsAds);
                break;
            case 3:
                _menuService.AddNextScreen(MenuScreenName.BrowseTutorsAds);
                break;
            case 4:
                _menuService.AddNextScreen(MenuScreenName.BrowseTutorsAdRequests);
                break;
            case 5:
                _menuService.AddNextScreen(MenuScreenName.BrowseTutorsScheduleItemRequests);
                break;
            case 6:
                _userIdentityService.LogOut();
                break;
            case 7:
                DisplaySelectedOption(items[selected]);
                return MenuNavigation.QuitProgram;
            default:
                break;
        }

        DisplaySelectedOption(items[selected]);
        return MenuNavigation.NextOrCurrent;
    }

    private void DisplaySelectedOption(string selected)
    {
        Console.WriteLine();
        Console.WriteLine($"Wybrałeś: {selected}");
        Console.Write("Kontynuuj...");
        Console.ReadKey(true);
    }
}
