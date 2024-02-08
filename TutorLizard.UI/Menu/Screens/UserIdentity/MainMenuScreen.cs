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
        int selected = SelectTool.SelectOne(["Zaloguj się", "Zarejestruj się", "Wyjdź"], new SelectOneOptions()
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
        string[] items = [
            "Przeglądaj wszystkie ogłoszenia", // 0
            "Wyślij zgłoszenie do ogłoszenia", // 1
            "Przeglądaj ogłoszenia, na które jesteś zapisany/a", // 2
            "Przeglądaj dostępne terminy zajęć", // 3
            "Wyślij zgłoszenie na termin zajęć", //4
            "Przeglądaj terminy zajęć, na które jesteś zapisany/a", //5
            "Wyloguj", // 6
            "Wyjdź" // 7
            ];

        int selected = SelectTool.SelectOne(items);

        switch (selected)
        {
            case 0:
                _menuService.AddNextScreen(MenuScreenName.BrowseAds);
                break;
            case 1:
                _menuService.AddNextScreen(MenuScreenName.CreateAdRequest);
                break;
            case 2:
                _menuService.AddNextScreen(MenuScreenName.BrowseStudentsAcceptedAds);
                break;
            case 3:
                _menuService.AddNextScreen(MenuScreenName.BrowseStudentsAvailableSchedule);
                break;
            case 4:
                _menuService.AddNextScreen(MenuScreenName.CreateScheduleItemRequest);
                break;
            case 5:
                _menuService.AddNextScreen(MenuScreenName.BrowseStudentsAcceptedSchedule);
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

    private MenuNavigation DisplayTutorMenu()
    {
        throw new NotImplementedException();
        // - Dodaj ogłoszenie
        // - Dodaj terminy do ogłoszenia
        // - Zobacz zgłoszenia do ogłoszeń
        // - Zobacz zgłoszenia na termin
        // - Wyświetl swoje ogłoszenia
        // - Wyświetl swoje terminy
        // - Wyloguj
        // - Wyjdź
    }

    private void DisplaySelectedOption(string selected)
    {
        Console.WriteLine();
        Console.WriteLine($"Wybrałeś: {selected}");
        Console.Write("Kontynuuj...");
        Console.ReadKey(true);
    }
}
