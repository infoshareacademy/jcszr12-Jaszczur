﻿using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Menu.Screens;

namespace TutorLizard.UI.Menu;
public class MenuService : IMenuService
{
    private readonly IUserIdentityService _userIdentityService;
    private readonly ITutorService _tutorService;
    private readonly IStudentService _studentService;
    private readonly Stack<IMenuScreen> _menuScreens = new();

    public MenuService(IUserIdentityService userIdentityService, ITutorService tutorService, IStudentService studentService)
    {
        _userIdentityService = userIdentityService;
        _tutorService = tutorService;
        _studentService = studentService;
    }

    public void Start()
    {
        AddNextScreen(MenuScreenName.Main);

        while (_menuScreens.Any())
        {
            DisplayScreen();
        }

        DisplayExitMessage();
    }

    public void AddNextScreen(MenuScreenName screenName)
    {
        IMenuScreen? screen = CreateScreen(screenName);
        if (screen is null)
            return;
        _menuScreens.Push(screen);
    }

    private void DisplayScreen()
    {
        Console.Clear();
        DisplayHeader();
        MenuNavigation navigation = _menuScreens.Peek().Display();
        Navigate(navigation);
    }

    private void Navigate(MenuNavigation navigation)
    {
        switch (navigation)
        {
            case MenuNavigation.NextOrCurrent:
                break;
            case MenuNavigation.Previous:
                _menuScreens.Pop();
                break;
            case MenuNavigation.QuitProgram:
                _menuScreens.Clear();
                break;
        }
    }

    private void DisplayHeader()
    {
        Console.Write("Tutor Lizard");
        Console.Write("\t\tUżytkownik: abc");
        Console.WriteLine();
        Console.WriteLine("-----------------------------------------");
    }

    private void DisplayExitMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Dziękujemy za skorzystanie z naszego programu.");
        Console.WriteLine("Wciśnij dowolny klawisz, aby zamknąć.");
        Console.ReadKey();
    }

    private IMenuScreen? CreateScreen(MenuScreenName name)
    {
        switch (name)
        {
            case MenuScreenName.Main:
                return new MainMenuScreen(this);
            default:
                return null;
        }
    }
}

public enum MenuScreenName
{
    Main
}