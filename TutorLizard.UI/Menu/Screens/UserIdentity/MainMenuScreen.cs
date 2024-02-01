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
}
