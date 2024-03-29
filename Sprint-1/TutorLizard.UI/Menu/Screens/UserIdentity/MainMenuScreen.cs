﻿using TutorLizard.BusinessLogic.Models;
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
        var userType = _userIdentityService.GetUserType();

        Console.WriteLine("Menu główne");
        Console.WriteLine();
        switch (userType)
        {
            case UserType.Tutor:
                return DisplayTutorMenu();
            case UserType.Student:
                return DisplayStudentMenu();
            case null:
            default:
                return DisplayNotLoggedInMenu();
        }
    }

    private MenuNavigation DisplayNotLoggedInMenu()
    {
        string[] items = [
            "Zaloguj się", // 0
            "Zarejestruj użytkownika", // 1
            "Wyjdź" // 2
            ];

        int selected = SelectTool.SelectOne(items);

        switch (selected)
        {
            case 0:
                _menuService.AddNextScreen(MenuScreenName.Login);
                return MenuNavigation.NextOrCurrent;
            case 1:
                _menuService.AddNextScreen(MenuScreenName.RegisterUser);
                return MenuNavigation.NextOrCurrent;
            case 2:
                DisplaySelectedOption(items[selected]);
                return MenuNavigation.QuitProgram;
            default:
                break;
        }

        DisplaySelectedOption(items[selected]);
        return MenuNavigation.NextOrCurrent;
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
        string[] items = [
            "Dodaj ogłoszenie", // 0
            "Dodaj termin zajęć do ogłoszenia", // 1
            "Przeglądaj swoje ogłoszenia", // 2
            "Przeglądaj swoje terminy zajęć", // 3
            "Przeglądaj oczekujące zgłoszenia do twoich ofert", //4
            "Przeglądaj oczekujące zgłoszenia na terminy zajęć", //5
            "Wyloguj", // 6
            "Wyjdź" // 7
            ];

        int selected = SelectTool.SelectOne(items);

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
                _menuService.AddNextScreen(MenuScreenName.BrowseTutorsSchedule);
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
