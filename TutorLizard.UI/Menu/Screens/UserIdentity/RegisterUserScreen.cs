using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.UserIdentity;
public class RegisterUserScreen : UserIdentityMenuScreenBase
{
    public RegisterUserScreen(IMenuService menuService, IUserIdentityService userIdentityService) : base(menuService, userIdentityService)
    {
    }

    public override MenuNavigation Display()
    {
        Console.WriteLine("Rejestracja użytkownika");
        Console.WriteLine();
        Console.WriteLine("Podaj nazwę użytkownika.");
        Console.WriteLine("Aby anulować, wciśnij Enter bez podawania wartości.");
        Console.WriteLine();

        string exitString = "";

        string? userName = ConsoleParser.AskForString(new()
        {
            ExitString = exitString,
            Message = "Nazwa użytkownika: ",
            Predicate = s => _userIdentityService.IsUserNameTaken(s) == false,
            RetryMessage = "Podana nazwa użytkownika nie jest dostępna. Spróbuj ponownie."
        });

        if (userName is null)
            return Cancel();

        UserType userType;

        int selectedType = SelectTool.SelectOne(["Uczeń", "Nauczyciel"]);
        if (selectedType == 0)
            userType = UserType.Student;
        else
            userType = UserType.Tutor;

        Console.WriteLine();
        Console.WriteLine($"Czy utworzyć użytkownika {userName} ({userType.UiName()})");
        int selectedAccept = SelectTool.SelectOne(["Nie","Tak"]);
        if (selectedAccept == 0)
            return Cancel();
        Console.WriteLine();

        int userId = _userIdentityService.RegisterUser(userName, userType);

        Console.WriteLine("Użytkownik zarejestrowany.");
        Console.WriteLine($"Twoje id to {userId} - zapamiętaj je! Będzie ci potrzebne w takcie logowania.");
        Console.Write("Wróć do menu głównego...");
        Console.ReadKey(true);
        return MenuNavigation.Previous;
    }

    private MenuNavigation Cancel()
    {
        Console.WriteLine();
        Console.WriteLine("Rejestracja użytkowika anulowana.");
        Console.Write("Wróć do menu głównego...");
        Console.ReadKey(true);
        return MenuNavigation.Previous;
    }
}
