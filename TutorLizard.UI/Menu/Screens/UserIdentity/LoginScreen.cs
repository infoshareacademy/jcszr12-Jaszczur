using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.UserIdentity;
public class LoginScreen : UserIdentityMenuScreenBase
{
    public LoginScreen(IMenuService menuService, IUserIdentityService userIdentityService) : base(menuService, userIdentityService)
    {
    }

    public override MenuNavigation Display()
    {
        Console.WriteLine("Zaloguj się");
        Console.WriteLine();
        Console.WriteLine("Podaj swoją nazwę użytkownika oraz Id użytkownika, aby się zalogować.");
        Console.WriteLine("Aby anulować, wciśnij Enter bez podawania wartości.");
        Console.WriteLine();

        string exitString = "";

        string? userName = ConsoleParser.AskForString(new()
        {
            ExitString = exitString,
            Message = "Nazwa użytkownika: "
        });
        if (userName is null)
            return Cancel();

        int? userId = ConsoleParser.AskForInt(new()
        {
            ExitString = exitString,
            Message = "Id użytkownika: ",
            RetryMessage = "Id użytkownika musi być liczbą całkowitą."
        });
        if (userId is null)
            return Cancel();

        Console.WriteLine();

        bool loggedIn = _userIdentityService.LogIn(userName, (int)userId);

        if (loggedIn)
        {
            Console.WriteLine("Zalogowano.");
            Console.Write("Wróć do menu głównego...");
            Console.ReadKey(true);
            return MenuNavigation.Previous;
        }

        Console.WriteLine("Nieprawidłowe dane logowania.");
        Console.WriteLine();
        int selected = SelectTool.SelectOne(["Spróbuj ponownie", "Wróć do menu głównego"]);
        if (selected == 0)
            return MenuNavigation.NextOrCurrent;

        return MenuNavigation.Previous;
    }

    public MenuNavigation Cancel()
    {
        Console.WriteLine();
        Console.WriteLine("Logowanie anulowane.");
        Console.Write("Wróć do menu głównego...");
        Console.ReadKey(true);
        return MenuNavigation.Previous;
    }
}
